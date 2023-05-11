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
    public class PayPeriodController : ControllerBase
    {
        private readonly IPayPeriodRepository _PayPeriodRepository;
        public PayPeriodController(IPayPeriodRepository PayPeriodRepository)
        {
            _PayPeriodRepository = PayPeriodRepository;
        }
        /// <summary>
        /// Getting  Pay Period List.
        /// UI Reffered - Pay Period Grid
        /// UI-Param - Businesskey
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPayPeriodsbyBusinessKey(int Businesskey)
        {
            var pay_periods = await _PayPeriodRepository.GetAllPayPeriodsbyBusinessKey(Businesskey);
            return Ok(pay_periods);
        }

        /// <summary>
        /// Insert Pay Period.
        /// UI Reffered - Pay Period
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoPayPeriod(DO_PayPeriod obj)
        {
            var msg = await _PayPeriodRepository.InsertIntoPayPeriod(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Pay Period.
        /// UI Reffered - Pay Period
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateIntoPayPeriod(DO_PayPeriod obj)
        {
            var msg = await _PayPeriodRepository.UpdateIntoPayPeriod(obj);
            return Ok(msg);

        }
    }
}