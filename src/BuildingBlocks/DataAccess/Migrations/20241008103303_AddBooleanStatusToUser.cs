using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBooleanStatusToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    categoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.categoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "PaymentSubscription",
                columns: table => new
                {
                    paymentSubscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    paymentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    subscriptionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    startDate = table.Column<DateOnly>(type: "date", nullable: true),
                    endDate = table.Column<DateOnly>(type: "date", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.paymentSubscriptionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    sizeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.sizeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    subscriptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    period = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.subscriptionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    fullName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: true),
                    subscriptionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    imgUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    orderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    startDate = table.Column<DateOnly>(type: "date", nullable: true),
                    endDate = table.Column<DateOnly>(type: "date", nullable: true),
                    paymentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    address = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.orderId);
                    table.ForeignKey(
                        name: "Order_ibfk_1",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    paymentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    method = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    transactionId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.paymentId);
                    table.ForeignKey(
                        name: "Payment_ibfk_1",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    productId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    oldPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    newPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    numberOfSold = table.Column<int>(type: "int", nullable: true),
                    imgUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.productId);
                    table.ForeignKey(
                        name: "Product_ibfk_1",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    orderItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    orderId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    unitPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    totalPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    sizeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.orderItemId);
                    table.ForeignKey(
                        name: "OrderItem_ibfk_1",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "orderId");
                    table.ForeignKey(
                        name: "OrderItem_ibfk_2",
                        column: x => x.sizeId,
                        principalTable: "Size",
                        principalColumn: "sizeId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    cartId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.cartId);
                    table.ForeignKey(
                        name: "Cart_ibfk_1",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "Cart_ibfk_2",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "productId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    favoriteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.favoriteId);
                    table.ForeignKey(
                        name: "Favorite_ibfk_1",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "Favorite_ibfk_2",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "productId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    productCategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    categoryId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.productCategoryId);
                    table.ForeignKey(
                        name: "ProductCategory_ibfk_1",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "categoryId");
                    table.ForeignKey(
                        name: "ProductCategory_ibfk_2",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "productId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "SizeProduct",
                columns: table => new
                {
                    sizeProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    productId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    sizeQuantity = table.Column<int>(type: "int", nullable: true),
                    sizeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.sizeProductId);
                    table.ForeignKey(
                        name: "SizeProduct_ibfk_1",
                        column: x => x.sizeId,
                        principalTable: "Size",
                        principalColumn: "sizeId");
                    table.ForeignKey(
                        name: "SizeProduct_ibfk_2",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "productId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    feedbackId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    orderItemId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    comment = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rating = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.feedbackId);
                    table.ForeignKey(
                        name: "Feedback_ibfk_1",
                        column: x => x.orderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "orderItemId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "productId",
                table: "Cart",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "userId",
                table: "Cart",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "productId1",
                table: "Favorite",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "userId1",
                table: "Favorite",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "orderItemId",
                table: "Feedback",
                column: "orderItemId");

            migrationBuilder.CreateIndex(
                name: "userId2",
                table: "Order",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "orderId",
                table: "OrderItem",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "sizeId",
                table: "OrderItem",
                column: "sizeId");

            migrationBuilder.CreateIndex(
                name: "userId3",
                table: "Payment",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "userId4",
                table: "Product",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "categoryId",
                table: "ProductCategory",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "productId2",
                table: "ProductCategory",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "productId3",
                table: "SizeProduct",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "sizeId1",
                table: "SizeProduct",
                column: "sizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "PaymentSubscription");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "SizeProduct");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
