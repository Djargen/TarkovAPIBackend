using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tarkov.Infrastructure.Data
{
    public class TarkovDbContextFactory : IDesignTimeDbContextFactory<TarkovDbContext>
    {
        public TarkovDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TarkovDbContext>();
            optionsBuilder.UseSqlite("Data Source=tarkov.db");

            return new TarkovDbContext(optionsBuilder.Options);
        }
    }
}