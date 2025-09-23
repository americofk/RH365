using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class des61 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeductionAmount",
                table: "EmployeeDeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsForTaxCalc",
                table: "DeductionCodeVersions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dduction_MultiplyAmount",
                table: "DeductionCodes",
                type: "decimal(32,16)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dduction_LimitAmount",
                table: "DeductionCodes",
                type: "decimal(32,16)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ctbution_MultiplyAmount",
                table: "DeductionCodes",
                type: "decimal(32,16)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ctbution_LimitAmount",
                table: "DeductionCodes",
                type: "decimal(32,16)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "IsForTaxCalc",
                table: "DeductionCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeductionAmount",
                table: "EmployeeDeductionCodes");

            migrationBuilder.DropColumn(
                name: "IsForTaxCalc",
                table: "DeductionCodeVersions");

            migrationBuilder.DropColumn(
                name: "IsForTaxCalc",
                table: "DeductionCodes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dduction_MultiplyAmount",
                table: "DeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,16)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dduction_LimitAmount",
                table: "DeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,16)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ctbution_MultiplyAmount",
                table: "DeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,16)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ctbution_LimitAmount",
                table: "DeductionCodes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(32,16)");
        }
    }
}
