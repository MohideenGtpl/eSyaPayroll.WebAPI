using eSyaPayroll.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaPayroll.IF
{
   public interface IVariableEntryRepository
    {
        Task<List<DO_ERCode>> GetActiveERCodes();
        Task<List<DO_PayPeriod>> GetActivePayPeriodsbyBusinesskey(int Businesskey);
        Task<List<DO_VariableEntry>> GetIncentiesbyBusinessKeyPayPeriodAndErCode(int Businesskey, int Payperiod, int Ercode);
        Task<DO_ReturnParameter> InsertOrUpdateVariableIncentive(List<DO_VariableEntry> obj);
    }
}
