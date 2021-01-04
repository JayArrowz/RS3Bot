using Microsoft.EntityFrameworkCore.Migrations;

namespace RS3BotWeb.Server.Migrations
{
    public partial class AddEquipmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EquipmentType",
                table: "EquipmentItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentType",
                table: "EquipmentItems");
        }
    }
}
