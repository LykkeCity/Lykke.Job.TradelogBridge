using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Lykke.Job.TradelogBridge.Sql.Migrations
{
    public partial class AddNewFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeTradeLogItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TradeLogItemId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "varchar(255)", nullable: false),
                    FromClientId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ToClientId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Asset = table.Column<string>(type: "varchar(255)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    SizeType = table.Column<string>(type: "varchar(255)", nullable: false),
                    Size = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTradeLogItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeTradeLogItem_Trades_TradeLogItemId",
                        column: x => x.TradeLogItemId,
                        principalTable: "Trades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeTradeLogItem_TradeLogItemId",
                table: "FeeTradeLogItem",
                column: "TradeLogItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeTradeLogItem");
        }
    }
}
