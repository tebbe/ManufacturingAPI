using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.Product
{
    public class ProductHistoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public int? ProductStandardTypeId { get; set; }
        public string Thickness { get; set; }
        public decimal? Length { get; set; }
        public int? UnitTypeId { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductTypeId { get; set; }
        public int AccountHeadId { get; set; }
        public int? HistoryById { get; set; }
        public DateTime? HistoryDate { get; set; }
        public string ProductStandardTypeName { get; set; }
        public string UnitTypeName { get; set; }
        public string ProductTypeName { get; set; }
        public int? BatchId { get; set; }
    }
}