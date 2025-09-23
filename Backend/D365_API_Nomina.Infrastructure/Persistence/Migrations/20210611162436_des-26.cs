using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "EmployeesAddress",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesAddress_CountryId",
                table: "EmployeesAddress",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesAddress_Countries_CountryId",
                table: "EmployeesAddress",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesAddress_Countries_CountryId",
                table: "EmployeesAddress");

            migrationBuilder.DropIndex(
                name: "IX_EmployeesAddress_CountryId",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "EmployeesAddress");
        }
    }
}
