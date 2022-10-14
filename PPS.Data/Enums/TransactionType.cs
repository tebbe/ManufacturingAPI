using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Enums
{
    public enum TransactionType
    {
        Payment = 1,
        Received = 2,
        Journal = 3,
        Contra = 4
    }
}