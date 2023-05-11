using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyatp
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public int PayPeriod { get; set; }
        public decimal WorkingDays { get; set; }
        public decimal Holidays { get; set; }
        public decimal WeeklyOffs { get; set; }
        public decimal AttendedDays { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal LateComingDays { get; set; }
        public decimal ArrearDays { get; set; }
        public decimal PayableDays { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }
    }
}
