using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.App.API.Migrations;

/// <inheritdoc />
public partial class AddGuidId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Users_UserId",
            table: "Transactions");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "PublicId",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValueSql: "NEWID()");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "Transactions",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "PublicId",
            table: "Transactions",
            type: "uniqueidentifier",
            nullable: false,
            defaultValueSql: "NEWID()");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Users_UserId",
            table: "Transactions",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Users_UserId",
            table: "Transactions");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PublicId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PublicId",
            table: "Transactions");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "Transactions",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Users_UserId",
            table: "Transactions",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id");
    }
}
