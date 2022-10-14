using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace PPS.API.Shared.ViewModel.Purchase
{
    public class RawMaterialTypeVm
    {
        public int Id { get; set; }
        public string RawMaterialTypeName { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeName { get; set; }
        public virtual UnitType UnitType { get; set; }
        public int AccountHeadId { get; set; }
        public double AvailableQty { get; set; }
        public double ActualQty { get; set; }
        public double OpeningQty { get; set; }
        public double ReceivedQty { get; set; }
        public double FloorStoreQty { get; set; }
    }
    public class UnitType
    {
        public int Id { get; set; }
        public string UnitTypeName { get; set; }
    }
}