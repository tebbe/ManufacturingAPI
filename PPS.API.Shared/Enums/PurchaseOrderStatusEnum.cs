using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.Enums
{
    public enum PurchaseOrderStatusEnum
    {
        Initiated = 1,
        Submitted,
        Verified,
        Approved,
        RFA,
        PA,
        Accepted
    }
}