using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaPayroll.DO
{
   public class DO_ERCodes
    {
        //Properties for Er Codes
        public int Ercode { get; set; }
        public string Erdesc { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }

        public List<DO_eSyaParameter> l_ERCodeParameter { get; set; }
    }
    public class DO_eSyaParameter
    {
        public int ParameterID { get; set; }
        public string ParameterValue { get; set; }
        public bool ParmAction { get; set; }
        public decimal ParmValue { get; set; }
        public decimal ParmAmount { get; set; }
        public decimal ParmPerct { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
