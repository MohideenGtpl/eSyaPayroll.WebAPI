using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEperru
    {
        public int Ercode { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public bool IsRuleBased { get; set; }
        public string PayRule { get; set; }
        public string PayRuleDesc { get; set; }
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
