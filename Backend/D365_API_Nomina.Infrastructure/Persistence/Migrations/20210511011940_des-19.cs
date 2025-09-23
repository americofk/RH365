using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Country_Country",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CountryId2",
                table: "Country");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Country_Country",
                table: "Employee",
                column: "Country",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Country_Country",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: "CH");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: "DOM");

            migrationBuilder.AddColumn<string>(
                name: "CountryId2",
                table: "Country",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryId2");

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId2", "CountryId", "Name" },
                values: new object[] { "DOM", "DOM", "República Dominicana" });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId2", "CountryId", "Name" },
                values: new object[] { "CH", "CH", "Chile" });

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "CountryId2",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Country_Country",
                table: "Employee",
                column: "Country",
                principalTable: "Country",
                principalColumn: "CountryId2",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
