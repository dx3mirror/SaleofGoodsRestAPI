using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductSalesEntity.Entity;

public partial class ProductSalesContext : DbContext
{
    public ProductSalesContext()
    {
    }

    public ProductSalesContext(DbContextOptions<ProductSalesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<TrackingNumber> TrackingNumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer("Server=DX3MIRROR;Initial Catalog=ProductSales;Integrated Security=True;Encrypt=False")
            .UseLazyLoadingProxies()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED4008A8B9");

            entity.HasIndex(e => e.Name, "UC_ProductName").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C41FD011B6E6");

            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SaleDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductSales");
        });

        modelBuilder.Entity<TrackingNumber>(entity =>
        {
            entity.HasKey(e => e.TrackingNumberId).HasName("PK__Tracking__E520E2629046AC66");

            entity.HasIndex(e => e.TrackingNumber1, "UC_TrackingNumber").IsUnique();

            entity.Property(e => e.TrackingNumberId).HasColumnName("TrackingNumberID");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.TrackingNumber1)
                .HasMaxLength(50)
                .HasColumnName("TrackingNumber");

            entity.HasOne(d => d.Sale).WithMany(p => p.TrackingNumbers)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_SaleTracking");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
