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
    [Migration("20241010084156_AddISoftDeleteToUser")]
    partial class AddISoftDeleteToUser
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

            modelBuilder.Entity("DataAccess.Models.Cart", b =>
                {
                    b.Property<Guid>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("cartId");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("userId");

                    b.HasKey("CartId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductId" }, "productId");

                    b.HasIndex(new[] { "UserId" }, "userId");

                    b.ToTable("Cart", (string)null);
                });

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
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("CategoryId")
                        .HasName("PRIMARY");

                    b.ToTable("Category", (string)null);
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

                    b.HasIndex(new[] { "ProductId" }, "productId")
                        .HasDatabaseName("productId1");

                    b.HasIndex(new[] { "UserId" }, "userId")
                        .HasDatabaseName("userId1");

                    b.ToTable("Favorite", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Feedback", b =>
                {
                    b.Property<Guid>("FeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("feedbackId");

                    b.Property<string>("Comment")
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<DateOnly?>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<Guid?>("OrderItemId")
                        .HasColumnType("char(36)")
                        .HasColumnName("orderItemId");

                    b.Property<int?>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.HasKey("FeedbackId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "OrderItemId" }, "orderItemId");

                    b.ToTable("Feedback", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("orderId");

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("endDate");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("char(36)")
                        .HasColumnName("paymentId");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("startDate");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'Pending'");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("userId");

                    b.HasKey("OrderId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserId" }, "userId")
                        .HasDatabaseName("userId2");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.OrderItem", b =>
                {
                    b.Property<Guid>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("orderItemId");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("char(36)")
                        .HasColumnName("orderId");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<Guid?>("SizeId")
                        .HasColumnType("char(36)")
                        .HasColumnName("sizeId");

                    b.Property<decimal?>("TotalPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("totalPrice");

                    b.Property<decimal?>("UnitPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("unitPrice");

                    b.HasKey("OrderItemId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "OrderId" }, "orderId");

                    b.HasIndex(new[] { "SizeId" }, "sizeId");

                    b.ToTable("OrderItem", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("paymentId");

                    b.Property<decimal?>("Amount")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("amount");

                    b.Property<DateTime?>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Method")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("method");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("status");

                    b.Property<string>("TransactionId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("transactionId");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("userId");

                    b.HasKey("PaymentId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserId" }, "userId")
                        .HasDatabaseName("userId3");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.PaymentSubscription", b =>
                {
                    b.Property<Guid>("PaymentSubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("paymentSubscriptionId");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("endDate");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("char(36)")
                        .HasColumnName("paymentId");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("price");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("startDate");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("char(36)")
                        .HasColumnName("subscriptionId");

                    b.HasKey("PaymentSubscriptionId")
                        .HasName("PRIMARY");

                    b.ToTable("PaymentSubscription", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("productId");

                    b.Property<int?>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("amount");

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("dateCreated")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

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
                        .HasColumnType("int")
                        .HasColumnName("numberOfSold");

                    b.Property<decimal?>("OldPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("oldPrice");

                    b.Property<string>("ProductName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("productName");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)")
                        .HasColumnName("userId");

                    b.HasKey("ProductId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserId" }, "userId")
                        .HasDatabaseName("userId4");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.ProductCategory", b =>
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
                        .HasDatabaseName("productId2");

                    b.ToTable("ProductCategory", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Size", b =>
                {
                    b.Property<Guid>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("sizeId");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("SizeId")
                        .HasName("PRIMARY");

                    b.ToTable("Size", (string)null);
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

                    b.Property<int?>("SizeQuantity")
                        .HasColumnType("int")
                        .HasColumnName("sizeQuantity");

                    b.HasKey("SizeProductId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductId" }, "productId")
                        .HasDatabaseName("productId3");

                    b.HasIndex(new[] { "SizeId" }, "sizeId")
                        .HasDatabaseName("sizeId1");

                    b.ToTable("SizeProduct", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Subscription", b =>
                {
                    b.Property<Guid>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("subscriptionId");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<DateOnly?>("Period")
                        .HasColumnType("date")
                        .HasColumnName("period");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("price");

                    b.HasKey("SubscriptionId")
                        .HasName("PRIMARY");

                    b.ToTable("Subscription", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("userId");

                    b.Property<DateTime?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("dateCreated")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset?>("DeletedWhen")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("fullName");

                    b.Property<string>("ImgUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("imgUrl");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("role");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'1'");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("char(36)")
                        .HasColumnName("subscriptionId");

                    b.HasKey("UserId")
                        .HasName("PRIMARY");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("DataAccess.Models.Cart", b =>
                {
                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("Carts")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("Cart_ibfk_2");

                    b.HasOne("DataAccess.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Cart_ibfk_1");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.Favorite", b =>
                {
                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("Favorites")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("Favorite_ibfk_2");

                    b.HasOne("DataAccess.Models.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Favorite_ibfk_1");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.Feedback", b =>
                {
                    b.HasOne("DataAccess.Models.OrderItem", "OrderItem")
                        .WithMany("Feedbacks")
                        .HasForeignKey("OrderItemId")
                        .HasConstraintName("Feedback_ibfk_1");

                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("DataAccess.Models.Order", b =>
                {
                    b.HasOne("DataAccess.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Order_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.OrderItem", b =>
                {
                    b.HasOne("DataAccess.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("OrderItem_ibfk_1");

                    b.HasOne("DataAccess.Models.Size", "Size")
                        .WithMany("OrderItems")
                        .HasForeignKey("SizeId")
                        .HasConstraintName("OrderItem_ibfk_2");

                    b.Navigation("Order");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("DataAccess.Models.Payment", b =>
                {
                    b.HasOne("DataAccess.Models.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Payment_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.Product", b =>
                {
                    b.HasOne("DataAccess.Models.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Product_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.ProductCategory", b =>
                {
                    b.HasOne("DataAccess.Models.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("ProductCategory_ibfk_1");

                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("ProductCategory_ibfk_2");

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DataAccess.Models.SizeProduct", b =>
                {
                    b.HasOne("DataAccess.Models.Product", "Product")
                        .WithMany("SizeProducts")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("SizeProduct_ibfk_2");

                    b.HasOne("DataAccess.Models.Size", "Size")
                        .WithMany("SizeProducts")
                        .HasForeignKey("SizeId")
                        .HasConstraintName("SizeProduct_ibfk_1");

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("DataAccess.Models.Category", b =>
                {
                    b.Navigation("ProductCategories");
                });

            modelBuilder.Entity("DataAccess.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("DataAccess.Models.OrderItem", b =>
                {
                    b.Navigation("Feedbacks");
                });

            modelBuilder.Entity("DataAccess.Models.Product", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Favorites");

                    b.Navigation("ProductCategories");

                    b.Navigation("SizeProducts");
                });

            modelBuilder.Entity("DataAccess.Models.Size", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("SizeProducts");
                });

            modelBuilder.Entity("DataAccess.Models.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Favorites");

                    b.Navigation("Orders");

                    b.Navigation("Payments");

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
