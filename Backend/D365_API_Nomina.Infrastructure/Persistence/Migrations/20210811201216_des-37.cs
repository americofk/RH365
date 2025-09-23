using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeDeductionCodes",
                table: "EmployeeDeductionCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "EmployeeDeductionCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeDeductionCodes",
                table: "EmployeeDeductionCodes",
                columns: new[] { "DeductionCodeId", "EmployeeId", "PayrollId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeDeductionCodes",
                table: "EmployeeDeductionCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "EmployeeDeductionCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeDeductionCodes",
                table: "EmployeeDeductionCodes",
                columns: new[] { "DeductionCodeId", "EmployeeId" });
        }
    }
}
