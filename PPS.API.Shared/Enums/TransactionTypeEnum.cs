using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.API.Shared.Enums
{
    public enum TransactionTypeEnum
    {
        Payment = 1,
        Receipt = 2,
        Journal = 3,
        Contra = 4,
        Sales = 5,
        Purchase = 6,
        SalesReturn=7
    }
}
