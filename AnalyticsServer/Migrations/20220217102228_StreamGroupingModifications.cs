using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class StreamGroupingModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamsGrouping",
                table: "StreamsGrouping");

            migrationBuilder.AlterColumn<string>(
                name: "StreamId",
                table: "StreamsGrouping",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StreamsGrouping",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeAdded",
                table: "StreamsGrouping",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamsGrouping",
                table: "StreamsGrouping",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamsGrouping",
                table: "StreamsGrouping");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StreamsGrouping");

            migrationBuilder.DropColumn(
                name: "TimeAdded",
                table: "StreamsGrouping");

            migrationBuilder.AlterColumn<string>(
                name: "StreamId",
                table: "StreamsGrouping",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamsGrouping",
                table: "StreamsGrouping",
                column: "StreamId");
        }
    }
}
