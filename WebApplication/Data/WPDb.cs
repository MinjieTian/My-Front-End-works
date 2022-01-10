using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class WPDb : DbContext
    {
        public WPDb(DbContextOptions<WPDb> options) : base(options) { }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<TheComment> Comments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
