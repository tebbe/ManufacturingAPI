using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Caching
{
    public struct CacheKey
    {
        public const string UserRolePolicy = "UserRolePolicy";

        public const string CustomerList = "CustomerList";        

        public const string AccountLedgerTypeList = "AccountLedgerTypeList";                
        public const string AccountLedgerPrimaryHeadList = "AccountLedgerPrimaryHeadList";
        public const string AccountLedgerSubHeadList = "AccountLedgerSubHeadList";
        public const string AccountLedgerHeadList = "AccountLedgerHeadList";

        public const string AccountLedgerPrimaryHeadListForLedger = "AccountLedgerPrimaryHeadListForLedger";

        public const string AccountLedgerBankCashAccountHeadList = "AccountLedgerBankCashAccountHeadList";
        public const string AccountLedgerSalesAccountList = "AccountLedgerSalesAccountList";
        public const string AccountLedgerLCAccountHeadList = "AccountLedgerLCAccountHeadList";
    }
}
