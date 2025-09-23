using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeExtraHours",
                table: "EmployeeExtraHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeExtraHours_EmployeeId",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "TotalExtraHour",
                table: "EmployeeExtraHours");

            migrationBuilder.DropColumn(
                name: "TotalHour",
                table: "EmployeeExtraHours");

            migrationBuilder.AlterColumn<string>(
                name: "LoanId",
                table: "EmployeeLoans",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "LoanId", "EmployeeId", "PayrollId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeExtraHours",
                table: "EmployeeExtraHours",
                columns: new[] { "EmployeeId", "EarningCodeId", "WorkedDay" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                table: "EmployeeLoans",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExtraHours_EarningCodeId",
                table: "EmployeeExtraHours",
                column: "EarningCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLoans_Loans_LoanId",
                table: "EmployeeLoans",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "LoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLoans_Loans_LoanId",
                table: "EmployeeLoans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                table: "EmployeeLoans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeExtraHours",
                table: "EmployeeExtraHours");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeExtraHours_EarningCodeId",
                table: "EmployeeExtraHours");

            migrationBuilder.AlterColumn<string>(
                name: "LoanId",
                table: "EmployeeLoans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AddColumn<int>(
                name: "TotalExtraHour",
                table: "EmployeeExtraHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalHour",
                table: "EmployeeExtraHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeLoans",
                table: "EmployeeLoans",
                columns: new[] { "EmployeeId", "PayrollId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeExtraHours",
                table: "EmployeeExtraHours",
                columns: new[] { "EarningCodeId", "WorkedDay", "PayrollId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeExtraHours_EmployeeId",
                table: "EmployeeExtraHours",
                column: "EmployeeId");
        }
    }
}
