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
   public class VariableEntryRepository: IVariableEntryRepository
    {
        public async Task<List<DO_ERCode>> GetActiveERCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpercd
                        .Where(w => w.ActiveStatus==true)
                        .Select(r => new DO_ERCode
                        {
                            Ercode = r.Ercode,
                            Erdesc = r.Erdesc
                        }).OrderBy(o => o.Erdesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_PayPeriod>> GetActivePayPeriodsbyBusinesskey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpypyp
                        .Where(w =>w.BusinessKey== Businesskey && w.ActiveStatus == true)
                        .Select(r => new DO_PayPeriod
                        {
                            PayPeriod = r.PayPeriod
                            
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_VariableEntry>> GetIncentiesbyBusinessKeyPayPeriodAndErCode(int Businesskey, int Payperiod,int Ercode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var result = await db.GtEpyemh.Where(x => x.BusinessKey == Businesskey && x.ActiveStatus == true)
                    .GroupJoin(db.GtEpypve.Where(x => x.BusinessKey == Businesskey && x.PayPeriod == Payperiod && x.Ercode==Ercode),
                     m => m.EmployeeNumber,
                             l => l.EmployeeNumber,
                             (m, l) => new
                             { m, l }).SelectMany(z => z.l.DefaultIfEmpty(),
                             (a, b) => new DO_VariableEntry
                             {
                                 EmployeeName = a.m.Title + "." + a.m.EmployeeName,
                                 EmployeeNumber = a.m.EmployeeNumber,
                                 BusinessKey = a.m.BusinessKey,
                                 Amount = b != null ? b.Amount : 0,
                                 PayPeriod = b != null ? b.PayPeriod : Payperiod,
                                 Ercode = b != null ? b.Ercode : Ercode,
                                 ActiveStatus = b != null ? b.ActiveStatus : false
                             }).OrderBy(o => o.EmployeeName).ToListAsync();

                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateVariableIncentive(List<DO_VariableEntry> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var vi in obj.Where(x =>x.Amount != 0))
                        {
                            GtEpypve v_inc = db.GtEpypve.Where(x => x.BusinessKey == vi.BusinessKey
                                            && x.PayPeriod == vi.PayPeriod && x.EmployeeNumber == vi.EmployeeNumber && x.Ercode==vi.Ercode).FirstOrDefault();
                            if (v_inc == null)
                            {
                                var add = new GtEpypve
                                {
                                    BusinessKey = vi.BusinessKey,
                                    PayPeriod = vi.PayPeriod,
                                    EmployeeNumber = vi.EmployeeNumber,
                                    Ercode = vi.Ercode,
                                    Amount = vi.Amount,
                                    ActiveStatus = vi.ActiveStatus,
                                    FormId = vi.FormId,
                                    CreatedBy = vi.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = vi.TerminalID
                                };
                                db.GtEpypve.Add(add);
                            }
                            else
                            {
                                v_inc.Amount = vi.Amount;
                                v_inc.ActiveStatus = vi.ActiveStatus;
                                v_inc.ModifiedBy = vi.UserID;
                                v_inc.ModifiedOn = System.DateTime.Now;
                                v_inc.ModifiedTerminal = vi.TerminalID;
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


    }
}
