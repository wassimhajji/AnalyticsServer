using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hardware",
                columns: table => new
                {
                    SlaveId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CpuUser = table.Column<double>(type: "float", nullable: false),
                    CpuNice = table.Column<double>(type: "float", nullable: false),
                    CpuSys = table.Column<double>(type: "float", nullable: false),
                    CpuIoWait = table.Column<double>(type: "float", nullable: false),
                    CpuIRQ = table.Column<double>(type: "float", nullable: false),
                    CpuSoft = table.Column<double>(type: "float", nullable: false),
                    CpuSteal = table.Column<double>(type: "float", nullable: false),
                    CpuGuest = table.Column<double>(type: "float", nullable: false),
                    CpuGnice = table.Column<double>(type: "float", nullable: false),
                    CpuIdle = table.Column<double>(type: "float", nullable: false),
                    RamTotal = table.Column<int>(type: "int", nullable: false),
                    RamUsed = table.Column<int>(type: "int", nullable: false),
                    RamCache = table.Column<int>(type: "int", nullable: false),
                    RamSwap = table.Column<int>(type: "int", nullable: false),
                    RamBoot = table.Column<int>(type: "int", nullable: false),
                    IONetIn = table.Column<int>(type: "int", nullable: false),
                    IONetOut = table.Column<int>(type: "int", nullable: false),
                    IOTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IODiskRead = table.Column<int>(type: "int", nullable: false),
                    IODiskWrite = table.Column<int>(type: "int", nullable: false),
                    DiskFileSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiskSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiskUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiskAvailable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiskUse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiskMontedOn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardware", x => x.SlaveId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hardware");
        }
    }
}
