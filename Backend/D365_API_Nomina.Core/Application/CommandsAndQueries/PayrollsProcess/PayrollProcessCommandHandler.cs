using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Helpers;
using D365_API_Nomina.Core.Application.Common.Model.PayrollsProcess;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.PayrollsProcess
{
    public interface IPayrollProcessCommandHandler :
        ICreateCommandHandler<PayrollProcessRequest>,
        IDeleteCommandHandler,
        IUpdateCommandHandler<PayrollProcessRequest>
    {
        public Task<Response<object>> CalcProcessPayroll(string payrollprocessid);
        public Task<Response<object>> ProcessPayroll(string payrollprocessid);
        public Task<Response<object>> PayPayroll(string payrollprocessid);
        public Task<Response<object>> CancelPayroll(string payrollprocessid);
    }

    public class PayrollProcessCommandHandler : IPayrollProcessCommandHandler
    {
        private int cont;
        private decimal amountsalary; //Monto del salario
        private decimal EarningTssAmount; //Monto aplicable a tss
        private decimal EarningTssAndTaxAmount; //Monto aplicable a tss y a impuesto al mismo tiempo
        private decimal amounttaxsalary; //Monto del salario aplicable a impuestos

        private readonly IApplicationDbContext _dbContext;
        private List<PayrollProcessAction> payrollProcessActions;

        public PayrollProcessCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }


        public async Task<Response<object>> Create(PayrollProcessRequest model)
        {
            var payroll = await _dbContext.Payrolls.Where(x => x.PayrollId == model.PayrollId).FirstOrDefaultAsync();

            if (payroll == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La nómina seleccionada no existe" },
                    StatusHttp = 404
                };
            }

            var payrollprocess = await _dbContext.PayrollsProcess.Where(x => x.PayrollId == model.PayrollId
                                                                        && x.PayrollProcessStatus != PayrollProcessStatus.Paid
                                                                        && x.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                                                        ).FirstOrDefaultAsync();

            if (payrollprocess != null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"No se puede generar otra proceso de nómina con el id de nómina {model.PayrollId}, el mismo se encuentra en otro proceso que no se ha cerrado." },
                    StatusHttp = 404
                };
            }

            var paycycle = await _dbContext.PayCycles.OrderBy(x => x.PayCycleId)
                                                     .Where(x => x.PayrollId == model.PayrollId
                                                            && x.StatusPeriod == StatusPeriod.Open)
                                                     .FirstOrDefaultAsync();

            if (paycycle == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"Ya no hay períodos de pago disponibles para el id de nómina {model.PayrollId}" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<PayrollProcess>.OnBuild(model, new PayrollProcess());

            entity.PeriodStartDate = paycycle.PeriodStartDate;
            entity.PeriodEndDate = paycycle.PeriodEndDate;
            entity.IsPayCycleTax = paycycle.IsForTax;
            entity.PayCycleId = paycycle.PayCycleId;
            entity.IsRoyaltyPayroll = payroll.IsRoyaltyPayroll;

            //Actualización para los cálculos de deducciones
            entity.IsPayCycleTss = paycycle.IsForTss;
            //Actualización para los cálculos de deducciones
            
            //Actualización para los cálculos de salario por hora
            entity.IsForHourPayroll = payroll.IsForHourPayroll;
            //Actualización para los cálculos de salario por hora

            _dbContext.PayrollsProcess.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<bool>> Delete(List<string> ids)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.PayrollsProcess.Where(x => x.PayrollProcessId == item
                                                                          && (x.PayrollProcessStatus == PayrollProcessStatus.Created
                                                                          || x.PayrollProcessStatus == PayrollProcessStatus.Canceled))
                                                                   .FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe o no está en estado creada o cancelada - id {item}");

                    }

                    _dbContext.PayrollsProcess.Remove(response);
                    await _dbContext.SaveChangesAsync();
                }

                transaction.Commit();
                return new Response<bool>(true) { Message = "Registros elimandos con éxito" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new Response<bool>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<Response<object>> Update(string id, PayrollProcessRequest model)
        {
            var response = await _dbContext.PayrollsProcess.Where(x => x.PayrollProcessId == id
                                                                  && x.PayrollProcessStatus == PayrollProcessStatus.Created).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está en estado creado" },
                    StatusHttp = 404
                };
            }

            var payrollprocess = await _dbContext.PayrollsProcess.Where(x => x.PayrollId == model.PayrollId
                                                                        && x.PayrollProcessStatus != PayrollProcessStatus.Closed
                                                                        && x.PayrollProcessStatus != PayrollProcessStatus.Paid
                                                                        && x.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                                                        && x.PayrollProcessId != id).FirstOrDefaultAsync();

            if (payrollprocess != null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"No se puede generar otra proceso de nómina con el id de nómina {model.PayrollId}, el mismo se encuentra en otro proceso que no se ha cerrado." },
                    StatusHttp = 404
                };
            }

            var paycycle = await _dbContext.PayCycles.OrderBy(x => x.PayCycleId)
                                                     .Where(x => x.PayrollId == model.PayrollId
                                                            && x.StatusPeriod == StatusPeriod.Open)
                                                     .FirstOrDefaultAsync();

            if (paycycle == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"Ya no hay períodos de pago disponibles para el id de nómina {model.PayrollId}" },
                    StatusHttp = 404
                };
            }

            //Se llena la entidad con los campos a actualizar
            var entity = BuildDtoHelper<PayrollProcess>.OnBuild(model, response);

            entity.PeriodStartDate = paycycle.PeriodStartDate;
            entity.PeriodEndDate = paycycle.PeriodEndDate;
            entity.IsPayCycleTax = paycycle.IsForTax;
            entity.PayCycleId = paycycle.PayCycleId;

            //Actualización para los cálculos de deducciones
            entity.IsPayCycleTss = paycycle.IsForTss;
            //Actualización para los cálculos de deducciones

            _dbContext.PayrollsProcess.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        //Sección de procesos 
        public async Task<Response<object>> ProcessPayroll(string payrollprocessid)
        {
            var a = await ClearProcessDetails(payrollprocessid);
            if (a != null)
                return a;            

            string payrollid;
            var payrollprocess = await _dbContext.PayrollsProcess.Where(x => x.PayrollProcessId == payrollprocessid)
                                            .FirstOrDefaultAsync();

            if (payrollprocess == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El proceso de nómina seleccionado no existe." },
                    StatusHttp = 404
                };
            }

            payrollid = payrollprocess.PayrollId;

            var b = await CleanEarningForHour(payrollprocessid,payrollid, payrollprocess.PeriodEndDate, payrollprocess.PeriodStartDate);
            if (b != null)
                return b;

            List<PayrollProcessDetail> payrollProcessDetails = new List<PayrollProcessDetail>();

            List<EmpPayrollProcessHelper> employees = null;
            //Switch for royaltyPayroll
            if (payrollprocess.IsRoyaltyPayroll)
            {
                employees = await _dbContext.Employees
                        .Where(x => x.EndWorkDate > payrollprocess.PeriodEndDate
                                && x.StartWorkDate <= payrollprocess.PeriodStartDate
                                && x.WorkStatus == WorkStatus.Employ)
                        .GroupBy(x => new { x.EmployeeId, x.PayMethod, x.Name, x.LastName })
                        .Select(x => new EmpPayrollProcessHelper
                        {
                            EmployeeId = x.Key.EmployeeId,
                            PayMethod = x.Key.PayMethod,
                            EmployeeName = $"{x.Key.Name} {x.Key.LastName}"
                        })
                        .ToListAsync();
            }
            else
            {
                employees = await _dbContext.Employees
                            .Join(_dbContext.EmployeeEarningCodes,
                                e => e.EmployeeId,
                                ec => ec.EmployeeId,
                                (e, ec) => new { E = e, Ec = ec })
                            .Where(x => x.E.EndWorkDate > payrollprocess.PeriodEndDate
                                    && x.E.StartWorkDate <= payrollprocess.PeriodStartDate //Modificación para no buscar empleados que no están contratados
                                    && x.Ec.PayrollId == payrollid
                                    && x.E.WorkStatus == WorkStatus.Employ && x.E.EmployeeStatus == true)
                            .GroupBy(x => new { x.E.EmployeeId, x.E.PayMethod, x.E.Name, x.E.LastName, x.E.StartWorkDate })
                            .Select(x => new EmpPayrollProcessHelper
                            {
                                EmployeeId = x.Key.EmployeeId,
                                PayMethod = x.Key.PayMethod,
                                EmployeeName = $"{x.Key.Name} {x.Key.LastName}",
                                StartWorkDate = x.Key.StartWorkDate
                            })
                            .ToListAsync();
            }

            if (employees != null)
            {
                foreach (var item in employees)
                {
                    //Bancos
                    var bankaccount = await _dbContext.EmployeeBankAccounts
                        .Where(x => x.EmployeeId == item.EmployeeId && x.IsPrincipal == true)
                        .Select(x => new
                        {
                            BankAccount = x.AccountNum,
                            BankName = x.BankName
                        }).FirstOrDefaultAsync();

                    //Números de identificación
                    var document = await _dbContext.EmployeeDocuments
                        .Where(x => x.EmployeeId == item.EmployeeId && x.IsPrincipal == true)
                        .Select(x => new
                        {
                            DocumentNumber = x.DocumentNumber
                        }).FirstOrDefaultAsync();

                    //Departamento
                    var department = await _dbContext.EmployeePositions
                        .Join(_dbContext.Positions,
                            ep => ep.PositionId,
                            p => p.PositionId,
                            (ep, p) => new { Ep = ep, P = p })
                        .Join(_dbContext.Departments,
                            epp => epp.P.DepartmentId,
                            d => d.DepartmentId,
                            (epp, d) => new { Epp = epp, D = d })
                        .Where(x => x.Epp.Ep.EmployeeId == item.EmployeeId
                               && x.Epp.Ep.ToDate >= payrollprocess.PeriodEndDate)
                        .Select(x => new
                        {
                            DepartmentId = x.D.DepartmentId,
                            DepartmentName = x.D.Name
                        }).FirstOrDefaultAsync();

                    //Guardar en la lista
                    payrollProcessDetails.Add(new PayrollProcessDetail()
                    {
                        EmployeeId = item.EmployeeId,
                        EmployeeName = item.EmployeeName,
                        PayrollProcessId = payrollprocessid,
                        BankAccount = bankaccount == null ? "N/A" : bankaccount.BankAccount,
                        BankName = bankaccount == null ? "N/A" : bankaccount.BankName,
                        Document = document == null ? "N/A" : document.DocumentNumber,
                        PayMethod = item.PayMethod,
                        DepartmentId = department == null ? "N/A" : department.DepartmentId,
                        DepartmentName = department == null ? "N/A" : department.DepartmentName,
                        PayrollProcessStatus = PayrollProcessStatus.Processed,
                        StartWorkDate = item.StartWorkDate
                    });
                }
            }

            Response<object> returnvalue;

            if (payrollProcessDetails.Count == 0)
            {
                returnvalue = new Response<object>(null)
                {
                    Errors = new List<string>() { "No se crearon registros" },
                    StatusHttp = 404
                };
            }
            else
            {
                //Guardar el detalle del proceso de nómina
                _dbContext.PayrollProcessDetails.AddRange(payrollProcessDetails);
                await _dbContext.SaveChangesAsync();

                //Actualizar el status del proceso de nómina
                payrollprocess.PayrollProcessStatus = PayrollProcessStatus.Processed;
                payrollprocess.EmployeeQuantity = payrollProcessDetails.Count();
                payrollprocess.EmployeeQuantityForPay = payrollProcessDetails.Count();
                _dbContext.PayrollsProcess.Update(payrollprocess);
                await _dbContext.SaveChangesAsync();

                returnvalue = new Response<object>(payrollProcessDetails)
                {
                    Message = "Registros creados correctamente"
                };
            }

            return returnvalue;
        }


        public async Task<Response<object>> CalcProcessPayroll(string payrollprocessid)
        {
            var a = await ClearProcessActions(payrollprocessid);
            if (a != null)
                return a;

            List<PayrollProcessDetail> updateprocessdetail = new List<PayrollProcessDetail>(); //Lista de process details para actualizar los montos del salario

            payrollProcessActions = new List<PayrollProcessAction>();

            var payrollprocess = await _dbContext.PayrollsProcess.Where(x => x.PayrollProcessId == payrollprocessid)
                                            .FirstOrDefaultAsync();

            if (payrollprocess == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El proceso de nómina seleccionado no existe." },
                    StatusHttp = 404
                };
            }

            var payrollprocessdetail = await _dbContext.PayrollProcessDetails.Where(x => x.PayrollProcessId == payrollprocessid)
                .ToListAsync();

            if (payrollprocessdetail != null)
            {

                //Process for royalty payroll
                if (payrollprocess.IsRoyaltyPayroll)
                {
                    foreach (var item in payrollprocessdetail)
                    {
                        await GetRoyaltyAmounts(item.EmployeeId, payrollprocess.PayrollProcessId, payrollprocess.PeriodStartDate, payrollprocess.PeriodEndDate);

                        //Actualizar la información de los empleados
                        //var employeedetail = payrollprocessdetail.Where(x => x.PayrollProcessId == payrollprocessid && x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var employeedetail = item;
                        employeedetail.TotalAmount = amountsalary;
                        employeedetail.TotalTaxAmount = amounttaxsalary;
                        updateprocessdetail.Add(employeedetail);
                    }
                }
                else
                {
                    foreach (var item in payrollprocessdetail)
                    {
                        //Proceso para pago por hora
                        if (payrollprocess.IsForHourPayroll)
                        {
                            await CreateEarningForHour(item.EmployeeId, payrollprocessid, payrollprocess.PayrollId, payrollprocess.PayCycleId, payrollprocess.PeriodEndDate, payrollprocess.PeriodStartDate);
                        }

                        cont = 0;

                        await GetEarningCodes(item.EmployeeId, payrollprocess.PayrollId,
                                                                      payrollprocess.PayCycleId, payrollprocess.PayrollProcessId,
                                                                      payrollprocess.PeriodEndDate,
                                                                      payrollprocess.PeriodStartDate,
                                                                      item.StartWorkDate);

                        await GetExtraHours(item.EmployeeId, payrollprocess.PayrollId, payrollprocess.PayrollProcessId, payrollprocess.PeriodEndDate);


                        await GetDeductions(item.EmployeeId, payrollprocess.PayrollId,
                                                                    payrollprocess.PayCycleId, payrollprocess.PayrollProcessId,
                                                                    payrollprocess.PeriodEndDate,
                                                                    payrollprocess.PeriodStartDate);


                        if (payrollprocess.IsPayCycleTss)
                        {
                            await GetDeductionsForTss(item.EmployeeId, payrollprocess.PayrollId,
                                                                    payrollprocess.PayCycleId, payrollprocess.PayrollProcessId,
                                                                    payrollprocess.PeriodEndDate,
                                                                    payrollprocess.PeriodStartDate);
                        }


                        await GetLoans(item.EmployeeId, payrollprocess.PayrollId,
                                            payrollprocess.PayCycleId, payrollprocess.PayrollProcessId,
                                            payrollprocess.PeriodEndDate);


                        if (payrollprocess.IsPayCycleTax)
                        {
                            await GetTaxes(item.EmployeeId, payrollprocess.PayrollId, payrollprocess.PayrollProcessId, payrollprocess.PeriodEndDate);
                        }


                        //Actualizar la información de los empleados
                        //var employeedetail = payrollprocessdetail.Where(x => x.PayrollProcessId == payrollprocessid && x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var employeedetail = item;
                        employeedetail.TotalAmount = amountsalary;
                        employeedetail.TotalTaxAmount = amounttaxsalary;

                        //Agregar monto para el cálculo de las deducciones
                        employeedetail.TotalTssAmount = EarningTssAmount;
                        employeedetail.TotalTssAndTaxAmount = EarningTssAndTaxAmount;
                        //Agregar monto para el cálculo de las deducciones

                        updateprocessdetail.Add(employeedetail);
                    }
                }
            }

            if (payrollProcessActions.Count > 0)
            {
                //Guardar las novedades de empleados
                _dbContext.PayrollProcessActions.AddRange(payrollProcessActions);
                await _dbContext.SaveChangesAsync();

                //Actualizar el status del proceso de nómina, si fue usado para el calculo de impuesto y el monto a pagar
                payrollprocess.PayrollProcessStatus = PayrollProcessStatus.Calculated;
                payrollprocess.UsedForTax = payrollprocess.IsPayCycleTax;
                payrollprocess.UsedForTss = payrollprocess.IsPayCycleTss;
                payrollprocess.TotalAmountToPay = updateprocessdetail.Sum(x => x.TotalAmount);

                _dbContext.PayrollsProcess.Update(payrollprocess);
                await _dbContext.SaveChangesAsync();


                //Actualizar los montos de los detalles de procesos de nómina
                _dbContext.PayrollProcessDetails.UpdateRange(updateprocessdetail);
                await _dbContext.SaveChangesAsync();

                return new Response<object>(payrollProcessActions)
                {
                    Message = "Registro creado correctamente"
                };
            }
            else
            {
                return new Response<object>()
                {
                    Errors = new List<string>() { "No se procesaron registros" },
                    StatusHttp = 404
                };
            }
        }


        public async Task<Response<object>> PayPayroll(string payrollprocessid)
        {
            var response = await _dbContext.PayrollsProcess.Where(x => x.PayrollProcessId == payrollprocessid
                                                       && x.PayrollProcessStatus == PayrollProcessStatus.Calculated).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está en estado calculado" },
                    StatusHttp = 404
                };
            }

            //Se marcan como usados para impuesto
            if (response.IsPayCycleTax)
            {
                await UpdateUsedForTax(response.PayrollId, response.PayrollProcessId);
            }

            //Se marcan como usados para tss
            if (response.IsPayCycleTss)
            {
                await UpdateUsedForTss(response.PayrollId, response.PayrollProcessId);
            }
            
            //Se marcan como usados para calculo por hora
            if (response.IsForHourPayroll)
            {
                await UpdateWorkControlToPaid(response.PayrollProcessId);
            }


            //Se llena la entidad con los campos a actualizar
            var entity = response;
            entity.PayrollProcessStatus = PayrollProcessStatus.Paid;

            _dbContext.PayrollsProcess.Update(entity);
            await _dbContext.SaveChangesAsync();


            var paycycle = await _dbContext.PayCycles.OrderBy(x => x.PayCycleId)
                                                     .Where(x => x.PayrollId == response.PayrollId
                                                            && x.PayCycleId == response.PayCycleId
                                                            && x.StatusPeriod == StatusPeriod.Open)
                                                     .FirstOrDefaultAsync();

            if (paycycle == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"El período de pago no existe" },
                    StatusHttp = 404
                };
            }

            var cycle = paycycle;
            cycle.StatusPeriod = StatusPeriod.Paid;
            cycle.PayDate = response.PaymentDate;
            cycle.AmountPaidPerPeriod = response.TotalAmountToPay;

            _dbContext.PayCycles.Update(cycle);
            await _dbContext.SaveChangesAsync();


            //Se  marcan las horas extras como pagadas y se descuentan los préstamos
            List<EmployeeExtraHour> extraHours = new List<EmployeeExtraHour>();
            List<EmployeeLoan> loans = new List<EmployeeLoan>();
            List<EmployeeLoanHistory> loanhistories = new List<EmployeeLoanHistory>();

            var payrollprocessdetails = await _dbContext.PayrollProcessDetails
                                                     .Where(x => x.PayrollProcessId == payrollprocessid)
                                                     .ToListAsync();

            foreach (var item in payrollprocessdetails)
            {
                //Horas
                var a = await _dbContext.EmployeeExtraHours.Where(x => x.EmployeeId == item.EmployeeId
                                                                    && x.PayrollId == response.PayrollId
                                                                    //&& x.WorkedDay <= response.PeriodEndDate
                                                                    && x.CalcPayrollDate <= response.PeriodEndDate
                                                                    && x.StatusExtraHour == StatusExtraHour.Open)
                                                            .ToListAsync();
                foreach (var hour in a)
                {
                    var b = hour;
                    b.StatusExtraHour = StatusExtraHour.Pagada;
                    extraHours.Add(b);
                }


                //Préstamos
                var c = await _dbContext.EmployeeLoans.Where(x => x.EmployeeId == item.EmployeeId
                                                                && x.PayrollId == response.PayrollId
                                                                && x.ValidTo > response.PeriodEndDate
                                                                && x.PendingAmount > 0)
                                                           .ToListAsync();
                foreach (var loan in c)
                {
                    var d = loan;
                    d.PaidAmount = d.PaidAmount + d.AmountByDues;
                    d.PendingAmount = d.PendingAmount - d.AmountByDues;
                    loans.Add(d);


                    var currentLoanHistory = await _dbContext.EmployeeLoanHistories.Where(x => x.EmployeeId == d.EmployeeId
                                            && x.ParentInternalId == d.InternalId).OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

                    var loanhistory = BuildDtoHelper<EmployeeLoanHistory>.OnBuild(d, new EmployeeLoanHistory());

                    loanhistory.InternalId = currentLoanHistory == null ? 1 : currentLoanHistory.InternalId + 1;
                    loanhistory.ParentInternalId = d.InternalId;
                    loanhistory.PayrollProcessId = payrollprocessid;
                    loanhistory.PeriodEndDate = response.PeriodEndDate;
                    loanhistory.PeriodStartDate = response.PeriodStartDate;

                    loanhistories.Add(loanhistory);
                }
            }

            //Agrego el historial de préstamos
            if (loanhistories.Count > 0)
            {
                _dbContext.EmployeeLoanHistories.AddRange(loanhistories);
                await _dbContext.SaveChangesAsync();
            }

            //Actualizo los préstamos
            if (loans.Count > 0)
            {
                _dbContext.EmployeeLoans.UpdateRange(loans);
                await _dbContext.SaveChangesAsync();
            }

            //Actualizo las horas
            if (extraHours.Count > 0)
            {
                _dbContext.EmployeeExtraHours.UpdateRange(extraHours);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> CancelPayroll(string payrollprocessid)
        {
            var response = await _dbContext.PayrollsProcess.Where(x => x.PayrollProcessId == payrollprocessid
                                                       && x.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                                       && x.PayrollProcessStatus != PayrollProcessStatus.Paid).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está disponible para cancelar" },
                    StatusHttp = 404
                };
            }

            //Se llena la entidad con los campos a actualizar
            var entity = response;
            entity.PayrollProcessStatus = PayrollProcessStatus.Canceled;

            _dbContext.PayrollsProcess.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        //Proceso de nómina de regalía
        private async Task GetRoyaltyAmounts(string employeeid, string payrollprocessid, DateTime startperioddate, DateTime endperioddate)
        {
            EarningTssAmount = 0;
            EarningTssAndTaxAmount = 0;
            amountsalary = 0;
            amounttaxsalary = 0;

            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();

            var earningPayrollAction = await _dbContext.PayrollProcessActions
                .Join(_dbContext.PayrollsProcess,
                    ppa => ppa.PayrollProcessId,
                    pp => pp.PayrollProcessId,
                    (ppa, pp) => new { Ppa = ppa, Pp = pp })
                .Where(x => x.Ppa.EmployeeId == employeeid
                       && x.Pp.PeriodStartDate >= startperioddate && x.Pp.PeriodEndDate <= endperioddate
                       && x.Pp.IsRoyaltyPayroll == false
                       && x.Ppa.PayrollActionType == PayrollActionType.Earning
                       && x.Ppa.ApplyRoyaltyPayroll == true
                       && (x.Pp.PayrollProcessStatus == PayrollProcessStatus.Paid || x.Pp.PayrollProcessStatus == PayrollProcessStatus.Closed))
                .Select(x => new
                {
                    ActionName = x.Ppa.ActionName,
                    ActionId = x.Ppa.ActionId,
                    ActionAmount = x.Ppa.ActionAmount
                })
                .ToListAsync();

            foreach (var item in earningPayrollAction)
            {
                cont++;

                localPayrollProcessActions.Add(new PayrollProcessAction()
                {
                    PayrollActionType = PayrollActionType.Earning,
                    EmployeeId = employeeid,
                    PayrollProcessId = payrollprocessid,
                    InternalId = cont,
                    ActionName = item.ActionName,
                    ActionAmount = item.ActionAmount,
                    ActionId = item.ActionId
                });

                amountsalary += item.ActionAmount;
            }

            if (localPayrollProcessActions.Count > 0)
            {
                payrollProcessActions.AddRange(localPayrollProcessActions);
                amounttaxsalary = 0;
                amountsalary = amountsalary / 12;
            }
        }

        //Códigos de ganancia
        private async Task GetEarningCodes(string employeeid, string payrollid, int paycycleid, string payrollprocessid, DateTime endperioddate, DateTime startperioddate, DateTime startWorkDate)
        {
            EarningTssAmount = 0;
            EarningTssAndTaxAmount = 0;
            amountsalary = 0;
            amounttaxsalary = 0;

            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();
            //Ecuation: (ciclo - inicio)/repeticion
            var earningcode = await _dbContext.EmployeeEarningCodes
                .Join(_dbContext.EarningCodes,
                    eec => eec.EarningCodeId,
                    ec => ec.EarningCodeId,
                    (eec, ec) => new { Eec = eec, Ec = ec })
                .Where(x => x.Eec.EmployeeId == employeeid
                       && x.Eec.PayrollId == payrollid
                       && x.Eec.FromDate <= startperioddate
                       && x.Eec.ToDate >= endperioddate
                       && x.Eec.IsUseCalcHour == false)
                .Select(x => new
                {
                    ActionName = x.Ec.Name,
                    ActionId = x.Ec.EarningCodeId,
                    ActionAmount = x.Eec.IndexEarning,
                    ActionAmountDiary = x.Eec.IndexEarningDiary,
                    ApplyTSS = x.Ec.IsTSS,
                    ApplyTax = x.Ec.IsISR,
                    ApplyRoyaltyPayroll = x.Ec.IsRoyaltyPayroll,
                    StartPeriodForPaid = x.Eec.StartPeriodForPaid,
                    QtyPeriodForPaid = x.Eec.QtyPeriodForPaid
                })
                .ToListAsync();

            foreach (var item in earningcode)
            {
                if (ValidateIntValue(paycycleid, item.StartPeriodForPaid, item.QtyPeriodForPaid) == true)
                {
                    cont++;

                    localPayrollProcessActions.Add(new PayrollProcessAction()
                    {
                        PayrollActionType = PayrollActionType.Earning,
                        EmployeeId = employeeid,
                        PayrollProcessId = payrollprocessid,
                        InternalId = cont,
                        ActionName = item.ActionName,
                        //ActionAmount = startWorkDate > startperioddate? (endperioddate - startWorkDate).Days: item.ActionAmount,
                        ActionAmount = startWorkDate > startperioddate ? (endperioddate - startWorkDate).Days * item.ActionAmountDiary : item.ActionAmount,
                        ApplyTSS = item.ApplyTSS,
                        ApplyTax = item.ApplyTax,
                        ApplyRoyaltyPayroll = item.ApplyRoyaltyPayroll,
                        ActionId = item.ActionId
                    });

                    amountsalary += startWorkDate > startperioddate ? (endperioddate - startWorkDate).Days * item.ActionAmountDiary : item.ActionAmount;
                }
            }

            if (localPayrollProcessActions.Count > 0)
            {
                payrollProcessActions.AddRange(localPayrollProcessActions);
                EarningTssAmount = localPayrollProcessActions.Where(x => x.ApplyTSS == true).Sum(x => x.ActionAmount);
                EarningTssAndTaxAmount = localPayrollProcessActions.Where(x => x.ApplyTSS == true && x.ApplyTax == true).Sum(x => x.ActionAmount);
                amounttaxsalary = localPayrollProcessActions.Where(x => x.ApplyTax == true).Sum(x => x.ActionAmount);
            }
        }

        //Préstamos
        private async Task GetLoans(string employeeid, string payrollid, int paycycleid, string payrollprocessid, DateTime endperioddate)
        {
            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();

            var loans = await _dbContext.EmployeeLoans
                .Join(_dbContext.Loans,
                    el => el.LoanId,
                    l => l.LoanId,
                    (el, l) => new { El = el, L = l })
                .Where(x => x.El.EmployeeId == employeeid
                       && x.El.PendingAmount > 0
                       && x.El.PayrollId == payrollid
                       && x.El.ValidTo > endperioddate
                       )
                .Select(x => new
                {
                    ActionName = x.L.Name,
                    ActionId = x.L.LoanId,
                    ActionAmount = x.El.AmountByDues,
                    ApplyTSS = false,
                    ApplyTax = false,
                    StartPeriodForPaid = x.El.StartPeriodForPaid,
                    QtyPeriodForPaid = x.El.QtyPeriodForPaid
                })
                .ToListAsync();

            foreach (var item in loans)
            {
                if (ValidateIntValue(paycycleid, item.StartPeriodForPaid, item.QtyPeriodForPaid) == true)
                {
                    cont++;

                    localPayrollProcessActions.Add(new PayrollProcessAction()
                    {
                        PayrollActionType = PayrollActionType.Loan,
                        EmployeeId = employeeid,
                        PayrollProcessId = payrollprocessid,
                        InternalId = cont,
                        ActionName = item.ActionName,
                        ActionAmount = item.ActionAmount,
                        ApplyTSS = item.ApplyTSS,
                        ApplyTax = item.ApplyTax,
                        ActionId = item.ActionId
                    });

                    amountsalary -= item.ActionAmount;
                }
            }

            if (localPayrollProcessActions.Count > 0)
            {
                //amountsalary -= localPayrollProcessActions.Sum(x => x.ActionAmount);
                payrollProcessActions.AddRange(localPayrollProcessActions);
            }
        }

        //Deducciones
        private async Task GetDeductions(string employeeid, string payrollid, int paycycleid, string payrollprocessid, DateTime endperioddate, DateTime startperioddate)
        {
            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();

            var deductions = await _dbContext.EmployeeDeductionCodes
                .Join(_dbContext.DeductionCodeVersions,
                    ed => ed.DeductionCodeId,
                    dc => dc.DeductionCodeId,
                    (ed, dc) => new { Ed = ed, Dc = dc })
                .Where(x => x.Ed.EmployeeId == employeeid
                       && x.Ed.PayrollId == payrollid
                       && x.Ed.ToDate >= endperioddate
                       && x.Dc.ValidFrom <= endperioddate && x.Dc.ValidTo >= endperioddate
                       && x.Dc.IsForTssCalc == false
                       )
                .Select(x => new
                {
                    ActionName = x.Dc.Name,
                    ActionId = x.Dc.DeductionCodeId,
                    PayrollAction = x.Dc.PayrollAction,
                    DeductionAmount = x.Dc.Dduction_MultiplyAmount,
                    ContributionAmount = x.Dc.Ctbution_MultiplyAmount,
                    DeductionType = x.Dc.Dduction_IndexBase,
                    ContributionType = x.Dc.Ctbution_IndexBase,
                    ContributionLimitAmount = x.Dc.Ctbution_LimitAmount,
                    ContributionFixedAmount = x.Dc.Ctbution_LimitAmountToApply,
                    DeductionLimitAmount = x.Dc.Dduction_LimitAmount,
                    DeductionFixedAmount = x.Dc.Dduction_LimitAmountToApply,
                    ApplyTSS = false,
                    ApplyTax = false,
                    EmployeeDeductionAmount = x.Ed.DeductionAmount,
                    IsForTaxCalc = x.Dc.IsForTaxCalc,
                    StartPeriodForPaid = x.Ed.StartPeriodForPaid,
                    QtyPeriodForPaid = x.Ed.QtyPeriodForPaid
                })
                .ToListAsync();

            foreach (var item in deductions)
            {
                if (ValidateIntValue(paycycleid, item.StartPeriodForPaid, item.QtyPeriodForPaid) == true)
                {

                    decimal deductionamount = 0;
                    decimal deductionamountWithTax = 0;
                    decimal contributionamount = 0;

                    if (item.PayrollAction == PayrollAction.Deduction || item.PayrollAction == PayrollAction.Both)
                    {
                        //Porcentaje
                        if (item.DeductionType == IndexBase.EarningPercent)
                        {
                            deductionamount = EarningTssAmount * (item.DeductionAmount / 100);
                            deductionamountWithTax = EarningTssAndTaxAmount * (item.DeductionAmount / 100);
                        }

                        //Monto fijo
                        if (item.DeductionType == IndexBase.FixedAmount)
                        {
                            if (item.EmployeeDeductionAmount > 0)
                            {
                                deductionamount = item.EmployeeDeductionAmount;
                                deductionamountWithTax = item.EmployeeDeductionAmount;
                            }
                            else
                            {
                                deductionamount = item.DeductionAmount;
                                deductionamountWithTax = item.DeductionAmount;
                            }
                        }
                    }


                    if (item.PayrollAction == PayrollAction.Contribution || item.PayrollAction == PayrollAction.Both)
                    {
                        //Porcentaje
                        if (item.ContributionType == IndexBase.EarningPercent)
                        {
                            contributionamount = EarningTssAmount * (item.ContributionAmount / 100);
                        }

                        //Monto fijo
                        if (item.ContributionType == IndexBase.FixedAmount)
                        {
                            contributionamount = item.ContributionAmount;
                        }
                    }

                    //Se evalua si hay deducciones
                    if (deductionamount > 0)
                    {
                        //if (item.DeductionLimitAmount != 0 && deductionformonth >= item.DeductionLimitAmount)
                        ////if (item.DeductionLimitAmount != 0 && deductionamount > item.DeductionLimitAmount)
                        //{
                        //    deductionamount = CalcAmountForMonth(payrollinfo.PayFrecuency, item.DeductionLimitAmount);
                        //    deductionamountWithTax = CalcAmountForMonth(payrollinfo.PayFrecuency, item.DeductionLimitAmount);
                        //    //deductionamount = item.DeductionLimitAmount;
                        //    //deductionamountWithTax = item.DeductionLimitAmount;
                        //}

                        cont++;

                        localPayrollProcessActions.Add(new PayrollProcessAction()
                        {
                            PayrollActionType = PayrollActionType.Deduction,
                            EmployeeId = employeeid,
                            PayrollProcessId = payrollprocessid,
                            InternalId = cont,
                            ActionName = item.ActionName,
                            ActionAmount = deductionamount,
                            ApplyTSS = false,
                            ApplyTax = false,
                            ActionId = item.ActionId
                        });

                        amountsalary -= deductionamount;

                        if (item.IsForTaxCalc)
                            amounttaxsalary -= deductionamountWithTax;
                    }

                    if (contributionamount > 0)
                    {
                        //if (item.ContributionLimitAmount != 0 && contributionformonth >= item.ContributionLimitAmount)
                        //{
                        //    contributionamount = CalcAmountForMonth(payrollinfo.PayFrecuency, item.ContributionLimitAmount);
                        //    //contributionamount = item.ContributionLimitAmount;
                        //}

                        cont++;

                        localPayrollProcessActions.Add(new PayrollProcessAction()
                        {
                            PayrollActionType = PayrollActionType.Contribution,
                            EmployeeId = employeeid,
                            PayrollProcessId = payrollprocessid,
                            InternalId = cont,
                            ActionName = item.ActionName,
                            ActionAmount = contributionamount,
                            ApplyTSS = false,
                            ApplyTax = false,
                            ActionId = item.ActionId
                        });
                    }
                }
            }

            if (localPayrollProcessActions.Count > 0)
            {
                payrollProcessActions.AddRange(localPayrollProcessActions);
            }
        }


        //Deducciones para tss
        private async Task GetDeductionsForTss(string employeeid, string payrollid, int paycycleid, string payrollprocessid, DateTime endperioddate, DateTime startperioddate)
        {
            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();

            var deductions = await _dbContext.EmployeeDeductionCodes
                .Join(_dbContext.DeductionCodeVersions,
                    ed => ed.DeductionCodeId,
                    dc => dc.DeductionCodeId,
                    (ed, dc) => new { Ed = ed, Dc = dc })
                .Where(x => x.Ed.EmployeeId == employeeid
                       && x.Ed.PayrollId == payrollid
                       && x.Ed.ToDate >= endperioddate
                       && x.Dc.ValidFrom <= endperioddate && x.Dc.ValidTo >= endperioddate
                       && x.Dc.IsForTssCalc == true
                       )
                .Select(x => new
                {
                    ActionName = x.Dc.Name,
                    ActionId = x.Dc.DeductionCodeId,
                    PayrollAction = x.Dc.PayrollAction,
                    DeductionAmount = x.Dc.Dduction_MultiplyAmount,
                    ContributionAmount = x.Dc.Ctbution_MultiplyAmount,
                    DeductionType = x.Dc.Dduction_IndexBase,
                    ContributionType = x.Dc.Ctbution_IndexBase,
                    ContributionLimitAmount = x.Dc.Ctbution_LimitAmount,
                    ContributionFixedAmount = x.Dc.Ctbution_LimitAmountToApply,
                    DeductionLimitAmount = x.Dc.Dduction_LimitAmount,
                    DeductionFixedAmount = x.Dc.Dduction_LimitAmountToApply,
                    ApplyTSS = false,
                    ApplyTax = false,
                    EmployeeDeductionAmount = x.Ed.DeductionAmount,
                    IsForTaxCalc = x.Dc.IsForTaxCalc,
                    StartPeriodForPaid = x.Ed.StartPeriodForPaid,
                    QtyPeriodForPaid = x.Ed.QtyPeriodForPaid
                })
                .ToListAsync();

            //Realiza el calculo del total de salario de tss para todos los procesos de nómina anteriores que no eran de tipo tss
            var lastTotalTssAmount = await _dbContext.PayrollProcessDetails
                                                .Join(_dbContext.PayrollsProcess,
                                                    details => details.PayrollProcessId,
                                                    process => process.PayrollProcessId,
                                                    (details, process) => new { Details = details, Process = process })
                                                .Where(x => x.Process.UsedForTss == false
                                                        && x.Process.PayrollId == payrollid
                                                        && x.Process.PayrollProcessId != payrollprocessid
                                                        && x.Process.IsPayCycleTss == false
                                                        && x.Details.EmployeeId == employeeid
                                                        && x.Details.TotalTssAmount != 0)
                                                .GroupBy(x => x.Details.EmployeeId)
                                                .Select(x => new
                                                {
                                                    TotalTssAndTax = x.Sum(x => x.Details.TotalTssAndTaxAmount),
                                                    TotalTss = x.Sum(x => x.Details.TotalTssAmount)
                                                })
                                                .FirstOrDefaultAsync();

            //informacion de la nómina
            var payrollinfo = await _dbContext.Payrolls.Where(x => x.PayrollId == payrollid).FirstOrDefaultAsync();

            //Se busca el monto del código de ganancia que corresponde al salario, 
            //aplica para calculo de horas extras 
            var localSalaryAmount = await _dbContext.EmployeeEarningCodes
                .Join(_dbContext.EarningCodes,
                    eec => eec.EarningCodeId,
                    ec => ec.EarningCodeId,
                    (eec, ec) => new { Eec = eec, Ec = ec })
                .Where(x => x.Eec.EmployeeId == employeeid
                       && x.Eec.PayrollId == payrollid
                       && x.Eec.FromDate <= startperioddate
                       && x.Eec.ToDate >= endperioddate
                       && x.Ec.IsExtraHours == true
                       && x.Eec.IsUseCalcHour == false)
                .Select(x => x.Eec.IndexEarningMonthly)
                .FirstOrDefaultAsync();

            bool anyPrevAmount = false;
            decimal totaltss = 0;
            decimal totaltssandtax = 0;
            if (lastTotalTssAmount != null)
            {
                totaltss = lastTotalTssAmount.TotalTss;
                totaltssandtax = lastTotalTssAmount.TotalTssAndTax;
            }
            anyPrevAmount = totaltss == 0 ? false : true;

            foreach (var item in deductions)
            {
                decimal totalEarningTssAmount = EarningTssAmount + totaltss;
                decimal totalEarningTssAndTaxAmount = EarningTssAndTaxAmount + totaltssandtax;

                decimal deductionamount = 0;
                decimal deductionamountWithTax = 0;
                decimal contributionamount = 0;
                decimal deductionformonth = 0;
                decimal contributionformonth = 0;

                if (item.PayrollAction == PayrollAction.Deduction || item.PayrollAction == PayrollAction.Both)
                {
                    //Porcentaje
                    if (item.DeductionType == IndexBase.EarningPercent)
                    {
                        deductionamount = totalEarningTssAmount * (item.DeductionAmount / 100);
                        deductionamountWithTax = totalEarningTssAndTaxAmount * (item.DeductionAmount / 100);

                        deductionformonth = anyPrevAmount ? deductionamount : localSalaryAmount * (item.DeductionAmount / 100);
                    }

                    //Monto fijo
                    if (item.DeductionType == IndexBase.FixedAmount)
                    {
                        if (item.EmployeeDeductionAmount > 0)
                        {
                            deductionamount = item.EmployeeDeductionAmount;
                            deductionamountWithTax = item.EmployeeDeductionAmount;
                        }
                        else
                        {
                            deductionamount = item.DeductionAmount;
                            deductionamountWithTax = item.DeductionAmount;
                        }
                    }
                }


                if (item.PayrollAction == PayrollAction.Contribution || item.PayrollAction == PayrollAction.Both)
                {
                    //Porcentaje
                    if (item.ContributionType == IndexBase.EarningPercent)
                    {
                        contributionamount = totalEarningTssAmount * (item.ContributionAmount / 100);
                        contributionformonth = anyPrevAmount ? contributionamount : localSalaryAmount * (item.ContributionAmount / 100); ;

                    }

                    //Monto fijo
                    if (item.ContributionType == IndexBase.FixedAmount)
                    {
                        contributionamount = item.ContributionAmount;
                    }
                }

                //se evalua si el monto supera el limite configurado
                if (deductionamount > 0)
                {
                    if (item.DeductionLimitAmount != 0 && deductionformonth >= item.DeductionLimitAmount)
                    //if (item.DeductionLimitAmount != 0 && deductionamount > item.DeductionLimitAmount)
                    {
                        if (anyPrevAmount)
                        {
                            deductionamount = item.DeductionLimitAmount;
                            deductionamountWithTax = item.DeductionLimitAmount;
                        }
                        else
                        {
                            deductionamount = CalcAmountForMonth(payrollinfo.PayFrecuency, item.DeductionLimitAmount);
                            deductionamountWithTax = CalcAmountForMonth(payrollinfo.PayFrecuency, item.DeductionLimitAmount);
                        }
                    }

                    cont++;

                    localPayrollProcessActions.Add(new PayrollProcessAction()
                    {
                        PayrollActionType = PayrollActionType.Deduction,
                        EmployeeId = employeeid,
                        PayrollProcessId = payrollprocessid,
                        InternalId = cont,
                        ActionName = item.ActionName,
                        ActionAmount = deductionamount,
                        ApplyTSS = false,
                        ApplyTax = false,
                        ActionId = item.ActionId
                    });

                    amountsalary -= deductionamount;

                    if (item.IsForTaxCalc)
                        amounttaxsalary -= deductionamountWithTax;
                }

                if (contributionamount > 0)
                {

                    if (item.ContributionLimitAmount != 0 && contributionformonth >= item.ContributionLimitAmount)
                    {
                        if (anyPrevAmount)
                        {
                            contributionamount = item.ContributionLimitAmount;
                        }
                        else
                        {
                            contributionamount = CalcAmountForMonth(payrollinfo.PayFrecuency, item.ContributionLimitAmount);
                        }
                    }

                    cont++;

                    localPayrollProcessActions.Add(new PayrollProcessAction()
                    {
                        PayrollActionType = PayrollActionType.Contribution,
                        EmployeeId = employeeid,
                        PayrollProcessId = payrollprocessid,
                        InternalId = cont,
                        ActionName = item.ActionName,
                        ActionAmount = contributionamount,
                        ApplyTSS = false,
                        ApplyTax = false,
                        ActionId = item.ActionId
                    });
                }


            }

            if (localPayrollProcessActions.Count > 0)
            {
                payrollProcessActions.AddRange(localPayrollProcessActions);
            }
        }


        //Impuestos
        private async Task GetTaxes(string employeeid, string payrollid, string payrollprocessid, DateTime endperioddate)
        {
            //Realiza el calculo del total de salario de impuesto para todos los procesos de nómina anteriores que no eran de tipo impuesto
            var lastTotalTaxAmount = await _dbContext.PayrollProcessDetails
                                                .Join(_dbContext.PayrollsProcess,
                                                    details => details.PayrollProcessId,
                                                    process => process.PayrollProcessId,
                                                    (details, process) => new { Details = details, Process = process })
                                                .Where(x => x.Process.UsedForTax == false
                                                        && x.Process.PayrollId == payrollid
                                                        && x.Process.PayrollProcessId != payrollprocessid
                                                        && x.Process.IsPayCycleTax == false
                                                        && x.Details.EmployeeId == employeeid
                                                        && x.Details.TotalTaxAmount != 0)
                                                .GroupBy(x => x.Details.EmployeeId)
                                                .Select(x => x.Sum(x => x.Details.TotalTaxAmount))
                                                .FirstOrDefaultAsync();

            int qtyCycles = 12;
            decimal totaltax = (amounttaxsalary + lastTotalTaxAmount) * qtyCycles;
            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();

            //Buscar los impuestos asociados al empleado para aplicarles los porcentajes al monto total.
            var taxes = await _dbContext.EmployeeTaxes
                .Join(_dbContext.Taxes,
                    et => et.TaxId,
                    t => t.TaxId,
                    (et, t) => new { Et = et, T = t })
                .Join(_dbContext.TaxDetails,
                    join => join.T.TaxId,
                    td => td.TaxId,
                    (join, td) => new { Join = join, Td = td })
                .Where(x => x.Join.Et.EmployeeId == employeeid
                       && x.Join.Et.PayrollId == payrollid
                       && x.Join.Et.ValidTo >= endperioddate
                       && x.Td.AnnualAmountHigher <= totaltax
                       && x.Td.AnnualAmountNotExceed >= totaltax)
                .Select(x => new
                {
                    ApplicableScale = x.Td.ApplicableScale,
                    Percent = x.Td.Percent / 100,
                    FixedAmount = x.Td.FixedAmount,
                    TaxId = x.Td.TaxId
                })
                .ToListAsync();

            foreach (var item in taxes)
            {
                decimal dif = totaltax - item.ApplicableScale;
                decimal taxAmountTotal = (dif * item.Percent) + item.FixedAmount;

                //if (taxAmountTotal > item.FixedAmount && taxAmountTotal != 0 && item.FixedAmount != 0)
                //    taxAmountTotal = item.FixedAmount;

                taxAmountTotal = taxAmountTotal / qtyCycles;

                if (taxAmountTotal > 0)
                {
                    cont++;

                    localPayrollProcessActions.Add(new PayrollProcessAction()
                    {
                        PayrollActionType = PayrollActionType.Tax,
                        EmployeeId = employeeid,
                        PayrollProcessId = payrollprocessid,
                        InternalId = cont,
                        ActionName = item.TaxId,
                        ActionAmount = taxAmountTotal,
                        ApplyTSS = false,
                        ApplyTax = false,
                        ActionId = item.TaxId
                    });
                }
            }

            if (localPayrollProcessActions.Count > 0)
            {
                payrollProcessActions.AddRange(localPayrollProcessActions);
            }
        }

        //Horas extras
        private async Task GetExtraHours(string employeeid, string payrollid, string payrollprocessid, DateTime endperioddate)
        {

            List<PayrollProcessAction> localPayrollProcessActions = new List<PayrollProcessAction>();

            var earningcode = await _dbContext.EmployeeExtraHours
                .Join(_dbContext.EarningCodes,
                    eeh => eeh.EarningCodeId,
                    ec => ec.EarningCodeId,
                    (eeh, ec) => new { Eeh = eeh, Ec = ec })
                .Where(x => x.Eeh.EmployeeId == employeeid
                       && x.Eeh.PayrollId == payrollid
                       //&& x.Eeh.WorkedDay <= endperioddate
                       && x.Eeh.CalcPayrollDate <= endperioddate //Cambio para usar la fecha de uso para cálculo de horas extras
                       && x.Eeh.StatusExtraHour == StatusExtraHour.Open)
                .GroupBy(x => new { x.Eeh.EarningCodeId, x.Ec.IsISR, x.Ec.IsTSS, x.Ec.Name, x.Ec.IsRoyaltyPayroll })
                .Select(x => new
                {
                    ActionName = x.Key.Name,
                    ActionId = x.Key.EarningCodeId,
                    ActionAmount = x.Sum(x => x.Eeh.Amount),
                    ApplyTSS = x.Key.IsTSS,
                    ApplyTax = x.Key.IsISR,
                    ApplyRoyaltyPayroll = x.Key.IsRoyaltyPayroll
                })
                .ToListAsync();

            foreach (var item in earningcode)
            {
                cont++;

                localPayrollProcessActions.Add(new PayrollProcessAction()
                {
                    PayrollActionType = PayrollActionType.ExtraHours,
                    EmployeeId = employeeid,
                    PayrollProcessId = payrollprocessid,
                    InternalId = cont,
                    ActionName = item.ActionName,
                    ActionAmount = item.ActionAmount,
                    ApplyTSS = item.ApplyTSS,
                    ApplyTax = item.ApplyTax,
                    ApplyRoyaltyPayroll = item.ApplyRoyaltyPayroll,
                    ActionId = item.ActionId
                });

                amountsalary += item.ActionAmount;
            }

            if (localPayrollProcessActions.Count > 0)
            {
                payrollProcessActions.AddRange(localPayrollProcessActions);
                EarningTssAmount += localPayrollProcessActions.Where(x => x.ApplyTSS == true).Sum(x => x.ActionAmount);
                amounttaxsalary += localPayrollProcessActions.Where(x => x.ApplyTax == true).Sum(x => x.ActionAmount);
                EarningTssAndTaxAmount += localPayrollProcessActions.Where(x => x.ApplyTSS == true && x.ApplyTax == true).Sum(x => x.ActionAmount);
            }
        }

        //Validar números enteros 
        private bool ValidateIntValue(decimal y, decimal i, decimal r)
        {
            string total = ((y - i) / r).ToString();
            //bool result = int.TryParse(total, out int value);
            bool result = total.Contains(".") || total.Contains("-"); //Contiene, no lo tomo en cuenta = true

            //return !result;
            return result ? false : true;
        }

        //Función para limpiar la tabla de detalles y actions
        private async Task<Response<object>> ClearProcessDetails(string payrollprocessid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var responsedetails = await _dbContext.PayrollProcessDetails.Where(x => x.PayrollProcessId == payrollprocessid).ToListAsync();

                if (responsedetails != null)
                {
                    _dbContext.PayrollProcessDetails.RemoveRange(responsedetails);
                    await _dbContext.SaveChangesAsync();
                }

                var responseactions = await _dbContext.PayrollProcessActions.Where(x => x.PayrollProcessId == payrollprocessid).ToListAsync();

                if (responseactions != null)
                {
                    _dbContext.PayrollProcessActions.RemoveRange(responseactions);
                    await _dbContext.SaveChangesAsync();
                }

                transaction.Commit();
                return null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }

        //Función para limpiar la tabla de actions
        private async Task<Response<object>> ClearProcessActions(string payrollprocessid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var response = await _dbContext.PayrollProcessActions.Where(x => x.PayrollProcessId == payrollprocessid).ToListAsync();

                if (response != null)
                {
                    _dbContext.PayrollProcessActions.RemoveRange(response);
                    await _dbContext.SaveChangesAsync();
                }

                transaction.Commit();
                return null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }

        //Función para actualizar los procesos de nóminas anteriores que no se usaron para impuesto
        private async Task<bool> UpdateUsedForTax(string payrollid, string payrollprocessid)
        {
            //Buscamos los procesos de nómina anteriores sin calculo de impuesto
            var payrollprocess = await _dbContext.PayrollsProcess.Where(x => x.UsedForTax == false
                                                                        && x.PayrollId == payrollid
                                                                        && x.PayrollProcessId != payrollprocessid
                                                                        && x.IsPayCycleTax == false).ToListAsync();

            List<PayrollProcess> updateprocess = new List<PayrollProcess>();

            foreach (var item in payrollprocess)
            {
                var process = item;
                process.UsedForTax = true;
                updateprocess.Add(process);
            }

            if (updateprocess.Count > 0)
            {
                _dbContext.PayrollsProcess.UpdateRange(updateprocess);
                await _dbContext.SaveChangesAsync();
            }

            return true;

        }

        //Función para actualizar los procesos de nóminas anteriores que no se usaron para tss
        private async Task<bool> UpdateUsedForTss(string payrollid, string payrollprocessid)
        {
            //Buscamos los procesos de nómina anteriores sin calculo de impuesto
            var payrollprocess = await _dbContext.PayrollsProcess.Where(x => x.UsedForTss == false
                                                                        && x.PayrollId == payrollid
                                                                        && x.PayrollProcessId != payrollprocessid
                                                                        && x.IsPayCycleTss == false).ToListAsync();

            List<PayrollProcess> updateprocess = new List<PayrollProcess>();

            foreach (var item in payrollprocess)
            {
                var process = item;
                process.UsedForTss = true;
                updateprocess.Add(process);
            }

            if (updateprocess.Count > 0)
            {
                _dbContext.PayrollsProcess.UpdateRange(updateprocess);
                await _dbContext.SaveChangesAsync();
            }

            return true;

        }

        //Función para obtener el número de periodos para completar el mes
        private decimal CalcAmountForMonth(PayFrecuency _PayFrecuency, decimal amount)
        {
            decimal newamount = 0;

            switch (_PayFrecuency)
            {
                case PayFrecuency.Diary:
                    newamount = amount / 30;
                    break;
                case PayFrecuency.Weekly:
                    newamount = amount / 4;
                    break;
                case PayFrecuency.TwoWeekly:
                    newamount = amount / 2;
                    break;
                case PayFrecuency.BiWeekly:
                    newamount = amount / 2;
                    break;
                case PayFrecuency.Monthly:
                    newamount = 1;
                    break;
                case PayFrecuency.ThreeMonth:
                    newamount = amount * 3;
                    break;
                case PayFrecuency.FourMonth:
                    newamount = amount * 4;
                    break;
                case PayFrecuency.Biannual:
                    newamount = amount * 6;
                    break;
                case PayFrecuency.Yearly:
                    newamount = amount * 12;
                    break;
            }

            return newamount;
        }



        //Función para buscar el calculo por horas
        private async Task CreateEarningForHour(string employeeid, string payrollprocessid, string payrollid, int paycycle, DateTime endperioddate, DateTime startperioddate)
        {


            var workcontrol = await _dbContext.EmployeeWorkControlCalendars
                                                .Where(x => x.EmployeeId == employeeid
                                                       && x.CalendarDate >= startperioddate
                                                       && x.CalendarDate <= endperioddate
                                                       && x.StatusWorkControl == StatusWorkControl.Pendint)
                                                .ToListAsync();

            decimal totalhours = workcontrol.Sum(x => x.TotalHour);

            if (totalhours != 0)
            {
                var response = await _dbContext.EmployeeEarningCodes.Where(x => x.EmployeeId == employeeid).OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();


                var earningcodes = await _dbContext.EmployeeEarningCodes.Where(x => x.IsUseCalcHour == true && x.PayrollId == payrollid && x.EmployeeId == employeeid
                                                                               && x.FromDate <= startperioddate
                                                                               && x.ToDate >= endperioddate).ToListAsync();

                int cont = 1;
                foreach (var item in earningcodes)
                {
                    EmployeeEarningCode earningCode = new EmployeeEarningCode()
                    {
                        InternalId = response == null ? 1 : response.InternalId + cont,
                        EmployeeId = employeeid,
                        EarningCodeId = item.EarningCodeId,
                        IndexEarningMonthly = item.IndexEarningHour * totalhours,
                        IndexEarning = item.IndexEarningHour * totalhours,
                        StartPeriodForPaid = paycycle,
                        FromDate = startperioddate,
                        ToDate = endperioddate,
                        IsUseDGT = false,
                        IsUseCalcHour = false,
                        PayrollProcessId = payrollprocessid,
                        QtyPeriodForPaid = 1,
                        PayrollId = payrollid
                    };

                    _dbContext.EmployeeEarningCodes.Add(earningCode);
                    await _dbContext.SaveChangesAsync();

                    cont++;
                }

                cont++;
            }

            //Actualizar el estado de las horas a en proceso
            List<EmployeeWorkControlCalendar> employeeWorks = new List<EmployeeWorkControlCalendar>();

            foreach (var item in workcontrol)
            {
                var entity = item;
                entity.StatusWorkControl = StatusWorkControl.InProcess;
                entity.PayrollProcessId = payrollprocessid;
                employeeWorks.Add(entity);
            }

            if (employeeWorks.Count > 0)
            {
                _dbContext.EmployeeWorkControlCalendars.UpdateRange(employeeWorks);
                await _dbContext.SaveChangesAsync();
            }
        }

        //Función para remover los codigos de ganancia po rhoras
        private async Task<Response<object>> CleanEarningForHour(string payrollprocessid, string payrollid, DateTime endperioddate, DateTime startperioddate)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var earningcodes = await _dbContext.EmployeeEarningCodes.Where(x => x.IsUseCalcHour == false && x.PayrollId == payrollid
                                                                           && x.FromDate <= startperioddate
                                                                           && x.ToDate >= endperioddate
                                                                           && x.PayrollProcessId == payrollprocessid).ToListAsync();
                foreach (var item in earningcodes)
                {
                    var entity = item;
                    _dbContext.EmployeeEarningCodes.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                }

                var workcontrol = await _dbContext.EmployeeWorkControlCalendars
                                                    .Where(x => x.CalendarDate >= startperioddate
                                                           && x.CalendarDate <= endperioddate
                                                           && x.StatusWorkControl == StatusWorkControl.InProcess
                                                           && x.PayrollProcessId == payrollprocessid)
                                                    .ToListAsync();

                List<EmployeeWorkControlCalendar> employeeWorks = new List<EmployeeWorkControlCalendar>();

                foreach (var item in workcontrol)
                {
                    var entity = item;
                    entity.StatusWorkControl = StatusWorkControl.Pendint;
                    entity.PayrollProcessId = "";
                    employeeWorks.Add(entity);
                }

                if (employeeWorks.Count > 0)
                {
                    _dbContext.EmployeeWorkControlCalendars.UpdateRange(employeeWorks);
                    await _dbContext.SaveChangesAsync();
                }

                transaction.Commit();

                return null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }

        //Función para marcar las asistencias como pagadas
        private async Task<bool> UpdateWorkControlToPaid(string payrollprocessid)
        {
            var workcontrol = await _dbContext.EmployeeWorkControlCalendars
                                                .Where(x => x.StatusWorkControl == StatusWorkControl.InProcess
                                                       && x.PayrollProcessId == payrollprocessid)
                                                .ToListAsync();

            List<EmployeeWorkControlCalendar> employeeWorks = new List<EmployeeWorkControlCalendar>();

            foreach (var item in workcontrol)
            {
                var entity = item;
                entity.StatusWorkControl = StatusWorkControl.Paid;
                employeeWorks.Add(entity);
            }

            if (employeeWorks.Count > 0)
            {
                _dbContext.EmployeeWorkControlCalendars.UpdateRange(employeeWorks);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
