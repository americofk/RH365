using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseLocations_CourseLocationId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseLocationId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseLocationId",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseLocationId",
                table: "Courses",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseLocationId",
                table: "Courses",
                column: "CourseLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseLocations_CourseLocationId",
                table: "Courses",
                column: "CourseLocationId",
                principalTable: "CourseLocations",
                principalColumn: "CourseLocationId");
        }
    }
}
