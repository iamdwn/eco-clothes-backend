using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddISoftDeleteToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "User",
                newName: "dateCreated");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "User",
                type: "tinyint(1)",
                nullable: true,
                defaultValueSql: "'1'",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateCreated",
                table: "User",
                type: "datetime",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedWhen",
                table: "User",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "User",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateCreated",
                table: "Product",
                type: "datetime",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "Payment",
                type: "datetime",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Order",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValueSql: "'Pending'",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedWhen",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "User");

            migrationBuilder.DropColumn(
                name: "dateCreated",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "date",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "dateCreated",
                table: "User",
                newName: "DateCreated");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "User",
                type: "tinyint(1)",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true,
                oldDefaultValueSql: "'1'");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateCreated",
                table: "User",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
