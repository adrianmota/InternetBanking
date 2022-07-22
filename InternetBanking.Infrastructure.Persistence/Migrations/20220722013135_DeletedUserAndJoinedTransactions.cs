using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetBanking.Infrastructure.Persistence.Migrations
{
    public partial class DeletedUserAndJoinedTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiaries_Users_ClientId",
                table: "Beneficiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_ClientId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_ClientId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Pays");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Beneficiaries");

            migrationBuilder.CreateTable(
               name: "Beneficiaries",
               columns: table => new
               {
                   ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   AccountId = table.Column<int>(type: "int", nullable: false),
                   Id = table.Column<int>(type: "int", nullable: false),
                   CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                   ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
               },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiaries", x => new { x.AccountId });
                });

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ClientId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Products_ClientId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Beneficiaries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries",
                column: "AccountId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Beneficiaries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountFromId = table.Column<int>(type: "int", nullable: false),
                    AccountToId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pays_Products_AccountFromId",
                        column: x => x.AccountFromId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pays_Products_AccountToId",
                        column: x => x.AccountToId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pays_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ClientId",
                table: "Transactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ClientId",
                table: "Products",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_AccountFromId",
                table: "Pays",
                column: "AccountFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_AccountToId",
                table: "Pays",
                column: "AccountToId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_ClientId",
                table: "Pays",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_Products_AccountId",
                table: "Beneficiaries",
                column: "AccountId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiaries_Users_ClientId",
                table: "Beneficiaries",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_ClientId",
                table: "Products",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_ClientId",
                table: "Transactions",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
