using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.RequestVm
{
    public class PurchaseOrderWithSupplierRequestVm : RequestVm
    {
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
    }
}