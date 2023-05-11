using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaPayroll.DO;
using eSyaPayroll.IF;
using Microsoft.AspNetCore.Mvc;

namespace eSyaPayroll.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VariableEntryController : ControllerBase
    {
        private readonly IVariableEntryRepository _VariableEntryRepository;
        public VariableEntryController(IVariableEntryRepository VariableEntryRepository)
        {
            _VariableEntryRepository = VariableEntryRepository;
        }

        /// <summary>
        /// Getting Active ER Code  List for drop down.
        /// UI Reffered - Variable Entry
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveERCodes()
        {
            var er_codes = await _VariableEntryRepository.GetActiveERCodes();
            return Ok(er_codes);
        }

        /// <summary>
        /// Getting  Active Pay Period List for drop down.
        /// UI Reffered - Variable Entry
        /// UI-Param - Businesskey
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActivePayPeriodsbyBusinesskey(int Businesskey)
        {
            var pay_periods = await _VariableEntryRepository.GetActivePayPeriodsbyBusinesskey(Businesskey);
            return Ok(pay_periods);
        }

        /// <summary>
        /// Getting  Variable Entry List for Grid.
        /// UI Reffered - Variable Entry
        /// UI-Param - Businesskey, Payperiod, Ercode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetIncentiesbyBusinessKeyPayPeriodAndErCode(int Businesskey, int Payperiod, int Ercode)
        {
            var var_entries = await _VariableEntryRepository.GetIncentiesbyBusinessKeyPayPeriodAndErCode( Businesskey,Payperiod,Ercode);
            return Ok(var_entries);
        }
        /// <summary>
        /// Insert Or Update Variable Incentive.
        /// UI Reffered - Variable Entry
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateVariableIncentive(List<DO_VariableEntry> obj)
        {
            var msg = await _VariableEntryRepository.InsertOrUpdateVariableIncentive(obj);
            return Ok(msg);

        }
    }
}