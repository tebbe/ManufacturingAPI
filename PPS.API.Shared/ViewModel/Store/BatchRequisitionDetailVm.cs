namespace PPS.API.Shared.ViewModel.Store
{
    public class BatchRequisitionDetailVm
    {
        public int Id { get; set; }
        public int BatchRequisitionId { get; set; }
        public int RawMaterialTypeId { get; set; }
        public double Quantity { get; set; }
        public object RawMaterialTypeName { get; set; }
        public object UnitTypeName { get; set; }
    }
}