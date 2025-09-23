using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des97 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BatchHistories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "BatchHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "BatchHistories",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "BatchHistories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "BatchHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EmployeeWorkCalendars",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CalendarDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalendarDay = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakWorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakWorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InCompany = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkCalendars", x => new { x.EmployeeId, x.CalendarDate });
                    table.ForeignKey(
                        name: "FK_EmployeeWorkCalendars_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkCalendars");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BatchHistories");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "BatchHistories");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "BatchHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "BatchHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "BatchHistories");
        }
    }
}
