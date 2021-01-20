using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class ImageDBContext : DbContext
    {

        public ImageDBContext(DbContextOptions<ImageDBContext> options) : base(options)
        {

        }

        public DbSet<Image> Images { get; set; }
    }
}
