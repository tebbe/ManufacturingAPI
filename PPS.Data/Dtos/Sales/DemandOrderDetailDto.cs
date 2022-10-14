using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Dtos.Sales
{
    public class DemandOrderDetailDto
    {
        public int Id { get; set; }
        public int DemandOrderId { get; set; }
        public int ProductTypeId { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
    }
}