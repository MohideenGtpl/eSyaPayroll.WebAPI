using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyeci
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public string PermanentOrCurrent { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public string Pincode { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public string LandLineNumber { get; set; }
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
