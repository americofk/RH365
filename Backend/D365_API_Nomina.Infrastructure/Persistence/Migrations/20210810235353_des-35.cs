using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "EmployeeEarningCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes",
                columns: new[] { "EarningCodeId", "EmployeeId", "PayrollId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "EmployeeEarningCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes",
                columns: new[] { "EarningCodeId", "EmployeeId" });
        }
    }
}
