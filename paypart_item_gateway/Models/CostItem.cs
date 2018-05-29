using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_item_gateway.Models
{
    public class CostItem
    {
        [Key]
        public int id { get; set; }
        public int serviceid { get; set; }
        public string title { get; set; }
        public decimal cost { get; set; }
        public string currency { get; set; }
        public string mandate { get; set; }
        public int status { get; set; }
    }
}
