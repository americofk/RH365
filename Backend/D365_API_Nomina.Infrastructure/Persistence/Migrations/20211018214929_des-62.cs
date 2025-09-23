using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des62 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRoyaltyPayroll",
                table: "PayrollsProcess",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApplyRoyaltyPayroll",
                table: "PayrollProcessActions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRoyaltyPayroll",
                table: "EarningCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRoyaltyPayroll",
                table: "PayrollsProcess");

            migrationBuilder.DropColumn(
                name: "ApplyRoyaltyPayroll",
                table: "PayrollProcessActions");

            migrationBuilder.DropColumn(
                name: "IsRoyaltyPayroll",
                table: "EarningCodes");
        }
    }
}
