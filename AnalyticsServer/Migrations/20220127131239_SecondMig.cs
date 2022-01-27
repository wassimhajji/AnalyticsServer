using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class SecondMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiskAvailable",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "DiskFileSystem",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "DiskMontedOn",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "DiskSize",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "DiskUse",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "DiskUsed",
                table: "Hardware");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiskAvailable",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiskFileSystem",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiskMontedOn",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiskSize",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiskUse",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiskUsed",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
