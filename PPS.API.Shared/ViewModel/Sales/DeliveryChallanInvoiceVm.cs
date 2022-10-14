using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class DeliveryChallanInvoiceVm
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerCode { get; set; }
        public  List<DeliveryChallanInvoiceDetailVm> DeliveryChallanInvoiceDetailList { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeCode { get; set; }
    }
   

}