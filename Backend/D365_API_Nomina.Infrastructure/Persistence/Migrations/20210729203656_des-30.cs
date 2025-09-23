using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPayCycleTax",
                table: "PayrollsProcess",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsedForTax",
                table: "PayrollsProcess",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "PayrollProcessDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForTax",
                table: "PayCycles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QtyPeriodForPaid",
                table: "EmployeeEarningCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayCycleTax",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "UsedForTax",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "IsForTax",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "QtyPeriodForPaid",
                table: "EmployeeEarningCodes");
        }
    }
}
