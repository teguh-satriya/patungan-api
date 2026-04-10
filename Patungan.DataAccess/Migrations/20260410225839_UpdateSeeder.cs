using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patungan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "TransactionTypeTemplate",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "attach_money",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "TransactionType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "attach_money",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Category");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: "attach_money");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: "work_outline");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: "trending_up");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icon",
                value: "shopping_cart");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icon",
                value: "electrical_services");

            migrationBuilder.UpdateData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 6,
                column: "Icon",
                value: "movie_filter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "TransactionTypeTemplate",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Category",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "attach_money");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "TransactionType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Category",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "attach_money");

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
    }
}
