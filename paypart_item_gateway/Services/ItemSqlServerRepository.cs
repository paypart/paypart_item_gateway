using Microsoft.EntityFrameworkCore;
using paypart_item_gateway.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_item_gateway.Services
{
    public class ItemSqlServerRepository : IItemSqlServerRepository
    {
        private readonly ItemSqlServerContext _context = null;

        public ItemSqlServerRepository(ItemSqlServerContext context)
        {
            _context = context;
        }

        public async Task<List<CostItem>> GetCostItems(int serviceid)
        {
            return await _context.CostItem.Where(c => c.serviceid == serviceid && c.status == (int)Status.Active).ToListAsync();
        }

        public async Task<CostItem> GetCostItem(int id)
        {
            return await _context.CostItem.Where(c => c.id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<FormItem>> GetFormItems(int serviceid)
        {
            return await _context.FormItem.Where(c => c.serviceid == serviceid && c.status == (int)Status.Active).ToListAsync();
        }

        public async Task<FormItem> GetFormItem(int id)
        {
            return await _context.FormItem.Where(c => c.id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<CostItem> AddCostItems(CostItem item)
        {
            CostItem citem = null;
            //await _context.CostItem.AddRangeAsync(items);
            await _context.CostItem.AddAsync(item);

            await _context.SaveChangesAsync();
            citem = await GetCostItem(item.id);
            return citem;
        }

        public async Task<List<FormItem>> AddFormItems(List<FormItem> items)
        {
            List<FormItem> fitems = null;

            await _context.FormItem.AddRangeAsync(items);
            await _context.SaveChangesAsync();
            if (items.Count > 0)
                fitems = await GetFormItems(items[0].serviceid);
            return fitems;
        }

        public async Task<List<CostItem>> AddUpdateCostItems(List<CostItem> items)
        {
            List<CostItem> citems = new List<CostItem>();
            CostItem citem = null;
            foreach (var item in items)
            {
                if (item.id == 0)
                {
                    citem = await AddCostItems(item);
                    citems.Add(citem);
                }
                else if (item.id > 0 && item.status == 2)
                {
                    citem = await UpdateCostItems(item);
                    citems.Add(citem);
                }
            }
            //await _context.CostItem.AddRangeAsync(items);
            //await _context.SaveChangesAsync();
            //if (items.Count > 0)
            //    citems = await GetCostItems(items[0].serviceid);
            return citems;
        }
        //public async Task<DeleteResult> RemoveBiller(string id)
        //{
        //    return await _context.Billers.Remove(
        //                 Builders<Biller>.Filter.Eq(s => s._id, id));
        //}

        public async Task<CostItem> UpdateCostItems(CostItem item)
        {
            var citem = _context.CostItem.FirstOrDefault(c => c.id == item.id);
            citem.status = item.status;

            await _context.SaveChangesAsync();
            citem = await GetCostItem(item.id);

            return citem;
        }

        //public async Task<ReplaceOneResult> UpdateBiller(string id, Biller item)
        //{
        //    return await _context.Billers
        //                         .ReplaceOneAsync(n => n._id.Equals(id)
        //                                             , item
        //                                             , new UpdateOptions { IsUpsert = true });
        //}

        //public async Task<DeleteResult> RemoveAllBillers()
        //{
        //    return await _context.Billers.DeleteManyAsync(new BsonDocument());
        //}
    }
}
