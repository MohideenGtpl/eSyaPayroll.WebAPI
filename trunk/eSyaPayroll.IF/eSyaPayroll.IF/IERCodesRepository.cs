﻿using System;
using System.Collections.Generic;
using System.Text;
using eSyaPayroll.DO;
using System.Threading.Tasks;
namespace eSyaPayroll.IF
{
  public  interface IERCodesRepository
    {
        #region ER Codes

        Task<List<DO_ERCodes>> GetAllERCodes();

        Task<List<DO_ERCodes>> GetActiveERCodes();

        Task<DO_ERCodes> GetERCodebyERCode(int ERcode);

        Task<DO_ReturnParameter> InsertIntoERCodes(DO_ERCodes obj);

        Task<DO_ReturnParameter> UpdateERCodes(DO_ERCodes obj);
        #endregion ER Codes

        #region ER Rules

        Task<List<DO_ERRules>> GetERRulesbyERCode(int ERCode);

        Task<DO_ReturnParameter> InsertOrUpdateERRules(DO_ERRules obj);

        #endregion ER Rules

        #region ER Rates

        Task<List<DO_ERRates>> GetERRatesbyERCode(int ERCode);

        Task<DO_ReturnParameter> InsertOrUpdateERRates(DO_ERRates obj);

        #endregion ER Rates
    }
}
