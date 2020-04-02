using Microsoft.EntityFrameworkCore.Migrations;

namespace OSA.Data.Migrations
{
    public partial class ChangeAveragePriceToStringSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AveragePrice",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePrice",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
