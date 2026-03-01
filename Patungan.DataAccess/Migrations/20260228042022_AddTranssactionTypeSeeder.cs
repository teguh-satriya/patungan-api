using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Patungan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTranssactionTypeSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionTypeTemplate",
                columns: new[] { "Id", "Description", "Name", "Nature" },
                values: new object[,]
                {
                    { 1, "Primary monthly income", "Salary", "income" },
                    { 2, "Secondary income streams", "Side Hustle", "income" },
                    { 3, "", "Investasi", "income" },
                    { 4, "", "Belanja Kebutuhan", "outcome" },
                    { 5, "", "Utilitas", "outcome" },
                    { 6, "", "Hiburan", "outcome" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TransactionTypeTemplate",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
