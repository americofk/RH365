using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Reports;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Reports
{
    public interface IReportQueryHandler
    {
        public Task<PagedResponse<IEnumerable<ReportPayrollPaymentResponse>>> PayrollPaymentReport(string payrollProcessId, string employeeid, string departmentid);
        public Task<PagedResponse<IEnumerable<ReportResumePayrollResponse>>> ResumePaymentReport(string payrollProcessId, string departmentid);
        public Task<Response<ReportPayrollProcessResponse>> PayrollProcessReport(string payrollProcessId, string departmentId);
        public Task<Response<ReportEmployeeResponse>> EmployeeReport(string departmentId);
        public Task<Response<ReportTSSFileResponse>> TSSReport(int year, int month, string payrollid, string typetss);
        public Task<Response<DGT4Response>> DGT4Report(int year, int month);
        public Task<Response<DGT2Response>> DGT2Report(int year, int month);
        public Task<Response<DGT3Response>> DGT3Report(int year, int month);
        public Task<Response<DGT5Response>> DGT5Report(int year, int month);
        public Task<Response<DGT9Response>> DGT9Report(int year, int month);
        public Task<Response<DGT12Response>> DGT12Report(int year, int month);
    }

    public class ReportQueryHandler : IReportQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;

        private readonly ICurrentUserInformation _CurrentUserInformation;

        public ReportQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserInformation currentUserInformation)
        {
            _dbContext = applicationDbContext;
            _CurrentUserInformation = currentUserInformation;
        }


        //Reporte de recibo de pago de nómina
        public async Task<PagedResponse<IEnumerable<ReportPayrollPaymentResponse>>> PayrollPaymentReport(string payrollProcessId, string employeeid, string departmentid)
        {
            List<ReportPayrollPaymentResponse> report = new List<ReportPayrollPaymentResponse>();
            //Loan, ExtraHours, Tax
            var configuration = await _dbContext.ReportsConfig.FirstOrDefaultAsync();

            if (configuration == null)
            {
                configuration = new ReportConfig();
            };

            if (string.IsNullOrEmpty(departmentid))
                departmentid = "-";

            if (string.IsNullOrEmpty(employeeid))
                employeeid = "-";


            var a = await _dbContext.PayrollsProcess
                .Join(_dbContext.PayrollProcessDetails,
                    pp => pp.PayrollProcessId,
                    ppd => ppd.PayrollProcessId,
                    (pp, ppd) => new { Pp = pp, Ppd = ppd })
                .Where(x => x.Pp.PayrollProcessId == payrollProcessId
                        && x.Ppd.DepartmentId.Contains(departmentid)
                        && x.Ppd.EmployeeId.Contains(employeeid)
                        && x.Pp.PayrollProcessStatus == PayrollProcessStatus.Paid
                        || x.Pp.PayrollProcessStatus == PayrollProcessStatus.Closed)
                .Select(x => new
                {
                    EmployeeName = x.Ppd.EmployeeName,
                    EmployeeId = x.Ppd.EmployeeId,
                    StartWorkDate = DateTime.Today,
                    Document = x.Ppd.Document,
                    Department = x.Ppd.DepartmentName,
                    PayrollName = x.Pp.PayrollId,
                    Period = $"{x.Pp.PeriodStartDate.ToShortDateString()} - {x.Pp.PeriodEndDate.ToShortDateString()}",
                    PaymentDate = x.Pp.PaymentDate,
                    PayMethod = x.Ppd.PayMethod,
                    Total = x.Ppd.TotalAmount,
                    BankAccount = x.Ppd.BankAccount,
                    PositionName = _dbContext.EmployeePositions
                                         .Join(_dbContext.Positions,
                                            ep => ep.PositionId,
                                            p => p.PositionId,
                                            (ep, p) => new { ep.EmployeeId, ep.EmployeePositionStatus, p.PositionName })
                                         .Where(y => y.EmployeeId == x.Ppd.EmployeeId && y.EmployeePositionStatus == true)
                                         .Select(x => x.PositionName)
                                         .FirstOrDefault(),

                    EmployeeEmail = _dbContext.EmployeeContactsInf
                                         .Where(y => y.EmployeeId == x.Ppd.EmployeeId && y.IsPrincipal == true && y.ContactType == ContactType.Email)
                                         .Select(x => x.NumberAddress)
                                         .FirstOrDefault(),
                }).ToListAsync();

            if (a != null)
            {
                var actions = await _dbContext.PayrollProcessActions.Where(x => x.PayrollProcessId == payrollProcessId).ToListAsync();

                foreach (var item in a)
                {
                    var b = actions.Where(x => x.EmployeeId == item.EmployeeId).ToList();

                    var s = b.Where(x => x.ActionId == configuration.Salary).Select(x => x.ActionAmount).ToList().Sum();
                    var com = b.Where(x => x.ActionId == configuration.Comission).Select(x => x.ActionAmount).ToList().Sum();
                    var eh = b.Where(x => x.PayrollActionType == PayrollActionType.ExtraHours).Select(x => x.ActionAmount).ToList().Sum();
                    var oEarning = b.Where(x => x.PayrollActionType == PayrollActionType.Earning && x.ActionId != configuration.Salary && x.ActionId != configuration.Comission)
                                    .Select(x => x.ActionAmount).ToList().Sum();


                    var afp = b.Where(x => x.ActionId == configuration.AFP && x.PayrollActionType == PayrollActionType.Deduction).Select(x => x.ActionAmount).ToList().Sum();
                    var sfs = b.Where(x => x.ActionId == configuration.SFS && x.PayrollActionType == PayrollActionType.Deduction).Select(x => x.ActionAmount).ToList().Sum();
                    var loanCoop = b.Where(x => x.ActionId == configuration.LoanCooperative).Select(x => x.ActionAmount).ToList().Sum();
                    var t = b.Where(x => x.PayrollActionType == PayrollActionType.Tax).Select(x => x.ActionAmount).ToList().Sum();
                    var loan = b.Where(x => x.PayrollActionType == PayrollActionType.Loan).Select(x => x.ActionAmount).ToList().Sum();
                    var oDiscount = b.Where(x => x.PayrollActionType == PayrollActionType.Deduction && x.ActionId != configuration.AFP && x.ActionId != configuration.LoanCooperative && x.ActionId != configuration.SFS)
                                    .Select(x => x.ActionAmount).ToList().Sum();

                    //Actualización abono de cooperativa
                    var deductCoop = b.Where(x => x.ActionId == configuration.DeductionCooperative).Select(x => x.ActionAmount).ToList().Sum();

                    report.Add(new ReportPayrollPaymentResponse
                    {
                        EmployeeName = item.EmployeeName,
                        StartWorkDate = item.StartWorkDate,
                        Document = item.Document,
                        Department = item.Department,
                        PayrollName = item.PayrollName,
                        Period = item.Period,
                        PaymentDate = item.PaymentDate,
                        PayMethod = item.PayMethod,
                        PositionName = string.IsNullOrEmpty(item.PositionName) ? "N/A" : item.PositionName,
                        Salary = s,
                        ExtraHour = eh,
                        Commision = com,
                        OtherEarning = oEarning,
                        BankAccount= item.BankAccount,
                        AFP = afp,
                        SFS = sfs,
                        DeductionCooperative = deductCoop,

                        LoanCooperative = loanCoop,
                        Loan = Math.Abs(loan - loanCoop),

                        ISR = t,
                        OtherDiscount = oDiscount,

                        Total = item.Total,

                        EmployeeEmail = string.IsNullOrEmpty(item.EmployeeEmail) ? "N/A" : item.EmployeeEmail
                    });
                }
            }


            return new PagedResponse<IEnumerable<ReportPayrollPaymentResponse>>(report, 0, 0);

        }



        //Reporte de resumen de pago de nómina
        public async Task<PagedResponse<IEnumerable<ReportResumePayrollResponse>>> ResumePaymentReport(string payrollProcessId, string departmentid)
        {
            Department department;
            string departmentName = "Todos";

            List<ReportResumePayrollResponse> report = new List<ReportResumePayrollResponse>();
            //Loan, ExtraHours, Tax
            var configuration = await _dbContext.ReportsConfig.FirstOrDefaultAsync();

            if (configuration == null)
            {
                configuration = new ReportConfig();
            };

            if (string.IsNullOrEmpty(departmentid))
            {
                departmentid = "-";
            }
            else
            {
                department = await _dbContext.Departments.Where(x => x.DepartmentId == departmentid)
                                             .FirstOrDefaultAsync();

                departmentName = department.Name;
            }

            var a = await _dbContext.PayrollsProcess
                .Join(_dbContext.PayrollProcessDetails,
                    pp => pp.PayrollProcessId,
                    ppd => ppd.PayrollProcessId,
                    (pp, ppd) => new { Pp = pp, Ppd = ppd })
                .Where(x => x.Pp.PayrollProcessId == payrollProcessId
                        && x.Ppd.DepartmentId.Contains(departmentid)
                        && (x.Pp.PayrollProcessStatus == PayrollProcessStatus.Paid
                        || x.Pp.PayrollProcessStatus == PayrollProcessStatus.Closed
                        || x.Pp.PayrollProcessStatus == PayrollProcessStatus.Calculated))
                .GroupBy(x => new
                {
                    x.Ppd.PayMethod,
                    x.Pp.PayrollId,
                    x.Pp.PeriodEndDate,
                    x.Pp.PeriodStartDate,
                    x.Pp.PaymentDate,
                    x.Pp.EmployeeQuantity,
                    x.Pp.TotalAmountToPay,
                    x.Pp.ProjId
                })
                .Select(x => new
                {
                    PayrollName = x.Key.PayrollId,
                    Period = $"{x.Key.PeriodStartDate.ToShortDateString()} - {x.Key.PeriodEndDate.ToShortDateString()}",
                    PaymentDate = x.Key.PaymentDate,
                    TotalEmployee = x.Key.EmployeeQuantity,
                    Total = x.Key.TotalAmountToPay,
                    Project = x.Key.ProjId,
                    QtyMethodPay = x.Count(),
                    PayMethod = x.Key.PayMethod
                }).ToListAsync();

            if (a != null)
            {
                var actions = await _dbContext.PayrollProcessActions.Where(x => x.PayrollProcessId == payrollProcessId).ToListAsync();

                var b = actions;

                var s = b.Where(x => x.ActionId == configuration.Salary).Select(x => x.ActionAmount).ToList().Sum();
                var com = b.Where(x => x.ActionId == configuration.Comission).Select(x => x.ActionAmount).ToList().Sum();
                var eh = b.Where(x => x.PayrollActionType == PayrollActionType.ExtraHours).Select(x => x.ActionAmount).ToList().Sum();
                var oEarning = b.Where(x => x.PayrollActionType == PayrollActionType.Earning && x.ActionId != configuration.Salary && x.ActionId != configuration.Comission)
                                   .Select(x => x.ActionAmount).ToList().Sum();

                var afp = b.Where(x => x.ActionId == configuration.AFP && x.PayrollActionType == PayrollActionType.Deduction).Select(x => x.ActionAmount).ToList().Sum();
                var sfs = b.Where(x => x.ActionId == configuration.SFS && x.PayrollActionType == PayrollActionType.Deduction).Select(x => x.ActionAmount).ToList().Sum();
                var loanCoop = b.Where(x => x.ActionId == configuration.LoanCooperative).Select(x => x.ActionAmount).ToList().Sum();
                var t = b.Where(x => x.PayrollActionType == PayrollActionType.Tax).Select(x => x.ActionAmount).ToList().Sum();
                var loan = b.Where(x => x.PayrollActionType == PayrollActionType.Loan).Select(x => x.ActionAmount).ToList().Sum();
                var oDiscount = b.Where(x => x.PayrollActionType == PayrollActionType.Deduction && x.ActionId != configuration.AFP && x.ActionId != configuration.LoanCooperative && x.ActionId != configuration.SFS)
                                   .Select(x => x.ActionAmount).ToList().Sum();

                var deductCoop = b.Where(x => x.ActionId == configuration.DeductionCooperative).Select(x => x.ActionAmount).ToList().Sum();

                var firstValue = a.First();
                ReportResumePayrollResponse resumen = new ReportResumePayrollResponse()
                {
                    PayrollName = firstValue.PayrollName,
                    Period = firstValue.Period,
                    PaymentDate = firstValue.PaymentDate,

                    Salary = s,
                    ExtraHour = eh,
                    Commision = com,
                    OtherEarning = oEarning,

                    AFP = afp,
                    SFS = sfs,

                    DeductionCooperative = deductCoop,

                    LoanCooperative = loanCoop,
                    Loan =  Math.Abs(loan - loanCoop),
                    ISR = t,
                    OtherDiscount = oDiscount,

                    Total = firstValue.Total,
                    TotalEmployee = firstValue.TotalEmployee,
                    Project = firstValue.Project,

                    DepartmentName = departmentName
                };

                List<PayMethodTotal> payMethodTotals = new List<PayMethodTotal>();
                foreach (var item in a)
                {
                    payMethodTotals.Add(new PayMethodTotal()
                    {
                        Total = item.QtyMethodPay,
                        PayMethod = item.PayMethod
                    });
                }

                resumen.PayMethodTotal = payMethodTotals;

                report.Add(resumen);
            }

            return new PagedResponse<IEnumerable<ReportResumePayrollResponse>>(report, 0, 0);

        }


        //Reporte de procesos de nómina
        public async Task<Response<ReportPayrollProcessResponse>> PayrollProcessReport(string payrollProcessId, string departmentId)
        {
            ReportPayrollProcessResponse report = null;

            //Loan, ExtraHours, Tax
            var configuration = await _dbContext.ReportsConfig.FirstOrDefaultAsync();

            if (configuration == null)
            {
                configuration = new ReportConfig();
            };

            if (string.IsNullOrEmpty(departmentId))
                departmentId = "-";

            var a = await _dbContext.PayrollsProcess
            .Join(_dbContext.PayrollProcessDetails,
                pp => pp.PayrollProcessId,
                ppd => ppd.PayrollProcessId,
                (pp, ppd) => new { Pp = pp, Ppd = ppd })
            .Where(x => x.Pp.PayrollProcessId == payrollProcessId
                    && x.Ppd.DepartmentId.Contains(departmentId)
                    && (x.Pp.PayrollProcessStatus == PayrollProcessStatus.Paid
                    || x.Pp.PayrollProcessStatus == PayrollProcessStatus.Closed
                    || x.Pp.PayrollProcessStatus == PayrollProcessStatus.Calculated))
            .Select(x => new
            {
                EmployeeId = x.Ppd.EmployeeId,
                EmployeeName = x.Ppd.EmployeeName,
                PayrollName = x.Pp.PayrollId,
                Period = $"{x.Pp.PeriodStartDate.ToShortDateString()} - {x.Pp.PeriodEndDate.ToShortDateString()}",
                PaymentDate = x.Pp.PaymentDate,
                Total = x.Pp.TotalAmountToPay,
                ProjId = x.Pp.ProjId,
                TotalEmployee = x.Pp.EmployeeQuantity,
                DepartmentId = x.Ppd.DepartmentId,
                DepartmentName = x.Ppd.DepartmentName
            }).ToListAsync();

            if (a != null)
            {

                List<ReportPayrollEmployeeInfo> details;
                List<GroupReportPayrollEmployeeInfo> detailsGroup = new List<GroupReportPayrollEmployeeInfo>();

                var actions = await _dbContext.PayrollProcessActions.Where(x => x.PayrollProcessId == payrollProcessId).ToListAsync();

                var departmentGroup = a.GroupBy(x => new { x.DepartmentId, x.DepartmentName }).Select(x => new
                {
                    DepartmentId = x.Key.DepartmentId,
                    DepartmentName = x.Key.DepartmentName,
                    Details = x.Select(x => x)
                }).ToList();

                foreach (var group in departmentGroup)
                {
                    details = new List<ReportPayrollEmployeeInfo>();
                    foreach (var item in group.Details)
                    {
                        var b = actions.Where(x => x.EmployeeId == item.EmployeeId).ToList();

                        var s = b.Where(x => x.ActionId == configuration.Salary).Select(x => x.ActionAmount).ToList().Sum();
                        var com = b.Where(x => x.ActionId == configuration.Comission).Select(x => x.ActionAmount).ToList().Sum();
                        var eh = b.Where(x => x.PayrollActionType == PayrollActionType.ExtraHours).Select(x => x.ActionAmount).ToList().Sum();
                        var oEarning = b.Where(x => x.PayrollActionType == PayrollActionType.Earning && x.ActionId != configuration.Salary && x.ActionId != configuration.Comission)
                                        .Select(x => x.ActionAmount).ToList().Sum();


                        var afp = b.Where(x => x.ActionId == configuration.AFP && x.PayrollActionType == PayrollActionType.Deduction).Select(x => x.ActionAmount).ToList().Sum();
                        var sfs = b.Where(x => x.ActionId == configuration.SFS && x.PayrollActionType == PayrollActionType.Deduction).Select(x => x.ActionAmount).ToList().Sum();
                        var loanCoop = b.Where(x => x.ActionId == configuration.LoanCooperative).Select(x => x.ActionAmount).ToList().Sum();
                        var t = b.Where(x => x.PayrollActionType == PayrollActionType.Tax).Select(x => x.ActionAmount).ToList().Sum();
                        var loan = b.Where(x => x.PayrollActionType == PayrollActionType.Loan).Select(x => x.ActionAmount).ToList().Sum();
                        var oDiscount = b.Where(x => x.PayrollActionType == PayrollActionType.Deduction && x.ActionId != configuration.AFP && x.ActionId != configuration.LoanCooperative && x.ActionId != configuration.SFS)
                                        .Select(x => x.ActionAmount).ToList().Sum();

                        var deductCoop = b.Where(x => x.ActionId == configuration.DeductionCooperative).Select(x => x.ActionAmount).ToList().Sum();

                        details.Add(new ReportPayrollEmployeeInfo
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeName = item.EmployeeName,

                            Salary = s,
                            ExtraHour = eh,
                            Commision = com,
                            OtherEarning = oEarning,
                            TotalEarning = s + eh + com + oEarning,

                            AFP = afp,
                            SFS = sfs,

                            //Actualización abono cooperativa
                            DeductionCooperative = deductCoop,

                            LoanCooperative = loanCoop,
                            Loan = Math.Abs(loan - loanCoop),
                            ISR = t,
                            OtherDiscount = oDiscount,
                            TotalDiscount = afp + sfs + loanCoop + Math.Abs(loan - loanCoop) + t + oDiscount,
                            TotalAmount = (s + eh + com + oEarning) - (afp + sfs + loanCoop + Math.Abs(loan - loanCoop) + t + oDiscount)
                        });
                    }

                    detailsGroup.Add(new GroupReportPayrollEmployeeInfo()
                    {
                        DepartmentName = group.DepartmentName,
                        Details = details,

                        Salary = details.Sum(x => x.Salary),
                        ExtraHour = details.Sum(x => x.ExtraHour),
                        Commision = details.Sum(x => x.Commision),
                        OtherEarning = details.Sum(x => x.OtherEarning),
                        TotalEarning = details.Sum(x => x.TotalEarning),

                        AFP = details.Sum(x => x.AFP),
                        SFS = details.Sum(x => x.SFS),

                        DeductionCooperative = details.Sum(x => x.DeductionCooperative),

                        LoanCooperative = details.Sum(x => x.LoanCooperative),
                        Loan = details.Sum(x => x.Loan),
                        ISR = details.Sum(x => x.ISR),
                        OtherDiscount = details.Sum(x => x.OtherDiscount),
                        TotalDiscount = details.Sum(x => x.TotalDiscount),
                        TotalAmount = details.Sum(x => x.TotalAmount),
                    });
                }

                var firstValue = a.First();

                report = new ReportPayrollProcessResponse()
                {
                    PayrollProcessId = payrollProcessId,
                    PayrollName = firstValue.PayrollName,
                    Period = firstValue.Period,
                    PaymentDate = firstValue.PaymentDate,
                    Total = firstValue.Total,
                    ProjId = firstValue.ProjId,
                    TotalEmployee = firstValue.TotalEmployee,

                    Salary = detailsGroup.Sum(x => x.Salary),
                    ExtraHour = detailsGroup.Sum(x => x.ExtraHour),
                    Commision = detailsGroup.Sum(x => x.Commision),
                    OtherEarning = detailsGroup.Sum(x => x.OtherEarning),
                    TotalEarning = detailsGroup.Sum(x => x.TotalEarning),

                    //Actualizacion abono cooperativa
                    DeductionCooperative = detailsGroup.Sum(x => x.DeductionCooperative),

                    AFP = detailsGroup.Sum(x => x.AFP),
                    SFS = detailsGroup.Sum(x => x.SFS),
                    LoanCooperative = detailsGroup.Sum(x => x.LoanCooperative),
                    Loan = detailsGroup.Sum(x => x.Loan),
                    ISR = detailsGroup.Sum(x => x.ISR),
                    OtherDiscount = detailsGroup.Sum(x => x.OtherDiscount),
                    TotalDiscount = detailsGroup.Sum(x => x.TotalDiscount)
                };

                report.DepartmentGroups = detailsGroup;
            }

            return new Response<ReportPayrollProcessResponse>(report);
        }



        //Reporte global de empleados
        public async Task<Response<ReportEmployeeResponse>> EmployeeReport(string departmentId)
        {
            ReportEmployeeResponse report = null;
            List<ReportEmployeeInfo> employeeinfo;
            List<GroupReportEmployeeInfo> groupemployeeinfo =  new List<GroupReportEmployeeInfo>();

            if (string.IsNullOrEmpty(departmentId))
                departmentId = "-";

            var a = await _dbContext.Employees
                .Join(_dbContext.EmployeePositions,
                    e => e.EmployeeId,
                    ep => ep.EmployeeId,
                    (e, ep) => new { E = e, Ep = ep })
                .Join(_dbContext.Positions,
                    ep => ep.Ep.PositionId,
                    p => p.PositionId,
                    (ep, p) => new { Ep = ep, P = p })
            .Where(x => x.Ep.E.WorkStatus == WorkStatus.Employ
                    && x.Ep.Ep.EmployeePositionStatus == true
                    && x.P.DepartmentId.Contains(departmentId))
            .Select(x => new
            {
                x.Ep.E.EmployeeId,
                EmployeeName = $"{x.Ep.E.Name} {x.Ep.E.LastName}",
                x.Ep.E.BirthDate,
                Gender = x.Ep.E.Gender.ToString(),
                x.Ep.E.Age,
                x.Ep.E.MaritalStatus,
                x.Ep.E.NSS,
                x.Ep.E.ARS,
                x.Ep.E.AFP,
                Country = _dbContext.Countries.Where(y => y.CountryId == x.Ep.E.Country).FirstOrDefault().NationalityName,
                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.Ep.E.EmployeeId && y.IsPrincipal == true).FirstOrDefault(),
                x.Ep.E.StartWorkDate,
                x.Ep.E.EndWorkDate,
                PositionName = x.P.PositionName,
                Department = _dbContext.Departments.Where(y => y.DepartmentId == x.P.DepartmentId).FirstOrDefault()
            }).ToListAsync();

            var departmentGroup = a.GroupBy(x => new { x.Department.DepartmentId, x.Department.Name }).Select(x => new
            {
                DepartmentId = x.Key.DepartmentId,
                DepartmentName = x.Key.Name,
                Details = x.Select(x => x)
            }).ToList();

            foreach (var group in departmentGroup)
            {
                employeeinfo = new List<ReportEmployeeInfo>();
                foreach (var item in group.Details)
                {
                    employeeinfo.Add(new ReportEmployeeInfo
                    {
                        EmployeeId = item.EmployeeId,
                        EmployeeName = item.EmployeeName,
                        BirthDate = item.BirthDate,
                        Gender = item.Gender,
                        Age = item.Age,
                        NSS = item.NSS,
                        ARS = item.ARS,
                        AFP = item.AFP,
                        Country = item.Country,
                        StartWorkDate = item.StartWorkDate,
                        EndWorkDate = item.EndWorkDate,
                        PositionName = item.PositionName,
                        DocumentNumber = item.Document == null?"":item.Document.DocumentNumber
                    });
                }

                groupemployeeinfo.Add(new GroupReportEmployeeInfo()
                {
                    DepartmentName = group.DepartmentName,
                    EmployeeInfo = employeeinfo
                });
            }

            report = new ReportEmployeeResponse()
            {
                TotalEmployee = a.Count(),
                GroupEmployeeInfo = groupemployeeinfo
            };

            return new Response<ReportEmployeeResponse>(report);
        }

        //Reporte de dgt4
        public async Task<Response<DGT4Response>> DGT4Report(int year, int month)
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
                                                BirthDate = x.Joins.Join.E.BirthDate.ToString("dd-MM-yyyy"),
                                                Gender = x.Joins.Join.E.Gender.ToString().Substring(0, 1),
                                                AdmissionDate = x.Joins.Join.E.StartWorkDate.ToString("dd-MM-yyyy"),
                                                x.Joins.Join.E.OccupationId,
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
                x.OccupationId,
                x.NoveltyType,
                x.Document
            })
            .Select(x => new
            {
                Employee = x.Key,
                Salary = x.Sum(x => x.Salary)
            }).ToList();

            List<DGT4Detail> dgt4details = new List<DGT4Detail>();

            foreach (var item in a)
            {
                dgt4details.Add(
                    new DGT4Detail()
                    {
                        DocumentNumber = item.Employee.Document == null ? "" : item.Employee.Document.DocumentNumber,
                        EmployeeName = item.Employee.Name,
                        LastName = item.Employee.LastName,
                        BirthDate = item.Employee.BirthDate,
                        Gender = item.Employee.Gender,
                        AdmissionDate = item.Employee.AdmissionDate,
                        Salary = item.Salary.ToString(),
                        NoveltyType = item.Employee.NoveltyType
                    }
                );
            }

            return new Response<DGT4Response>(new DGT4Response()
            {
                RNC = company.Identification,
                Period = period,

                Details = dgt4details,

                RegisterQty = (2 + dgt4details.Count()).ToString()
            });
        }

        //Reporte de dgt2
        public async Task<Response<DGT2Response>> DGT2Report(int year, int month)
        {
            Func<Employee, string> EmployeeName = (e) =>
            {
                return e != null ? $"{e.Name} {e.LastName}" : "";
            };

            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            List<int> days = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                days.Add(i);
            }

            var hours = await _dbContext.EmployeeExtraHours.Where(x => x.WorkedDay >= startDate && x.WorkedDay <= endDate
                                                                  && x.StatusExtraHour == StatusExtraHour.Pagada)
                                                           .Select(x => new
                                                           {
                                                               x.Quantity,
                                                               x.Amount,
                                                               x.EmployeeId,
                                                               x.Indice,
                                                               EmployeeName = EmployeeName(_dbContext.Employees.Where(y => y.EmployeeId == x.EmployeeId).FirstOrDefault())
                                                           })
                                                           .ToListAsync();

            var employees = hours.GroupBy(x => x.EmployeeId)
                .Select(x => x.Key).ToList();


            string hours_days = string.Empty;

            List<DGT2Detail> dgt2details = new List<DGT2Detail>();

            foreach (var item in employees)
            {
                var extrahour = hours.Where(y => y.EmployeeId == item).ToList();

                dgt2details.Add(
                    new DGT2Detail()
                    {
                        EmployeeName = extrahour.First().EmployeeName,
                        QtyExtraHour = extrahour.Sum(x => x.Quantity),
                        TotalAmountExtraHour = extrahour.Sum(x => x.Amount),
                    }
                );
            }

            return new Response<DGT2Response>(new DGT2Response()
            {
                RNC = company.Identification,
                Period = period,

                Details = dgt2details,

                RegisterQty = (2 + dgt2details.Count()).ToString()
            });
        }

        //Reporte de dgt3
        public async Task<Response<DGT3Response>> DGT3Report(int year, int month)
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
                                                BirthDate = x.Join.E.BirthDate.ToString("dd-MM-yyyy"),
                                                Gender = x.Join.E.Gender.ToString().Substring(0, 1),
                                                AdmissionDate = x.Join.E.StartWorkDate.ToString("dd-MM-yyyy"),
                                                x.Join.E.OccupationId,
                                                x.Join.Eec.IndexEarningMonthly
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
                x.OccupationId,
                x.Document
            })
            .Select(x => new
            {
                employee = x.Key,
                salary = x.Sum(x => x.IndexEarningMonthly)
            }).ToList();

            List<DGT3Detail> dgt3details = new List<DGT3Detail>();

            foreach (var item in a)
            {
                dgt3details.Add(
                    new DGT3Detail()
                    {
                        DocumentNumber = item.employee.Document == null ? "" : item.employee.Document.DocumentNumber,
                        EmployeeName = item.employee.Name,
                        LastName = item.employee.LastName,
                        BirthDate = item.employee.BirthDate,
                        Gender = item.employee.Gender,
                        AdmissionDate = item.employee.AdmissionDate,
                        Salary = item.salary.ToString()
                    }
                );
            }

            return new Response<DGT3Response>(new DGT3Response()
            {
                RNC = company.Identification,
                Period = period,

                Details = dgt3details,

                RegisterQty = (2 + dgt3details.Count()).ToString()
            });
        }

        //Reporte de dgt5
        public async Task<Response<DGT5Response>> DGT5Report(int year, int month)
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
                                            .Join(_dbContext.EmployeeDocuments,
                                                join => join.E.EmployeeId,
                                                ed => ed.EmployeeId,
                                                (join, ed) => new { Join = join, Ed = ed })
                                            .Where(x => x.Join.Eh.RegisterDate >= startDate && x.Join.Eh.RegisterDate <= endDate
                                                   && x.Join.E.EmployeeType == EmployeeType.Contractor
                                                   && x.Join.Eh.Type == "NI")
                                            .Select(x => new
                                            {
                                                x.Join.E.Name,
                                                x.Join.E.LastName,
                                                x.Ed.DocumentNumber,
                                                BirthDate = x.Join.E.BirthDate.ToString("ddMMyyyy"),
                                                Gender = x.Join.E.Gender.ToString().Substring(0, 1),
                                                AdmissionDate = x.Join.E.StartWorkDate.ToString("ddMMyyyy"),
                                                x.Join.E.OccupationId,
                                                x.Join.Eh.Type
                                            })
                                            .ToListAsync();

            List<DGT5Detail> dgt5details = new List<DGT5Detail>();

            foreach (var item in employees)
            {
                dgt5details.Add(
                    new DGT5Detail()
                    {
                        DocumentNumber = item.DocumentNumber,
                        EmployeeName = item.Name,
                        LastName = item.LastName,
                        BirthDate = item.BirthDate,
                        Gender = item.Gender,
                        AdmissionDate = item.AdmissionDate,
                        NoveltyType = item.Type
                    }
                );
            }

            return new Response<DGT5Response>(new DGT5Response()
            {
                RNC = company.Identification,
                Period = period,

                Details = dgt5details,

                RegisterQty = (2 + dgt5details.Count()).ToString()
            });
        }

        //Reporte de dgt9
        public async Task<Response<DGT9Response>> DGT9Report(int year, int month)
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
                                            .Where(x => x.Eh.IsUseDGT == true
                                                   && x.E.WorkStatus == WorkStatus.Employ
                                                   && x.E.EmployeeType == EmployeeType.Employee
                                                   && x.Eh.RegisterDate >= startDate && x.Eh.RegisterDate <= endDate
                                                   && x.Eh.Type == "NO")
                                            .Select(x => new
                                            {
                                                x.E.Name,
                                                x.E.LastName,                                                
                                                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.E.EmployeeId).Select(
                                                                                              y => new { y.DocumentType, y.DocumentNumber }).FirstOrDefault(),
                                                x.E.LocationId,
                                                Contact = _dbContext.EmployeeContactsInf.Where(y => y.EmployeeId == x.E.EmployeeId && y.IsPrincipal == true && y.ContactType == ContactType.Phone).Select(
                                                                                              y => new { y.NumberAddress }).FirstOrDefault(),

                                                Address = _dbContext.EmployeesAddress.Where(y => y.EmployeeId == x.E.EmployeeId && y.IsPrincipal == true).FirstOrDefault()
                                            })
                                            .ToListAsync();

            List<DGT9Detail> dgt9details = new List<DGT9Detail>();

            foreach (var item in employees)
            {
                dgt9details.Add(
                    new DGT9Detail()
                    {
                        DocumentNumber = item.Document == null? "NA" : item.Document.DocumentNumber,
                        DocumentType   = item.Document == null ? "NA" : TypeDocument(item.Document.DocumentType),
                        EmployeeName   = item.Name,
                        LastName       = item.LastName,
                        PhoneNumber    = item.Contact == null? "" : item.Contact.NumberAddress,
                        LocationId     = item.LocationId,
                        Province       = item.Address == null? "" : item.Address.ProvinceName,
                        Address        = item.Address == null ? "" : CreateAddressFormat(item.Address)
                    }
                );
            }

            return new Response<DGT9Response>(new DGT9Response()
            {
                RNC = company.Identification,
                Period = period,

                Details = dgt9details,

                RegisterQty = (2 + dgt9details.Count()).ToString()
            });
        }

        //Reporte de dgt12
        public async Task<Response<DGT12Response>> DGT12Report(int year, int month)
        {
            Func<DocumentType, string> TypeDocument = (e) =>
            {
                switch (e)
                {
                    case DocumentType.IdentificationCard:
                        return "C";
                    case DocumentType.Passport:
                        return "P";
                    default:
                        return "X";
                }
            };

            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}{year}" : $"{month}{year}";

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month + 1, 1).AddDays(-1);

            var employees = await _dbContext.Employees
                                            .Join(_dbContext.EmployeeHistories,
                                                e => e.EmployeeId,
                                                eh => eh.EmployeeId,
                                                (e, eh) => new { E = e, Eh = eh })
                                            .Where(x => x.E.EmployeeStatus == true
                                                   && x.Eh.Type == "NS"
                                                   && x.Eh.RegisterDate >= startDate && x.Eh.RegisterDate <= endDate)
                                            .Select(x => new
                                            {
                                                x.E.Name,
                                                x.E.LastName,
                                                Document = _dbContext.EmployeeDocuments.Where(y => y.EmployeeId == x.E.EmployeeId).Select(
                                                                                              y => new { y.DocumentType, y.DocumentNumber }).FirstOrDefault(),
                                                x.E.LocationId,
                                                x.Eh.RegisterDate
                                            })
                                            .ToListAsync();

            List<DGT12Detail> dgt12details = new List<DGT12Detail>();

            foreach (var item in employees)
            {
                dgt12details.Add(
                    new DGT12Detail()
                    {
                        DocumentNumber = item.Document.DocumentNumber,
                        DocumentType = TypeDocument(item.Document.DocumentType),
                        EmployeeName = item.Name,
                        LastName = item.LastName,
                        DepartureDate = item.RegisterDate.ToString("dd-MM-yyyy"),
                        LocationId = item.LocationId
                    }
                );
            }

            return new Response<DGT12Response>(new DGT12Response()
            {
                RNC = company.Identification,
                Month = month.ToString(),
                Year = year.ToString(),

                Details = dgt12details,

                RegisterQty = (2 + dgt12details.Count()).ToString()
            });
        }

        //Reporte tss
        public async Task<Response<ReportTSSFileResponse>> TSSReport(int year, int month, string payrollid, string typetss)
        {
            var company = await _dbContext.Companies.Where(x => x.CompanyId == _CurrentUserInformation.Company).FirstOrDefaultAsync();
            var payroll = await _dbContext.Payrolls.Where(x => x.PayrollId == payrollid).FirstOrDefaultAsync();

            string period = month < 10 ? $"0{month}-{year}" : $"{month}-{year}";

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
                                                Gender = x.E.Gender.ToString().Substring(0,1),
                                                BirthDate = x.E.BirthDate.ToString("dd/MM/yyyy"),
                                                x.Join.Ppd.TotalAmount,
                                                x.Join.Ppd.TotalTaxAmount,
                                                x.E.EmployeeId
                                            })
                                            .ToListAsync();

            List<TSSFile> tssdetails = new List<TSSFile>();

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
                    new TSSFile()
                    {
                        DocumentType = TypeDocument(item.DocumentType),
                        DocumentNumber = item.DocumentNumber,
                        EmployeeName = item.Name,
                        EmployeeLastName = item.LastName,
                        BirthDate = item.BirthDate,
                        Gender = item.Gender,
                        Salary = item.TotalAmount,
                        Salary_ISR = item.TotalTaxAmount
                    }
                );
            }

            return new Response<ReportTSSFileResponse>(new ReportTSSFileResponse()
            {
                RNC = company.Identification,
                Period = period,
                PayrollName = payroll.Name,
                Process = typetss,
                Details = tssdetails
            });
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

        private string CreateAddressFormat(EmployeeAddress employeeAddress)
        {
            return $"{employeeAddress.Street} {employeeAddress.Home} " +
                $"{employeeAddress.Sector} {employeeAddress.City} {employeeAddress.ProvinceName} {employeeAddress.CountryId}";
        }
    }
}
