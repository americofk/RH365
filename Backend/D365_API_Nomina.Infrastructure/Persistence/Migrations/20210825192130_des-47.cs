using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                table: "EmployeeLoans");

            migrationBuilder.AlterColumn<string>(
                name: "LoanId",
                table: "EmployeeLoans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "EmployeeId", "PayrollId" });

            migrationBuilder.CreateTable(
                name: "EmployeeExtraHours",
                columns: table => new
                {
                    WorkedDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EarningCodeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    StartHour = table.Column<int>(type: "int", nullable: false),
                    EndHour = table.Column<int>(type: "int", nullable: false),
                    TotalHour = table.Column<int>(type: "int", nullable: false),
                    TotalExtraHour = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Indice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StatusExtraHour = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeExtraHours", x => new { x.EarningCodeId, x.WorkedDay, x.PayrollId });
                    table.ForeignKey(
                        name: "FK_EmployeeExtraHours_EarningCodes_EarningCodeId",
                        column: x => x.EarningCodeId,
                        principalTable: "EarningCodes",
                        principalColumn: "EarningCodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeExtraHours_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeExtraHours_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExtraHours_EmployeeId",
                table: "EmployeeExtraHours",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExtraHours_PayrollId",
                table: "EmployeeExtraHours",
                column: "PayrollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeExtraHours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.AlterColumn<string>(
                name: "LoanId",
                table: "EmployeeLoans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "LoanId", "EmployeeId", "PayrollId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                table: "EmployeeLoans",
                column: "EmployeeId");
        }
    }
}
