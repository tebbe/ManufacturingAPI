using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class DemandOrderTransactionVm
    {
        public int Id { get; set; }
        public int DemandOrderId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}