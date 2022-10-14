using PPS.Service.ServiceInterfaces;
using System.Collections.Generic;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.API.Shared.ViewModel.Store;

namespace PPS.Service.Services
{
    public class BatchService : IBatchInterface
    {
        private IBatchRepository _batchRepository;
        public BatchService()
        {
            _batchRepository = new BatchRepository();
        }

        public IList<BatchRequisitionVm> GetBatchRequisitionList(int userId)
        {
            return _batchRepository.GetBatchRequisitionList(userId);
        }

        public IList<RawMaterialTypeVm> GetRawMaterialType(int userId)
        {
            return _batchRepository.GetRawMaterialType(userId);
        }

        public BatchRequisitionVm SaveBatchRequisition(BatchRequisitionVm batchRequisitionModel)
        {
            var batchRequisitionDto = _batchRepository.SaveBatchRequisition(batchRequisitionModel);
            return batchRequisitionDto;
        }
    }
}