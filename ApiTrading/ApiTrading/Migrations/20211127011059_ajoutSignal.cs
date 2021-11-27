using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ApiTrading.Migrations
{
    public partial class ajoutSignal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignalInfoStrategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Strategy = table.Column<string>(type: "text", nullable: true),
                    Timeframe = table.Column<string>(type: "text", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    Signal = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EntryLevel = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    StopLoss = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TakeProfit = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalInfoStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignalInfoStrategies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignalInfoStrategies_UserId",
                table: "SignalInfoStrategies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignalInfoStrategies");
        }
    }
}
