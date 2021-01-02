using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class Store
    {
        [Key]
        public Int64 RecId { get; set; }
        public string StoreId { get; set; }

        public string Name { get; set; }
    }
}
