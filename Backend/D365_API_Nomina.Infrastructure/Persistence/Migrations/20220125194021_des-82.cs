using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des82 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IndexEarningDiary",
                table: "EmployeeEarningCodes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsUseDGT",
                table: "EarningCodeVersions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUseDGT",
                table: "EarningCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndexEarningDiary",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "IsUseDGT",
                table: "EarningCodeVersions");

            migrationBuilder.DropColumn(
                name: "IsUseDGT",
                table: "EarningCodes");
        }
    }
}
