using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Patungan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "TransactionTypeTemplate",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "TransactionType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Category");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "TransactionTypeTemplate");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "TransactionType");
        }
    }
}
