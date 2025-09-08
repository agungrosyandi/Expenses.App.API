using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.App.API.Migrations;

/// <inheritdoc />
public partial class UpdateUserId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "userId",
            table: "Transactions",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_userId",
            table: "Transactions",
            column: "userId");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Users_userId",
            table: "Transactions",
            column: "userId",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Users_userId",
            table: "Transactions");

        migrationBuilder.DropIndex(
            name: "IX_Transactions_userId",
            table: "Transactions");

        migrationBuilder.DropColumn(
            name: "userId",
            table: "Transactions");
    }
}
