using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_item_gateway.Models
{
    public class ItemModel
    {
        public List<CostItem> costitems { get; set; }
        public List<FormItem> formitems { get; set; }
    }
}
