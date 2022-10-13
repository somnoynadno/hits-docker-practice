using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mockups.Migrations
{
    public partial class nullableUserDataEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EntranceNumber",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("004f29dc-0a2d-4950-ac67-35161a1f1ba4"),
                column: "ConcurrencyStamp",
                value: "294a24a0-072a-4833-9dd7-20eb6e5dc5ed");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dea06712-e644-4e53-97d8-3a0810bce815"),
                column: "ConcurrencyStamp",
                value: "8c8de9c9-b05e-4cea-8c31-7810ff00b645");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntranceNumber",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("004f29dc-0a2d-4950-ac67-35161a1f1ba4"),
                column: "ConcurrencyStamp",
                value: "14e7f47f-740d-47bd-88b2-df8a829b6cef");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dea06712-e644-4e53-97d8-3a0810bce815"),
                column: "ConcurrencyStamp",
                value: "fe23aff2-2920-412a-9a2f-4616bb4e070e");
        }
    }
}
