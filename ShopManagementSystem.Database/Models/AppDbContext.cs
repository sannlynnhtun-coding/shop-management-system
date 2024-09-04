using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopManagementSystem.Database.Models;

public partial class AppDbContext : DbContext
{
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
        modelBuilder.Entity<StaffShopLink>(entity =>
        {
            entity.HasKey(e => new { e.StaffId, e.ShopId });

            entity.HasOne(d => d.Shop)
                .WithMany(p => p.StaffShopLinks)
                .HasForeignKey(d => d.ShopId);

            entity.HasOne(d => d.Staff)
                .WithMany(p => p.StaffShopLinks)
                .HasForeignKey(d => d.StaffId);
        });
    }
}
