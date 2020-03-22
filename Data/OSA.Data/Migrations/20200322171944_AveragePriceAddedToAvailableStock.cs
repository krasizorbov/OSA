using Microsoft.EntityFrameworkCore.Migrations;

namespace OSA.Data.Migrations
{
    public partial class AveragePriceAddedToAvailableStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSoldAmount",
                table: "AvailableStocks");

            migrationBuilder.AddColumn<decimal>(
                name: "AveragePrice",
                table: "AvailableStocks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSoldPrice",
                table: "AvailableStocks",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "AvailableStocks");

            migrationBuilder.DropColumn(
                name: "TotalSoldPrice",
                table: "AvailableStocks");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSoldAmount",
                table: "AvailableStocks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
