using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des75 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaxes_Taxes_TaxId",
                table: "EmployeeTaxes");

            migrationBuilder.DropForeignKey(
                name: "FK_TaxDetails_Taxes_TaxId",
                table: "TaxDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Taxes",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_TaxDetails_TaxId",
                table: "TaxDetails");

            migrationBuilder.AlterColumn<string>(
                name: "InCompany",
                table: "Taxes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InCompany",
                table: "TaxDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InCompany",
                table: "EmployeeTaxes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Taxes",
                table: "Taxes",
                columns: new[] { "TaxId", "InCompany" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_TaxId_InCompany",
                table: "TaxDetails",
                columns: new[] { "TaxId", "InCompany" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTaxes_TaxId_InCompany",
                table: "EmployeeTaxes",
                columns: new[] { "TaxId", "InCompany" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaxes_Taxes_TaxId_InCompany",
                table: "EmployeeTaxes",
                columns: new[] { "TaxId", "InCompany" },
                principalTable: "Taxes",
                principalColumns: new[] { "TaxId", "InCompany" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaxDetails_Taxes_TaxId_InCompany",
                table: "TaxDetails",
                columns: new[] { "TaxId", "InCompany" },
                principalTable: "Taxes",
                principalColumns: new[] { "TaxId", "InCompany" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaxes_Taxes_TaxId_InCompany",
                table: "EmployeeTaxes");

            migrationBuilder.DropForeignKey(
                name: "FK_TaxDetails_Taxes_TaxId_InCompany",
                table: "TaxDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Taxes",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_TaxDetails_TaxId_InCompany",
                table: "TaxDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeTaxes_TaxId_InCompany",
                table: "EmployeeTaxes");

            migrationBuilder.AlterColumn<string>(
                name: "InCompany",
                table: "Taxes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "InCompany",
                table: "TaxDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "InCompany",
                table: "EmployeeTaxes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Taxes",
                table: "Taxes",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_TaxId",
                table: "TaxDetails",
                column: "TaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaxes_Taxes_TaxId",
                table: "EmployeeTaxes",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "TaxId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaxDetails_Taxes_TaxId",
                table: "TaxDetails",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "TaxId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
