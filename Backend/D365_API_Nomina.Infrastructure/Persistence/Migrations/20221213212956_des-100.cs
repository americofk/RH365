using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFixedWorkCalendar",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHoliday",
                table: "EarningCodeVersions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkFrom",
                table: "EarningCodeVersions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkTo",
                table: "EarningCodeVersions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsHoliday",
                table: "EarningCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkFrom",
                table: "EarningCodes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkTo",
                table: "EarningCodes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixedWorkCalendar",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsHoliday",
                table: "EarningCodeVersions");

            migrationBuilder.DropColumn(
                name: "WorkFrom",
                table: "EarningCodeVersions");

            migrationBuilder.DropColumn(
                name: "WorkTo",
                table: "EarningCodeVersions");

            migrationBuilder.DropColumn(
                name: "IsHoliday",
                table: "EarningCodes");

            migrationBuilder.DropColumn(
                name: "WorkFrom",
                table: "EarningCodes");

            migrationBuilder.DropColumn(
                name: "WorkTo",
                table: "EarningCodes");
        }
    }
}
