using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DCPServiceRepository.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallLog",
                columns: table => new
                {
                    CallId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    RequestStartTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    ElapsedTime = table.Column<long>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: false),
                    Expirytime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallLog", x => x.CallId);
                });

            migrationBuilder.CreateTable(
                name: "OrderCode",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Request = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    FinishTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCode", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallLog");

            migrationBuilder.DropTable(
                name: "OrderCode");
        }
    }
}
