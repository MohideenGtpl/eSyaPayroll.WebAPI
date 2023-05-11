using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyebi
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public string AccountNumber { get; set; }
        public int SalaryPaymentMode { get; set; }
        public int BankCode { get; set; }
        public string Ifsccode { get; set; }
        public string BankBranch { get; set; }
        public bool AccountStatus { get; set; }
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
