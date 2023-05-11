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
    public class AttendanceProcessController : ControllerBase
    {
        private readonly IAttendanceProcessRepository _AttendanceProcessRepository;
        public AttendanceProcessController(IAttendanceProcessRepository AttendanceProcessRepository)
        {
            _AttendanceProcessRepository = AttendanceProcessRepository;
        }

        #region Attendance Process
        /// <summary>
        /// Getting  Pay Period List for drop down.
        /// UI Reffered - Attandance Process
        /// UI-Param - Businesskey
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPayPeriodbyBusinessKey(int Businesskey)
        {
            var pay_periods = await _AttendanceProcessRepository.GetPayPeriodbyBusinessKey(Businesskey);
            return Ok(pay_periods);
        }

        /// <summary>
        /// Getting  Employees List for Grid.
        /// UI Reffered - Attandance Process Grid
        /// UI-Param - Businesskey, Payperiod
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmployeesbyBusinessKeyAndPayperiod(int Businesskey, int Payperiod)
        {
            var att_proc = await _AttendanceProcessRepository.GetEmployeesbyBusinessKeyAndPayperiod(Businesskey, Payperiod);
            return Ok(att_proc);
        }

        /// <summary>
        /// Insert Or Updated Attandance Process.
        /// UI Reffered - Attandance Process
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertorUpdateAttendanceProcess(List<DO_AttendanceProcess> obj)
        {
            var msg = await _AttendanceProcessRepository.InsertorUpdateAttendanceProcess(obj);
            return Ok(msg);

        }
        #endregion Attendance Process

        #region Loss of Pay

        /// <summary>
        /// Getting Loss of Pay List for Grid.
        /// UI Reffered -Loss of Pay Grid
        /// UI-Param - Businesskey, Payperiod
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLossofPaybyBusinessKeyAndPayperiod(int Businesskey, int Payperiod)
        {
            var l_pays = await _AttendanceProcessRepository.GetLossofPaybyBusinessKeyAndPayperiod(Businesskey, Payperiod);
            return Ok(l_pays);
        }

        /// <summary>
        /// Updated Loss of Pay.
        /// UI Reffered - Loss of Pay
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateLossofPAY(List<DO_LossofPAY> obj)
        {
            var msg = await _AttendanceProcessRepository.UpdateLossofPAY(obj);
            return Ok(msg);

        }
        #endregion Loss of Pay

        #region Arrear Details
        /// <summary>
        /// Getting  Active Employees.
        /// UI Reffered - Arrear Details
        ///  /// UI-Param- Business Key
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmployeebyBusinessKey(int Businesskey)
        {
            var att_proc = await _AttendanceProcessRepository.GetEmployeebyBusinessKey(Businesskey);
            return Ok(att_proc);
        }

        /// <summary>
        /// Getting  Paid To.
        /// UI Reffered - Paid To Grid
        ///  /// UI-Param- Business Key,Pay Period,employeeNumber
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaidToEmployees(int Businesskey, int Payperiod, int employeeNumber)
        {
            var att_proc = await _AttendanceProcessRepository.GetPaidToEmployees(Businesskey, Payperiod, employeeNumber);
            return Ok(att_proc);
        }
        /// <summary>
        /// Getting  Arrear Details.
        /// UI Reffered - Paid Grid
        ///  /// UI-Param- Business Key,Pay Period,employeeNumber
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetArreardays(int Businesskey, int Payperiod, int employeeNumber)
        {
            var att_proc = await _AttendanceProcessRepository.GetArreardays(Businesskey, Payperiod, employeeNumber);
            return Ok(att_proc);
        }
        /// <summary>
        /// Insert or Update Arrear days.
        /// UI Reffered - Paid To Grid
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateArreardays(List<DO_ArrearDays> obj)
        {
            var msg = await _AttendanceProcessRepository.InsertOrUpdateArreardays(obj);
            return Ok(msg);

        }
        #endregion Arrear Details
    }
}