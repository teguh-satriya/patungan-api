using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patungan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionTypeTemplateIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: "account_balance");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: "account_balance");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: "account_balance");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icon",
                value: "account_balance");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icon",
                value: "account_balance");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 6,
                column: "Icon",
                value: "account_balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: "AttachMoney");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: "WorkOutline");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: "TrendingUp");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icon",
                value: "ShoppingCart");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icon",
                value: "ElectricalServices");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 6,
                column: "Icon",
                value: "MovieFilter");
        }
    }
}
