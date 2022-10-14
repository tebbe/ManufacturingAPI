using PPS.Service.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.API.Shared.ViewModel.Store;
using PPS.API.Shared.ViewModel.User;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.Service.Services
{
    public class StoreService : IStoreInterface
    {
        private IStoreRepository _storeRepository;
        public StoreService()
        {
            _storeRepository = new StoreRepository();
        }

        public IList<BatchRequisitionVm> GetBatchRequisitionList(int userId)
        {
            return _storeRepository.GetBatchRequisitionList(userId);
        }

        public IList<RawMaterialTypeVm> GetRawMaterialType(int userId)
        {
            return _storeRepository.GetRawMaterialType(userId);
        }

        public BatchRequisitionVm SaveBatchRequisition(BatchRequisitionVm batchRequisitionModel)
        {
            var batchRequisitionDto = _storeRepository.SaveBatchRequisition(batchRequisitionModel);
            return batchRequisitionDto;
        }

        public IList<PurchaseOrderVm> GetPendingPOList(int userId)
        {
            return _storeRepository.GetPendingPOList(userId);
        }

        public IList<PurchaseOrderVm> GetAcceptedPOList(int userId)
        {
            return _storeRepository.GetAcceptedPOList(userId);
        }

        public PurchaseOrderModel GetPendingPOById(int userId, int poId)
        {
            var vm = _storeRepository.GetPendingPOById(userId, poId);
            return vm;
        }

        public List<StoreRawMaterialVm> SaveAcceptedPurchaseOrder(List<StoreRawMaterialVm> storeRawMaterialVm)
        {
            var storeRawMaterialDto = _storeRepository.SaveAcceptedPurchaseOrder(storeRawMaterialVm);
            return storeRawMaterialDto;
        }

        public BatchRequisitionVm GetBatchRequisitionById(int userId, int brId)
        {
            var vm = _storeRepository.GetBatchRequisitionById(userId, brId);
            return vm;
        }

        public async Task<bool> DeliveryBR(int userId, int Id)
        {
            return await _storeRepository.DeliveryBR(userId, Id);
        }
        public async Task<bool> ReceiveBR(int userId, int Id)
        {
            return await _storeRepository.ReceiveBR(userId, Id);
        }
        public async Task<bool> SendToProductionBR(int userId, BatchRequisitionVm batchRequisitionVm)
        {
            return await _storeRepository.SendToProductionBR(userId, batchRequisitionVm);
        }

        public List<FinishedGoodVm> SaveFinishedGood(List<FinishedGoodVm> finishedGoods, bool isClosedBatch)
        {
            var newFinishedGood = _storeRepository.SaveFinishedGood(finishedGoods, isClosedBatch);
            return newFinishedGood;
        }

        //public IList<BatchRequisitionVm> GetBatchRequisitionListFromFloorStore(int userId)
        //{
        //    return _storeRepository.GetBatchRequisitionListFromFloorStore(userId);
        //}

        public IList<ProductionGroupVm> GetProductionGroupListFromFloorStore(int userId)
        {
            return _storeRepository.GetProductionGroupListFromFloorStore(userId);
        }

        public IList<FinishedGoodVm> GetFinishedGood(int userId)
        {
            return _storeRepository.GetFinishedGood(userId);
        }

        public async Task<bool> ApproveFinishedGood(int userId, int pgId)
        {
            return await _storeRepository.ApproveFinishedGood(userId, pgId);
        }

        public async Task<bool> CloseBatch(int userId, int brId)
        {
            return await _storeRepository.CloseBatch(userId, brId);
        }

        public List<BatchRequisitionProductionEstimationVm> SaveBRProductEstimation(List<BatchRequisitionProductionEstimationVm> bRProductionEstimation)
        {
            var newBRProductionEstimation = _storeRepository.SaveBRProductEstimation(bRProductionEstimation);
            return newBRProductionEstimation;
        }

        public ProductionGroupVm SaveProductionGroup(ProductionGroupVm productionGroupModel)
        {
            return _storeRepository.SaveProductionGroup(productionGroupModel);
        }

        public IList<ProductionGroupVm> GetProductionGroupList(int userId)
        {
            return _storeRepository.GetProductionGroupList(userId);
        }
        public async Task<bool> CloseProductionGroup(int userId, int pgId)
        {
            return await _storeRepository.CloseProductionGroup(userId, pgId);
        }

        public IList<FinishedGoodVm> GetFinishedGoodForFiltering(int userId, FilterVm filterVm)
        {
            return _storeRepository.GetFinishedGoodForFiltering(userId, filterVm);
        }
    }
}