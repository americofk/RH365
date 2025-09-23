using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalTssAmount",
                table: "PayrollProcessDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsForTss",
                table: "PayCycles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForTssCalc",
                table: "DeductionCodeVersions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForTssCalc",
                table: "DeductionCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTssAmount",
                table: "PayrollProcessDetails");

            migrationBuilder.DropColumn(
                name: "IsForTss",
                table: "PayCycles");

            migrationBuilder.DropColumn(
                name: "IsForTssCalc",
                table: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "IsForTssCalc",
                table: "DeductionCodes");
        }
    }
}
