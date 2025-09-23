using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.DashboardInfo;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.DashboardInfo
{
    public interface IDashboardInfoQueryHandler
    {
        public Task<Response<object>> GetCount();
        public Task<Response<object>> GetGraphics(int _year, string _payrollid);
    }

    public class DashboardInfoQueryHandler : IDashboardInfoQueryHandler
    {
        private readonly IApplicationDbContext dbContext;
        private int year;
        private string payrollid;

        public DashboardInfoQueryHandler(IApplicationDbContext _dbcontext)
        {
            dbContext = _dbcontext;
        }

        public async Task<Response<object>> GetCount()
        {
            var employees = await dbContext.Employees
                .Where(x => x.EmployeeStatus == true)
                .GroupBy(x => x.EmployeeStatus)
                .Select(x => x.Count()).FirstOrDefaultAsync();

            var departments = await dbContext.Departments
                .Where(x => x.DepartamentStatus == true)
                .GroupBy(x => x.DepartamentStatus)
                .Select(x => x.Count()).FirstOrDefaultAsync();

            var positions = await dbContext.Positions
                .Where(x => x.PositionStatus == true)
                .GroupBy(x => x.PositionStatus)
                .Select(x => x.Count()).FirstOrDefaultAsync();

            var courses = await dbContext.Courses.CountAsync();

            DashboardCardInfo cardInfo = new DashboardCardInfo();
            cardInfo.Employees =  employees;
            cardInfo.Departments = departments;
            cardInfo.Positions = positions;
            cardInfo.Courses = courses;

            return new Response<object>(cardInfo);            
        }

        public async Task<Response<object>> GetGraphics(int _year, string _payrollid)
        {
            year = _year;
            payrollid = _payrollid;

            var employeebydepartment = await EmployeeByDepartment();
            var dtbutionctbutionbyyear = await DtbutionCtbutionByYear();
            var actionsamount = await ActionsAmount();
            var trimestralpayrollamount = await PayrollAmountTrimestral();

            return new Response<object>(new DashboardGraphicsInfo() 
            { 
                EmployeeByDepartments = employeebydepartment,
                DtbutionCtbutionByYear = dtbutionctbutionbyyear,
                AmountByAction = actionsamount,
                TrimestralPayrollAmount = trimestralpayrollamount
            });
        }






        public async Task<EmployeeByDepartments> EmployeeByDepartment()
        {
            List<string> key = new List<string>();
            List<int> value = new List<int>();

            var dptGraphicsData = await dbContext.EmployeePositions
                .Join(dbContext.Positions,
                    ep => ep.PositionId,
                    p => p.PositionId,
                    (ep, p) => new { Ep = ep, P = p })
                .Join(dbContext.Departments,
                    epp => epp.P.DepartmentId,
                    d => d.DepartmentId,
                    (epp, d) => new { Epp = epp, D = d })
                .Where(x => x.Epp.Ep.EmployeePositionStatus == true)
                .GroupBy(x => x.D.Name)
                .Select(x => new
                {
                    Departments = x.Key,
                    Quantity = x.Count()
                }).ToListAsync();

            foreach (var item in dptGraphicsData)
            {
                key.Add(item.Departments);
                value.Add(item.Quantity);
            }

            return new EmployeeByDepartments() { Keys = key, Values = value };
        }

        public async Task<DeductionsContributionsByYear> DtbutionCtbutionByYear()
        {
            string[] months = new string[]
            {
                "None",
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            };

            List<string> key = new List<string>();
            List<decimal> Ctbutionvalue = new List<decimal>();
            List<decimal> Dtbutionvalue = new List<decimal>();

            var graphicsData = await dbContext.PayrollProcessActions
                .Join(dbContext.PayrollsProcess,
                    ppa => ppa.PayrollProcessId,
                    pp => pp.PayrollProcessId,
                    (ppa, pp) => new { Ppa = ppa, Pp = pp })
                .Where(x => (x.Ppa.PayrollActionType == Domain.Enums.PayrollActionType.Deduction 
                        || x.Ppa.PayrollActionType == Domain.Enums.PayrollActionType.Contribution)
                        && x.Pp.PeriodEndDate.Year == year
                        && x.Pp.PayrollProcessStatus != PayrollProcessStatus.Canceled
                        && x.Pp.PayrollId == payrollid)
                .GroupBy(x => new { x.Pp.PeriodEndDate.Month, x.Ppa.PayrollActionType })
                .Select(x => new
                {
                    Month = x.Key.Month,
                    PayrollActionType = x.Key.PayrollActionType,
                    Amount = x.Sum(x => x.Ppa.ActionAmount)
                }).ToListAsync();

            foreach (var item in graphicsData.GroupBy(x => x.Month).ToList())
            {
                key.Add(months[item.Key]);

                var a = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType == PayrollActionType.Contribution)
                    .FirstOrDefault();

                Ctbutionvalue.Add(a == null?0:a.Amount);

                var b = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType == PayrollActionType.Deduction)
                    .FirstOrDefault();

                Dtbutionvalue.Add(b == null ? 0 : b.Amount);
            }

            //for (int i = 1; i < 9; i++)
            //{
            //    key.Add("Otro mes");
            //    Dtbutionvalue.Add(25000);
            //    Ctbutionvalue.Add(25000);
            //}

            return new DeductionsContributionsByYear() { Keys = key, DtbutionValues = Dtbutionvalue, CtbutionValues = Ctbutionvalue };
        }

        public async Task<AmountByAction> ActionsAmount()
        {
            List<string> key = new List<string>();
            List<decimal> Value = new List<decimal>();

            var graphicsData = await dbContext.PayrollProcessActions
                .Join(dbContext.PayrollsProcess,
                    ppa => ppa.PayrollProcessId,
                    pp => pp.PayrollProcessId,
                    (ppa, pp) => new { Ppa = ppa, Pp = pp })
                .Where( x=> x.Pp.PeriodEndDate.Year == year && x.Pp.PayrollProcessStatus != PayrollProcessStatus.Canceled
                        && x.Pp.PayrollId == payrollid)
                .GroupBy(x => x.Ppa.ActionName)
                .Select(x => new
                {
                    ActionName = x.Key,
                    Amount = x.Sum(x => x.Ppa.ActionAmount)
                }).ToListAsync();

            foreach (var item in graphicsData)
            {
                key.Add(item.ActionName);
                Value.Add(item.Amount);
            }

            return new AmountByAction() { Keys = key, Values = Value};
        }

        public async Task<TrimestralPayrollAmount> PayrollAmountTrimestral()
        {
            int groupmonth = 0;

            string[] trimonth = new string[]
            {
                "Primer trimestre",
                "Segundo trimestre",
                "Tercer trimestre",
                "Cuarto trimestre",
            };

            List<string> key = new List<string>();
            List<decimal> FirtBar = new List<decimal>();
            List<decimal> SecondBar = new List<decimal>();
            List<decimal> ThirtBar = new List<decimal>();

            var graphicsData = await dbContext.PayrollProcessActions
                .Join(dbContext.PayrollsProcess,
                    ppa => ppa.PayrollProcessId,
                    pp => pp.PayrollProcessId,
                    (ppa, pp) => new { Ppa = ppa, Pp = pp })
                .Where(x => x.Ppa.PayrollActionType != Domain.Enums.PayrollActionType.Contribution
                       && x.Pp.PeriodEndDate.Year == year
                       && x.Pp.PayrollProcessStatus != PayrollProcessStatus.Canceled
                       && x.Pp.PayrollId == payrollid)
                .GroupBy(x => new { x.Pp.PeriodEndDate.Month, x.Ppa.PayrollActionType })
                .Select(x => new
                {
                    Month = x.Key.Month,
                    Amount = x.Sum(x => x.Ppa.ActionAmount),
                    PayrollActionType = x.Key.PayrollActionType
                }).ToListAsync();


            var a = graphicsData.OrderBy(x => x.Month).GroupBy(x => x.Month).ToList();
            int count = 1;
            decimal total = 0;
            for (int i = 1; i <= 12; i++)
            {
                total = 0;
                var item = a.Where(x => x.Key == i).FirstOrDefault();
                if (item != null)
                {
                    if(count == 1)
                    {
                        var plus = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType == PayrollActionType.Earning)                                        
                                        .FirstOrDefault();

                        total += plus == null ? 0 : plus.Amount;

                        var minus = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType != PayrollActionType.Earning)
                                        .GroupBy(x => x.Month)
                                        .Select(x => new
                                        {
                                            Amount = x.Sum(x => x.Amount)
                                        })
                                        .FirstOrDefault();
                        total -= minus == null ? 0 : minus.Amount;


                        FirtBar.Add(total);
                    }

                    if (count == 2)
                    {
                        var plus = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType == PayrollActionType.Earning)
                                        .FirstOrDefault();
                        total += plus == null ? 0 : plus.Amount;

                        var minus = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType != PayrollActionType.Earning)
                                        .GroupBy(x => x.Month)
                                        .Select(x => new
                                        {
                                            Amount = x.Sum(x => x.Amount)
                                        })
                                        .FirstOrDefault();
                        total -= minus == null ? 0 : minus.Amount;


                        SecondBar.Add(total);
                    }

                    if (count == 3)
                    {
                        var plus = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType == PayrollActionType.Earning)
                                        .FirstOrDefault();
                        total += plus == null ? 0 : plus.Amount;

                        var minus = graphicsData.Where(x => x.Month == item.Key && x.PayrollActionType != PayrollActionType.Earning)
                                            .GroupBy(x => x.Month)
                                            .Select(x => new
                                            {
                                                Amount = x.Sum(x => x.Amount)
                                            })
                                            .FirstOrDefault();
                        total -= minus == null ? 0 : minus.Amount;


                        ThirtBar.Add(total);
                    }
                }
                else
                {
                    if (count == 1) FirtBar.Add(0);
                    if (count == 2) SecondBar.Add(0);
                    if (count == 3) ThirtBar.Add(0);
                }

                count++;
                if (count > 3)
                {
                    count = 1;
                    key.Add(trimonth[groupmonth]);
                    groupmonth++;
                }
            }

            return new TrimestralPayrollAmount() { Keys = key, FirtBar = FirtBar, SecondBar = SecondBar, ThirtBar = ThirtBar };
        }
    }
}
