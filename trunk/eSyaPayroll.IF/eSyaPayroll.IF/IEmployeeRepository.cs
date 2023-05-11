using eSyaPayroll.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaPayroll.IF
{
   public interface IEmployeeRepository
    {
        #region Employee Details Info

        Task<List<DO_EmployeeClass>> GetEmployeeClassforCombo();

        List<DO_EmployeeInfo> GetEmployeesInfobySuffix(string Alphabet);

        Task<DO_EmployeeInfo> GetEmployeeInfobyEmployeeNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeeInfo(DO_EmployeeInfo obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveEmployee(bool status, int EmpNo);
        #endregion Employee Details Info

        #region Employee Personal Info
        Task<List<DO_ApplicationCodes>> GetLanguagebyCodeType(int CodeType);

        Task<List<DO_EmployeeLanguage>> GetEmployeeLanguagebyEmpNumber(int EmpNumber);

        Task<DO_EmployeePersonalInfo> GetEmployeePersonalInfobyEmployeeNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeePersonalInfo(DO_EmployeePersonalInfo obj);

        #endregion Employee Personal Info

        #region Employee Bank Info

        Task<List<DO_ERCode>> GetERCodeforCombo();

         Task<List<DO_EmployeeBankInfo>> GetEmployeeBankInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertEmployeeBankInfo(DO_EmployeeBankInfo obj);

        Task<DO_ReturnParameter> UpdateEmployeeBankInfo(DO_EmployeeBankInfo obj);

        #endregion Employee Bank Info

        #region Employee Salary Info

        Task<List<DO_EmployeeSalaryInfo>> GetEmployeeSalaryInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeeSalaryInfo(DO_EmployeeSalaryInfo obj);

        #endregion Employee Salary Info

        #region Employee Family Info

        Task<List<DO_EmployeeFamilyInfo>> GetEmployeeFamilyInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeeFamilyInfo(DO_EmployeeFamilyInfo obj);

        #endregion Employee Family Info

        #region Employee Education Info

        Task<List<DO_EmployeeEducationInfo>> GetEmployeeEducationInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeeEducationInfo(DO_EmployeeEducationInfo obj);

        #endregion Employee Education Info

        #region Employee Previous Job Info

        Task<List<DO_EmployeePreviousJobInfo>> GetEmployeePreviousJobInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeePreviousJobInfo(DO_EmployeePreviousJobInfo obj);

        #endregion Employee Previous Job Info

        #region Employee Current Job Info

        Task<List<DO_EmployeeCurrentJobInfo>> GetEmployeeCurrentJobInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeeCurrentJobInfo(DO_EmployeeCurrentJobInfo obj);

        #endregion Employee Current Job Info

        #region Fixed Deduction Info

        Task<List<DO_EmployeeFixedDeductionInfo>> GetEmployeeFixedDeductionInfobyEmpNumber(int EmpNumber);

        Task<DO_ReturnParameter> InsertOrUpdateEmployeeFixedDeductionInfo(DO_EmployeeFixedDeductionInfo obj);

        #endregion Employee Deduction Info
    }
}
