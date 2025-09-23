using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "CourseId",
                minValue: 1L,
                maxValue: 999999L);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.CourseId),'CO-00000000#')"),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseTypeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    IsMatrixTraining = table.Column<bool>(type: "bit", nullable: false),
                    InternalExternal = table.Column<int>(type: "int", nullable: false),
                    CourseParentId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CourseLocationId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ClassRoomId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinStudents = table.Column<int>(type: "int", nullable: false),
                    MaxStudents = table.Column<int>(type: "int", nullable: false),
                    Periodicity = table.Column<int>(type: "int", nullable: false),
                    QtySessions = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Objetives = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Topics = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CourseStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_ClassRooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "ClassRooms",
                        principalColumn: "ClassRoomId");
                    table.ForeignKey(
                        name: "FK_Courses_CourseLocations_CourseLocationId",
                        column: x => x.CourseLocationId,
                        principalTable: "CourseLocations",
                        principalColumn: "CourseLocationId");
                    table.ForeignKey(
                        name: "FK_Courses_Courses_CourseParentId",
                        column: x => x.CourseParentId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_Courses_CourseTypes_CourseTypeId",
                        column: x => x.CourseTypeId,
                        principalTable: "CourseTypes",
                        principalColumn: "CourseTypeId");
                });

            migrationBuilder.CreateTable(
                name: "CourseInstructors",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseInstructors", x => new { x.CourseId, x.InstructorId });
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "InstructorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructors_InstructorId",
                table: "CourseInstructors",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ClassRoomId",
                table: "Courses",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseLocationId",
                table: "Courses",
                column: "CourseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseParentId",
                table: "Courses",
                column: "CourseParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseTypeId",
                table: "Courses",
                column: "CourseTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseInstructors");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropSequence(
                name: "CourseId");
        }
    }
}
