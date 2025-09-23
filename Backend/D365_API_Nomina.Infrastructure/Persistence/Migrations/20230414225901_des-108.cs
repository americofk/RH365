using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeWorkControlCalendars",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalendarDay = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakWorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakWorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalHour = table.Column<decimal>(type: "decimal(32,16)", nullable: false),
                    StatusExtraHour = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InCompany = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkControlCalendars", x => new { x.InternalId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeWorkControlCalendars_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkControlCalendars_EmployeeId",
                table: "EmployeeWorkControlCalendars",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkControlCalendars");
        }
    }
}
