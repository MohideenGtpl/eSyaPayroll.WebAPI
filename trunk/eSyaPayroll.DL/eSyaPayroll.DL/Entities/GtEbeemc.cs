using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEbeemc
    {
        public int EmployeeClass { get; set; }
        public string ClassDesc { get; set; }
        public int PeriodInMonths { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
