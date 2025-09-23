using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_FormatCode_FormatCodeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormatCode",
                table: "FormatCode");

            migrationBuilder.RenameTable(
                name: "FormatCode",
                newName: "FormatCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Departments",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormatCodes",
                table: "FormatCodes",
                column: "FormatCodeId");

            migrationBuilder.CreateTable(
                name: "CompaniesAssignedToUsers",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompaniesAssignedToUsers", x => new { x.Alias, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompaniesAssignedToUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompaniesAssignedToUsers_Users_Alias",
                        column: x => x.Alias,
                        principalTable: "Users",
                        principalColumn: "Alias",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuAssignedToUsers",
                columns: table => new
                {
                    Alias = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    MenuId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PrivilegeView = table.Column<bool>(type: "bit", nullable: false),
                    PrivilegeEdit = table.Column<bool>(type: "bit", nullable: false),
                    PrivilegeDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuAssignedToUsers", x => new { x.Alias, x.MenuId });
                    table.ForeignKey(
                        name: "FK_MenuAssignedToUsers_MenusApp_MenuId",
                        column: x => x.MenuId,
                        principalTable: "MenusApp",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuAssignedToUsers_Users_Alias",
                        column: x => x.Alias,
                        principalTable: "Users",
                        principalColumn: "Alias",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Alias",
                keyValue: "Admin",
                column: "ElevationType",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompaniesAssignedToUsers_CompanyId",
                table: "CompaniesAssignedToUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuAssignedToUsers_MenuId",
                table: "MenuAssignedToUsers",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FormatCodes_FormatCodeId",
                table: "Users",
                column: "FormatCodeId",
                principalTable: "FormatCodes",
                principalColumn: "FormatCodeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_FormatCodes_FormatCodeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CompaniesAssignedToUsers");

            migrationBuilder.DropTable(
                name: "MenuAssignedToUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormatCodes",
                table: "FormatCodes");

            migrationBuilder.RenameTable(
                name: "FormatCodes",
                newName: "FormatCode");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Departments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormatCode",
                table: "FormatCode",
                column: "FormatCodeId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Alias",
                keyValue: "Admin",
                column: "ElevationType",
                value: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FormatCode_FormatCodeId",
                table: "Users",
                column: "FormatCodeId",
                principalTable: "FormatCode",
                principalColumn: "FormatCodeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
