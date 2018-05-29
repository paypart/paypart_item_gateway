using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using paypart_item_gateway.Services;
using Microsoft.Extensions.Options;
using paypart_item_gateway.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace paypart_item_gateway.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/formitem")]
    public class FormItemController : Controller
    {
        private readonly IItemSqlServerRepository serviceSqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        public FormItemController(IOptions<Settings> _settings,
          IItemSqlServerRepository _serviceSqlRepo, IDistributedCache _cache)
        {
            settings = _settings;
            serviceSqlRepo = _serviceSqlRepo;
            cache = _cache;
        }

        [HttpGet("getformitems/{serviceid}")]
        [ProducesResponseType(typeof(FormItem), 200)]
        [ProducesResponseType(typeof(ItemError), 400)]
        [ProducesResponseType(typeof(ItemError), 500)]
        public async Task<IActionResult> getformitems(int serviceid)
        {
            List<FormItem> formitems = null;
            ItemError e = new ItemError();

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            Redis redis = new Redis(settings, cache);
            string key = "formitems_" + serviceid;

            //check redis cache for details
            try
            {
                formitems = await redis.getformitems(key, cts.Token);

                if (formitems != null && formitems.Count > 0)
                {
                    return CreatedAtAction("getformitems", formitems);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Get cost items from Sql
            try
            {
                formitems = await serviceSqlRepo.GetFormItems(serviceid);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Write to Redis
            try
            {
                if (formitems != null && formitems.Count > 0)
                    await redis.setformitems(key, formitems, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return CreatedAtAction("getformitems", formitems);
        }

        [HttpPost("addformitems")]
        [ProducesResponseType(typeof(CostItem), 200)]
        [ProducesResponseType(typeof(ItemError), 400)]
        [ProducesResponseType(typeof(ItemError), 500)]
        public async Task<IActionResult> addformitems([FromBody]List<FormItem> formitems)
        {
            List<FormItem> _formitems = null;
            ItemError e = new ItemError();
            Redis redis = new Redis(settings, cache);

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<ItemError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }


            //Add to sql server
            try
            {
                _formitems = await serviceSqlRepo.AddFormItems(formitems);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }

            //Write to Redis
            try
            {
                string key = "formitems_" + _formitems[0].serviceid;

                if (_formitems != null && _formitems.Count > 0)
                    await redis.setformitems(key, _formitems, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return CreatedAtAction("addformitems", _formitems);

        }
    }
}
