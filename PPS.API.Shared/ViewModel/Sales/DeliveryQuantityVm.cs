using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class DeliveryQuantityVm
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public DateTime ChallanDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public  List<DeliveryQuantityDetailVm> DeliveryQuantityDetail { get; set; }
        public string CreatedByName { get; set; }
        public int? InvoiceNo { get; set; }
        public string Note { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }
    }
}