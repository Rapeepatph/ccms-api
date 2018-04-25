using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CCMSAPI.DBModels
{
    public partial class CCMSAngular5NewContext : DbContext
    {
        public virtual DbSet<Buildings> Buildings { get; set; }
        public virtual DbSet<Equipments> Equipments { get; set; }
        public virtual DbSet<MainServices> MainServices { get; set; }
        public virtual DbSet<Services> Services { get; set; }

        public CCMSAngular5NewContext(DbContextOptions options)
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

            modelBuilder.Entity<Equipments>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Equipments)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipments_Buildings");
            });

            modelBuilder.Entity<MainServices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.MainServices)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MainServices_Buildings");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataEquipment).IsUnicode(false);

                entity.Property(e => e.MainServiceId).HasColumnName("MainServiceID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.MainServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Services_MainServices");
            });
        }
    }
}
