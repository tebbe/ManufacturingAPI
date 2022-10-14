using PPS.API.Shared.ViewModel.Purchase;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Store;
using PPS.API.Shared.ViewModel.User;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IStoreRepository
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
        IList<ProductionGroupVm> GetProductionGroupList(int userId);
        Task<bool> CloseProductionGroup(int userId, int pgId);
        IList<FinishedGoodVm> GetFinishedGoodForFiltering(int userId, FilterVm filterVm);
    }
}