using Microsoft.EntityFrameworkCore.Migrations;

namespace RS3BotWeb.Server.Migrations
{
    public partial class AddReplay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Command",
                table: "CurrentTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MessageId",
                table: "CurrentTasks",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Command",
                table: "CurrentTasks");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "CurrentTasks");
        }
    }
}
