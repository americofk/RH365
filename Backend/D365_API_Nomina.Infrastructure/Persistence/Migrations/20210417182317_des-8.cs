using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0001");

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Alias = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Alias);
                });

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: "DOM",
                column: "Name",
                value: "República Dominicana");

            migrationBuilder.InsertData(
                table: "MenusApp",
                columns: new[] { "MenuId", "Action", "Description", "Icon", "MenuFather", "MenuName", "Sort" },
                values: new object[] { "MENU-0006", "Click", "Título de configuración", "fa fa-setting", null, "Configuración", 0 });

            migrationBuilder.UpdateData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0002",
                column: "MenuFather",
                value: "MENU-0006");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.DeleteData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0006");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: "DOM",
                column: "Name",
                value: "República Dóminicana");

            migrationBuilder.InsertData(
                table: "MenusApp",
                columns: new[] { "MenuId", "Action", "Description", "Icon", "MenuFather", "MenuName", "Sort" },
                values: new object[] { "MENU-0001", "Click", "Título de configuración", "fa fa-setting", null, "Configuración", 0 });

            migrationBuilder.UpdateData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0002",
                column: "MenuFather",
                value: "MENU-0001");
        }
    }
}
