using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopManagementSystem.Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffShopLink> StaffShopLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07B1F58C96");

            entity.ToTable("Location");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shop__3214EC07ADB874E3");

            entity.ToTable("Shop");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Staff__3214EC074596F6FD");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<StaffShopLink>(entity =>
        {
            entity.HasKey(e => new { e.StaffId, e.ShopId }).HasName("PK__StaffSho__10A8FE6B764702B0");

            entity.ToTable("StaffShopLink");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
