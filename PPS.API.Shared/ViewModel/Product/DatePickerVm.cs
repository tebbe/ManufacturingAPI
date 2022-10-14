using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Product
{
    public class DatePickerVm
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? PartyCode { get; set; }
        public int? ProductId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}