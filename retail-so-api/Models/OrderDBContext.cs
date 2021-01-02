using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options) : base(options)
        {

        }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<OrderLine> OrdersLine { get; set; }

        public DbSet<Pool> Pools { get; set; }

        public DbSet<Serie> Series { get; set; }
        public DbSet<Model> Models { get; set; }

        public DbSet<Store> Stores { get; set; }
    }
}
