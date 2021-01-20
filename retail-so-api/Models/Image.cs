using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class Image
    {
        [Key]
        public Int64 RecId { get; set; }
        public byte Images { get; set; }
        public string Name { get; set; }
        public Int64 SODaily { get; set; }
        public DateTime CreateDateTime { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
