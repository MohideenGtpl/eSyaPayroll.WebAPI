using eSyaPayroll.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaPayroll.IF
{
   public interface IPayPeriodRepository
    {
        Task<List<DO_PayPeriod>> GetAllPayPeriodsbyBusinessKey(int Businesskey);

        Task<DO_ReturnParameter> InsertIntoPayPeriod(DO_PayPeriod obj);

        Task<DO_ReturnParameter> UpdateIntoPayPeriod(DO_PayPeriod obj);
    }
}
