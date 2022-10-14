using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPS.Data.Entities.Account
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int User_Id { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        //[ForeignKey("Id")]
        public int AssignedBy_Id { get; set; }
        //public virtual User AssignedBy { get; set; }        
    }
}