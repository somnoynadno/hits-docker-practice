using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mockups.Migrations
{
    public partial class AddedAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMainAddress = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "Type" },
                values: new object[] { new Guid("004f29dc-0a2d-4950-ac67-35161a1f1ba4"), "14e7f47f-740d-47bd-88b2-df8a829b6cef", "Пользователь", "ПОЛЬЗОВАТЕЛЬ", 1 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "Type" },
                values: new object[] { new Guid("dea06712-e644-4e53-97d8-3a0810bce815"), "fe23aff2-2920-412a-9a2f-4616bb4e070e", "Администратор", "АДМИНИСТРАТОР", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_userId",
                table: "Addresses",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("004f29dc-0a2d-4950-ac67-35161a1f1ba4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dea06712-e644-4e53-97d8-3a0810bce815"));
        }
    }
}
