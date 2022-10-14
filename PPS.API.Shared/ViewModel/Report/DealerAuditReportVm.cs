using PPS.API.Shared.ViewModel.Sales;
using System;
using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Report
{
    public class DealerAuditReportVm
    {
        public int Id { get; set; }
        public int CustomerCode { get; set; }
        public IList<AuditDemandOrderVm> DemandOrder { get; set; }
        public IList<ProductListVm> ProductList { get; set; }
        public string CustomerName { get; set; }
    }
    
}