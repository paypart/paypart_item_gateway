using System.Collections.Generic;
using paypart_item_gateway.Models;
using System.Threading.Tasks;

namespace paypart_item_gateway.Services
{
    public interface IItemSqlServerRepository
    {
        Task<List<CostItem>> GetCostItems(int serviceid);
        Task<CostItem> GetCostItem(int id);
        Task<List<FormItem>> GetFormItems(int serviceid);
        Task<FormItem> GetFormItem(int id);
        Task<CostItem> AddCostItems(CostItem items);
        Task<List<FormItem>> AddFormItems(List<FormItem> items);

        Task<List<CostItem>> AddUpdateCostItems(List<CostItem> items);
        Task<CostItem> UpdateCostItems(CostItem items);

        //Task<DeleteResult> RemoveBiller(string id);
        //Task<UpdateResult> UpdateBiller(string id, string title);

        // demo interface - full document update
        //Task<ReplaceOneResult> UpdateBiller(string id, Biller item);

        // should be used with high cautious, only in relation with demo setup
        //Task<DeleteResult> RemoveAllBillers();

    }
}
