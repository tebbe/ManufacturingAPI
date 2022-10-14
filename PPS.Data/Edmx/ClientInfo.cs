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
    
    public partial class ClientInfo
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public Nullable<int> MajorActivity { get; set; }
        public Nullable<int> CustomerCategory { get; set; }
        public int AccountHeadId { get; set; }
        public double CreditLimit { get; set; }
        public bool Active { get; set; }
        public string AlternateName { get; set; }
        public string ContactPerson { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<int> Company_Id { get; set; }
    
        public virtual AccountHead AccountHead { get; set; }
        public virtual Company Company { get; set; }
    }
}