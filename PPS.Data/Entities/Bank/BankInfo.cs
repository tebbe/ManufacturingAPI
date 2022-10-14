using PPS.Data.Entities.GroupOfCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.Bank
{
    public class BankInfo : BaseEntity
    {
        [Required]
        [StringLength(15)]
        [Column(TypeName = "VARCHAR")]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string BankName { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Address { get; set; }

        public bool Active { get; set; }

        public virtual Company Company { get; set; }
    }
}