using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PPS.Data.Entities.Ledger;
using PPS.Data.Entities.GroupOfCompany;

namespace PPS.Data.Entities.Client
{
    public class ClientInfo : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string ClientName { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Location { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Fax { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Email { get; set; }
        
        public int? MajorActivity { get; set; }

        public int? CustomerCategory { get; set; }

        [Required]
        [ForeignKey("AccountHead")]
        public int AccountHeadId { get; set; }
        public virtual AccountHead AccountHead { get; set; }

        [Required]
        public double CreditLimit { get; set; }

        public bool Active { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string AlternateName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string ContactPerson { get; set; }

        public virtual Company Company { get; set; }
    }
}