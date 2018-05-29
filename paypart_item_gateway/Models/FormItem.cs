using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_item_gateway.Models
{
    public class FormItem
    {
        [Key]
        public int id { get; set; }
        public int serviceid { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string mandate { get; set; }
        public int status { get; set; }
    }
}
