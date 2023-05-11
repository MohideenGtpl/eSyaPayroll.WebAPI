﻿using eSyaPayroll.DL.Entities;
using eSyaPayroll.DO;
using eSyaPayroll.IF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eSyaPayroll.DL.Repository
{
   public class EmployeeRepository:IEmployeeRepository
    {
        #region Employee Details Info

        public async Task<List<DO_EmployeeClass>> GetEmployeeClassforCombo()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEbeemc
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_EmployeeClass
                        {
                            EmployeeClass=r.EmployeeClass,
                             ClassDesc=r.ClassDesc,

                        }).OrderBy(o => o.ClassDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DO_EmployeeInfo> GetEmployeesInfobySuffix(string Alphabet)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    List<GtEpyemh> emp_list = new List<GtEpyemh>();

                    if (!string.IsNullOrEmpty(Alphabet))
                    {

                        emp_list = db.GtEpyemh.Where(x => x.EmployeeName.ToUpper().Trim().StartsWith(Alphabet.ToUpper().Trim())).ToList();
                    }
                    if (Alphabet == "All")
                    {
                        emp_list = db.GtEpyemh.ToList();
                    }
                    var result = emp_list.
                     Select(x => new DO_EmployeeInfo
                     {
                         BusinessKey=x.BusinessKey,
                         EmployeeNumber=x.EmployeeNumber,
                         EmployeeId=x.EmployeeId,
                         BiometricId=x.BiometricId,
                         Title=x.Title,
                         EmployeeName=x.EmployeeName,
                         Gender=x.Gender,
                         MobileNumber=x.MobileNumber,
                         
                         ExemptedFromAttendance=x.ExemptedFromAttendance,
                         WillingnessToWorkInShifts=x.WillingnessToWorkInShifts,
                         EmployeeStatus=x.EmployeeStatus,
                         ActiveStatus = x.ActiveStatus,
                         FormId = x.FormId
                     }).OrderBy(x => x.EmployeeName).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DO_EmployeeInfo> GetEmployeeInfobyEmployeeNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                   var ds = db.GtEpyemh.Where(x=>x.EmployeeNumber==EmpNumber).
                   Join(db.GtEpyemc, u => u.EmployeeNumber, uir => uir.EmployeeNumber,
                   (u, uir) => new { u, uir }).
                   Join(db.GtEpyems, r => r.uir.EmployeeNumber, ro => ro.EmployeeNumber, (r, ro) => new { r, ro })
                  .Select(m => new DO_EmployeeInfo
                  {
                      //Employee Details
                      BusinessKey = m.r.u.BusinessKey,
                      EmployeeNumber = m.r.u.EmployeeNumber,
                      EmployeeId = m.r.u.EmployeeId,
                      BiometricId = m.r.u.BiometricId,
                      Title = m.r.u.Title,
                      EmployeeName = m.r.u.EmployeeName,
                      Gender = m.r.u.Gender,
                      MobileNumber = m.r.u.MobileNumber,
                      ExemptedFromAttendance = m.r.u.ExemptedFromAttendance,
                      WillingnessToWorkInShifts = m.r.u.WillingnessToWorkInShifts,
                      EmployeeStatus = m.r.u.EmployeeStatus,
                      ActiveStatus = m.r.u.ActiveStatus,
                      Photo = m.r.u.Photo,

                      //Employee Classification
                      EmployeeClass = m.r.uir.EmployeeClass,
                      EmployeeGroup = m.r.uir.EmployeeGroup,
                      EmployeePayCategory = m.r.uir.EmployeePayCategory,
                      DateProbationTill = m.r.uir.DateProbationTill,

                      //Employee Status
                      DateOfJoining = m.ro.DateOfJoining,
                      DateOfConfirmation = m.ro.DateOfConfirmation,
                      DateOfResignation = m.ro.DateOfResignation,
                      DateOfRelieving = m.ro.DateOfRelieving,
                      DateOfTermination = m.ro.DateOfTermination,
                      AnySuspension = m.ro.AnySuspension,
                      CurrentDepartment = m.ro.CurrentDepartment,
                      CurrentLocationPosted = m.ro.CurrentLocationPosted,
                      CurrentBasic = m.ro.CurrentBasic,
                      CurrentGross = m.ro.CurrentGross,
                     }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeeInfo(DO_EmployeeInfo obj)
        {
            try
            {
                if (obj.EmployeeNumber != 0)
                {
                    return await UpdateEmployeeInfo(obj);
                }
                else
                {
                    return await InsertIntoEmployeeInfo(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoEmployeeInfo(DO_EmployeeInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEpyemh is_empIdExists = db.GtEpyemh.FirstOrDefault(emp => emp.EmployeeId.ToUpper().Replace(" ", "") == obj.EmployeeId.ToUpper().Replace(" ", ""));
                        if (is_empIdExists == null)
                        {
                            int maxval = db.GtEpyemh.Select(e => e.EmployeeNumber).DefaultIfEmpty().Max();
                            int Emp_number = maxval + 1;
                            var emp_info = new GtEpyemh
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = Emp_number,
                                EmployeeId = obj.EmployeeId,
                                BiometricId = obj.BiometricId,
                                Title = obj.Title,
                                EmployeeName = obj.EmployeeName,
                                Gender=obj.Gender,
                                MobileNumber=obj.MobileNumber,
                                ExemptedFromAttendance=obj.ExemptedFromAttendance,
                                WillingnessToWorkInShifts=obj.WillingnessToWorkInShifts,
                                Photo=obj.Photo,
                                EmployeeStatus=obj.EmployeeStatus,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyemh.Add(emp_info);

                            var emp_classification = new GtEpyemc
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = Emp_number,
                                EmployeeGroup=obj.EmployeeGroup,
                                EmployeeClass=obj.EmployeeClass,
                                EmployeePayCategory=obj.EmployeePayCategory,
                                DateProbationTill=obj.DateProbationTill,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyemc.Add(emp_classification);

                            var emp_status = new GtEpyems
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = Emp_number,
                                DateOfJoining=obj.DateOfJoining,
                                DateOfConfirmation=obj.DateOfConfirmation,
                                DateOfResignation=obj.DateOfResignation,
                                DateOfRelieving=obj.DateOfRelieving,
                                DateOfTermination=obj.DateOfTermination,
                                AnySuspension=obj.AnySuspension,
                                CurrentDepartment=obj.CurrentDepartment,
                                CurrentLocationPosted=obj.CurrentLocationPosted,
                                CurrentBasic=obj.CurrentBasic,
                                CurrentGross=obj.CurrentGross,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyems.Add(emp_status);

                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, EmpNumber = Emp_number,Businesskey= obj.BusinessKey, Message = "Employee Created Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Employee Id is already Exists try another one." };
                        }
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

        public async Task<DO_ReturnParameter> UpdateEmployeeInfo(DO_EmployeeInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try
                    {
                        GtEpyemh is_empIdExists = db.GtEpyemh.FirstOrDefault(emp => emp.EmployeeNumber != obj.EmployeeNumber && emp.EmployeeId.ToUpper().Replace(" ", "") == obj.EmployeeId.ToUpper().Replace(" ", ""));
                        if (is_empIdExists == null)
                        {
                            var emp_info = db.GtEpyemh.FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber);
                            if (obj != null)
                            {
                                    emp_info.EmployeeId = obj.EmployeeId;
                                    emp_info.BiometricId = obj.BiometricId;
                                    emp_info.Title = obj.Title;
                                    emp_info.EmployeeName = obj.EmployeeName;
                                    emp_info.Gender = obj.Gender;
                                    emp_info.MobileNumber = obj.MobileNumber;
                                    emp_info.ExemptedFromAttendance = obj.ExemptedFromAttendance;
                                    emp_info.WillingnessToWorkInShifts = obj.WillingnessToWorkInShifts;
                                    emp_info.Photo = obj.Photo;
                                    emp_info.EmployeeStatus = obj.EmployeeStatus;
                                    emp_info.ActiveStatus = obj.ActiveStatus;
                                    emp_info.ModifiedBy = obj.UserID;
                                    emp_info.ModifiedOn = DateTime.Now;
                                    emp_info.ModifiedTerminal = obj.TerminalID;

                                var emp_classification= db.GtEpyemc.FirstOrDefault(c => c.EmployeeNumber == obj.EmployeeNumber);
                                    emp_classification.EmployeeGroup = obj.EmployeeGroup;
                                    emp_classification.EmployeeClass = obj.EmployeeClass;
                                    emp_classification.EmployeePayCategory = obj.EmployeePayCategory;
                                    emp_classification.DateProbationTill = obj.DateProbationTill;
                                    emp_classification.ModifiedBy = obj.UserID;
                                    emp_classification.ModifiedOn = DateTime.Now;
                                    emp_classification.ModifiedTerminal = obj.TerminalID;

                                var emp_status = db.GtEpyems.FirstOrDefault(s => s.EmployeeNumber == obj.EmployeeNumber);
                                    emp_status.DateOfJoining = obj.DateOfJoining;
                                    emp_status.DateOfConfirmation = obj.DateOfConfirmation;
                                    emp_status.DateOfResignation = obj.DateOfResignation;
                                    emp_status.DateOfRelieving = obj.DateOfRelieving;
                                    emp_status.DateOfTermination = obj.DateOfTermination;
                                    emp_status.AnySuspension = obj.AnySuspension;
                                    emp_status.CurrentDepartment = obj.CurrentDepartment;
                                    emp_status.CurrentLocationPosted = obj.CurrentLocationPosted;
                                    emp_status.CurrentBasic = obj.CurrentBasic;
                                    emp_status.CurrentGross = obj.CurrentGross;
                                    emp_status.ModifiedBy = obj.UserID;
                                    emp_status.ModifiedOn = DateTime.Now;
                                    emp_status.ModifiedTerminal = obj.TerminalID;

                                await db.SaveChangesAsync();

                                dbContext.Commit();

                                return new DO_ReturnParameter() { Status = true, EmpNumber = obj.EmployeeNumber, Businesskey = obj.BusinessKey, Message = "Employee Updated Successfully." };
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Couldn't find Employee Details." };
                            }
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Employee Id is already Exists try another one." };
                        }
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveEmployee(bool status, int EmpNo)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEpyemh emp = db.GtEpyemh.Where(x => x.EmployeeNumber == EmpNo).FirstOrDefault();
                        if (emp == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Employee is not exist" };
                        }

                        emp.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Employee De Activated Successfully." };
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
        #endregion Employee Details Info

        #region Employee Personal Info

        public async Task<List<DO_ApplicationCodes>> GetLanguagebyCodeType(int CodeType)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcapcd.Where(x => x.CodeType == CodeType && x.ActiveStatus == true)

                            .Select(c => new DO_ApplicationCodes
                            {
                                ApplicationCode = c.ApplicationCode,
                                CodeDesc = c.CodeDesc
                            }).OrderBy(y => y.CodeDesc).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EmployeeLanguage>> GetEmployeeLanguagebyEmpNumber(int EmpNumber)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEpyeln.Where(x => x.EmployeeNumber == EmpNumber).
                      Select(l => new DO_EmployeeLanguage
                      {
                          BusinessKey = l.BusinessKey,
                          EmployeeNumber = l.EmployeeNumber,
                          Language = l.Language,
                          Speak = l.Speak,
                          Reads = l.Reads,
                          Write = l.Write
                      }).ToListAsync();
                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DO_EmployeePersonalInfo> GetEmployeePersonalInfobyEmployeeNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyepi.Where(x => x.EmployeeNumber == EmpNumber) .
                        Join(db.GtEpyeci, u => u.EmployeeNumber, uir => uir.EmployeeNumber,
                        (u, uir) => new { u, uir })
                   .Select(x => new DO_EmployeePersonalInfo
                   {
                      //Employee Personal Info
                       BusinessKey = x.u.BusinessKey,
                       EmployeeNumber = x.u.EmployeeNumber,
                       DateOfBirth=x.u.DateOfBirth,
                       BloodGroup=x.u.BloodGroup,
                       MotherTongue=x.u.MotherTongue,
                       Religion=x.u.Religion,
                       Caste=x.u.Caste,
                       EmployeeUniqueId=x.u.EmployeeUniqueId,
                       EmployeeUniqueInfo=x.u.EmployeeUniqueInfo,
                       //Employee Classification
                       PermanentOrCurrent= x.uir.PermanentOrCurrent,
                       Address = x.uir.Address,
                       City= x.uir.City,
                       Pincode = x.uir.Pincode,
                       State = x.uir.State,
                       Country= x.uir.Country,
                       LandLineNumber= x.uir.LandLineNumber,
                       PermanenActiveStatus= x.uir.ActiveStatus
                   }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeePersonalInfo(DO_EmployeePersonalInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEpyepi is_personalinfoExists = db.GtEpyepi.FirstOrDefault(emp => emp.EmployeeNumber == obj.EmployeeNumber && emp.BusinessKey==obj.BusinessKey);
                        if (is_personalinfoExists == null)
                        {
                           
                            var personal_info = new GtEpyepi
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                DateOfBirth = obj.DateOfBirth,
                                BloodGroup = obj.BloodGroup,
                                MotherTongue = obj.MotherTongue,
                                Religion = obj.Religion,
                                Caste = obj.Caste,
                                EmployeeUniqueId = obj.EmployeeUniqueId,
                                EmployeeUniqueInfo = obj.EmployeeUniqueInfo,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyepi.Add(personal_info);
                        }
                        else
                        {
                            is_personalinfoExists.DateOfBirth = obj.DateOfBirth;
                            is_personalinfoExists.BloodGroup = obj.BloodGroup;
                            is_personalinfoExists.MotherTongue = obj.MotherTongue;
                            is_personalinfoExists.Religion = obj.Religion;
                            is_personalinfoExists.Caste = obj.Caste;
                            is_personalinfoExists.EmployeeUniqueId = obj.EmployeeUniqueId;
                            is_personalinfoExists.EmployeeUniqueInfo = obj.EmployeeUniqueInfo;
                            is_personalinfoExists.ModifiedBy = obj.UserID;
                            is_personalinfoExists.ModifiedOn = DateTime.Now;
                            is_personalinfoExists.ModifiedTerminal = obj.TerminalID;
                        }

                        GtEpyeci is_continfoExists = db.GtEpyeci.FirstOrDefault(con => con.EmployeeNumber == obj.EmployeeNumber && con.BusinessKey == obj.BusinessKey &&
                        con.PermanentOrCurrent.ToUpper().Replace(" ", "") == obj.PermanentOrCurrent.ToUpper().Replace(" ", ""));

                        if (is_continfoExists == null)
                        {

                            var contact_info = new GtEpyeci
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                PermanentOrCurrent = obj.PermanentOrCurrent,
                                Address = obj.Address,
                                City = obj.City,
                                Pincode = obj.Pincode,
                                State = obj.State,
                                Country = obj.Country,
                                LandLineNumber = obj.LandLineNumber,
                                ActiveStatus= obj.PermanenActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyeci.Add(contact_info);
                        }
                        else
                        {
                            is_continfoExists.Address = obj.Address;
                            is_continfoExists.City = obj.City;
                            is_continfoExists.Pincode = obj.Pincode;
                            is_continfoExists.State = obj.State;
                            is_continfoExists.Country = obj.Country;
                            is_continfoExists.LandLineNumber = obj.LandLineNumber;
                            is_continfoExists.ActiveStatus = obj.PermanenActiveStatus;
                            is_continfoExists.ModifiedBy = obj.UserID;
                            is_continfoExists.ModifiedOn = DateTime.Now;
                            is_continfoExists.ModifiedTerminal = obj.TerminalID;
                        }


                        List<GtEpyeln> emp_languge = db.GtEpyeln.Where(c => c.EmployeeNumber == obj.EmployeeNumber && c.BusinessKey==obj.BusinessKey).ToList();
                        if (obj.EmplanguageList != null)
                        {
                            if (emp_languge.Count > 0)
                            {
                                foreach (var item in emp_languge)
                                {
                                    db.GtEpyeln.Remove(item);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var lang in obj.EmplanguageList)
                            {
                                GtEpyeln objkeys = new GtEpyeln
                                {
                                    BusinessKey = obj.BusinessKey,
                                    EmployeeNumber = obj.EmployeeNumber,
                                    Language = lang.Language,
                                    Speak = lang.Speak,
                                    Reads = lang.Reads,
                                    Write = lang.Write,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEpyeln.Add(objkeys);
                                await db.SaveChangesAsync();

                            }

                        }
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true,  Message = "Employee Personal Info Created/Updated Successfully." };
                        
                       
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

        #endregion Employee Personal Info

        #region Employee Bank Info
        public async Task<List<DO_ERCode>> GetERCodeforCombo()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpercd
                        .Where(w => w.ActiveStatus)
                        .Select(e => new DO_ERCode
                        {
                            Ercode = e.Ercode,
                            Erdesc = e.Erdesc,

                        }).OrderBy(o => o.Erdesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EmployeeBankInfo>> GetEmployeeBankInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyebi.Where(x=>x.EmployeeNumber==EmpNumber)
                        .Select(b => new DO_EmployeeBankInfo
                        {
                            BusinessKey =b.BusinessKey,
                            EmployeeNumber=b.EmployeeNumber,
                            AccountNumber=b.AccountNumber,
                            SalaryPaymentMode=b.SalaryPaymentMode,
                            BankCode=b.BankCode,
                            Ifsccode=b.Ifsccode,
                            BankBranch=b.BankBranch,
                            AccountStatus=b.AccountStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertEmployeeBankInfo(DO_EmployeeBankInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEpyebi is_accountNumberExists = db.GtEpyebi.FirstOrDefault(ac => ac.AccountNumber.ToUpper().Replace(" ", "") == obj.AccountNumber.ToUpper().Replace(" ", ""));
                        if (is_accountNumberExists == null)
                        {
                            var bank_info = new GtEpyebi
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                AccountNumber = obj.AccountNumber,
                                SalaryPaymentMode = obj.SalaryPaymentMode,
                                BankCode = obj.BankCode,
                                Ifsccode = obj.Ifsccode,
                                BankBranch = obj.BankBranch,
                                AccountStatus = obj.AccountStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyebi.Add(bank_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true,  Message = "Bank Information Created Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Account Number is already Exists try another one." };
                        }
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

        public async Task<DO_ReturnParameter> UpdateEmployeeBankInfo(DO_EmployeeBankInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try
                    {
                        GtEpyebi is_accountNumberExists = db.GtEpyebi.FirstOrDefault(ac => ac.EmployeeNumber != obj.EmployeeNumber && ac.AccountNumber.ToUpper().Replace(" ", "") == obj.AccountNumber.ToUpper().Replace(" ", ""));
                        if (is_accountNumberExists == null)
                        {
                            var bank_info = db.GtEpyebi.FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber);
                            if (bank_info != null)
                            {
                                bank_info.AccountNumber = obj.AccountNumber;
                                bank_info.SalaryPaymentMode = obj.SalaryPaymentMode;
                                bank_info.BankCode = obj.BankCode;
                                bank_info.Ifsccode = obj.Ifsccode;
                                bank_info.BankBranch = obj.BankBranch;
                                bank_info.AccountStatus = obj.AccountStatus;
                                bank_info.ModifiedBy = obj.UserID;
                                bank_info.ModifiedOn = DateTime.Now;
                                bank_info.ModifiedTerminal = obj.TerminalID;
                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, Message = "Bank Information Updated Successfully." };
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Couldn't find Bank Information." };
                            }
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Account Number is already Exists try another one." };
                        }
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
        #endregion Employee Bank Info

        #region Employee Salary Info

        public async Task<List<DO_EmployeeSalaryInfo>> GetEmployeeSalaryInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyesd.Where(x=>x.EmployeeNumber==EmpNumber)
                        .Select(s => new DO_EmployeeSalaryInfo
                        {
                            BusinessKey = s.BusinessKey,
                            EmployeeNumber = s.EmployeeNumber,
                            Ercode = s.Ercode,
                            Amount=s.Amount,
                            EffectiveFrom = s.EffectiveFrom,
                            EffectiveTill = s.EffectiveTill,
                            ActiveStatus=s.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeeSalaryInfo(DO_EmployeeSalaryInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEpyesd is_salExists = db.GtEpyesd.FirstOrDefault(sl =>sl.BusinessKey== obj.BusinessKey&&sl.EmployeeNumber== obj.EmployeeNumber
                        && sl.Ercode == obj.Ercode && sl.EffectiveFrom== obj.EffectiveFrom);
                        if (is_salExists == null)
                        {
                            var sal_info = new GtEpyesd
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                Ercode = obj.Ercode,
                                EffectiveFrom = obj.EffectiveFrom,
                                Amount = obj.Amount,
                                EffectiveTill = obj.EffectiveTill,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyesd.Add(sal_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Salary Information Created Successfully." };
                        }
                        else
                        {
                            is_salExists.Amount = obj.Amount;
                            is_salExists.EffectiveTill = obj.EffectiveTill;
                            is_salExists.ActiveStatus = obj.ActiveStatus;
                            is_salExists.ModifiedBy = obj.UserID;
                            is_salExists.ModifiedOn = DateTime.Now;
                            is_salExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Salary Information Updated Successfully." };

                        }
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

        #endregion Employee Salary Info

        #region Employee Family Info

        public async Task<List<DO_EmployeeFamilyInfo>> GetEmployeeFamilyInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyefi.Where(x => x.EmployeeNumber == EmpNumber)
                        .Select(f => new DO_EmployeeFamilyInfo
                        {
                            BusinessKey = f.BusinessKey,
                            EmployeeNumber = f.EmployeeNumber,
                            //MaritalStatus = f.MaritalStatus,
                            SpouseName = f.SpouseName,
                            NoOfChildren = f.NoOfChildren,
                            FatherName = f.FatherName,
                            MotherName = f.MotherName
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeeFamilyInfo(DO_EmployeeFamilyInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEpyefi is_familyExists = db.GtEpyefi.FirstOrDefault(sl => sl.BusinessKey == obj.BusinessKey && sl.EmployeeNumber == obj.EmployeeNumber);
                        
                        if (is_familyExists == null)
                        {
                            var family_info = new GtEpyefi
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                //MaritalStatus = obj.MaritalStatus,
                                MaritalStatus = false,
                                SpouseName = obj.SpouseName,
                                NoOfChildren = obj.NoOfChildren,
                                FatherName = obj.FatherName,
                                MotherName = obj.MotherName,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyefi.Add(family_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Family Information Created Successfully." };
                        }
                        else
                        {
                            //is_familyExists.MaritalStatus = obj.MaritalStatus;
                            is_familyExists.SpouseName = obj.SpouseName;
                            is_familyExists.NoOfChildren = obj.NoOfChildren;
                            is_familyExists.FatherName = obj.FatherName;
                            is_familyExists.MotherName = obj.MotherName;
                            is_familyExists.ModifiedBy = obj.UserID;
                            is_familyExists.ModifiedOn = DateTime.Now;
                            is_familyExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Family Information Updated Successfully." };

                        }
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

        #endregion Employee Family Info

        #region Employee Education Info

        public async Task<List<DO_EmployeeEducationInfo>> GetEmployeeEducationInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyeei.Where(x => x.EmployeeNumber == EmpNumber)
                        .Select(e => new DO_EmployeeEducationInfo
                        {
                            BusinessKey = e.BusinessKey,
                            EmployeeNumber = e.EmployeeNumber,
                            EducationLevel = e.EducationLevel,
                            Institution = e.Institution,
                            University = e.University,
                            YearofPassing = e.YearofPassing,
                            PercentageofMarks = e.PercentageofMarks
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeeEducationInfo(DO_EmployeeEducationInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEpyeei is_eduExists = db.GtEpyeei.FirstOrDefault(e => e.BusinessKey == obj.BusinessKey && e.EmployeeNumber == obj.EmployeeNumber
                        && e.EducationLevel.ToUpper().Replace(" ", "") == obj.EducationLevel.ToUpper().Replace(" ", ""));
                        if (is_eduExists == null)
                        {
                            var edu_info = new GtEpyeei
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                EducationLevel = obj.EducationLevel,
                                Institution=obj.Institution,
                                University = obj.University,
                                YearofPassing = obj.YearofPassing,
                                PercentageofMarks = obj.PercentageofMarks,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyeei.Add(edu_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Educational Information Created Successfully." };
                        }
                        else
                        {
                            is_eduExists.Institution = obj.Institution;
                            is_eduExists.University = obj.University;
                            is_eduExists.YearofPassing = obj.YearofPassing;
                            is_eduExists.PercentageofMarks = obj.PercentageofMarks;
                            is_eduExists.ModifiedBy = obj.UserID;
                            is_eduExists.ModifiedOn = DateTime.Now;
                            is_eduExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Educational Information Updated Successfully." };

                        }
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

        #endregion Employee Education Info

        #region Employee Previous Job Info

        public async Task<List<DO_EmployeePreviousJobInfo>> GetEmployeePreviousJobInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyepe.Where(x => x.EmployeeNumber == EmpNumber)
                        .Select(p => new DO_EmployeePreviousJobInfo
                        {
                            BusinessKey = p.BusinessKey,
                            EmployeeNumber = p.EmployeeNumber,
                            Organization = p.Organization,
                            ServicePeriodFrom = p.ServicePeriodFrom,
                            ServicePeriodTill = p.ServicePeriodTill,
                            Designation = p.Designation,
                            ReasonforLeaving = p.ReasonforLeaving
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeePreviousJobInfo(DO_EmployeePreviousJobInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEpyepe is_prejobExists = db.GtEpyepe.FirstOrDefault(p => p.BusinessKey == obj.BusinessKey && p.EmployeeNumber == obj.EmployeeNumber
                        && p.Organization.ToUpper().Replace(" ", "") == obj.Organization.ToUpper().Replace(" ", ""));
                        if (is_prejobExists == null)
                        {
                            var prejob_info = new GtEpyepe
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                Organization = obj.Organization,
                                ServicePeriodFrom = obj.ServicePeriodFrom,
                                ServicePeriodTill = obj.ServicePeriodTill,
                                Designation = obj.Designation,
                                ReasonforLeaving = obj.ReasonforLeaving,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyepe.Add(prejob_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Previous Job Information Created Successfully." };
                        }
                        else
                        {
                            is_prejobExists.ServicePeriodFrom = obj.ServicePeriodFrom;
                            is_prejobExists.ServicePeriodTill = obj.ServicePeriodTill;
                            is_prejobExists.Designation = obj.Designation;
                            is_prejobExists.ReasonforLeaving = obj.ReasonforLeaving;
                            is_prejobExists.ModifiedBy = obj.UserID;
                            is_prejobExists.ModifiedOn = DateTime.Now;
                            is_prejobExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Previous Job Information Updated Successfully." };
                        }
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

        #endregion Employee Previous Job Info

        #region Employee Current Job Info

        public async Task<List<DO_EmployeeCurrentJobInfo>> GetEmployeeCurrentJobInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyecj.Where(x => x.EmployeeNumber == EmpNumber)
                        .Select(p => new DO_EmployeeCurrentJobInfo
                        {
                            BusinessKey = p.BusinessKey,
                            EmployeeNumber = p.EmployeeNumber,
                            FromDate = p.FromDate,
                            TillDate = p.TillDate,
                            Department = p.Department,
                            Designation = p.Designation,
                            FunctionalReportingTo = p.FunctionalReportingTo,
                            AdministrativeReportingTo = p.AdministrativeReportingTo
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeeCurrentJobInfo(DO_EmployeeCurrentJobInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEpyecj is_currentjobExists = db.GtEpyecj.FirstOrDefault(p => p.BusinessKey == obj.BusinessKey && p.EmployeeNumber == obj.EmployeeNumber
                        && p.FromDate== obj.FromDate);
                        if (is_currentjobExists == null)
                        {
                            var currentjob_info = new GtEpyecj
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                FromDate = obj.FromDate,
                                TillDate = obj.TillDate,
                                Department = obj.Department,
                                Designation = obj.Designation,
                                FunctionalReportingTo = obj.FunctionalReportingTo,
                                AdministrativeReportingTo=obj.AdministrativeReportingTo,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyecj.Add(currentjob_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Current Job Information Created Successfully." };
                        }
                        else
                        {
                            is_currentjobExists.TillDate = obj.TillDate;
                            is_currentjobExists.Department = obj.Department;
                            is_currentjobExists.Designation = obj.Designation;
                            is_currentjobExists.FunctionalReportingTo = obj.FunctionalReportingTo;
                            is_currentjobExists.AdministrativeReportingTo = obj.AdministrativeReportingTo;
                            is_currentjobExists.ModifiedBy = obj.UserID;
                            is_currentjobExists.ModifiedOn = DateTime.Now;
                            is_currentjobExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Current Job Information Updated Successfully." };
                        }
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

        #endregion Employee Current Job Info

        #region Fixed Deduction Info

        public async Task<List<DO_EmployeeFixedDeductionInfo>> GetEmployeeFixedDeductionInfobyEmpNumber(int EmpNumber)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEpyefd.Where(x => x.EmployeeNumber == EmpNumber)
                        .Select(p => new DO_EmployeeFixedDeductionInfo
                        {
                            BusinessKey = p.BusinessKey,
                            EmployeeNumber = p.EmployeeNumber,
                            Ercode = p.Ercode,
                            Amount = p.Amount,
                            NoOfinstallment = p.NoOfinstallment,
                            PaidAmount = p.PaidAmount,
                            ReferenceDetail = p.ReferenceDetail,
                            Status = p.Status,
                            SkipinPayroll = p.SkipinPayroll,
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmployeeFixedDeductionInfo(DO_EmployeeFixedDeductionInfo obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEpyefd is_fixdeductionExists = db.GtEpyefd.FirstOrDefault(p => p.BusinessKey == obj.BusinessKey && p.EmployeeNumber == obj.EmployeeNumber
                        && p.Ercode == obj.Ercode);
                        if (is_fixdeductionExists == null)
                        {
                            var fixed_info = new GtEpyefd
                            {
                                BusinessKey = obj.BusinessKey,
                                EmployeeNumber = obj.EmployeeNumber,
                                Ercode = obj.Ercode,
                                Amount = obj.Amount,
                                NoOfinstallment = obj.NoOfinstallment,
                                PaidAmount = obj.PaidAmount,
                                ReferenceDetail = obj.ReferenceDetail,
                                Status = obj.Status,
                                SkipinPayroll = obj.SkipinPayroll,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEpyefd.Add(fixed_info);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Fixed Deduction Created Successfully." };
                        }
                        else
                        {
                            is_fixdeductionExists.Amount = obj.Amount;
                            is_fixdeductionExists.NoOfinstallment = obj.NoOfinstallment;
                            is_fixdeductionExists.PaidAmount = obj.PaidAmount;
                            is_fixdeductionExists.ReferenceDetail = obj.ReferenceDetail;
                            is_fixdeductionExists.Status = obj.Status;
                            is_fixdeductionExists.SkipinPayroll = obj.SkipinPayroll;
                            is_fixdeductionExists.ModifiedBy = obj.UserID;
                            is_fixdeductionExists.ModifiedOn = DateTime.Now;
                            is_fixdeductionExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Employee Fixed Deduction Updated Successfully." };
                        }
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

        #endregion Employee Deduction Info
  
    }
}
