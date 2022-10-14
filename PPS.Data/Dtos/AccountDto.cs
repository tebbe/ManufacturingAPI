using PPS.API.Shared.ViewModel.Account;
using System;

namespace PPS.Data.Dtos
{
    public class AccountDto
    {
        //public int Id { get; set; }
        public int AccountNatureId { get; set; }
        public string AccountNature { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public int PrimaryHeadId { get; set; }
        public string PrimaryHead { get; set; }
        public int SubHeadId { get; set; }
        public string SubHead { get; set; }
        public int HeadId { get; set; }
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
        public bool Active { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public int CompanyId { get; set; }
        public double DrAmount { get; set; }
        public double CrAmount { get; set; }
        public int FiscalYear { get; set; }

        public AccountHeadModel ToAccountHeadModel()
        {
            var vm = new AccountHeadModel
            {
                AccountNature = this.AccountNature,
                AccountType = this.AccountType,
                PrimaryHead = this.PrimaryHead,
                SubHead = this.SubHead,
                HeadId = this.HeadId,
                HeadCode = this.HeadCode,
                HeadName = this.HeadName,
                CompanyId = this.CompanyId,
                Active = this.Active,
                DrAmount = this.DrAmount,
                CrAmount = this.CrAmount,
                FiscalYear = this.FiscalYear
            };
            return vm;
        }
        public AccountTypeModel ToAccountTypeModel()
        {
            var vm = new AccountTypeModel
            {
                AccountNatureId = this.AccountNatureId,
                AccountNature = this.AccountNature,
                AccountTypeId = this.AccountTypeId,
                AccountType = this.AccountType
            };
            return vm;
        }
        public AccountPrimaryHeadModel ToAccountPrimaryHeadModel()
        {
            var vm = new AccountPrimaryHeadModel
            {
                AccountTypeId = this.AccountTypeId,
                AccountType = this.AccountType,
                PrimaryHeadId = this.PrimaryHeadId,
                PrimaryHead = this.PrimaryHead
            };
            return vm;
        }

        public AccountSubHeadModel ToAccountSubHeadModel()
        {
            var vm = new AccountSubHeadModel
            {
                SubHeadId = this.SubHeadId,
                SubHead = this.SubHead,
                PrimaryHeadId = this.PrimaryHeadId,
                PrimaryHead = this.PrimaryHead
            };
            return vm;
        }
    }
}