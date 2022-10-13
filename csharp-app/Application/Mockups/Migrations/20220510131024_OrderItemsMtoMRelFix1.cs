using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mockups.Migrations
{
    public partial class OrderItemsMtoMRelFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemOrder");

            migrationBuilder.CreateTable(
                name: "OrderMenuItems",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMenuItems", x => new { x.OrderId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_OrderMenuItems_MenuItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderMenuItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderMenuItems_ItemId",
                table: "OrderMenuItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderMenuItems");

            migrationBuilder.CreateTable(
                name: "MenuItemOrder",
                columns: table => new
                {
                    ItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrdersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemOrder", x => new { x.ItemsId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_MenuItemOrder_MenuItems_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemOrder_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemOrder_OrdersId",
                table: "MenuItemOrder",
                column: "OrdersId");
        }
    }
}
