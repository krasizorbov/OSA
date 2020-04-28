using Microsoft.EntityFrameworkCore.Migrations;

namespace OSA.Data.Migrations
{
    public partial class ChangeBookValueToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BookValue",
                table: "AvailableStocks",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BookValue",
                table: "AvailableStocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
