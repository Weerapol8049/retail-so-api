using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class OrderLine
    {
        [Key]
        public Int64 RecId { get; set; }
        public Int64 RecIdHeader { get; set; }
        
        public string Series { get; set; }

        public string Model { get; set; }

        public string Sink { get; set; }

        public string Top { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

    }
}
