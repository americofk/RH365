using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Departments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.DepartmentId),'DPT-00000000#')",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValueSql: "FORMAT((NEXT VALUE FOR dbo.DepartmentId),'Dpt-00000000#')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Departments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.DepartmentId),'Dpt-00000000#')",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValueSql: "FORMAT((NEXT VALUE FOR dbo.DepartmentId),'DPT-00000000#')");
        }
    }
}
