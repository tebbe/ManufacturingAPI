using System;

namespace PPS.API.Shared.ViewModel.Account
{
    public class AccountHeadModel
    {
        public string AccountNature { get; set; }
        public string AccountType { get; set; }
        public int PrimaryHeadId { get; set; }
        public string PrimaryHead { get; set; }
        public int SubHeadId { get; set; }
        public string SubHead { get; set; }
        public int HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public int FiscalYear { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}