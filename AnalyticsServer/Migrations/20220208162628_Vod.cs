using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class Vod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExistantList",
                columns: table => new
                {
                    ExistantListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExistantId = table.Column<int>(type: "int", nullable: false),
                    SlaveId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExistantList", x => new { x.ExistantId, x.ExistantListId });
                });

            migrationBuilder.CreateTable(
                name: "Vods",
                columns: table => new
                {
                    VodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlaveId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExistantListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vods", x => x.VodId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExistantList");

            migrationBuilder.DropTable(
                name: "Vods");
        }
    }
}
