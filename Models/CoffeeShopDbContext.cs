using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheCoffeeShop.Models;

public partial class CoffeeShopDbContext : DbContext
{
    public CoffeeShopDbContext()
    {
    }

    public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267253E47F43CF9");

            entity.ToTable("Account");

            entity.HasIndex(e => e.UserName, "UQ__Account__66DCF95C33AA0155").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E61641353F723").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.Adress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("adress");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.IsBan)
                .HasDefaultValue(false)
                .HasColumnName("isBan");
            entity.Property(e => e.PassWord)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("passWord");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__FA0AA70D755DB08C");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("blogID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.BlogContent)
                .IsUnicode(false)
                .HasColumnName("blogContent");
            entity.Property(e => e.BlogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("blogDate");
            entity.Property(e => e.BlogImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("blogImage");
            entity.Property(e => e.BlogName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("blogName");
            entity.Property(e => e.ProductId).HasColumnName("productID");

            entity.HasOne(d => d.Account).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Blog__accountID__4BAC3F29");

            entity.HasOne(d => d.Product).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Blog__productID__4CA06362");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__415B03D8E8DD3FC7");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quality).HasColumnName("quality");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Cart__accountID__4222D4EF");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cart__productID__4316F928");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1F89561B4FF");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337D15092FDD");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("orderDate");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Order__accountID__47DBAE45");

            entity.HasOne(d => d.Cart).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__Order__cartID__46E78A0C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D14A757E6391");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("productImage");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("productName");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("productPrice");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__categor__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
