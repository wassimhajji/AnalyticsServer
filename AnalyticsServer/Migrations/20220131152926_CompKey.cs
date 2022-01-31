using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class CompKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HardwareDisks",
                table: "HardwareDisks");

            migrationBuilder.DropColumn(
                name: "DiskId",
                table: "HardwareDisks");

            migrationBuilder.AlterColumn<string>(
                name: "SlaveId",
                table: "HardwareDisks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileSystem",
                table: "HardwareDisks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HardwareDisks",
                table: "HardwareDisks",
                columns: new[] { "SlaveId", "FileSystem" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HardwareDisks",
                table: "HardwareDisks");

            migrationBuilder.AlterColumn<string>(
                name: "FileSystem",
                table: "HardwareDisks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SlaveId",
                table: "HardwareDisks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "DiskId",
                table: "HardwareDisks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_HardwareDisks",
                table: "HardwareDisks",
                column: "DiskId");
        }
    }
}
