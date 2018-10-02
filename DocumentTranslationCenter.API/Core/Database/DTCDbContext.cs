using DocumentTranslationCenter.API.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentTranslationCenter.API.Core.Database
{
    public class DtcDbContext : DbContext
    {
        public DtcDbContext(DbContextOptions<DtcDbContext> context) : base(context)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Deleted).HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
