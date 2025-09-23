using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IndexEarningMonthly",
                table: "EmployeeEarningCodes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PayFrecuency",
                table: "EmployeeEarningCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndexEarningMonthly",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "PayFrecuency",
                table: "EmployeeEarningCodes");
        }
    }
}
