using eSyaPayroll.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace eSyaPayroll.IF
{
   public interface IAttendanceProcessRepository
    {
        #region Attendance Process
        Task<List<DO_PayPeriod>> GetPayPeriodbyBusinessKey(int Businesskey);

        Task<List<DO_AttendanceProcess>> GetEmployeesbyBusinessKeyAndPayperiod(int Businesskey, int Payperiod);

        Task<DO_ReturnParameter> InsertorUpdateAttendanceProcess(List<DO_AttendanceProcess> obj);
        #endregion Attendance Process

        #region Loss of Pay
        Task<List<DO_LossofPAY>> GetLossofPaybyBusinessKeyAndPayperiod(int Businesskey, int Payperiod);
        Task<DO_ReturnParameter> UpdateLossofPAY(List<DO_LossofPAY> obj);
        #endregion Loss of Pay

        #region Arrear Details
        Task<List<DO_EmployeeInfo>> GetEmployeebyBusinessKey(int Businesskey);
        Task<List<DO_ArrearDays>> GetPaidToEmployees(int Businesskey, int Payperiod, int employeeNumber);
        Task<List<DO_ArrearDays>> GetArreardays(int Businesskey, int Payperiod, int employeeNumber);
        Task<DO_ReturnParameter> InsertOrUpdateArreardays(List<DO_ArrearDays> obj);
        #endregion Arrear Details
    }
}
