using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class UndeliveryQuantityVm
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<UndeliveryQuantityDetailVm> UndeliveryQuantityDetailVm { get; set; }
        public int InvoiceNo { get; set; }
        public string CreatedByName { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}