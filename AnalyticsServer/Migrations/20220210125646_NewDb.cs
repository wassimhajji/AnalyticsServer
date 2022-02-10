using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class NewDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExistantList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vods",
                table: "Vods");

            migrationBuilder.DropColumn(
                name: "ExistantListId",
                table: "Vods");

            migrationBuilder.RenameTable(
                name: "Vods",
                newName: "Vod");

            migrationBuilder.AddColumn<string>(
                name: "ExistantList",
                table: "Vod",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vod",
                table: "Vod",
                column: "VodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vod",
                table: "Vod");

            migrationBuilder.DropColumn(
                name: "ExistantList",
                table: "Vod");

            migrationBuilder.RenameTable(
                name: "Vod",
                newName: "Vods");

            migrationBuilder.AddColumn<Guid>(
                name: "ExistantListId",
                table: "Vods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vods",
                table: "Vods",
                column: "VodId");

            migrationBuilder.CreateTable(
                name: "ExistantList",
                columns: table => new
                {
                    ExistantId = table.Column<int>(type: "int", nullable: false),
                    ExistantListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlaveId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExistantList", x => new { x.ExistantId, x.ExistantListId });
                });
        }
    }
}
