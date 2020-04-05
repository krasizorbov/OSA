using Microsoft.EntityFrameworkCore.Migrations;

namespace OSA.Data.Migrations
{
    public partial class AddedSaldoAndOwnFundsToCashBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OwnFunds",
                table: "CashBooks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "CashBooks",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnFunds",
                table: "CashBooks");

            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "CashBooks");
        }
    }
}
