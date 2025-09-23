using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des99 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeWorkCalendars",
                table: "EmployeeWorkCalendars");

            migrationBuilder.AddColumn<int>(
                name: "InternalId",
                table: "EmployeeWorkCalendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeWorkCalendars",
                table: "EmployeeWorkCalendars",
                columns: new[] { "InternalId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkCalendars_EmployeeId",
                table: "EmployeeWorkCalendars",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeWorkCalendars",
                table: "EmployeeWorkCalendars");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeWorkCalendars_EmployeeId",
                table: "EmployeeWorkCalendars");

            migrationBuilder.DropColumn(
                name: "InternalId",
                table: "EmployeeWorkCalendars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeWorkCalendars",
                table: "EmployeeWorkCalendars",
                columns: new[] { "EmployeeId", "CalendarDate" });
        }
    }
}
