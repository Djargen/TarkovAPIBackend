using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tarkov.Infrastructure.Data.Entities;

namespace Tarkov.Infrastructure.Data
{
    internal class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ItemEntity> Items { get; set; }
    }
}
