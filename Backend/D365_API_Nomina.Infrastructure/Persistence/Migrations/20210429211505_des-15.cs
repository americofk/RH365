using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_CourseParentId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseParentId",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseParentId",
                table: "Courses",
                column: "CourseParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_CourseParentId",
                table: "Courses",
                column: "CourseParentId",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }
    }
}
