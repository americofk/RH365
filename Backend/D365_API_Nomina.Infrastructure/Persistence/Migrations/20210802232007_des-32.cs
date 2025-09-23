using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayFrecuency",
                table: "EmployeeLoans",
                newName: "TotalDues");

            migrationBuilder.RenameColumn(
                name: "PayDays",
                table: "EmployeeLoans",
                newName: "StartPeriodForPaid");

            migrationBuilder.AddColumn<int>(
                name: "PendingDues",
                table: "EmployeeLoans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QtyPeriodForPaid",
                table: "EmployeeLoans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartPeriodForPaid",
                table: "EmployeeEarningCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingDues",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "QtyPeriodForPaid",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "StartPeriodForPaid",
                table: "EmployeeEarningCodes");

            migrationBuilder.RenameColumn(
                name: "TotalDues",
                table: "EmployeeLoans",
                newName: "PayFrecuency");

            migrationBuilder.RenameColumn(
                name: "StartPeriodForPaid",
                table: "EmployeeLoans",
                newName: "PayDays");
        }
    }
}
