using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des77 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaxDetails",
                table: "TaxDetails");

            migrationBuilder.DeleteData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0006");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaxDetails",
                table: "TaxDetails",
                columns: new[] { "InternalId", "TaxId", "InCompany" });

            migrationBuilder.InsertData(
                table: "MenusApp",
                columns: new[] { "MenuId", "Action", "Description", "Icon", "IsViewMenu", "MenuFather", "MenuName", "Sort" },
                values: new object[] { "MENU-0057", "Click", "Título de configuración", "fa fa-setting", false, null, "Configuración", 0 });

            migrationBuilder.UpdateData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0002",
                column: "MenuFather",
                value: "MENU-0057");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaxDetails",
                table: "TaxDetails");

            migrationBuilder.DeleteData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0057");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaxDetails",
                table: "TaxDetails",
                columns: new[] { "InternalId", "TaxId" });

            migrationBuilder.InsertData(
                table: "MenusApp",
                columns: new[] { "MenuId", "Action", "Description", "Icon", "IsViewMenu", "MenuFather", "MenuName", "Sort" },
                values: new object[] { "MENU-0006", "Click", "Título de configuración", "fa fa-setting", false, null, "Configuración", 0 });

            migrationBuilder.UpdateData(
                table: "MenusApp",
                keyColumn: "MenuId",
                keyValue: "MENU-0002",
                column: "MenuFather",
                value: "MENU-0006");
        }
    }
}
