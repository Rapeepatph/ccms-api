using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CCMSAPI.DBModels
{
    public partial class CCMSAngular5TestContext : DbContext
    {
        public virtual DbSet<Buildings> Buildings { get; set; }

        public CCMSAngular5TestContext(DbContextOptions<CCMSAngular5TestContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buildings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
