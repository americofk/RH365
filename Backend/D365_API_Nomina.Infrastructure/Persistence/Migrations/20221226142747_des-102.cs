using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PayFrecuency",
                table: "EmployeeDeductionCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QtyPeriodForPaid",
                table: "EmployeeDeductionCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartPeriodForPaid",
                table: "EmployeeDeductionCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayFrecuency",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "QtyPeriodForPaid",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "StartPeriodForPaid",
                table: "EmployeeDeductionCodes");
        }
    }
}
