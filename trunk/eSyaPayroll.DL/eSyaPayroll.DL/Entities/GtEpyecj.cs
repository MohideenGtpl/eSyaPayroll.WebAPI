using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyecj
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? TillDate { get; set; }
        public int Department { get; set; }
        public int Designation { get; set; }
        public string FunctionalReportingTo { get; set; }
        public string AdministrativeReportingTo { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }
    }
}
