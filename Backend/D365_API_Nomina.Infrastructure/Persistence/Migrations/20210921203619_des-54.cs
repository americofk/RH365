using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_Departments_DepartmentId",
                table: "Taxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_ProjCategories_ProjCategory",
                table: "Taxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_Projects_ProjId",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_DepartmentId",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_ProjCategory",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_ProjId",
                table: "Taxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTaxes",
                table: "EmployeeTaxes");

            migrationBuilder.AlterColumn<string>(
                name: "ProjId",
                table: "Taxes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "ProjCategory",
                table: "Taxes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Taxes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "TaxId",
                table: "EmployeeTaxes",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "WorkStatus",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTaxes",
                table: "EmployeeTaxes",
                columns: new[] { "TaxId", "EmployeeId", "PayrollId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTaxes_Taxes_TaxId",
                table: "EmployeeTaxes",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "TaxId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTaxes_Taxes_TaxId",
                table: "EmployeeTaxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTaxes",
                table: "EmployeeTaxes");

            migrationBuilder.DropColumn(
                name: "WorkStatus",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "ProjId",
                table: "Taxes",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjCategory",
                table: "Taxes",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Taxes",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxId",
                table: "EmployeeTaxes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTaxes",
                table: "EmployeeTaxes",
                columns: new[] { "TaxId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_DepartmentId",
                table: "Taxes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ProjCategory",
                table: "Taxes",
                column: "ProjCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_ProjId",
                table: "Taxes",
                column: "ProjId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_Departments_DepartmentId",
                table: "Taxes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_ProjCategories_ProjCategory",
                table: "Taxes",
                column: "ProjCategory",
                principalTable: "ProjCategories",
                principalColumn: "ProjCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_Projects_ProjId",
                table: "Taxes",
                column: "ProjId",
                principalTable: "Projects",
                principalColumn: "ProjId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
