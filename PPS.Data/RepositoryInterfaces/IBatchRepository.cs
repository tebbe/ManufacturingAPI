using PPS.API.Shared.ViewModel.Purchase;
using System.Collections.Generic;
using PPS.API.Shared.ViewModel.Store;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IBatchRepository
    {
        IList<BatchRequisitionVm> GetBatchRequisitionList(int userId);
        IList<RawMaterialTypeVm> GetRawMaterialType(int userId);
        BatchRequisitionVm SaveBatchRequisition(BatchRequisitionVm batchRequisitionModel);
    }
}