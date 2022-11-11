using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OpenRestRestaurant_data.DataAccess;

public partial class OpenRestRestaurantDbContext : DbContext
{
    public OpenRestRestaurantDbContext()
    {
    }

    public OpenRestRestaurantDbContext(DbContextOptions<OpenRestRestaurantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<ProductMeal> ProductMeals { get; set; }

    public virtual DbSet<RestaurantCompany> RestaurantCompanies { get; set; }

    public virtual DbSet<RestaurantLocation> RestaurantLocations { get; set; }

    public virtual DbSet<RestaurantStaff> RestaurantStaffs { get; set; }

    public virtual DbSet<RestaurantTable> RestaurantTables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VL2FT7Q\\SQLEXPRESS;Database=OpenRestRestaurantDB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CloseOrderTime).HasColumnType("datetime");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.OpenOrderTime).HasColumnType("datetime");
            entity.Property(e => e.RestaurantCompanyId).HasColumnName("RestaurantCompanyID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.TableId).HasColumnName("TableID");

            entity.HasOne(d => d.Location).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Location__4BAC3F29");

            entity.HasOne(d => d.RestaurantCompany).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RestaurantCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Restaura__4D94879B");

            entity.HasOne(d => d.Staff).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__StaffID__4CA06362");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__Orders__TableID__4AB81AF0");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SubtotalLine).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TaxesLine).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__534D60F1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__5441852A");
        });

        modelBuilder.Entity<ProductMeal>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.OtherTaxes).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RestaurantCompanyId).HasColumnName("RestaurantCompanyID");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Taxes).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<RestaurantCompany>(entity =>
        {
            entity.ToTable("RestaurantCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.FiscalId).HasColumnName("FiscalID");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.RestaurantNumber).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<RestaurantLocation>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.FiscalId).HasColumnName("FiscalID");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.RestaurantCompanyId).HasColumnName("RestaurantCompanyID");

            entity.HasOne(d => d.Manager).WithMany(p => p.RestaurantLocations)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Restauran__Manag__34C8D9D1");

            entity.HasOne(d => d.RestaurantCompany).WithMany(p => p.RestaurantLocations)
                .HasForeignKey(d => d.RestaurantCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Restauran__Resta__35BCFE0A");
        });

        modelBuilder.Entity<RestaurantStaff>(entity =>
        {
            entity.ToTable("RestaurantStaff");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.FiscalId).HasColumnName("FiscalID");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.RestaurantCompanyId).HasColumnName("RestaurantCompanyID");
            entity.Property(e => e.RestaurantLocationId).HasColumnName("RestaurantLocationID");
            entity.Property(e => e.Ssn).HasColumnName("SSN");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.RestaurantCompany).WithMany(p => p.RestaurantStaffs)
                .HasForeignKey(d => d.RestaurantCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Restauran__Resta__4222D4EF");

            entity.HasOne(d => d.RestaurantLocation).WithMany(p => p.RestaurantStaffs)
                .HasForeignKey(d => d.RestaurantLocationId)
                .HasConstraintName("FK__Restauran__Resta__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.RestaurantStaffs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Restauran__UserI__412EB0B6");
        });

        modelBuilder.Entity<RestaurantTable>(entity =>
        {
            entity.ToTable("RestaurantTable");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.RestaurantLocationId).HasColumnName("RestaurantLocationID");
            entity.Property(e => e.UicoordLocation).HasColumnName("UICoordLocation");

            entity.HasOne(d => d.RestaurantLocation).WithMany(p => p.RestaurantTables)
                .HasForeignKey(d => d.RestaurantLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Restauran__Resta__49C3F6B7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
