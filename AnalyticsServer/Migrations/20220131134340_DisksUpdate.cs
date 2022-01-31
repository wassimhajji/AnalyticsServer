using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class DisksUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware");

            migrationBuilder.AlterColumn<string>(
                name: "SlaveId",
                table: "Hardware",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Hardware",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HardwareDisks",
                columns: table => new
                {
                    DiskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlaveId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Used = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Use = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontedOn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareDisks", x => x.DiskId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HardwareDisks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Hardware");

            migrationBuilder.AlterColumn<string>(
                name: "SlaveId",
                table: "Hardware",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware",
                column: "SlaveId");
        }
    }
}
