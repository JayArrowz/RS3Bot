using Microsoft.EntityFrameworkCore.Migrations;

namespace RS3BotWeb.Server.Migrations
{
    public partial class AddEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "UserItems",
                newName: "Item_ItemId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "UserItems",
                newName: "Item_Amount");

            migrationBuilder.AlterColumn<int>(
                name: "Item_ItemId",
                table: "UserItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Item_Amount",
                table: "UserItems",
                type: "decimal(20,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.CreateTable(
                name: "EquipmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentSlot = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Item_ItemId = table.Column<int>(type: "int", nullable: true),
                    Item_Amount = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_UserId",
                table: "EquipmentItems",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentItems");

            migrationBuilder.RenameColumn(
                name: "Item_ItemId",
                table: "UserItems",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "Item_Amount",
                table: "UserItems",
                newName: "Amount");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "UserItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "UserItems",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldNullable: true);
        }
    }
}
