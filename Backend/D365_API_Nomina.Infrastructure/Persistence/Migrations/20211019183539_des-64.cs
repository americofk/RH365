using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.AddColumn<int>(
                name: "InternalId",
                table: "EmployeeLoans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "InternalId", "EmployeeId" });

            migrationBuilder.CreateTable(
                name: "EmployeeLoanHistory",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false),
                    ParentInternalId = table.Column<int>(type: "int", nullable: false),
                    LoanId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PeriodStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PayrollProcessId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PendingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDues = table.Column<int>(type: "int", nullable: false),
                    PendingDues = table.Column<int>(type: "int", nullable: false),
                    AmountByDues = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLoanHistory", x => new { x.InternalId, x.ParentInternalId });
                    table.ForeignKey(
                        name: "FK_EmployeeLoanHistory_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeLoanHistory_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "LoanId");
                    table.ForeignKey(
                        name: "FK_EmployeeLoanHistory_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_LoanId",
                table: "EmployeeLoans",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoanHistory_EmployeeId",
                table: "EmployeeLoanHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoanHistory_LoanId",
                table: "EmployeeLoanHistory",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoanHistory_PayrollId",
                table: "EmployeeLoanHistory",
                column: "PayrollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLoanHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLoans_LoanId",
                table: "EmployeeLoans");

            migrationBuilder.DropColumn(
                name: "InternalId",
                table: "EmployeeLoans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "LoanId", "EmployeeId", "PayrollId" });
        }
    }
}
