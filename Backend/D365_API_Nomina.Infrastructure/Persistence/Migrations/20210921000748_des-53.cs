using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des53 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserImages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "UserImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "UserImages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "UserImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Taxes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TaxDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "TaxDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "TaxDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "TaxDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "TaxDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Positions",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "PositionRequirements",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Payrolls",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PayCycles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PayCycles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "PayCycles",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "PayCycles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "PayCycles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Loans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Loans",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Loans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Jobs",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Instructors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Instructors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Instructors",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Instructors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Instructors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeTaxes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeTaxes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeTaxes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeTaxes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeTaxes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Employees",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Employees",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Employees",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeePositions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeePositions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeePositions",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeePositions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeePositions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeLoans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeLoans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeLoans",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeLoans",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeLoans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeImages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeImages",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeImages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeExtraHours",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeExtraHours",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeExtraHours",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeExtraHours",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeExtraHours",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeEarningCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeEarningCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeEarningCodes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeEarningCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeEarningCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeDocuments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeDocuments",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeDocuments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeDepartments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeDepartments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeDepartments",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeDepartments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeDepartments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeDeductionCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeDeductionCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeDeductionCodes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeDeductionCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeDeductionCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeContactsInf",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeContactsInf",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeContactsInf",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeContactsInf",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeContactsInf",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeBankAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EmployeeBankAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "EmployeeBankAccounts",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "EmployeeBankAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "EmployeeBankAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Departments",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "DeductionCodes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CourseTypes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CourseTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "CourseTypes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "CourseTypes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "CourseTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "Courses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoursePositions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CoursePositions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "CoursePositions",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "CoursePositions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "CoursePositions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CourseLocations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CourseLocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "CourseLocations",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "CourseLocations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "CourseLocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CourseInstructors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CourseInstructors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "CourseInstructors",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "CourseInstructors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "CourseInstructors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CourseEmployees",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CourseEmployees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "CourseEmployees",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "CourseEmployees",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "CourseEmployees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ClassRooms",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ClassRooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InCompany",
                table: "ClassRooms",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "ClassRooms",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDateTime",
                table: "ClassRooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "PositionRequirements");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeTaxes");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeTaxes");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeTaxes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeTaxes");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeTaxes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeePositions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeePositions");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeePositions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeePositions");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeePositions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeImages");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeImages");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeImages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeImages");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeDepartments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeContactsInf");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeContactsInf");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeContactsInf");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeContactsInf");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeContactsInf");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeBankAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EmployeeBankAccounts");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "EmployeeBankAccounts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "EmployeeBankAccounts");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "EmployeeBankAccounts");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "DeductionCodes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CourseTypes");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CourseTypes");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "CourseTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CourseTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "CourseTypes");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoursePositions");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CoursePositions");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "CoursePositions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CoursePositions");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "CoursePositions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CourseLocations");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CourseLocations");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "CourseLocations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CourseLocations");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "CourseLocations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CourseInstructors");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CourseInstructors");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "CourseInstructors");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CourseInstructors");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "CourseInstructors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CourseEmployees");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CourseEmployees");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "CourseEmployees");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CourseEmployees");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "CourseEmployees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "InCompany",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "ModifiedDateTime",
                table: "ClassRooms");
        }
    }
}
