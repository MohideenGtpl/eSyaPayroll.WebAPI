using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaPayroll.DO
{
    public class DO_ERRules
    {
        public int Ercode { get; set; }
        public string Erdesc { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public bool IsRuleBased { get; set; }
        public string PayRule { get; set; }
        public string PayRuleDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        
    }
}
