using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des70 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeAction",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes",
                columns: new[] { "EarningCodeId", "EmployeeId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "EmployeeAction",
                table: "Employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes",
                columns: new[] { "EarningCodeId", "EmployeeId", "PayrollId" });
        }
    }
}
