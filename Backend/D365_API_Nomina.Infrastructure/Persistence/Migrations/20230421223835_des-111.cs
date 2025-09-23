using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankSecuence",
                table: "Payrolls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankSecuence",
                table: "Payrolls");
        }
    }
}
