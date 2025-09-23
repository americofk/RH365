using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des65 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoanHistory_Employees_EmployeeId",
                table: "EmployeeLoanHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoanHistory_Loans_LoanId",
                table: "EmployeeLoanHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoanHistory_Payrolls_PayrollId",
                table: "EmployeeLoanHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoanHistory",
                table: "EmployeeLoanHistory");

            migrationBuilder.RenameTable(
                name: "EmployeeLoanHistory",
                newName: "EmployeeLoanHistories");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeLoanHistory_PayrollId",
                table: "EmployeeLoanHistories",
                newName: "IX_EmployeeLoanHistories_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeLoanHistory_LoanId",
                table: "EmployeeLoanHistories",
                newName: "IX_EmployeeLoanHistories_LoanId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeLoanHistory_EmployeeId",
                table: "EmployeeLoanHistories",
                newName: "IX_EmployeeLoanHistories_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoanHistories",
                table: "EmployeeLoanHistories",
                columns: new[] { "InternalId", "ParentInternalId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoanHistories_Employees_EmployeeId",
                table: "EmployeeLoanHistories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoanHistories_Loans_LoanId",
                table: "EmployeeLoanHistories",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoanHistories_Payrolls_PayrollId",
                table: "EmployeeLoanHistories",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "PayrollId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoanHistories_Employees_EmployeeId",
                table: "EmployeeLoanHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoanHistories_Loans_LoanId",
                table: "EmployeeLoanHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoanHistories_Payrolls_PayrollId",
                table: "EmployeeLoanHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoanHistories",
                table: "EmployeeLoanHistories");

            migrationBuilder.RenameTable(
                name: "EmployeeLoanHistories",
                newName: "EmployeeLoanHistory");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeLoanHistories_PayrollId",
                table: "EmployeeLoanHistory",
                newName: "IX_EmployeeLoanHistory_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeLoanHistories_LoanId",
                table: "EmployeeLoanHistory",
                newName: "IX_EmployeeLoanHistory_LoanId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeLoanHistories_EmployeeId",
                table: "EmployeeLoanHistory",
                newName: "IX_EmployeeLoanHistory_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoanHistory",
                table: "EmployeeLoanHistory",
                columns: new[] { "InternalId", "ParentInternalId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoanHistory_Employees_EmployeeId",
                table: "EmployeeLoanHistory",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoanHistory_Loans_LoanId",
                table: "EmployeeLoanHistory",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoanHistory_Payrolls_PayrollId",
                table: "EmployeeLoanHistory",
                column: "PayrollId",
                principalTable: "Payrolls",
                principalColumn: "PayrollId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
