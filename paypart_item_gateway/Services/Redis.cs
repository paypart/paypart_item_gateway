using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using paypart_item_gateway.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;

namespace paypart_item_gateway.Services
{
    public class Redis
    {
        IOptions<Settings> settings;
        IDistributedCache redis;
        public delegate void SetService(string key, ItemModel item);
        public delegate void SetServices(string key, IEnumerable<ItemModel> items);

        public Redis(IOptions<Settings> _settings, IDistributedCache _redis)
        {
            settings = _settings;
            redis = _redis;
        }
        public async Task<ItemModel> getitem(string key, CancellationToken ctx)
        {
            ItemModel items = new ItemModel();
            try
            {
                var item = await redis.GetStringAsync(key, ctx);
                if (item != null)
                    items = JsonHelper.fromJson<ItemModel>(item);
            }
            catch (Exception)
            {

            }
            return items;
        }

        public async Task<List<ItemModel>> getitems(string key, CancellationToken ctx)
        {
            List<ItemModel> items = new List<ItemModel>();
            try
            {
                var item = await redis.GetStringAsync(key, ctx);
                if (item != null)
                    items = JsonHelper.fromJson<List<ItemModel>>(item);
            }
            catch (Exception)
            {

            }
            return items;
        }

        public async Task<List<CostItem>> getcostitems(string key, CancellationToken ctx)
        {
            List<CostItem> items = new List<CostItem>();
            try
            {
                var item = await redis.GetStringAsync(key, ctx);
                if (item != null)
                    items = JsonHelper.fromJson<List<CostItem>>(item);
            }
            catch (Exception)
            {

            }
            return items;
        }

        public async Task<List<FormItem>> getformitems(string key, CancellationToken ctx)
        {
            List<FormItem> items = new List<FormItem>();
            try
            {
                var item = await redis.GetStringAsync(key, ctx);
                if (item != null)
                    items = JsonHelper.fromJson<List<FormItem>>(item);
            }
            catch (Exception)
            {

            }
            return items;
        }
        public async void setitem(string key, ItemModel items)
        {
            try
            {
                var item = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(item))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(items);

                await redis.SetStringAsync(key, value);
            }
            catch (Exception)
            {

            }

        }
        public async Task setitemAsync(string key, ItemModel items, CancellationToken cts)
        {
            try
            {
                var item = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(item))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(items);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {
            }

        }
        public async Task setitems(string key, List<ItemModel> items, CancellationToken cts)
        {
            try
            {
                var item = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(item))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(items);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }
        }
        public async Task setcostitems(string key, List<CostItem> items, CancellationToken cts)
        {
            try
            {
                var item = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(item))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(items);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }
        }
        public async Task setformitems(string key, List<FormItem> items, CancellationToken cts)
        {
            try
            {
                var item = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(item))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(items);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }

        }
    }
}
