using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.Enums
{
    public enum PaymentStatusEnum
    {
        All = 0,
        Unpaid = 1,
        PartiallyPaid = 2,
        Paid = 3
    }
}