using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class addActivePropertyConfirmtaionToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "ConfirmationToken");

            migrationBuilder.RenameColumn(
                name: "expiredAt",
                table: "ConfirmationToken",
                newName: "ExpiredAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "ConfirmationToken",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ResendEmailConfirmationToken",
                table: "ConfirmationToken",
                newName: "ResendToken");

            migrationBuilder.RenameColumn(
                name: "ConfirmationTokenId",
                table: "ConfirmationToken",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredAt",
                table: "ConfirmationToken",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ConfirmationToken",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "ConfirmationToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isUsed",
                table: "ConfirmationToken",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "ConfirmationToken");

            migrationBuilder.DropColumn(
                name: "isUsed",
                table: "ConfirmationToken");

            migrationBuilder.RenameColumn(
                name: "ExpiredAt",
                table: "ConfirmationToken",
                newName: "expiredAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ConfirmationToken",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "ResendToken",
                table: "ConfirmationToken",
                newName: "ResendEmailConfirmationToken");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ConfirmationToken",
                newName: "ConfirmationTokenId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expiredAt",
                table: "ConfirmationToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "ConfirmationToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "ConfirmationToken",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
