using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Lykke.Job.TradelogBridge.Sql.Migrations
{
    public partial class AddTrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TradeId = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    WalletId = table.Column<string>(type: "varchar(255)", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(255)", nullable: false),
                    OrderType = table.Column<string>(type: "varchar(255)", nullable: false),
                    Direction = table.Column<string>(type: "varchar(255)", nullable: false),
                    Asset = table.Column<string>(type: "varchar(255)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    OppositeOrderId = table.Column<string>(type: "varchar(255)", nullable: false),
                    OppositeAsset = table.Column<string>(type: "varchar(255)", nullable: true),
                    OppositeVolume = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");
        }
    }
}
