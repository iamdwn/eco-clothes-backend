using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DataAccess.Persistences;

public partial class EcoClothesContext : DbContext
{
    public EcoClothesContext()
    {
    }

    public EcoClothesContext(DbContextOptions<EcoClothesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentSubscription> PaymentSubscriptions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<SizeProduct> SizeProducts { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=database.hdang09.me;database=eco_clothes_schema;uid=root;pwd=my-secret-pw", ServerVersion.Parse("9.0.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PRIMARY");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Cart_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Cart_ibfk_1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PRIMARY");

            entity.ToTable("Favorite");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.FavoriteId).HasColumnName("favoriteId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Favorite_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Favorite_ibfk_1");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PRIMARY");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.OrderItemId, "orderItemId");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackId");
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.OrderItemId).HasColumnName("orderItemId");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("Feedback_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Order_ibfk_1");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PRIMARY");

            entity.ToTable("OrderItem");

            entity.HasIndex(e => e.OrderId, "orderId");

            entity.HasIndex(e => e.SizeId, "sizeId");

            entity.Property(e => e.OrderItemId).HasColumnName("orderItemId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeId).HasColumnName("sizeId");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10, 2)
                .HasColumnName("totalPrice");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unitPrice");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("OrderItem_ibfk_1");

            entity.HasOne(d => d.Size).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("OrderItem_ibfk_2");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .HasColumnName("method");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("transactionId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Payment_ibfk_1");
        });

        modelBuilder.Entity<PaymentSubscription>(entity =>
        {
            entity.HasKey(e => e.PaymentSubscriptionId).HasName("PRIMARY");

            entity.ToTable("PaymentSubscription");

            entity.Property(e => e.PaymentSubscriptionId).HasColumnName("paymentSubscriptionId");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.SubscriptionId).HasColumnName("subscriptionId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("imgUrl");
            entity.Property(e => e.NewPrice)
                .HasPrecision(10, 2)
                .HasColumnName("newPrice");
            entity.Property(e => e.NumberOfSold).HasColumnName("numberOfSold");
            entity.Property(e => e.OldPrice)
                .HasPrecision(10, 2)
                .HasColumnName("oldPrice");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Products)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Product_ibfk_1");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PRIMARY");

            entity.ToTable("ProductCategory");

            entity.HasIndex(e => e.CategoryId, "categoryId");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.ProductId).HasColumnName("productId");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("ProductCategory_ibfk_1");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("ProductCategory_ibfk_2");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PRIMARY");

            entity.ToTable("Size");

            entity.Property(e => e.SizeId).HasColumnName("sizeId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SizeProduct>(entity =>
        {
            entity.HasKey(e => e.SizeProductId).HasName("PRIMARY");

            entity.ToTable("SizeProduct");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.HasIndex(e => e.SizeId, "sizeId");

            entity.Property(e => e.SizeProductId).HasColumnName("sizeProductId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.SizeId).HasColumnName("sizeId");
            entity.Property(e => e.SizeQuantity).HasColumnName("sizeQuantity");

            entity.HasOne(d => d.Product).WithMany(p => p.SizeProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("SizeProduct_ibfk_2");

            entity.HasOne(d => d.Size).WithMany(p => p.SizeProducts)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("SizeProduct_ibfk_1");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PRIMARY");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionId).HasColumnName("subscriptionId");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("imgUrl");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasColumnName("status");
            entity.Property(e => e.SubscriptionId).HasColumnName("subscriptionId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
