using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class Login
    {
        [Key]
        public int RecId { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }

        public int Type { get; set; }

        public string UserName { get; set; }

    }
}
