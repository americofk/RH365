using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des98 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarHolidays",
                columns: table => new
                {
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarHolidays", x => x.CalendarDate);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarHolidays");
        }
    }
}
