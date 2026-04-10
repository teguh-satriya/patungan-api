using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patungan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNatureToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert enum columns to text first, then to varchar
            migrationBuilder.Sql(@"
                ALTER TABLE ""TransactionTypeTemplate"" 
                ALTER COLUMN ""Nature"" TYPE text USING ""Nature""::text;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""TransactionType"" 
                ALTER COLUMN ""Nature"" TYPE text USING ""Nature""::text;
            ");

            // Now alter to varchar with max length
            migrationBuilder.AlterColumn<string>(
                name: "Nature",
                table: "TransactionTypeTemplate",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nature",
                table: "TransactionType",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            // Drop the enum type
            migrationBuilder.Sql(@"DROP TYPE IF EXISTS public.transaction_nature CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:public.transaction_nature", "income,outcome");

            migrationBuilder.AlterColumn<string>(
                name: "Nature",
                table: "TransactionTypeTemplate",
                type: "public.transaction_nature",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Nature",
                table: "TransactionType",
                type: "public.transaction_nature",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);
        }
    }
}
