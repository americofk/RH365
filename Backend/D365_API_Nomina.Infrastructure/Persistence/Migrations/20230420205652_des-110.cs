using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des110 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusExtraHour",
                table: "EmployeeWorkControlCalendars",
                newName: "StatusWorkControl");

            migrationBuilder.AddColumn<bool>(
                name: "IsForHourPayroll",
                table: "PayrollsProcess",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PayrollProcessId",
                table: "EmployeeWorkControlCalendars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayrollProcessId",
                table: "EmployeeEarningCodes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForHourPayroll",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "PayrollProcessId",
                table: "EmployeeWorkControlCalendars");

            migrationBuilder.DropColumn(
                name: "PayrollProcessId",
                table: "EmployeeEarningCodes");

            migrationBuilder.RenameColumn(
                name: "StatusWorkControl",
                table: "EmployeeWorkControlCalendars",
                newName: "StatusExtraHour");
        }
    }
}
