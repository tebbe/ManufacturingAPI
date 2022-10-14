using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Sales
{
    public class SaleTypeModel
    {
        public int Id { get; set; }
        public string SaleTypeName { get; set; }
        public int Duration { get; set; }
    }
}