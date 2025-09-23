using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "LoanId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "PayrollProcessId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "ProcessDetailsId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "ProjCategoryId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "ProjId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndWorkDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PayMethod",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartWorkDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrincipal",
                table: "EmployeeDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmployeeLoans",
                columns: table => new
                {
                    LoanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayDays = table.Column<int>(type: "int", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PendingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PayFrecuency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLoans", x => new { x.LoanId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeLoans_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeLoans_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayrollsProcess",
                columns: table => new
                {
                    PayrollProcessId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.PayrollProcessId),'PPAY-00000000#')"),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeQuantity = table.Column<int>(type: "int", nullable: false),
                    ProjId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PeriodStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayCycleId = table.Column<int>(type: "int", nullable: false),
                    EmployeeQuantityForPay = table.Column<int>(type: "int", nullable: false),
                    PayrollProcessStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollsProcess", x => x.PayrollProcessId);
                    table.ForeignKey(
                        name: "FK_PayrollsProcess_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId");
                });

            migrationBuilder.CreateTable(
                name: "ProjCategories",
                columns: table => new
                {
                    ProjCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.ProjCategoryId),'PRJC-00000000#')"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LedgerAccount = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjCategoryStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjCategories", x => x.ProjCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.ProjId),'PRJ-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LedgerAccount = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjId);
                });

            migrationBuilder.CreateTable(
                name: "PayrollProcessAction",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false),
                    PayrollProcessId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PayrollActionType = table.Column<int>(type: "int", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplyTax = table.Column<bool>(type: "bit", nullable: false),
                    ApplyTSS = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollProcessAction", x => new { x.InternalId, x.PayrollProcessId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_PayrollProcessAction_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollProcessAction_PayrollsProcess_PayrollProcessId",
                        column: x => x.PayrollProcessId,
                        principalTable: "PayrollsProcess",
                        principalColumn: "PayrollProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayrollProcessDetails",
                columns: table => new
                {
                    PayrollProcessId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayMethod = table.Column<int>(type: "int", nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Document = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DepartmentId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    DepartmentName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PayrollProcessStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollProcessDetails", x => new { x.PayrollProcessId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_PayrollProcessDetails_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollProcessDetails_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollProcessDetails_PayrollsProcess_PayrollProcessId",
                        column: x => x.PayrollProcessId,
                        principalTable: "PayrollsProcess",
                        principalColumn: "PayrollProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    LoanId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.LoanId),'LO-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LedgerAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    IndexBase = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ProjCategoryId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ProjId = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loans_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_ProjCategories_ProjCategoryId",
                        column: x => x.ProjCategoryId,
                        principalTable: "ProjCategories",
                        principalColumn: "ProjCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Projects_ProjId",
                        column: x => x.ProjId,
                        principalTable: "Projects",
                        principalColumn: "ProjId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    TaxId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LedgerAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LimitPeriod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndexBase = table.Column<int>(type: "int", nullable: false),
                    ProjId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ProjCategory = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TaxStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.TaxId);
                    table.ForeignKey(
                        name: "FK_Taxes_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Taxes_ProjCategories_ProjCategory",
                        column: x => x.ProjCategory,
                        principalTable: "ProjCategories",
                        principalColumn: "ProjCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Taxes_Projects_ProjId",
                        column: x => x.ProjId,
                        principalTable: "Projects",
                        principalColumn: "ProjId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxDetails",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    AnnualAmountHigher = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnnualAmountNotExceed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FixedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicableScale = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetails", x => new { x.InternalId, x.TaxId });
                    table.ForeignKey(
                        name: "FK_TaxDetails_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                table: "EmployeeLoans",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_PayrollId",
                table: "EmployeeLoans",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_DepartmentId",
                table: "Loans",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ProjCategoryId",
                table: "Loans",
                column: "ProjCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ProjId",
                table: "Loans",
                column: "ProjId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollProcessAction_EmployeeId",
                table: "PayrollProcessAction",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollProcessAction_PayrollProcessId",
                table: "PayrollProcessAction",
                column: "PayrollProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollProcessDetails_DepartmentId",
                table: "PayrollProcessDetails",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollProcessDetails_EmployeeId",
                table: "PayrollProcessDetails",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollsProcess_PayrollId",
                table: "PayrollsProcess",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_TaxId",
                table: "TaxDetails",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_DepartmentId",
                table: "Taxes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ProjCategory",
                table: "Taxes",
                column: "ProjCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ProjId",
                table: "Taxes",
                column: "ProjId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLoans");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "PayrollProcessAction");

            migrationBuilder.DropTable(
                name: "PayrollProcessDetails");

            migrationBuilder.DropTable(
                name: "TaxDetails");

            migrationBuilder.DropTable(
                name: "PayrollsProcess");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "ProjCategories");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropSequence(
                name: "LoanId");

            migrationBuilder.DropSequence(
                name: "PayrollProcessId");

            migrationBuilder.DropSequence(
                name: "ProcessDetailsId");

            migrationBuilder.DropSequence(
                name: "ProjCategoryId");

            migrationBuilder.DropSequence(
                name: "ProjId");

            migrationBuilder.DropColumn(
                name: "EndWorkDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PayMethod",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "StartWorkDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsPrincipal",
                table: "EmployeeDocuments");
        }
    }
}
