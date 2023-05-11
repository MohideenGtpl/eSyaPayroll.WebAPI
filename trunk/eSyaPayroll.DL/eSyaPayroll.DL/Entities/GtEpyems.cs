﻿using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Entities
{
    public partial class GtEpyems
    {
        public int BusinessKey { get; set; }
        public int EmployeeNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime? DateOfConfirmation { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public DateTime? DateOfRelieving { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public bool AnySuspension { get; set; }
        public int CurrentDepartment { get; set; }
        public int CurrentLocationPosted { get; set; }
        public decimal CurrentBasic { get; set; }
        public decimal CurrentGross { get; set; }
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
