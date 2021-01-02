using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class Serie
    {
        [Key]
        public string Series { get; set; }
    }
}
