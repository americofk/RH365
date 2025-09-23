using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des55 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EarningCodeVersions",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EarningCodeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsTSS = table.Column<bool>(type: "bit", nullable: false),
                    IsISR = table.Column<bool>(type: "bit", nullable: false),
                    IsExtraHours = table.Column<bool>(type: "bit", nullable: false),
                    ProjId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IndexBase = table.Column<int>(type: "int", nullable: false),
                    MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LedgerAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarningCodeVersions", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_EarningCodeVersions_EarningCodes_EarningCodeId",
                        column: x => x.EarningCodeId,
                        principalTable: "EarningCodes",
                        principalColumn: "EarningCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EarningCodeVersions_EarningCodeId",
                table: "EarningCodeVersions",
                column: "EarningCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EarningCodeVersions");
        }
    }
}
