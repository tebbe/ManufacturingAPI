using PPS.Data.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using PPS.Data.Edmx;
using System.Collections.Concurrent;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.API.Shared.ViewModel.Store;

namespace PPS.Data.Repositories
{
    public class BatchRepository : IBatchRepository
    {
        private PPSDbContext _ppsDbContext;

        public BatchRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public IList<BatchRequisitionVm> GetBatchRequisitionList(int userId)
        {
            var brList = _ppsDbContext.BatchRequisition.ToList();
            var brVm = new ConcurrentBag<BatchRequisitionVm>();
            brList.ForEach(p =>
            {
                var bRStatusName = "";
                if (p.CreatedBy > 0)
                {
                    bRStatusName = "Initiated";
                    if (p.DeliveredBy > 0)
                    {
                        bRStatusName = "Delivered";
                        if (p.ReceivedBy > 0)
                        {
                            bRStatusName = "Received";
                            if (p.SendToProductionBy > 0)
                            {
                                bRStatusName = "Send To Production";
                            }
                        }
                    }
                }
                brVm.Add(new BatchRequisitionVm
                {
                    Id = p.Id,
                    BatchRequisitionNo = p.BatchRequisitionNo,
                    BRStatusName = bRStatusName,
                    CreatedByName = StringExtension.ToFullName(p.User.FirstName, p.User.LastName),
                    CreatedOn = p.CreatedOn,
                    //IsClosed = p.IsClosed
                });
            });
            return brVm.ToList();
        }

        public IList<RawMaterialTypeVm> GetRawMaterialType(int userId)
        {
            var rawMaterialType = (from rawMaterial in _ppsDbContext.RawMaterialType
                                   select new RawMaterialTypeVm
                                   {
                                       Id = rawMaterial.Id,
                                       RawMaterialTypeName = rawMaterial.RawMaterialTypeName,
                                       UnitTypeId = rawMaterial.UnitTypeId,
                                       UnitTypeName = rawMaterial.UnitType.UnitTypeName
                                   }).ToList();
            return rawMaterialType.ToList();
        }

        public BatchRequisitionVm SaveBatchRequisition(BatchRequisitionVm batchRequisitionEntry)
        {
            var lastBatchRequisition = _ppsDbContext.BatchRequisition
                .OrderByDescending(x => x.BatchRequisitionNo)
                .FirstOrDefault();

            var lastNumber = 0;
            if (lastBatchRequisition != null)
            {
                lastNumber = lastBatchRequisition.BatchRequisitionNo;
            }
            batchRequisitionEntry.BatchRequisitionNo = lastNumber + 1;

            var brEntry = new BatchRequisition
            {
                BatchRequisitionNo = batchRequisitionEntry.BatchRequisitionNo,
                CreatedBy = batchRequisitionEntry.CreatedBy,
                CreatedOn = batchRequisitionEntry.CreatedOn
                //IsCurrentRecord = true
            };

            brEntry.BatchRequisitionDetail = new List<BatchRequisitionDetail>();

            foreach (var tempBrEntry in batchRequisitionEntry.BatchRequisitionDetail)
            {
                brEntry.BatchRequisitionDetail.Add(
                    new BatchRequisitionDetail
                    {
                        BatchRequisitionId = brEntry.Id,
                        RawMaterialTypeId = tempBrEntry.RawMaterialTypeId,
                        Quantity = tempBrEntry.Quantity,
                    });
            }

            brEntry.BatchRequisitionProductionEstimation = new List<BatchRequisitionProductionEstimation>();

            foreach (var tempBrEntry in batchRequisitionEntry.BatchRequisitionProductionEstimation)
            {
                brEntry.BatchRequisitionProductionEstimation.Add(
                    new BatchRequisitionProductionEstimation
                    {
                        BatchRequisitionId = brEntry.Id,
                        ProductId = tempBrEntry.ProductId,
                        Quantity = tempBrEntry.Quantity
                    });
            }

            _ppsDbContext.BatchRequisition.Add(brEntry);
            _ppsDbContext.SaveChanges();
            batchRequisitionEntry.Id = brEntry.Id;
            return batchRequisitionEntry;
        }
    }
}
