using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace retail_so_api.Models
{
    public class LoginDBContext : DbContext
    {

        public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options)
        {

        }

        public DbSet<Login> Login { get; set; }
    }
}
