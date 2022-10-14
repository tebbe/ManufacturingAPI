using PPS.API.Shared.ViewModel.Purchase;
using System.Collections.Generic;
using PPS.API.Shared.ViewModel.Store;

namespace PPS.Service.ServiceInterfaces
{
    public interface IBatchInterface
    {
        IList<BatchRequisitionVm> GetBatchRequisitionList(int userId);
        IList<RawMaterialTypeVm> GetRawMaterialType(int userId);
        BatchRequisitionVm SaveBatchRequisition(BatchRequisitionVm batchRequisitionModel);
    }
}