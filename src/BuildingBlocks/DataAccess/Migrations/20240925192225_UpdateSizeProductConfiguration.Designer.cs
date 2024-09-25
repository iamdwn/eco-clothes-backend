﻿// <auto-generated />
using System;
using DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(EcoClothesContext))]
    [Migration("20240925192225_UpdateSizeProductConfiguration")]
    partial class UpdateSizeProductConfiguration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Models.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("categoryId");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("CategoryId")
                        .HasName("PRIMARY");

                    b.ToTable("category", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Favorite", b =>
                {
                    b.Property<Guid>("FavoriteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("favoriteId");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("userId");

                    b.HasKey("FavoriteId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductId" }, "productId");

                    b.HasIndex(new[] { "UserId" }, "userId");

                    b.ToTable("favorite", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("amount");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImgUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("imgUrl");

                    b.Property<decimal?>("NewPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("newPrice");

                    b.Property<int?>("NumberOfSold")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("numberOfSold")
                        .HasDefaultValueSql("'0'");

                    b.Property<decimal?>("OldPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("oldPrice");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("productName");

                    b.HasKey("ProductId")
                        .HasName("PRIMARY");

                    b.ToTable("product", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Productcategory", b =>
                {
                    b.Property<Guid>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("productCategoryId");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("char(36)")
                        .HasColumnName("categoryId");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.HasKey("ProductCategoryId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CategoryId" }, "categoryId");

                    b.HasIndex(new[] { "ProductId" }, "productId")
                        .HasDatabaseName("productId1");

                    b.ToTable("productcategory", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Size", b =>
                {
                    b.Property<Guid>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("sizeId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("SizeId")
                        .HasName("PRIMARY");

                    b.ToTable("size", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.SizeProduct", b =>
                {
                    b.Property<Guid>("SizeProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("sizeProductId");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.Property<Guid?>("SizeId")
                        .HasColumnType("char(36)")
                        .HasColumnName("sizeId");

                    b.Property<int>("SizeQuantity")
                        .HasColumnType("int")
                        .HasColumnName("sizeQuantity");

                    b.HasKey("SizeProductId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductId" }, "productId")
                        .HasDatabaseName("productId2");

                    b.HasIndex(new[] { "SizeId" }, "sizeId");

                    b.ToTable("sizeProduct", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Favorite", b =>
                {
                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("Favorites")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("favorite_ibfk_2");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DataAccess.Models.Productcategory", b =>
                {
                    b.HasOne("DataAccess.Models.Category", "Category")
                        .WithMany("Productcategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("productcategory_ibfk_2");

                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("Productcategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("productcategory_ibfk_1");

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DataAccess.Models.SizeProduct", b =>
                {
                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("SizeProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("sizeProduct_ibfk_1");

                    b.HasOne("DataAccess.Models.Size", "Size")
                        .WithMany("SizeProducts")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("sizeProduct_ibfk_2");

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("DataAccess.Models.Category", b =>
                {
                    b.Navigation("Productcategories");
                });

            modelBuilder.Entity("DataAccess.Models.Product", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Productcategories");

                    b.Navigation("SizeProducts");
                });

            modelBuilder.Entity("DataAccess.Models.Size", b =>
                {
                    b.Navigation("SizeProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
