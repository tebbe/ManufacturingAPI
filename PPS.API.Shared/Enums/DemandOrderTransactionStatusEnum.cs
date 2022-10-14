using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.API.Shared.Enums
{
    public enum DemandOrderTransactionStatusEnum
    {
        NotApproved = 0,
        Paid = 1,
        Regular,
        Warning,
        PayDay,
        Danger
    }
}
