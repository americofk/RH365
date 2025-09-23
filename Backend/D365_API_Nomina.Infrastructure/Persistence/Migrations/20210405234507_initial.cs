using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D365_API_Nomina.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ClassRoomId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "CourseLocationId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "CourseTypeId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "DeductionCodeId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "DepartmentId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "EarningCodeId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "IntructorId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "JobId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "MenuId",
                minValue: 1L,
                maxValue: 9999L);

            migrationBuilder.CreateSequence<int>(
                name: "PayrollId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateSequence<int>(
                name: "PositionId",
                minValue: 1L,
                maxValue: 999999999L);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "CourseLocations",
                columns: table => new
                {
                    CourseLocationId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.CourseLocationId),'CLT-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLocations", x => x.CourseLocationId);
                });

            migrationBuilder.CreateTable(
                name: "CourseType",
                columns: table => new
                {
                    CourseTypeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.CourseTypeId),'CT-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseType", x => x.CourseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "DeductionCodes",
                columns: table => new
                {
                    DeductionCodeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.DeductionCodeId),'D-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProjId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProjCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    LedgerAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayrollAction = table.Column<int>(type: "int", nullable: false),
                    Ctbution_IndexBase = table.Column<int>(type: "int", nullable: false),
                    Ctbution_MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ctbution_PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    Ctbution_LimitPeriod = table.Column<int>(type: "int", nullable: false),
                    Ctbution_LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dduction_IndexBase = table.Column<int>(type: "int", nullable: false),
                    Dduction_MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dduction_PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    Dduction_LimitPeriod = table.Column<int>(type: "int", nullable: false),
                    Dduction_LimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionCodes", x => x.DeductionCodeId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.DepartmentId),'Dpt-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QtyWorkers = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "EarningCodes",
                columns: table => new
                {
                    EarningCodeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.EarningCodeId),'E-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsTSS = table.Column<bool>(type: "bit", nullable: false),
                    IsISR = table.Column<bool>(type: "bit", nullable: false),
                    ProjId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IndexBase = table.Column<int>(type: "int", nullable: false),
                    MultiplyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LedgerAccount = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarningCodes", x => x.EarningCodeId);
                });

            migrationBuilder.CreateTable(
                name: "FormatCode",
                columns: table => new
                {
                    FormatCodeId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatCode", x => x.FormatCodeId);
                });

            migrationBuilder.CreateTable(
                name: "Instructor",
                columns: table => new
                {
                    IntructorId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.IntructorId),'INT-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructor", x => x.IntructorId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.JobId),'J-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JobStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "MenusApp",
                columns: table => new
                {
                    MenuId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.MenuId),'MENU-000#')"),
                    MenuName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuFather = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenusApp", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_MenusApp_MenusApp_MenuFather",
                        column: x => x.MenuFather,
                        principalTable: "MenusApp",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    PayrollId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.PayrollId),'PAY-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PayFrecuency = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsRoyaltyPayroll = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.PayrollId);
                });

            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    ClassRoomId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.ClassRoomId),'CR-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CourseLocationId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    MaxStudentQty = table.Column<int>(type: "int", nullable: false),
                    AvailableTimeStart = table.Column<int>(type: "int", nullable: false),
                    AvailableTimeEnd = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.ClassRoomId);
                    table.ForeignKey(
                        name: "FK_ClassRooms_CourseLocations_CourseLocationId",
                        column: x => x.CourseLocationId,
                        principalTable: "CourseLocations",
                        principalColumn: "CourseLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Responsible = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrencyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyLogo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Companies_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "FORMAT((NEXT VALUE FOR dbo.PositionId),'POS-00000000#')"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsVacant = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PositionStatus = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_Positions_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayCycles",
                columns: table => new
                {
                    PayCycleId = table.Column<int>(type: "int", nullable: false),
                    PayrollId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PeriodStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DefaultPayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaidPerPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusPeriod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayCycles", x => new { x.PayCycleId, x.PayrollId });
                    table.ForeignKey(
                        name: "FK_PayCycles_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Alias = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FormatCodeId = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    ElevationType = table.Column<int>(type: "int", nullable: false),
                    CompanyDefaultId = table.Column<string>(type: "nvarchar(4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Alias);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyDefaultId",
                        column: x => x.CompanyDefaultId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_FormatCode_FormatCodeId",
                        column: x => x.FormatCodeId,
                        principalTable: "FormatCode",
                        principalColumn: "FormatCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositionRequirements",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PositionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_PositionRequirements_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyLogo", "CountryId", "CurrencyId", "Email", "Name", "Phone", "Responsible" },
                values: new object[] { "Root", null, null, null, "", "Empresa raiz", null, "Administrator" });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId", "Name" },
                values: new object[,]
                {
                    { "DOM", "República Dóminicana" },
                    { "CH", "Chile" }
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "CurrencyId", "Name" },
                values: new object[,]
                {
                    { "USD", "Dólares" },
                    { "DOP", "Pesos Dominicanos" }
                });

            migrationBuilder.InsertData(
                table: "FormatCode",
                columns: new[] { "FormatCodeId", "Name" },
                values: new object[,]
                {
                    { "en-US", "Estado Únidos" },
                    { "es-ES", "España" }
                });

            migrationBuilder.InsertData(
                table: "MenusApp",
                columns: new[] { "MenuId", "Action", "Description", "Icon", "MenuFather", "MenuName" },
                values: new object[] { "MENU-0001", "Click", "Título de configuración", "fa fa-setting", null, "Configuración" });

            migrationBuilder.InsertData(
                table: "MenusApp",
                columns: new[] { "MenuId", "Action", "Description", "Icon", "MenuFather", "MenuName" },
                values: new object[] { "MENU-0002", "Click", "Listado de colaboradores", "fa fa-user", "MENU-0001", "Colaboradores" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Alias", "CompanyDefaultId", "ElevationType", "Email", "FormatCodeId", "Name", "Password" },
                values: new object[] { "Admin", "Root", 1, "fflores@dynacorp365.com", "en-US", "Admin", "e10adc3949ba59abbe56e057f20f883e" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRooms_CourseLocationId",
                table: "ClassRooms",
                column: "CourseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CountryId",
                table: "Companies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrencyId",
                table: "Companies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MenusApp_MenuFather",
                table: "MenusApp",
                column: "MenuFather");

            migrationBuilder.CreateIndex(
                name: "IX_PayCycles_PayrollId",
                table: "PayCycles",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionRequirements_PositionId",
                table: "PositionRequirements",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_JobId",
                table: "Positions",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyDefaultId",
                table: "Users",
                column: "CompanyDefaultId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FormatCodeId",
                table: "Users",
                column: "FormatCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "CourseType");

            migrationBuilder.DropTable(
                name: "DeductionCodes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "EarningCodes");

            migrationBuilder.DropTable(
                name: "Instructor");

            migrationBuilder.DropTable(
                name: "MenusApp");

            migrationBuilder.DropTable(
                name: "PayCycles");

            migrationBuilder.DropTable(
                name: "PositionRequirements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CourseLocations");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "FormatCode");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropSequence(
                name: "ClassRoomId");

            migrationBuilder.DropSequence(
                name: "CourseLocationId");

            migrationBuilder.DropSequence(
                name: "CourseTypeId");

            migrationBuilder.DropSequence(
                name: "DeductionCodeId");

            migrationBuilder.DropSequence(
                name: "DepartmentId");

            migrationBuilder.DropSequence(
                name: "EarningCodeId");

            migrationBuilder.DropSequence(
                name: "IntructorId");

            migrationBuilder.DropSequence(
                name: "JobId");

            migrationBuilder.DropSequence(
                name: "MenuId");

            migrationBuilder.DropSequence(
                name: "PayrollId");

            migrationBuilder.DropSequence(
                name: "PositionId");
        }
    }
}
