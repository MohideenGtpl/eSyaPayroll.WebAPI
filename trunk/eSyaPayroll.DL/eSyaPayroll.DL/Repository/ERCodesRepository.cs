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
   public class ERCodesRepository:IERCodesRepository
    {
        #region ER Codes
        public async Task<List<DO_ERCodes>> GetAllERCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpercd.Select(
                         x=> new DO_ERCodes
                       {
                           Ercode = x.Ercode,
                           Erdesc = x.Erdesc,
                           ActiveStatus = x.ActiveStatus,
                       }).OrderBy(o => o.Erdesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ERCodes>> GetActiveERCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpercd
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_ERCodes
                        {
                            Ercode = r.Ercode,
                            Erdesc = r.Erdesc,
                            ActiveStatus = r.ActiveStatus
                        }).OrderBy(o => o.Erdesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ERCodes> GetERCodebyERCode(int ERcode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpercd
                        .Where(w => w.Ercode == ERcode)
                         .Select(r => new DO_ERCodes
                         {
                             Ercode = r.Ercode,
                             Erdesc = r.Erdesc,
                             ActiveStatus = r.ActiveStatus,
                             l_ERCodeParameter = db.GtEperpr.Where(x=>x.Ercode==ERcode).Select(p => new DO_eSyaParameter
                             {
                                 ParameterID = p.ParameterId,
                                 ParmAction = p.ParmAction
                             }).ToList()
                         }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoERCodes(DO_ERCodes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var Ercde_Exits = db.GtEpercd.Where(w => w.Ercode == obj.Ercode).Count();
                        if (Ercde_Exits > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "ER Code is already Exists" };
                        }

                        bool is_Ercdedesc_Exist = db.GtEpercd.Any(a => a.Erdesc.ToUpper().Replace(" ", "") == obj.Erdesc.ToUpper().Replace(" ", ""));
                        if (is_Ercdedesc_Exist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "ER Code Description is already Exists" };
                        }
                        var Er_code = new GtEpercd
                        {
                            Ercode = obj.Ercode,
                            Erdesc = obj.Erdesc,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEpercd.Add(Er_code);
                        foreach (var p in obj.l_ERCodeParameter)
                        {
                            var er_parm = new GtEperpr
                            {
                                Ercode = obj.Ercode,
                                ParameterId = p.ParameterID,
                                ParmAction = p.ParmAction,
                                ActiveStatus = obj.ActiveStatus,
                                ParmPerc = 0,
                                ParmDesc=null,
                                ParmValue=0,
                                FormId= obj.FormId,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                                CreatedBy = obj.UserID,
                            };
                            db.GtEperpr.Add(er_parm);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "ER Code Created Successfully" };
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

        public async Task<DO_ReturnParameter> UpdateERCodes(DO_ERCodes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_Ercdedesc_Exist = db.GtEpercd.Any(a => a.Ercode != obj.Ercode && a.Erdesc.ToUpper().Trim().Replace(" ", "") == obj.Erdesc.ToUpper().Trim().Replace(" ", ""));
                        if (is_Ercdedesc_Exist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "ER Code Description is already Exists" };
                        }

                        GtEpercd Er_code = db.GtEpercd.Where(w => w.Ercode == obj.Ercode).FirstOrDefault();
                        if (Er_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "ER Code is not exist" };
                        }

                        Er_code.Erdesc = obj.Erdesc;
                        Er_code.ActiveStatus = obj.ActiveStatus;
                        Er_code.ModifiedBy = obj.UserID;
                        Er_code.ModifiedOn = System.DateTime.Now;
                        Er_code.ModifiedTerminal = obj.TerminalID;
                        foreach (var p in obj.l_ERCodeParameter)
                        {
                            var is_ErparamExists = db.GtEperpr.Where(w => w.Ercode == obj.Ercode && w.ParameterId == p.ParameterID).FirstOrDefault();
                            if (is_ErparamExists == null)
                            {
                               var er_param = new GtEperpr
                                {
                                    Ercode = obj.Ercode,
                                    ParameterId = p.ParameterID,
                                    ParmAction = p.ParmAction,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId=obj.FormId,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,
                                    CreatedBy = obj.UserID,
                                };
                                db.GtEperpr.Add(er_param);
                            }
                            else
                            {
                                is_ErparamExists.ParmAction = p.ParmAction;
                                is_ErparamExists.ActiveStatus = obj.ActiveStatus;
                                is_ErparamExists.ModifiedBy = obj.UserID;
                                is_ErparamExists.ModifiedOn = System.DateTime.Now;
                                is_ErparamExists.ModifiedTerminal = obj.TerminalID;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = " ER Code Updated Successfully." };
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

        #endregion ER Codes

        #region ER Rules
        public async Task<List<DO_ERRules>> GetERRulesbyERCode(int ERCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (ERCode == 0)
                    {

                        return await db.GtEperru.Where(x=> x.EffectiveTill != null).Join(db.GtEpercd,
                              h => h.Ercode,
                              k => k.Ercode,
                             (h, k) => new DO_ERRules
                             {
                                 Ercode = h.Ercode,
                                 Erdesc = k.Erdesc,
                                 EffectiveFrom = h.EffectiveFrom,
                                 EffectiveTill = h.EffectiveTill,
                                 IsRuleBased = h.IsRuleBased,
                                 PayRule = h.PayRule,
                                 PayRuleDesc = h.PayRuleDesc,
                                 ActiveStatus = h.ActiveStatus
                             }).ToListAsync();


                    }
                    else
                    {
                        return await db.GtEperru.Where(x => x.Ercode == ERCode).Join(db.GtEpercd,
                             h => h.Ercode,
                             k => k.Ercode,
                            (h, k) => new DO_ERRules
                            {
                                Ercode = h.Ercode,
                                Erdesc = k.Erdesc,
                                EffectiveFrom = h.EffectiveFrom,
                                EffectiveTill = h.EffectiveTill,
                                IsRuleBased = h.IsRuleBased,
                                PayRule = h.PayRule,
                                PayRuleDesc = h.PayRuleDesc,
                                ActiveStatus = h.ActiveStatus
                            }).ToListAsync();
                    }


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateERRules(DO_ERRules obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_ruleExists = db.GtEperru.Where(r => r.Ercode == obj.Ercode && r.EffectiveFrom.Date == obj.EffectiveFrom.Date).FirstOrDefault();
                        if (is_ruleExists == null)
                        {
                            var er_rules = new GtEperru
                            {
                                Ercode = obj.Ercode,
                                EffectiveFrom = obj.EffectiveFrom,
                                EffectiveTill = obj.EffectiveTill,
                                IsRuleBased = obj.IsRuleBased,
                                PayRule = obj.PayRule,
                                PayRuleDesc = obj.PayRuleDesc,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEperru.Add(er_rules);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "ER Rule Created Successfully." };
                        }
                        else
                        {
                            is_ruleExists.Ercode = obj.Ercode;
                            is_ruleExists.EffectiveFrom = obj.EffectiveFrom;
                            is_ruleExists.EffectiveTill = obj.EffectiveTill;
                            is_ruleExists.IsRuleBased = obj.IsRuleBased;
                            is_ruleExists.PayRule = obj.PayRule;
                            is_ruleExists.PayRuleDesc = obj.PayRuleDesc;
                            is_ruleExists.ActiveStatus = obj.ActiveStatus;
                            is_ruleExists.ModifiedBy = obj.UserID;
                            is_ruleExists.ModifiedOn = System.DateTime.Now;
                            is_ruleExists.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "ER Rule Updated Successfully." };
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

        //public async Task<List<DO_ERRules>> GetERRulesbyERCode(int ERCode)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            if (ERCode == 0)
        //            {

        //                return await db.GtEpercd.Where(x=>x.ActiveStatus==true)
        //                   .GroupJoin(db.GtEperru.Where(x=>x.EffectiveTill !=null),
        //                   m => m.Ercode,
        //                   l => l.Ercode,
        //                   (m, l) => new
        //                   { m,l}).SelectMany(z=>z.l.DefaultIfEmpty(),
        //                   (a,b)=> new DO_ERRules
        //                   {
        //                       Ercode = a.m.Ercode,
        //                       Erdesc = a.m.Erdesc,
        //                       EffectiveFrom =b != null ? b.EffectiveFrom : DateTime.Now,
        //                       EffectiveTill = b != null ? b.EffectiveTill : null,
        //                       IsRuleBased = b != null ? b.IsRuleBased : false,
        //                       PayRule = b != null ? b.PayRule : "",
        //                       PayRuleDesc = b != null ? b.PayRuleDesc : "",
        //                       ActiveStatus = b != null ? b.ActiveStatus : false
        //                   }).ToListAsync();


        //            }
        //            else
        //            {
        //                return await db.GtEperru.Where(x=> x.Ercode == ERCode && x.EffectiveTill!=null).Join(db.GtEpercd,
        //                     h => h.Ercode,
        //                     k => k.Ercode,
        //                    (h, k) => new DO_ERRules
        //                    {
        //                        Ercode = h.Ercode,
        //                        Erdesc = k.Erdesc,
        //                        EffectiveFrom = h.EffectiveFrom,
        //                        EffectiveTill = h.EffectiveTill,
        //                        IsRuleBased = h.IsRuleBased,
        //                        PayRule = h.PayRule,
        //                        PayRuleDesc = h.PayRuleDesc,
        //                        ActiveStatus = h.ActiveStatus
        //                    }).ToListAsync();
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion ER Rules

        #region ER Rates
        public async Task<List<DO_ERRates>> GetERRatesbyERCode(int ERCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                   return await db.GtEperrt.Where(x => x.Ercode == ERCode).Join(db.GtEpercd,
                             x => x.Ercode,
                             y => y.Ercode,
                            (x, y) => new DO_ERRates
                            {
                                Ercode = y.Ercode,
                                Erdesc = y.Erdesc,
                                EffectiveFrom = x.EffectiveFrom,
                                EffectiveTill = x.EffectiveTill,
                                RangeFrom = x.RangeFrom,
                                RangeTill = x.RangeTill,
                                AmountToDeduct = x.AmountToDeduct,
                                ActiveStatus = x.ActiveStatus
                            }).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateERRates(DO_ERRates obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_rateExists = db.GtEperrt.Where(r => r.Ercode == obj.Ercode && r.EffectiveFrom.Date == obj.EffectiveFrom.Date).FirstOrDefault();
                        if (is_rateExists == null)
                        {
                            var er_rates = new GtEperrt
                            {
                                Ercode = obj.Ercode,
                                EffectiveFrom = obj.EffectiveFrom,
                                EffectiveTill = obj.EffectiveTill,
                                RangeFrom = obj.RangeFrom,
                                RangeTill = obj.RangeTill,
                                AmountToDeduct = obj.AmountToDeduct,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEperrt.Add(er_rates);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "ER Rate Created Successfully." };
                        }
                        else
                        {
                            is_rateExists.Ercode = obj.Ercode;
                            is_rateExists.EffectiveFrom = obj.EffectiveFrom;
                            is_rateExists.EffectiveTill = obj.EffectiveTill;
                            is_rateExists.RangeFrom = obj.RangeFrom;
                            is_rateExists.RangeTill = obj.RangeTill;
                            is_rateExists.AmountToDeduct = obj.AmountToDeduct;
                            is_rateExists.ActiveStatus = obj.ActiveStatus;
                            is_rateExists.ModifiedBy = obj.UserID;
                            is_rateExists.ModifiedOn = System.DateTime.Now;
                            is_rateExists.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "ER Rate Updated Successfully." };
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

        #endregion ER Rates
    }
}
