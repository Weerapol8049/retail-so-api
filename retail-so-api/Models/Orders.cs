using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class Orders
    {
        [Key]
        public Int64 RecId { get; set; }

        public string SalesId { get; set; }

        public string PurchId { get; set; }

        public string StoreId { get; set; }

        public string StoreName { get; set; }

        public string CustName { get; set; }

        public string Pool { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime ConfirmDate { get; set; }

        public int LineCount { get; set; }

        public int ImageCount { get; set; }

    }
}
