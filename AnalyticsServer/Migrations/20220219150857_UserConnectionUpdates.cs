using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class UserConnectionUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnusedSessions",
                table: "UsersConnection",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnusedSessions",
                table: "UsersConnection");
        }
    }
}
