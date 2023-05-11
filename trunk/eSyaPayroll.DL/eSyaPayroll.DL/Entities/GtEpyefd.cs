using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyefd
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public int Ercode { get; set; }
        public decimal Amount { get; set; }
        public int NoOfinstallment { get; set; }
        public decimal PaidAmount { get; set; }
        public string ReferenceDetail { get; set; }
        public string Status { get; set; }
        public bool SkipinPayroll { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEpyemh GtEpyemh { get; set; }
    }
}
