using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PositionRequirements",
                table: "PositionRequirements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PositionRequirements",
                table: "PositionRequirements",
                columns: new[] { "Name", "PositionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PositionRequirements",
                table: "PositionRequirements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PositionRequirements",
                table: "PositionRequirements",
                column: "Name");
        }
    }
}
