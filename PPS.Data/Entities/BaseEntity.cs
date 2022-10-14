using PPS.Data.Entities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities
{
    public class BaseEntity : IDisposable
    {
        //[NotMapped]
        //bool disposed = false;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public int UpdatedById { get; set; }
        public virtual User UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //        return;

        //    if (disposing)
        //    {
        //        this.Dispose();
        //    }
        //    disposed = true;
        //}
    }
}