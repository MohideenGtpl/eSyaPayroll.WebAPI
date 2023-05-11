using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyata
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public int PayPeriod { get; set; }
        public int PaidPeriod { get; set; }
        public decimal ArrearDays { get; set; }
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
