using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyemh
    {
        public GtEpyemh()
        {
            GtEpyebi = new HashSet<GtEpyebi>();
            GtEpyefd = new HashSet<GtEpyefd>();
            GtEpyeln = new HashSet<GtEpyeln>();
            GtEpyepe = new HashSet<GtEpyepe>();
            GtEpyesd = new HashSet<GtEpyesd>();
        }

        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public string EmployeeId { get; set; }
        public string BiometricId { get; set; }
        public string Title { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public bool ExemptedFromAttendance { get; set; }
        public int WillingnessToWorkInShifts { get; set; }
        public byte[] Photo { get; set; }
        public string EmployeeStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEpyefi GtEpyefi { get; set; }
        public virtual GtEpyemc GtEpyemc { get; set; }
        public virtual GtEpyems GtEpyems { get; set; }
        public virtual GtEpyepi GtEpyepi { get; set; }
        public virtual ICollection<GtEpyebi> GtEpyebi { get; set; }
        public virtual ICollection<GtEpyefd> GtEpyefd { get; set; }
        public virtual ICollection<GtEpyeln> GtEpyeln { get; set; }
        public virtual ICollection<GtEpyepe> GtEpyepe { get; set; }
        public virtual ICollection<GtEpyesd> GtEpyesd { get; set; }
    }
}
