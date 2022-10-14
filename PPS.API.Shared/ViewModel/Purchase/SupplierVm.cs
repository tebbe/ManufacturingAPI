using System.Collections.Generic;

namespace PPS.API.Shared.ViewModel.Purchase
{
    public class SupplierVm
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public int SupplierCode { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string ContactPersonPhone { get; set; }
        public string Email { get; set; }
        public string ContactPersonEmail { get; set; }
        public double TotalBalance { get; set; }
        public List<PurchaseOrderTransactionVm> PurchaseOrderTransaction { get; set; }
        public int AccountHeadId { get; set; }
    }
}