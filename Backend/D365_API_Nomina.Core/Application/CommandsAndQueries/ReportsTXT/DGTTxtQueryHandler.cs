using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Reports;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.ReportsTXT
{
    public interface IDGTTxtQueryHandler
    {
        public Task<Response<object>> CreateDGT3(int year, int month);
        public Task<Response<object>> CreateDGT4(int year, int month);
        public Task<Response<object>> CreateDGT5(int year, int month);
        public Task<Response<object>> CreateDGT2(int year, int month);
        public Task<Response<object>> CreateTSS(int year, int month, string payrollid, string typetss);
        public Task<Response<object>> CreatePayroll(string payrollprocessid, string payrollid);
    }

    public class DGTTxtQueryHandler : IDGTTxtQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserInformation _CurrentUserInformation;

        public DGTTxtQueryHandler(IApplicationDbContext dbContext, ICurrentUserInformation currentUserInformation)
        {
            _dbContext = dbContext;
            _CurrentUserInformation = currentUserInformation;
        }

        private string TypeDocument(DocumentType e)
        {
            switch (e)
            {
                case DocumentType.IdentificationCard:
                    return "C";
                case DocumentType.Passport:
                    return "P";
                case DocumentType.SocialSecurityNumber:
                    return "N";
                case DocumentType.MigrationCard:
                    return "M";
                case DocumentType.InteriorAndPolice:
                    return "I";
                default:
                    return "X";
            }
        }
        
        //Document type for txt payroll
        private string TypeDocumentPayroll(DocumentType e)
        {
            switch (e)
            {
                case DocumentType.IdentificationCard:
                    return "RN";
                case DocumentType.Passport:
                    return "PS";
                default:
                    return "XX";
            }
        }
        
        private string TypeBankAccountPayroll(AccountType e)
        {
            switch (e)
            {
                case AccountType.Ahorro:
                    return "2";
                case AccountType.Corriente:
                    return "1";
                default:
                    return "X";
            }
        }


        public async Task<Response<object>> CreateDGT3(int year, int month)
        {
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            var employees = await _dbContext.Employees
                                .Join(_dbContext.EmployeeEarningCodes,
                                    e => e.EmployeeId,
                                    eec => eec.EmployeeId,
                                    (e, eec) => new { E = e, Eec = eec })
                                .Join(_dbContext.EarningCodes,
                                    join => join.Eec.EarningCodeId,
                                    ec => ec.EarningCodeId,
                                    (join, ec) => new { Join = join, Ec = ec })
                                .Where(x => x.Ec.IsUseDGT == true &&
                                        x.Join.E.EndWorkDate == new DateTime(2134, 12, 31) && x.Join.E.WorkStatus == WorkStatus.Employ)

                                .Select(x => new
                                {                        
                                    x.Join.E.EmployeeId,
                                    x.Join.E.Name,
                                    x.Join.E.LastName,
                                    Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.Join.E.EmployeeId).FirstOrDefault(),
                                    BirthDate = x.Join.E.BirthDate.ToString("ddMMyyyy"),
                                    Gender = x.Join.E.Gender.ToString().Substring(0, 1),
                                    AdmissionDate = x.Join.E.StartWorkDate.ToString("ddMMyyyy"),
                                    x.Join.E.OccupationId,
                                    OccupationDescription = _dbContext.Occupations.Where(y => y.OccupationId == x.Join.E.OccupationId).FirstOrDefault().Description,

                                    StartVacation = x.Join.E.StartWorkDate.ToString("ddMMyyyy"),
                                    EndVacation = x.Join.E.StartWorkDate.AddDays(14).ToString("ddMMyyyy"),
                                    Turn = "1",
                                    x.Join.E.LocationId,
                                    x.Join.E.EducationLevelId,
                                    x.Join.E.DisabilityTypeId,
                                    Salary = x.Join.Eec.IndexEarningMonthly
                                })
                                .ToListAsync();

            var a = employees.GroupBy(x => new
            {
                x.EmployeeId,
                x.Name,
                x.LastName,
                x.Document,
                x.BirthDate,
                x.Gender,
                x.AdmissionDate,
                x.OccupationId,
                x.OccupationDescription,

                x.StartVacation,
                x.EndVacation,
                x.Turn,
                x.LocationId,
                x.EducationLevelId,
                x.DisabilityTypeId,
                x.Salary
            })
            .Select(x => new
            {
                Employee = x.Key,
                Salary = x.Sum(x => x.Salary)
            }).ToList();


            List<TXTModelDGT3Detail> dgt3details = new List<TXTModelDGT3Detail>();

            foreach (var item in a)
            {
                dgt3details.Add(
                    new TXTModelDGT3Detail()
                    {
                        DocumentType = item.Employee.Document == null? "":TypeDocument(item.Employee.Document.DocumentType),
                        DocumentNumber = item.Employee.Document == null ? "" : FillStringHelper.Fill(AlignDirection.Right, item.Employee.Document.DocumentNumber, 25, ' '),
                        Name = FillStringHelper.Fill(AlignDirection.Right, item.Employee.Name, 50, ' '),
                        LastName = FillStringHelper.Fill(AlignDirection.Right, item.Employee.LastName, 40, ' '),
                        SecondLastName = FillStringHelper.Fill(AlignDirection.Right, string.Empty, 40, ' '),
                        BirthDate = item.Employee.BirthDate,
                        Gender = item.Employee.Gender,
                        Salary = FillStringHelper.Fill(AlignDirection.Left, item.Employee.Salary.ToString(), 16, '0'),
                        AdmissionDate = item.Employee.AdmissionDate,
                        Occupation = FillStringHelper.Fill(AlignDirection.Left, item.Employee.OccupationId, 6, '0'),
                        OccupationDescription = FillStringHelper.Fill(AlignDirection.Right, item.Employee.OccupationDescription, 150, ' '),
                        StartVacation = item.Employee.StartVacation,
                        EndVacation = item.Employee.EndVacation,
                        Turn = FillStringHelper.Fill(AlignDirection.Left, item.Employee.Turn, 6, '0'),
                        Location = FillStringHelper.Fill(AlignDirection.Left, item.Employee.LocationId, 6, '0'),
                        EductionalLevel = FillStringHelper.Fill(AlignDirection.Left, item.Employee.EducationLevelId, 5, '0'),
                        Disability = FillStringHelper.Fill(AlignDirection.Right, item.Employee.Turn, 50, ' '),
                    }
                );
            }

            return new Response<object>(new TXTModelDGT3()
            {
                RNC = FillStringHelper.Fill(AlignDirection.Left, company.Identification, 11, '0'),
                Period = period,

                Details = dgt3details,

                RegisterQty = FillStringHelper.Fill(AlignDirection.Left, (2 + dgt3details.Count()).ToString(), 6, '0'),                 
            });
        }
        
        
        public async Task<Response<object>> CreateDGT4(int year, int month)
        {
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            var employees = await _dbContext.Employees
                                            .Join(_dbContext.EmployeeHistories,
                                                e => e.EmployeeId,
                                                eh => eh.EmployeeId,
                                                (e, eh) => new { E = e, Eh = eh })
                                            .Join(_dbContext.EmployeeEarningCodes,
                                                join => join.E.EmployeeId,
                                                eec => eec.EmployeeId,
                                                (join, eec) => new { Join = join, Eec = eec })
                                            .Join(_dbContext.EarningCodes,
                                                joins => joins.Eec.EarningCodeId,
                                                ec => ec.EarningCodeId,
                                                (joins, ec) => new { Joins = joins, Ec = ec })
                                            .Where(x => x.Ec.IsUseDGT == true &&
                                                   x.Joins.Join.E.EndWorkDate == new DateTime(2134, 12, 31) && x.Joins.Join.E.WorkStatus == WorkStatus.Employ
                                                   && x.Joins.Join.E.EmployeeType == EmployeeType.Employee
                                                   && x.Joins.Join.Eh.RegisterDate >= startDate && x.Joins.Join.Eh.RegisterDate <= endDate)
                                            .Select(x => new
                                            {
                                                x.Joins.Join.E.EmployeeId,
                                                x.Joins.Join.E.Name,
                                                x.Joins.Join.E.LastName,
                                                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.Joins.Join.E.EmployeeId).FirstOrDefault(),
                                                BirthDate = x.Joins.Join.E.BirthDate.ToString("ddMMyyyy"),
                                                Gender = x.Joins.Join.E.Gender.ToString().Substring(0, 1),
                                                AdmissionDate = x.Joins.Join.E.StartWorkDate.ToString("ddMMyyyy"),
                                                DismissDate = x.Joins.Join.E.EndWorkDate.ToString("ddMMyyyy"),
                                                x.Joins.Join.E.OccupationId,
                                                OccupationDescription = _dbContext.Occupations.Where(y => y.OccupationId == x.Joins.Join.E.OccupationId).FirstOrDefault().Description,

                                                StartVacation = x.Joins.Join.E.StartWorkDate.ToString("ddMMyyyy"),
                                                EndVacation = x.Joins.Join.E.StartWorkDate.AddDays(14).ToString("ddMMyyyy"),
                                                Turn = "1",
                                                x.Joins.Join.E.LocationId,
                                                x.Joins.Join.E.Nationality,
                                                DateChange = x.Joins.Join.Eh.RegisterDate.ToString("ddMMyyyy"),
                                                x.Joins.Join.E.EducationLevelId,
                                                x.Joins.Join.E.DisabilityTypeId,
                                                Salary = x.Joins.Eec.IndexEarningMonthly,
                                                NoveltyType = x.Joins.Join.Eh.Type
                                            })
                                            .ToListAsync();

            var a = employees.GroupBy(x => new
            {
                x.EmployeeId,
                x.Name,
                x.LastName,
                x.BirthDate,
                x.Gender,
                x.AdmissionDate,
                x.DismissDate,
                x.OccupationId,
                x.OccupationDescription,
                x.StartVacation,
                x.EndVacation,
                x.Turn,
                x.LocationId,
                x.Nationality,
                x.DateChange,
                x.EducationLevelId,
                x.DisabilityTypeId,
                x.Salary,
                x.Document,
                x.NoveltyType
            })
            .Select(x => new
            {
                Employee = x.Key,
                Salary = x.Sum(x => x.Salary)
            }).ToList();

            List<TXTModelDGT4Detail> dgt4details = new List<TXTModelDGT4Detail>();

            foreach (var item in a)
            {
                dgt4details.Add(
                    new TXTModelDGT4Detail()
                    {
                        ActionType = FillStringHelper.Fill(AlignDirection.Right, item.Employee.NoveltyType, 3, ' '),
                        DocumentType = item.Employee.Document == null? "":TypeDocument(item.Employee.Document.DocumentType),
                        DocumentNumber = item.Employee.Document == null ? "" : FillStringHelper.Fill(AlignDirection.Right, item.Employee.Document.DocumentNumber, 25, ' '),
                        Name = FillStringHelper.Fill(AlignDirection.Right, item.Employee.Name, 50, ' '),
                        LastName = FillStringHelper.Fill(AlignDirection.Right, item.Employee.LastName, 40, ' '),
                        SecondLastName = FillStringHelper.Fill(AlignDirection.Right, string.Empty, 40, ' '),
                        BirthDate = item.Employee.BirthDate,
                        Gender = item.Employee.Gender,
                        Salary = FillStringHelper.Fill(AlignDirection.Left, item.Employee.Salary.ToString(), 16, '0'),
                        AdmissionDate = item.Employee.AdmissionDate,
                        DismissDate = item.Employee.DismissDate,
                        Occupation = FillStringHelper.Fill(AlignDirection.Left, item.Employee.OccupationId, 6, '0'),
                        OccupationDescription = FillStringHelper.Fill(AlignDirection.Right, item.Employee.OccupationDescription, 150, ' '),
                        StartVacation = item.Employee.StartVacation,
                        EndVacation = item.Employee.EndVacation,
                        Turn = FillStringHelper.Fill(AlignDirection.Left, item.Employee.Turn, 6, '0'),
                        Location = FillStringHelper.Fill(AlignDirection.Left, item.Employee.LocationId, 6, '0'),
                        Nationality = FillStringHelper.Fill(AlignDirection.Right, item.Employee.Nationality, 3, ' '),
                        DateChange = item.Employee.DateChange,
                        EductionalLevel = FillStringHelper.Fill(AlignDirection.Left, item.Employee.EducationLevelId, 5, '0'),
                        Disability = FillStringHelper.Fill(AlignDirection.Right, item.Employee.Turn, 50, ' '),
                    }
                );
            }

            return new Response<object>(new TXTModelDGT4()
            {
                RNC = FillStringHelper.Fill(AlignDirection.Left, company.Identification, 11, '0'),
                Period = period,

                Details = dgt4details,

                RegisterQty = FillStringHelper.Fill(AlignDirection.Left, (2 + dgt4details.Count()).ToString(), 6, '0'),                 
            });
        }


        public async Task<Response<object>> CreateDGT5(int year, int month)
        {
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            var employees = await _dbContext.Employees
                                            .Join(_dbContext.EmployeeHistories,
                                                e => e.EmployeeId,
                                                eh => eh.EmployeeId,
                                                (e, eh) => new { E = e, Eh = eh })
                                            .Join(_dbContext.EmployeeEarningCodes,
                                                join => join.E.EmployeeId,
                                                eec => eec.EmployeeId,
                                                (join, eec) => new { Join = join, Eec = eec })
                                            .Join(_dbContext.EarningCodes,
                                                joins => joins.Eec.EarningCodeId,
                                                ec => ec.EarningCodeId,
                                                (joins, ec) => new { Joins = joins, Ec = ec })
                                            .Where(x => x.Ec.IsUseDGT == true 
                                                   && x.Joins.Join.E.WorkStatus == WorkStatus.Employ
                                                   && x.Joins.Join.Eh.RegisterDate >= startDate && x.Joins.Join.Eh.RegisterDate <= endDate
                                                   && x.Joins.Join.Eh.Type == "NI"
                                                   && x.Joins.Join.E.EmployeeType == EmployeeType.Contractor)
                                            .Select(x => new
                                            {
                                                x.Joins.Join.E.EmployeeId,
                                                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.Joins.Join.E.EmployeeId).FirstOrDefault(),
                                                AdmissionDate = x.Joins.Join.E.StartWorkDate,
                                                DissmisDate = x.Joins.Join.E.EndWorkDate,
                                                x.Joins.Join.E.OccupationId,
                                                OccupationDescription = _dbContext.Occupations.Where(y => y.OccupationId == x.Joins.Join.E.OccupationId).FirstOrDefault().Description,
                                                Turn = "1",
                                                x.Joins.Join.E.LocationId,
                                                Salary = x.Joins.Eec.IndexEarningMonthly,
                                                SalaryDiary = x.Joins.Eec.IndexEarningDiary,
                                                x.Joins.Join.E.EducationLevelId,
                                                x.Joins.Join.E.DisabilityTypeId
                                            })
                                            .ToListAsync();

            var a = employees.GroupBy(x => new
            {
                x.EmployeeId,
                x.AdmissionDate,
                x.DissmisDate,
                x.OccupationId,
                x.OccupationDescription,
                x.Turn,
                x.LocationId,
                x.EducationLevelId,
                x.DisabilityTypeId,
                x.Document,
            })
            .Select(x => new
            {
                Employee = x.Key,
                SalaryDiary = x.Sum(x => x.SalaryDiary),
                WorkedDays = (x.Key.DissmisDate - x.Key.AdmissionDate).Days
            }).ToList();

            List<TXTModelDGT5Detail> dgt5details = new List<TXTModelDGT5Detail>();

            foreach (var item in a)
            {
                dgt5details.Add(
                    new TXTModelDGT5Detail()
                    {
                        DocumentType = item.Employee.Document == null ? "" : TypeDocument(item.Employee.Document.DocumentType),
                        DocumentNumber = item.Employee.Document == null ? "" : FillStringHelper.Fill(AlignDirection.Right, item.Employee.Document.DocumentNumber, 25, ' '),
                        Salary = FillStringHelper.Fill(AlignDirection.Left, (item.SalaryDiary * item.WorkedDays).ToString(), 16, '0'),
                        SalaryDiary = FillStringHelper.Fill(AlignDirection.Left, item.SalaryDiary.ToString(), 16, '0'),
                        AdmissionDate = item.Employee.AdmissionDate.ToString("ddMMyyyy"),
                        WorkedDays = FillStringHelper.Fill(AlignDirection.Left, (item.Employee.DissmisDate - item.Employee.AdmissionDate).ToString(), 2, '0'),
                        Occupation = FillStringHelper.Fill(AlignDirection.Left, item.Employee.OccupationId, 6, '0'),
                        OccupationDescription = FillStringHelper.Fill(AlignDirection.Right, item.Employee.OccupationDescription, 150, ' '),
                        Turn = FillStringHelper.Fill(AlignDirection.Left, item.Employee.Turn, 6, '0'),
                        Location = FillStringHelper.Fill(AlignDirection.Left, item.Employee.LocationId, 6, '0'),
                        EductionalLevel = FillStringHelper.Fill(AlignDirection.Left, item.Employee.EducationLevelId, 5, '0'),
                        Disability = FillStringHelper.Fill(AlignDirection.Right, item.Employee.Turn, 50, ' '),
                    }
                );
            }

            return new Response<object>(new TXTModelDGT5()
            {
                RNC = FillStringHelper.Fill(AlignDirection.Left, company.Identification, 11, '0'),
                Period = period,

                Details = dgt5details,

                RegisterQty = FillStringHelper.Fill(AlignDirection.Left, (2 + dgt5details.Count()).ToString(), 6, '0'),
            });
        }
        
        public async Task<Response<object>> CreateDGT2(int year, int month)
        {
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            List<int> days = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                days.Add(i);
            }

            var hours = await _dbContext.Employees
                                         .Join(_dbContext.EmployeeExtraHours,
                                             e => e.EmployeeId,
                                             eh => eh.EmployeeId,
                                             (e, eh) => new { E = e, Eh = eh })
                                         .Join(_dbContext.EmployeeEarningCodes,
                                             join => join.E.EmployeeId,
                                             eec => eec.EmployeeId,
                                             (join, eec) => new { Join = join, Eec = eec })
                                         .Join(_dbContext.EarningCodes,
                                             joins => joins.Eec.EarningCodeId,
                                             ec => ec.EarningCodeId,
                                             (joins, ec) => new { Joins = joins, Ec = ec })
                                         .Where(x => x.Ec.IsUseDGT == true
                                                && x.Joins.Join.Eh.WorkedDay >= startDate && x.Joins.Join.Eh.WorkedDay <= endDate
                                                && x.Joins.Join.Eh.StatusExtraHour == StatusExtraHour.Pagada)
                                        .Select(x => new {
                                            x.Joins.Join.Eh.WorkedDay,
                                            x.Joins.Join.Eh.EmployeeId,
                                            x.Joins.Join.Eh.Indice,
                                            x.Joins.Eec.IndexEarningDiary,
                                            x.Joins.Join.E.LocationId,
                                            Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.Joins.Join.Eh.EmployeeId && y.IsPrincipal == true).FirstOrDefault()
                                        })
                                        .ToListAsync();

            var employees = hours.GroupBy(x => new { x.EmployeeId, x.Document.DocumentNumber, x.Document.DocumentType, x.LocationId })
                                    .Select(x => new
                                    {
                                        employeeid = x.Key.EmployeeId,
                                        document = x.Key.DocumentNumber,
                                        documentType = x.Key.DocumentType,
                                        SalaryDiary = x.Sum(x => x.IndexEarningDiary),
                                        location = x.Key.LocationId
                                    }).ToList();


            string hours_days = string.Empty;

            List<TXTModelDGT2Detail> dgt2details = new List<TXTModelDGT2Detail>();

            foreach (var item in employees)
            {
                //var d = days.Select(x => new {}).ToList();

                hours_days = string.Empty;
                foreach (var day in days)
                {
                    var extrahour = hours.Where(y => y.EmployeeId == item.employeeid && y.WorkedDay.Day == day).FirstOrDefault();
                    var hour = "00";
                    var index = "000.00";

                    if (extrahour != null)
                    {
                        hour = extrahour.WorkedDay.Day.ToString().PadLeft(2, '0');
                        index = (extrahour.Indice * 100).ToString().PadLeft(6, '0');
                    }

                    hours_days += $"{hour}.00{index}";
                }

                dgt2details.Add(
                    new TXTModelDGT2Detail()
                    {
                        DocumentType = TypeDocument(item.documentType),
                        DocumentNumber = FillStringHelper.Fill(AlignDirection.Right, item.document, 25, ' '),
                        Location = FillStringHelper.Fill(AlignDirection.Left, item.location, 6, '0'),
                        AmountByNormalHour = FillStringHelper.Fill(AlignDirection.Left, item.SalaryDiary.ToString(), 16, '0'),
                        DayH = hours_days,
                        Reason = FillStringHelper.Fill(AlignDirection.Right, "d", 15, ' ')
                    }
                );
            }

            return new Response<object>(new TXTModelDGT2()
            {
                RNC = FillStringHelper.Fill(AlignDirection.Left, company.Identification, 11, '0'),
                Period = period,

                Details = dgt2details,

                RegisterQty = FillStringHelper.Fill(AlignDirection.Left, (2 + dgt2details.Count()).ToString(), 6, '0'),
            });
        }

        public async Task<Response<object>> CreateTSS(int year, int month, string payrollid, string typetss)
        {
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();
            var payroll = await _dbContext.Payrolls.Where(x => x.PayrollId == payrollid).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            var employees = await _dbContext.PayrollsProcess
                                            .Join(_dbContext.PayrollProcessDetails,
                                                pp => pp.PayrollProcessId,
                                                ppd => ppd.PayrollProcessId,
                                                (pp, ppd) => new { Pp = pp, Ppd = ppd })
                                            .Join(_dbContext.Employees,
                                                join => join.Ppd.EmployeeId,
                                                e => e.EmployeeId,
                                                (join, e) => new { Join = join, E = e })
                                            .Where(x => x.Join.Pp.PeriodStartDate >= startDate && x.Join.Pp.PeriodEndDate <= endDate
                                                   && x.Join.Pp.PayrollProcessStatus == PayrollProcessStatus.Paid
                                                   && x.Join.Pp.PayrollId == payrollid)
                                            .Select(x => new
                                            {
                                                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.E.EmployeeId).FirstOrDefault(),
                                                x.E.Name,
                                                x.E.LastName,
                                                Gender = x.E.Gender.ToString().Substring(0, 1),
                                                BirthDate = x.E.BirthDate.ToString("ddMMyyyy"),
                                                x.Join.Ppd.TotalAmount,
                                                x.Join.Ppd.TotalTaxAmount,
                                                x.E.EmployeeId
                                            })
                                            .ToListAsync();

            List<TXTModelTSSDetail> tssdetails = new List<TXTModelTSSDetail>();

            var a = employees.GroupBy(x => new
            {
                x.Document.DocumentNumber,
                x.Document.DocumentType,
                x.Name,
                x.LastName,
                x.Gender,
                x.BirthDate,
                x.EmployeeId
            }).Select(x => new
            {
                x.Key.DocumentNumber,
                x.Key.DocumentType,
                x.Key.Name,
                x.Key.LastName,
                x.Key.Gender,
                x.Key.BirthDate,
                x.Key.EmployeeId,
                TotalAmount = x.Sum(x => x.TotalAmount),
                TotalTaxAmount = x.Sum(x => x.TotalTaxAmount),
            });

            foreach (var item in a)
            {
                tssdetails.Add(
                    new TXTModelTSSDetail()
                    {
                        DocumentType = TypeDocument(item.DocumentType),
                        DocumentNumber = FillStringHelper.Fill(AlignDirection.Right, item.DocumentNumber, 25, ' '),
                        EmployeeName = FillStringHelper.Fill(AlignDirection.Right, item.Name, 50, ' '),
                        EmployeeLastName = FillStringHelper.Fill(AlignDirection.Right, item.LastName, 40, ' '),
                        EmployeeSecondLastName = FillStringHelper.Fill(AlignDirection.Right, "", 40, ' '),
                        BirthDate = item.BirthDate,
                        Gender = item.Gender,
                        Salary = FillStringHelper.Fill(AlignDirection.Left, item.TotalAmount.ToString(), 16, '0'),
                        Salary_ISR = FillStringHelper.Fill(AlignDirection.Left, item.TotalTaxAmount.ToString(), 16, '0'),
                        EmptyAmount = FillStringHelper.Fill(AlignDirection.Left, "0.00", 16, '0'),
                    }
                );
            }

            return new Response<object>(new TXTModelTSS()
            {
                RNC = FillStringHelper.Fill(AlignDirection.Left, company.Identification, 11, '0'),
                Period = period,
                Process = typetss,
                Details = tssdetails,

                RegisterQty = FillStringHelper.Fill(AlignDirection.Left, (2 + tssdetails.Count()).ToString(), 6, '0'),
            });
        }

        public async Task<Response<object>> CreatePayroll(string payrollprocessid, string payrollid)
        {
            int cont = 1;
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();
            var payroll = await _dbContext.Payrolls.Where(x => x.PayrollId == payrollid).FirstOrDefaultAsync();

            var employees = await _dbContext.PayrollsProcess
                                            .Join(_dbContext.PayrollProcessDetails,
                                                pp => pp.PayrollProcessId,
                                                ppd => ppd.PayrollProcessId,
                                                (pp, ppd) => new { Pp = pp, Ppd = ppd })
                                            .Join(_dbContext.Employees,
                                                join => join.Ppd.EmployeeId,
                                                e => e.EmployeeId,
                                                (join, e) => new { Join = join, E = e })
                                            .Where(x => x.Join.Pp.PayrollProcessId == payrollprocessid
                                                   && x.Join.Pp.PayrollProcessStatus == PayrollProcessStatus.Paid
                                                   && x.Join.Pp.PayrollId == payrollid
                                                   && x.Join.Ppd.PayMethod == PayMethod.Transfer)
                                            .Select(x => new
                                            {
                                                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.E.EmployeeId && y.IsPrincipal == true).FirstOrDefault(),
                                                Contact = _dbContext.EmployeeContactsInf.Where(y => y.EmployeeId == x.E.EmployeeId && y.IsPrincipal == true && y.ContactType == ContactType.Email).FirstOrDefault(),
                                                Bank = _dbContext.EmployeeBankAccounts.Where(y => y.EmployeeId == x.E.EmployeeId && y.IsPrincipal == true).FirstOrDefault(),
                                                x.E.Name,
                                                x.E.LastName,
                                                x.Join.Ppd.TotalAmount,
                                                x.Join.Ppd.TotalTaxAmount,
                                                x.E.EmployeeId,
                                                x.Join.Pp.PaymentDate
                                            })
                                            .ToListAsync();

            int banksecuence = payroll.BankSecuence == 0 ? 1 : payroll.BankSecuence +1;

            List<TXTModelPayrollDetailBP> payrolldetails = new List<TXTModelPayrollDetailBP>();

            foreach (var item in employees)
            {
                string employeename = $"{item.Name} {item.LastName}".Length > 35 ? $"{item.Name} {item.LastName}".Substring(0,35) : $"{item.Name} {item.LastName}";

                payrolldetails.Add(
                    new TXTModelPayrollDetailBP()
                    {
                        RNC = FillStringHelper.Fill(AlignDirection.Right, company.Identification, 15, ' '),
                        Sequence = FillStringHelper.Fill(AlignDirection.Left, banksecuence.ToString(), 7, '0'),
                        TransactionSequence = FillStringHelper.Fill(AlignDirection.Left, cont.ToString(), 7, '0'),
                        ToAccount = FillStringHelper.Fill(AlignDirection.Right,(item.Bank == null?"": item.Bank.AccountNum), 20, ' '),
                        ToAccountType = FillStringHelper.Fill(AlignDirection.Right, (item.Bank == null ? "" : TypeBankAccountPayroll(item.Bank.AccountType)),1, ' '),
                        TransactionAmount = FillStringHelper.Fill(AlignDirection.Left, item.TotalAmount.ToString().Replace(".", ""), 13, '0'),
                        IdentificationType = item.Document == null ? "xx":TypeDocumentPayroll(item.Document.DocumentType),
                        IdentificationNumber = FillStringHelper.Fill(AlignDirection.Right, (item.Document == null ? "" : item.Document.DocumentNumber), 15, ' '),
                        EmployeeName = FillStringHelper.Fill(AlignDirection.Right, employeename, 35, ' '),
                        ReferenceNumber = FillStringHelper.Fill(AlignDirection.Right, payrollprocessid.Substring(5, 9), 12, ' '),
                        Description = FillStringHelper.Fill(AlignDirection.Right, "PAGO NOMINA", 40, ' '),
                        DueDate = FillStringHelper.Fill(AlignDirection.Right, "", 4, ' '),
                        ContactForm = "1",
                        EmployeeEmail = FillStringHelper.Fill(AlignDirection.Right, (item.Contact == null?"":item.Contact.NumberAddress), 40, ' '),
                        EmployeePhone = FillStringHelper.Fill(AlignDirection.Right, "", 12, ' '),
                        PaymentProcess = "00",
                        EmptyValue = FillStringHelper.Fill(AlignDirection.Right, "", 27, ' '),
                        Filler = FillStringHelper.Fill(AlignDirection.Right, "", 52, ' ')
                    }
                );

                cont++;
            }

            var entity = payroll;
            entity.BankSecuence = banksecuence;

            _dbContext.Payrolls.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(new TXTModelPayrollBP()
            {
                RNC = FillStringHelper.Fill(AlignDirection.Right, company.Identification, 11, ' '),
                CompanyName = FillStringHelper.Fill(AlignDirection.Right, company.Name, 35, ' '),
                ServiceType = "01",
                EfectiveDate = employees.First().PaymentDate.ToString("yyyyMMdd"),
                QtyDebit = FillStringHelper.Fill(AlignDirection.Left, "0", 11, '0'),
                TotalAmountDebit = FillStringHelper.Fill(AlignDirection.Left, "0", 13, '0'),
                QtyCredit = FillStringHelper.Fill(AlignDirection.Left, payrolldetails.Count().ToString(), 11, '0'),
                TotalAmountCredit = FillStringHelper.Fill(AlignDirection.Left, employees.Sum(x=> x.TotalAmount).ToString().Replace(".", ""), 13, '0'),
                Hour = DateTime.Now.ToString("HHMM"),
                Email = FillStringHelper.Fill(AlignDirection.Right, company.Email, 40, ' ') ,
                Status = "",
                Filler = FillStringHelper.Fill(AlignDirection.Right, "", 136, ' '),
                Details = payrolldetails,
            });
        }
    }
}
