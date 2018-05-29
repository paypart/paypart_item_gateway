using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using paypart_item_gateway.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using paypart_item_gateway.Models;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace paypart_item_gateway.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/costitem")]
    public class CostItemController : Controller
    {
        private readonly IItemSqlServerRepository serviceSqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        public CostItemController(IOptions<Settings> _settings,
          IItemSqlServerRepository _serviceSqlRepo, IDistributedCache _cache)
        {
            settings = _settings;
            serviceSqlRepo = _serviceSqlRepo;
            cache = _cache;
        }

        [HttpGet("getcostItems/{serviceid}")]
        [ProducesResponseType(typeof(CostItem), 200)]
        [ProducesResponseType(typeof(ItemError), 400)]
        [ProducesResponseType(typeof(ItemError), 500)]
        public async Task<IActionResult> getcostItems(int serviceid)
        {
            List<CostItem> costitems = null;
            ItemError e = new ItemError();

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            Redis redis = new Redis(settings, cache);
            string key = "costitems_" + serviceid;

            //check redis cache for details
            try
            {
                costitems = await redis.getcostitems(key, cts.Token);

                if (costitems != null && costitems.Count > 0)
                {
                    return CreatedAtAction("getcostItems", costitems);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Get cost items from Sql
            try
            {
                costitems = await serviceSqlRepo.GetCostItems(serviceid);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Write to Redis
            try
            {
                if (costitems != null && costitems.Count > 0)
                  await redis.setcostitems(key, costitems, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return CreatedAtAction("getcostItems", costitems);
        }
        [HttpPost("addcostitems")]
        [ProducesResponseType(typeof(CostItem), 200)]
        [ProducesResponseType(typeof(ItemError), 400)]
        [ProducesResponseType(typeof(ItemError), 500)]
        public async Task<IActionResult> addcostitems([FromBody]List<CostItem> costitems)
        {
            List<CostItem> _costitems = null;
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
                _costitems = await serviceSqlRepo.AddUpdateCostItems(costitems);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }

            //Write to Redis
            try
            {
                string key = "costitems_" + _costitems[0].serviceid;

                if (_costitems != null && _costitems.Count > 0)
                    await redis.setcostitems(key, _costitems, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return CreatedAtAction("addcostitems", _costitems);

        }
    }
}