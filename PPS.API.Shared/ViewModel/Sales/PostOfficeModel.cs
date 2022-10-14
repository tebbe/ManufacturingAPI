using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class PostOfficeModel
    {
        public int Id { get; set; }
        public string PostOfficeName { get; set; }
        public string PostCode { get; set; }
        public int PoliceStationId { get; set; }
    }
}