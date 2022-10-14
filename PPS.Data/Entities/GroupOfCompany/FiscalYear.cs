using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Entities.GroupOfCompany
{
    public class FiscalYear : BaseEntity
    {
        public int Year { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public bool Active { get; set; }
    }
}