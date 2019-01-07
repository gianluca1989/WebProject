using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Autentication;
using WebProject.Models;
using WebProject.Utils;

namespace WebProject.Entity
{
    public class MyContext : IdentityDbContext<StoreUser>
    {
        public MyContext()
        {

        }

        public MyContext(DbContextOptions<MyContext> o) : base(o)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder oB)
        {
            if (!oB.IsConfigured)
                oB.UseSqlServer(new Configuration().GetField("ConnectionStrings:DefaultConnection"));
        }

        public DbSet<Order> Order { get; set; }
        public DbSet<StoreUser> StoreUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
