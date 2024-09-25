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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<SizeProduct> SizeProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=database.hdang09.me;database=product_schema;uid=root;pwd=my-secret-pw", ServerVersion.Parse("9.0.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("category");

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

            entity.ToTable("favorite");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.FavoriteId).HasColumnName("favoriteId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("userId");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("favorite_ibfk_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("imgUrl");
            entity.Property(e => e.NewPrice)
                .HasPrecision(10, 2)
                .HasColumnName("newPrice");
            entity.Property(e => e.NumberOfSold)
                .HasDefaultValueSql("'0'")
                .HasColumnName("numberOfSold");
            entity.Property(e => e.OldPrice)
                .HasPrecision(10, 2)
                .HasColumnName("oldPrice");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
        });

        modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PRIMARY");

            entity.ToTable("productcategory");

            entity.HasIndex(e => e.CategoryId, "categoryId");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.ProductId).HasColumnName("productId");

            entity.HasOne(d => d.Category).WithMany(p => p.Productcategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("productcategory_ibfk_2");

            entity.HasOne(d => d.Product).WithMany(p => p.Productcategories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("productcategory_ibfk_1");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PRIMARY");

            entity.ToTable("size");

            entity.Property(e => e.SizeId).HasColumnName("sizeId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SizeProduct>(entity =>
        {
            entity.HasKey(e => e.SizeProductId).HasName("PRIMARY");

            entity.ToTable("sizeProduct");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.HasIndex(e => e.SizeId, "sizeId");

            entity.Property(e => e.SizeProductId).HasColumnName("sizeProductId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.SizeId).HasColumnName("sizeId");

            entity.HasOne(d => d.Product).WithMany(p => p.SizeProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sizeProduct_ibfk_1");

            entity.HasOne(d => d.Size).WithMany(p => p.SizeProducts)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sizeProduct_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
