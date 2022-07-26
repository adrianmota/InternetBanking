using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetBanking.Infrastructure.Persistence.Migrations
{
    public partial class NoTransactionFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_AccountFromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_AccountToId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountFromId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountToId",
                table: "Transactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountFromId",
                table: "Transactions",
                column: "AccountFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountToId",
                table: "Transactions",
                column: "AccountToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_AccountFromId",
                table: "Transactions",
                column: "AccountFromId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_AccountToId",
                table: "Transactions",
                column: "AccountToId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
