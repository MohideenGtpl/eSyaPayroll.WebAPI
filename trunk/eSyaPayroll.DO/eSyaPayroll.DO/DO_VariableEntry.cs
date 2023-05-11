using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaPayroll.DO
{
   public class DO_VariableEntry
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public int PayPeriod { get; set; }
        public int Ercode { get; set; }
        public decimal Amount { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
