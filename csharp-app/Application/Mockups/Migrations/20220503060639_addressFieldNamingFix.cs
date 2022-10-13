using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mockups.Migrations
{
    public partial class addressFieldNamingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_userId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Addresses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_userId",
                table: "Addresses",
                newName: "IX_Addresses_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Addresses",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                newName: "IX_Addresses_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_userId",
                table: "Addresses",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
