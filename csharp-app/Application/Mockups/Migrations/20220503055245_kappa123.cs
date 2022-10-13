using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mockups.Migrations
{
    public partial class kappa123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("004f29dc-0a2d-4950-ac67-35161a1f1ba4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dea06712-e644-4e53-97d8-3a0810bce815"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "Type" },
                values: new object[] { new Guid("004f29dc-0a2d-4950-ac67-35161a1f1ba4"), "294a24a0-072a-4833-9dd7-20eb6e5dc5ed", "Пользователь", "ПОЛЬЗОВАТЕЛЬ", 1 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "Type" },
                values: new object[] { new Guid("dea06712-e644-4e53-97d8-3a0810bce815"), "8c8de9c9-b05e-4cea-8c31-7810ff00b645", "Администратор", "АДМИНИСТРАТОР", 0 });
        }
    }
}
