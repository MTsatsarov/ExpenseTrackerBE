using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Data.Migrations
{
    public partial class StoresToExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Expenses_ExpenseId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ExpenseId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Stores");

            migrationBuilder.CreateTable(
                name: "ExpenseStore",
                columns: table => new
                {
                    ExpensesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseStore", x => new { x.ExpensesId, x.StoresId });
                    table.ForeignKey(
                        name: "FK_ExpenseStore_Expenses_ExpensesId",
                        column: x => x.ExpensesId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseStore_Stores_StoresId",
                        column: x => x.StoresId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStore_StoresId",
                table: "ExpenseStore",
                column: "StoresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseStore");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId",
                table: "Stores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ExpenseId",
                table: "Stores",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Expenses_ExpenseId",
                table: "Stores",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id");
        }
    }
}
