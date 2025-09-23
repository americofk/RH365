using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des57 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "EarningCodes");

            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "DeductionCodes");

            migrationBuilder.AddColumn<decimal>(
                name: "Ctbution_LimitAmountToApply",
                table: "DeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Dduction_LimitAmountToApply",
                table: "DeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DeductionCodeVersions",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeductionCodeId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProjCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PayrollAction = table.Column<int>(type: "int", nullable: false),
                    Ctbution_IndexBase = table.Column<int>(type: "int", nullable: false),
                    Ctbution_MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ctbution_PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    Ctbution_LimitPeriod = table.Column<int>(type: "int", nullable: false),
                    Ctbution_LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ctbution_LimitAmountToApply = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dduction_IndexBase = table.Column<int>(type: "int", nullable: false),
                    Dduction_MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dduction_PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    Dduction_LimitPeriod = table.Column<int>(type: "int", nullable: false),
                    Dduction_LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dduction_LimitAmountToApply = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionCodeVersions", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_DeductionCodeVersions_DeductionCodes_DeductionCodeId",
                        column: x => x.DeductionCodeId,
                        principalTable: "DeductionCodes",
                        principalColumn: "DeductionCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeductionCodeVersions_DeductionCodeId",
                table: "DeductionCodeVersions",
                column: "DeductionCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "Ctbution_LimitAmountToApply",
                table: "DeductionCodes");

            migrationBuilder.DropColumn(
                name: "Dduction_LimitAmountToApply",
                table: "DeductionCodes");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "EarningCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "DeductionCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
