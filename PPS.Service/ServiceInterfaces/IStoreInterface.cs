using PPS.API.Shared.ViewModel.Purchase;
using System.Collections.Generic;
using PPS.API.Shared.ViewModel.Store;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.Service.ServiceInterfaces
{
    public interface IStoreInterface
    {
        IList<BatchRequisitionVm> GetBatchRequisitionList(int userId);
        IList<RawMaterialTypeVm> GetRawMaterialType(int userId);
        BatchRequisitionVm SaveBatchRequisition(BatchRequisitionVm batchRequisitionModel);
        IList<PurchaseOrderVm> GetPendingPOList(int userId);
        IList<PurchaseOrderVm> GetAcceptedPOList(int userId);
        PurchaseOrderModel GetPendingPOById(int userId, int poId);
        List<StoreRawMaterialVm> SaveAcceptedPurchaseOrder(List<StoreRawMaterialVm> storeRawMaterialVm);
        BatchRequisitionVm GetBatchRequisitionById(int userId, int brId);
        Task<bool> DeliveryBR(int userId, int Id);
        Task<bool> ReceiveBR(int userId, int Id);
        Task<bool> SendToProductionBR(int userId, BatchRequisitionVm batchRequisitionVm);
        List<FinishedGoodVm> SaveFinishedGood(List<FinishedGoodVm> finishedGoods, bool isClosedBatch);
        //IList<BatchRequisitionVm> GetBatchRequisitionListFromFloorStore(int userId);
        IList<ProductionGroupVm> GetProductionGroupListFromFloorStore(int userId);
        IList<FinishedGoodVm> GetFinishedGood(int userId);
        Task<bool> ApproveFinishedGood(int userId, int pgId);
        Task<bool> CloseBatch(int userId, int brId);
        List<BatchRequisitionProductionEstimationVm> SaveBRProductEstimation(List<BatchRequisitionProductionEstimationVm> bRProductionEstimation);
        ProductionGroupVm SaveProductionGroup(ProductionGroupVm productionGroupModel);
        IList<ProductionGroupVm> GetProductionGroupList(int id);
        Task<bool> CloseProductionGroup(int userId, int pgId);
        IList<FinishedGoodVm> GetFinishedGoodForFiltering(int id, FilterVm filterVm);
    }
}