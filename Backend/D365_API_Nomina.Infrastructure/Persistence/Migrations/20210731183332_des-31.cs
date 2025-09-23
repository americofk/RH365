using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollProcessAction_Employees_EmployeeId",
                table: "PayrollProcessAction");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollProcessAction_PayrollsProcess_PayrollProcessId",
                table: "PayrollProcessAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollProcessAction",
                table: "PayrollProcessAction");

            migrationBuilder.RenameTable(
                name: "PayrollProcessAction",
                newName: "PayrollProcessActions");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollProcessAction_PayrollProcessId",
                table: "PayrollProcessActions",
                newName: "IX_PayrollProcessActions_PayrollProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollProcessAction_EmployeeId",
                table: "PayrollProcessActions",
                newName: "IX_PayrollProcessActions_EmployeeId");

            migrationBuilder.AddColumn<bool>(
                name: "LoanStatus",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollProcessActions",
                table: "PayrollProcessActions",
                columns: new[] { "InternalId", "PayrollProcessId", "EmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollProcessActions_Employees_EmployeeId",
                table: "PayrollProcessActions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollProcessActions_PayrollsProcess_PayrollProcessId",
                table: "PayrollProcessActions",
                column: "PayrollProcessId",
                principalTable: "PayrollsProcess",
                principalColumn: "PayrollProcessId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollProcessActions_Employees_EmployeeId",
                table: "PayrollProcessActions");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollProcessActions_PayrollsProcess_PayrollProcessId",
                table: "PayrollProcessActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollProcessActions",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "LoanStatus",
                table: "Loans");

            migrationBuilder.RenameTable(
                name: "PayrollProcessActions",
                newName: "PayrollProcessAction");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollProcessActions_PayrollProcessId",
                table: "PayrollProcessAction",
                newName: "IX_PayrollProcessAction_PayrollProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollProcessActions_EmployeeId",
                table: "PayrollProcessAction",
                newName: "IX_PayrollProcessAction_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollProcessAction",
                table: "PayrollProcessAction",
                columns: new[] { "InternalId", "PayrollProcessId", "EmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollProcessAction_Employees_EmployeeId",
                table: "PayrollProcessAction",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollProcessAction_PayrollsProcess_PayrollProcessId",
                table: "PayrollProcessAction",
                column: "PayrollProcessId",
                principalTable: "PayrollsProcess",
                principalColumn: "PayrollProcessId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
