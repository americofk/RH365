using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des109 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForHourPayroll",
                table: "Payrolls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "IndexEarningHour",
                table: "EmployeeEarningCodes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsUseCalcHour",
                table: "EmployeeEarningCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForHourPayroll",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "IndexEarningHour",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "IsUseCalcHour",
                table: "EmployeeEarningCodes");
        }
    }
}
