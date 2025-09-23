using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeDeductionCodes",
                columns: table => new
                {
                    DeductionCodeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IndexDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDeductionCodes", x => new { x.DeductionCodeId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeDeductionCodes_DeductionCodes_DeductionCodeId",
                        column: x => x.DeductionCodeId,
                        principalTable: "DeductionCodes",
                        principalColumn: "DeductionCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDeductionCodes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDeductionCodes_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEarningCodes",
                columns: table => new
                {
                    EarningCodeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IndexEarning = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEarningCodes", x => new { x.EarningCodeId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeEarningCodes_EarningCodes_EarningCodeId",
                        column: x => x.EarningCodeId,
                        principalTable: "EarningCodes",
                        principalColumn: "EarningCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEarningCodes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEarningCodes_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDeductionCodes_EmployeeId",
                table: "EmployeeDeductionCodes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDeductionCodes_PayrollId",
                table: "EmployeeDeductionCodes",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEarningCodes_EmployeeId",
                table: "EmployeeEarningCodes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEarningCodes_PayrollId",
                table: "EmployeeEarningCodes",
                column: "PayrollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDeductionCodes");

            migrationBuilder.DropTable(
                name: "EmployeeEarningCodes");
        }
    }
}
