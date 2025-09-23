using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des71 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEmployees_Instructors_EmployeeId",
                table: "CourseEmployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes");

            migrationBuilder.AddColumn<int>(
                name: "InternalId",
                table: "EmployeeEarningCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes",
                column: "InternalId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEarningCodes_EarningCodeId",
                table: "EmployeeEarningCodes",
                column: "EarningCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEmployees_Employees_EmployeeId",
                table: "CourseEmployees",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEmployees_Employees_EmployeeId",
                table: "CourseEmployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeEarningCodes_EarningCodeId",
                table: "EmployeeEarningCodes");

            migrationBuilder.DropColumn(
                name: "InternalId",
                table: "EmployeeEarningCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEarningCodes",
                table: "EmployeeEarningCodes",
                columns: new[] { "EarningCodeId", "EmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEmployees_Instructors_EmployeeId",
                table: "CourseEmployees",
                column: "EmployeeId",
                principalTable: "Instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
