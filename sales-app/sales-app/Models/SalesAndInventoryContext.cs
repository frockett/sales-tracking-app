using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace sales_app.Models;

public partial class SalesAndInventoryContext : DbContext
{
    public SalesAndInventoryContext()
    {
    }

    public SalesAndInventoryContext(DbContextOptions<SalesAndInventoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sale> Sales { get; set; }
    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=sales-and-inventory.db");
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sales");

            entity.Property(e => e.Cost).HasColumnType("INT");
            entity.Property(e => e.DateOfSale).HasColumnName("Date_of_sale");
            entity.Property(e => e.Margin).HasColumnType("DECIMAL");
            entity.Property(e => e.Profit).HasColumnType("INT");
            entity.Property(e => e.SalePrice)
                .HasColumnType("INT")
                .HasColumnName("Sale_price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
