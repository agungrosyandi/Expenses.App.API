using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.App.API.Migrations;

/// <inheritdoc />
public partial class FixUserId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Users_userId",
            table: "Transactions");

        migrationBuilder.RenameColumn(
            name: "userId",
            table: "Transactions",
            newName: "UserId");

        migrationBuilder.RenameIndex(
            name: "IX_Transactions_userId",
            table: "Transactions",
            newName: "IX_Transactions_UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Users_UserId",
            table: "Transactions",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Users_UserId",
            table: "Transactions");

        migrationBuilder.RenameColumn(
            name: "UserId",
            table: "Transactions",
            newName: "userId");

        migrationBuilder.RenameIndex(
            name: "IX_Transactions_UserId",
            table: "Transactions",
            newName: "IX_Transactions_userId");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Users_userId",
            table: "Transactions",
            column: "userId",
            principalTable: "Users",
            principalColumn: "Id");
    }
}
