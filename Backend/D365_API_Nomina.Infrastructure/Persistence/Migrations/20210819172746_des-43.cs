using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Departments_DepartmentId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_ProjCategories_ProjCategoryId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Projects_ProjId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_DepartmentId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_ProjCategoryId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_ProjId",
                table: "Loans");

            migrationBuilder.AlterColumn<string>(
                name: "LedgerAccount",
                table: "Loans",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LedgerAccount",
                table: "Loans",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_DepartmentId",
                table: "Loans",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ProjCategoryId",
                table: "Loans",
                column: "ProjCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ProjId",
                table: "Loans",
                column: "ProjId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Departments_DepartmentId",
                table: "Loans",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_ProjCategories_ProjCategoryId",
                table: "Loans",
                column: "ProjCategoryId",
                principalTable: "ProjCategories",
                principalColumn: "ProjCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Projects_ProjId",
                table: "Loans",
                column: "ProjId",
                principalTable: "Projects",
                principalColumn: "ProjId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
