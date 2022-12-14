//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPS.Data.Edmx
{
    using System;
    using System.Collections.Generic;
    
    public partial class LegalDocumentHistory
    {
        public int Id { get; set; }
        public int LegalDocumentId { get; set; }
        public int CompanyId { get; set; }
        public int DocumentTypeId { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public int DocumentRenewCategoryId { get; set; }
        public int DocumentStatusId { get; set; }
        public bool IsActive { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationContactEmail { get; set; }
        public string OrganizationContactName { get; set; }
        public string OrganizationPhoneNumber { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual DocumentRenewalCategory DocumentRenewalCategory { get; set; }
        public virtual DocumentStatus DocumentStatus { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
