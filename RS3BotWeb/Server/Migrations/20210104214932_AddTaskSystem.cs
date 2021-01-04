using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RS3BotWeb.Server.Migrations
{
    public partial class AddTaskSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentTaskId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrentTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnlockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notified = table.Column<bool>(type: "bit", nullable: false),
                    ChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CompletionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrentTaskXpGains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Skill = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CurrentTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentTaskXpGains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentTaskXpGains_CurrentTasks_CurrentTaskId",
                        column: x => x.CurrentTaskId,
                        principalTable: "CurrentTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentTasks_UserId",
                table: "CurrentTasks",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentTaskXpGains_CurrentTaskId",
                table: "CurrentTaskXpGains",
                column: "CurrentTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentTaskXpGains");

            migrationBuilder.DropTable(
                name: "CurrentTasks");

            migrationBuilder.DropColumn(
                name: "CurrentTaskId",
                table: "AspNetUsers");
        }
    }
}
