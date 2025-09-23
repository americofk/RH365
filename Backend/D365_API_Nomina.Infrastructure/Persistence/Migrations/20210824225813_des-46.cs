using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "LoanId", "EmployeeId", "PayrollId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "LoanId", "EmployeeId" });
        }
    }
}
