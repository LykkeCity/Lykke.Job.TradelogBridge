using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Job.TradelogBridge.Sql.Migrations
{
    public partial class AddTradesIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Trades_DateTime",
                table: "Trades",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_OrderId",
                table: "Trades",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_TradeId",
                table: "Trades",
                column: "TradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trades_DateTime",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_OrderId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_TradeId",
                table: "Trades");
        }
    }
}
