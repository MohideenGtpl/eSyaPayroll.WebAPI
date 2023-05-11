using System;
using System.Collections.Generic;
using System.Text;
using eSyaPayroll.DL.Entities;
using eSyaPayroll.DO;
using eSyaPayroll.IF;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eSyaPayroll.DL.Repository
{
   public class AttendanceProcessRepository: IAttendanceProcessRepository
    {
        #region Attendance Process
        public async Task<List<DO_PayPeriod>> GetPayPeriodbyBusinessKey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pp = await db.GtEpypyp.Where(x => x.BusinessKey == Businesskey && x.IsPayrollFreezed == false && x.ActiveStatus == true)
                          .Select(x => new DO_PayPeriod
                          {
                              PayPeriod = x.PayPeriod,
                              WorkingDays = x.WorkingDays,
                          }).ToListAsync();

                    return pp;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_AttendanceProcess>> GetEmployeesbyBusinessKeyAndPayperiod(int Businesskey, int Payperiod)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var emps= await db.GtEpyemh.Where(x => x.BusinessKey==Businesskey && x.ActiveStatus==true).Join(db.GtEpypyp.Where(x=>x.BusinessKey==Businesskey
                    && x.PayPeriod==Payperiod && x.ActiveStatus==true),
                            h => h.BusinessKey,
                            k => k.BusinessKey,
                           (h, k) => new DO_AttendanceProcess
                           {
                               BusinessKey =h.BusinessKey,
                               EmployeeNumber = h.EmployeeNumber,
                               EmployeeName = h.Title + "." + h.EmployeeName,
                               PayPeriod = k.PayPeriod,
                               TotalDays = k.WorkingDays,
                               WorkingDays = k.WorkingDays,
                               Holidays =  k.Holidays,
                               WeeklyOffs = k.WeeklyOffs,
                               ActiveStatus = h.ActiveStatus
                           }).OrderBy(o => o.EmployeeName).ToListAsync();
                    return emps;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertorUpdateAttendanceProcess(List<DO_AttendanceProcess> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var o = obj.FirstOrDefault();
                        var pp = db.GtEpypyp.Where(w => w.BusinessKey == o.BusinessKey && w.PayPeriod == o.PayPeriod).FirstOrDefault();
                        if (pp != null)
                        {
                            if (pp.IsPayrollFreezed)
                                return new DO_ReturnParameter() { Status = false, Message = "Payroll is Freezed. can't make changes." };
                        }

                        foreach (var a_proc in obj)
                        {
                            GtEpyatp _isExistsproc = db.GtEpyatp.FirstOrDefault(a => a.BusinessKey == a_proc.BusinessKey && a.PayPeriod == a_proc.PayPeriod && a.EmployeeNumber == a_proc.EmployeeNumber);

                            if (_isExistsproc == null)
                            {
                                var add = new GtEpyatp
                                {
                                    BusinessKey = a_proc.BusinessKey,
                                    EmployeeNumber = a_proc.EmployeeNumber,
                                    PayPeriod = a_proc.PayPeriod,
                                    WorkingDays=a_proc.WorkingDays,
                                    Holidays=a_proc.Holidays,
                                    WeeklyOffs=a_proc.WeeklyOffs,
                                    AttendedDays = 0,
                                    AbsentDays = 0,
                                    LateComingDays=0,
                                    ArrearDays=0,
                                    PayableDays=0,
                                    ActiveStatus = a_proc.ActiveStatus,
                                    FormId = a_proc.FormId,
                                    CreatedBy = a_proc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = a_proc.TerminalID
                                };
                                db.GtEpyatp.Add(add);
                            }
                            else
                            {
                                _isExistsproc.WorkingDays = a_proc.WorkingDays;
                                _isExistsproc.Holidays = a_proc.Holidays;
                                _isExistsproc.WeeklyOffs = a_proc.WeeklyOffs;
                                _isExistsproc.ActiveStatus = a_proc.ActiveStatus;
                                _isExistsproc.ModifiedBy = a_proc.UserID;
                                _isExistsproc.ModifiedOn = System.DateTime.Now;
                                _isExistsproc.ModifiedTerminal = a_proc.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Saved Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Attendance Process

        #region Loss of Pay

        public async Task<List<DO_LossofPAY>> GetLossofPaybyBusinessKeyAndPayperiod(int Businesskey, int Payperiod)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var emps = await db.GtEpyatp.Where(x => x.BusinessKey == Businesskey && x.PayPeriod == Payperiod).Join(db.GtEpyemh.Where(x => x.BusinessKey == Businesskey),
                            x => x.EmployeeNumber,
                            y => y.EmployeeNumber,
                           (x, y) => new DO_LossofPAY
                           {
                               BusinessKey=x.BusinessKey,
                               EmployeeNumber=x.EmployeeNumber,
                               PayPeriod=x.PayPeriod,
                               WorkingDays=x.WorkingDays,
                               Holidays=x.Holidays,
                               WeeklyOffs=x.WeeklyOffs,
                               AttendedDays=x.AttendedDays,
                               AbsentDays=x.AbsentDays,
                               LateComingDays=x.LateComingDays,
                               ArrearDays=x.ArrearDays,
                               PayableDays=x.PayableDays,
                               ActiveStatus=x.ActiveStatus,
                               EmployeeName=y.EmployeeName
                           }).OrderBy(o => o.EmployeeName).ToListAsync();
                    return emps;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> UpdateLossofPAY(List<DO_LossofPAY> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        foreach (var l_pay in obj)
                        {
                            GtEpyatp _isExistspay = db.GtEpyatp.FirstOrDefault(a => a.BusinessKey == l_pay.BusinessKey && a.PayPeriod == l_pay.PayPeriod && a.EmployeeNumber == l_pay.EmployeeNumber);

                            if (_isExistspay == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Does n't Exists." };
                            }
                            else
                            {
                                _isExistspay.AbsentDays = l_pay.AbsentDays;
                                _isExistspay.LateComingDays = l_pay.LateComingDays;
                                _isExistspay.ModifiedBy = l_pay.UserID;
                                _isExistspay.ModifiedOn = System.DateTime.Now;
                                _isExistspay.ModifiedTerminal = l_pay.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Saved Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Loss of Pay

        #region Arrear Details
        public async Task<List<DO_EmployeeInfo>> GetEmployeebyBusinessKey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyemh.Where(e => e.ActiveStatus == true && e.BusinessKey == Businesskey)
                        .Select(x => new DO_EmployeeInfo
                        {
                            EmployeeNumber = x.EmployeeNumber,
                            EmployeeName = x.EmployeeName
                        }).OrderBy(x => x.EmployeeName).ToListAsync();

                    return await ds;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ArrearDays>> GetPaidToEmployees(int Businesskey, int Payperiod, int employeeNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyatp.Where(x => x.BusinessKey == Businesskey && x.EmployeeNumber == employeeNumber).Select(
                      x => new DO_ArrearDays
                      {
                          BusinessKey = x.BusinessKey,
                          PayPeriod = x.PayPeriod,
                          EmployeeNumber = x.EmployeeNumber,
                          Lopdays = x.AbsentDays+x.LateComingDays,
                          PaidPeriod = 0,
                          ArrearDays = 0,
                          ActiveStatus = x.ActiveStatus
                      }).ToListAsync();
                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ArrearDays>> GetArreardays(int Businesskey, int Payperiod, int employeeNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyata.Where(x => x.BusinessKey == Businesskey && x.EmployeeNumber == employeeNumber)
                         .Join(db.GtEpyatp,
                            p => new { p.BusinessKey, p.EmployeeNumber, p.PayPeriod },
                            e => new { e.BusinessKey, e.EmployeeNumber, e.PayPeriod },
                            (p, e) => new { p, e })
                        .Select(
                      x => new DO_ArrearDays
                      {
                          BusinessKey = x.p.BusinessKey,
                          PayPeriod = x.p.PayPeriod,
                          EmployeeNumber = x.p.EmployeeNumber,
                          PaidPeriod = x.p.PaidPeriod,
                          Lopdays = x.e.LateComingDays+x.e.AbsentDays,
                          ArrearDays = x.p.ArrearDays,
                          ActiveStatus = x.p.ActiveStatus
                      }).ToListAsync();
                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateArreardays(List<DO_ArrearDays> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var a_proc in obj)
                        {
                            GtEpyatp _isExistsproc = db.GtEpyatp.FirstOrDefault(a => a.BusinessKey == a_proc.BusinessKey && a.PayPeriod == a_proc.PayPeriod && a.EmployeeNumber == a_proc.EmployeeNumber);
                            if (_isExistsproc != null)
                            {
                                _isExistsproc.AbsentDays = a_proc.ArrearDays;
                                _isExistsproc.ModifiedBy = a_proc.UserID;
                                _isExistsproc.ModifiedOn = System.DateTime.Now;
                                _isExistsproc.ModifiedTerminal = a_proc.TerminalID;
                            }

                            GtEpyata _isArdaysExists = db.GtEpyata.FirstOrDefault(a => a.BusinessKey == a_proc.BusinessKey && a.PayPeriod == a_proc.PayPeriod && a.EmployeeNumber == a_proc.EmployeeNumber);
                            if (_isArdaysExists == null)
                            {
                                var add = new GtEpyata
                                {
                                    BusinessKey = a_proc.BusinessKey,
                                    EmployeeNumber = a_proc.EmployeeNumber,
                                    PayPeriod = a_proc.PayPeriod,
                                    PaidPeriod = a_proc.PaidPeriod,
                                    ArrearDays = a_proc.ArrearDays,
                                    ActiveStatus = true,
                                    FormId = a_proc.FormId,
                                    CreatedBy = a_proc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = a_proc.TerminalID
                                };
                                db.GtEpyata.Add(add);
                            }
                            else
                            {
                                _isArdaysExists.ArrearDays = a_proc.ArrearDays;
                                _isArdaysExists.ActiveStatus = true;
                                _isArdaysExists.ModifiedBy = a_proc.UserID;
                                _isArdaysExists.ModifiedOn = System.DateTime.Now;
                                _isArdaysExists.ModifiedTerminal = a_proc.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Saved Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Arrear Details
    }
}
