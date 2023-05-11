using eSyaPayroll.DL.Entities;
using eSyaPayroll.DO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace eSyaPayroll.DL.Repository
{
   public class CommonMethod
        {
            public static string GetValidationMessageFromException(DbUpdateException ex)
            {
                string msg = ex.InnerException == null ? ex.ToString() : ex.InnerException.Message;

                if (msg.LastIndexOf(',') == msg.Length - 1)
                    msg = msg.Remove(msg.LastIndexOf(','));
                return msg;
            }
        }
    
}
