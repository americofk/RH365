using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des58 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PayrollsProcess",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PayrollsProcess",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "PayrollsProcess",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "PayrollsProcess",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "PayrollsProcess",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PayrollProcessDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PayrollProcessDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "PayrollProcessDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "PayrollProcessDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "PayrollProcessDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PayrollProcessActions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PayrollProcessActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "PayrollProcessActions",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "PayrollProcessActions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "PayrollProcessActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeesAddress",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeesAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeesAddress",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeesAddress",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeesAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EarningCodeVersions",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EarningCodes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DeductionCodeVersions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DeductionCodeVersions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "DeductionCodeVersions",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "DeductionCodeVersions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "DeductionCodeVersions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EarningCodeVersions");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EarningCodes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "DeductionCodeVersions");
        }
    }
}
