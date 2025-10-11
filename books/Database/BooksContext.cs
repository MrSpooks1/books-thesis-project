using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace books;

public partial class BooksContext : DbContext
{
    public BooksContext()
    {
        
    }

    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Outlet> Outlets { get; set; }

    public virtual DbSet<OutletProduct> OutletProducts { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ProviderType> ProviderTypes { get; set; }

    public virtual DbSet<ReceivedProduct> ReceivedProducts { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<SoldProduct> SoldProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=books;Password=root;Username=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.PassportSerialNumber)
                .HasMaxLength(10)
                .HasColumnName("passport_serial_number");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Login)
                .HasMaxLength(30)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.AccessLevel).HasColumnName("access_level");
        });

        modelBuilder.Entity<Outlet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("outlet_pkey");

            entity.ToTable("outlet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OutletAddress)
                .HasMaxLength(200)
                .HasColumnName("outlet_address");
        });

        modelBuilder.Entity<OutletProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("outlet_product_pkey");

            entity.ToTable("outlet_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OutletId).HasColumnName("outlet_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Outlet).WithMany(p => p.OutletProducts)
                .HasForeignKey(d => d.OutletId)
                .HasConstraintName("outlet_product_outlet_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OutletProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("outlet_product_product_id_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_method_pkey");

            entity.ToTable("payment_method");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Method)
                .HasMaxLength(20)
                .HasColumnName("method");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
            entity.Property(e => e.ProviderPrice).HasColumnName("provider_price");
            entity.Property(e => e.SalePrice).HasColumnName("sale_price");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("product_product_type_id_fkey");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_type_pkey");

            entity.ToTable("product_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provider_pkey");

            entity.ToTable("provider");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(40)
                .HasColumnName("email_address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.ProviderTypeId).HasColumnName("provider_type_id");

            entity.HasOne(d => d.ProviderType).WithMany(p => p.Providers)
                .HasForeignKey(d => d.ProviderTypeId)
                .HasConstraintName("provider_provider_type_id_fkey");
        });

        modelBuilder.Entity<ProviderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provider_type_pkey");

            entity.ToTable("provider_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasColumnName("type");
        });

        modelBuilder.Entity<ReceivedProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("received_product_pkey");

            entity.ToTable("received_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShipmentId).HasColumnName("shipment_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ReceivedProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("received_product_product_id_fkey");

            entity.HasOne(d => d.Shipment).WithMany(p => p.ReceivedProducts)
                .HasForeignKey(d => d.ShipmentId)
                .HasConstraintName("received_product_shipment_id_fkey");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.ReceiptNumber).HasName("sale_pkey");

            entity.ToTable("sale");

            entity.Property(e => e.ReceiptNumber).HasColumnName("receipt_number");
            entity.Property(e => e.CustomerCardNumber)
                .HasMaxLength(30)
                .HasColumnName("customer_card_number");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.OutletId).HasColumnName("outlet_id");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("sale_employee_id_fkey");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Sales)
                .HasForeignKey(d => d.OutletId)
                .HasConstraintName("sale_outlet_id_fkey");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("sale_payment_method_id_fkey");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shipment_pkey");

            entity.ToTable("shipment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.OutletId).HasColumnName("outlet_id");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("shipment_employee_id_fkey");

            entity.HasOne(d => d.Outlet).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.OutletId)
                .HasConstraintName("shipment_outlet_id_fkey");

            entity.HasOne(d => d.Provider).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("shipment_provider_id_fkey");
        });

        modelBuilder.Entity<SoldProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sold_product_pkey");

            entity.ToTable("sold_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");

            entity.HasOne(d => d.Product).WithMany(p => p.SoldProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("sold_product_product_id_fkey");

            entity.HasOne(d => d.Sale).WithMany(p => p.SoldProducts)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("sold_product_sale_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
