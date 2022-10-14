using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Document
{
    public class LegalDocumentVm
    {
        public int Id { get; set; }
        public int? LegalDocumentId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public DateTime IssueDate { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int DocumentRenewCategoryId { get; set; }
        public string DocumentRenewalCategoryName { get; set; }
        public int DocumentStatusId { get; set; }
        public string DocumentStatusName { get; set; }
        public bool IsActive { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationContactEmail { get; set; }
        public string OrganizationContactName { get; set; }
        public string OrganizationPhoneNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}