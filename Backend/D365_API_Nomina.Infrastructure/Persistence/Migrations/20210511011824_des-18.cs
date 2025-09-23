using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: "CH");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: "DOM");

            migrationBuilder.CreateSequence<int>(
                name: "EmployeeId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Country",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CountryId2",
                table: "Country",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Companies",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryId2");

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.EmployeeId),'EMP-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonalTreatment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    DependentsNumbers = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    NSS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ARS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AFP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EmployeeType = table.Column<int>(type: "int", nullable: false),
                    HomeOffice = table.Column<bool>(type: "bit", nullable: false),
                    OwnCar = table.Column<bool>(type: "bit", nullable: false),
                    HasDisability = table.Column<bool>(type: "bit", nullable: false),
                    WorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakWorkFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    BreakWorkTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Country_Country",
                        column: x => x.Country,
                        principalTable: "Country",
                        principalColumn: "CountryId2",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId2", "CountryId", "Name" },
                values: new object[] { "DOM", "DOM", "República Dominicana" });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId2", "CountryId", "Name" },
                values: new object[] { "CH", "CH", "Chile" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Country",
                table: "Employee",
                column: "Country");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId2",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropSequence(
                name: "EmployeeId");

            migrationBuilder.DropColumn(
                name: "CountryId2",
                table: "Country");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Country",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
