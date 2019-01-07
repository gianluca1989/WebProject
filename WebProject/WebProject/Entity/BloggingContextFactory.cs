using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Utils;

namespace WebProject.Entity
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
    
        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer(new Configuration().GetField("ConnectionStrings:DefaultConnection"));
            return new MyContext(optionsBuilder.Options);
        }
    }
}
