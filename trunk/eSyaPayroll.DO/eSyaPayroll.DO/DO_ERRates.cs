using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaPayroll.DO
{
   public class DO_ERRates
    {
        public int Ercode { get; set; }
        public string Erdesc { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTill { get; set; }
        public decimal AmountToDeduct { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
