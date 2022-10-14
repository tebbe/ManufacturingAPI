using System;
using PPS.Data.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using PPS.Data.Edmx;
using System.Collections.Concurrent;
using System.Data.Entity;
using PPS.API.Shared.Enums;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.API.Shared.ViewModel.Store;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.Filter;
using PPS.Shared.Service.Extensions;

namespace PPS.Data.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        public StoreRepository()
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
                    ProductionGroupIdName = p.ProductionGroup.ProductionGroupId,
                    BatchRequisitionNo = p.BatchRequisitionNo,
                    BRStatusName = bRStatusName,
                    CreatedByName = StringExtension.ToFullName(p.User.FirstName, p.User.LastName),
                    CreatedOn = p.CreatedOn,
                    IsClosed = p.ProductionGroup.IsClosed
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
                                       UnitTypeName = rawMaterial.UnitType.UnitTypeName,
                                       OpeningQty = rawMaterial.StoreRawMaterialOpening.Sum(l => (double?)l.Quantity) ?? 0,
                                       ReceivedQty = rawMaterial.StoreRawMaterial.Sum(l => (double?)l.Quantity) ?? 0,
                                       FloorStoreQty = rawMaterial.FloorStoreRawMaterial.Sum(l => (double?)l.Quantity) ?? 0,
                                       ActualQty = ((rawMaterial.StoreRawMaterial.Sum(l => (double?)l.Quantity) ?? 0) + (rawMaterial.StoreRawMaterialOpening.Sum(l => (double?)l.Quantity) ?? 0) - (rawMaterial.FloorStoreRawMaterial.Sum(l => (double?)l.Quantity) ?? 0)),
                                       AvailableQty = ((rawMaterial.StoreRawMaterial.Sum(l => (double?)l.Quantity) ?? 0) + (rawMaterial.StoreRawMaterialOpening.Sum(l => (double?)l.Quantity) ?? 0) - (rawMaterial.FloorStoreRawMaterial.Sum(l => (double?)l.Quantity) ?? 0) - (rawMaterial.BatchRequisitionDetail.Where(x=>x.BatchRequisition.SendToProductionBy==null).Sum(l => (double?)l.Quantity) ?? 0))
                                   }).ToList();
            return rawMaterialType;
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
                ProductionGroupId = batchRequisitionEntry.ProductionGroupId,
                BatchRequisitionNo = batchRequisitionEntry.BatchRequisitionNo,
                CreatedBy = batchRequisitionEntry.CreatedBy,
                CreatedOn = batchRequisitionEntry.CreatedOn
                //IsCurrentRecord = true
            };

            brEntry.BatchRequisitionDetail = new List<BatchRequisitionDetail>();

            batchRequisitionEntry.BatchRequisitionDetail.ToList().ForEach(x=> {
                var storeRawMaterialQty = _ppsDbContext.StoreRawMaterial.Where(k => k.RawMaterialTypeId == x.RawMaterialTypeId).Sum(l => (double?)l.Quantity) ?? 0;
                var storeRawMaterialOpeningQty = _ppsDbContext.StoreRawMaterialOpening.Where(k => k.RawMaterialTypeId == x.RawMaterialTypeId).Sum(l => (double?)l.Quantity) ?? 0;
                var floorStoreRawMaterialQty = _ppsDbContext.FloorStoreRawMaterial.Where(k => k.RawMaterialTypeId == x.RawMaterialTypeId).Sum(l => (double?)l.Quantity) ?? 0;
                var availableQty = storeRawMaterialQty + storeRawMaterialOpeningQty - floorStoreRawMaterialQty;
                if (availableQty < x.Quantity)
                {
                    throw new Exception("Quantity is not available");
                }
                brEntry.BatchRequisitionDetail.Add(
                    new BatchRequisitionDetail
                    {
                        BatchRequisitionId = brEntry.Id,
                        RawMaterialTypeId = x.RawMaterialTypeId,
                        Quantity = x.Quantity,
                    });
            });

            //foreach (var tempBrEntry in batchRequisitionEntry.BatchRequisitionDetail)
            //{
                
                
            //    brEntry.BatchRequisitionDetail.Add(
            //        new BatchRequisitionDetail
            //        {
            //            BatchRequisitionId = brEntry.Id,
            //            RawMaterialTypeId = tempBrEntry.RawMaterialTypeId,
            //            Quantity = tempBrEntry.Quantity,
            //        });
            //}

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

        public IList<PurchaseOrderVm> GetPendingPOList(int userId)
        {
            var poList = _ppsDbContext.PurchaseOrder.Where(x => x.PurchaseOrderStatusId >= (int)PurchaseOrderStatusEnum.Approved && x.PurchaseOrderStatusId != (int)PurchaseOrderStatusEnum.Accepted).ToList();

            if (poList == null)
            {
                throw new KeyNotFoundException("Pending PO List not found");
            }
            var poVm = new ConcurrentBag<PurchaseOrderVm>();
            poList.ForEach(p =>
            {
                var storeRmMaterials = _ppsDbContext.StoreRawMaterial.Where(x => x.PurchaseOrderId == p.Id).ToList();
                var pOStatus = PurchaseOrderStatusEnum.RFA.ToString();
                if (storeRmMaterials.Count > 0)
                {
                    pOStatus = PurchaseOrderStatusEnum.PA.ToString();
                }

                poVm.Add(new PurchaseOrderVm
                {
                    POId = p.Id,
                    PurchaseOrderNo = p.PurchaseOrderNo,
                    PurchaseOrderDate = p.PurchaseOrderDate,
                    SupplierName = p.Supplier?.SupplierName,
                    Note = p.Note,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    POStatusName = pOStatus,
                    CreatedByName = StringExtension.ToFullName(p.User.FirstName, p.User.LastName),
                    CreatedDate = p.CreatedOn
                });
            });
            return poVm.ToList();
        }

        public IList<PurchaseOrderVm> GetAcceptedPOList(int userId)
        {
            var poList = _ppsDbContext.PurchaseOrder.Where(x => x.PurchaseOrderStatusId == (int)PurchaseOrderStatusEnum.Accepted).ToList();
            if (poList == null)
            {
                throw new KeyNotFoundException("Accepted PO list not found.");
            }
            var poVm = new ConcurrentBag<PurchaseOrderVm>();
            poList.ForEach(p =>
            {
                poVm.Add(new PurchaseOrderVm
                {
                    POId = p.Id,
                    PurchaseOrderNo = p.PurchaseOrderNo,
                    PurchaseOrderDate = p.PurchaseOrderDate,
                    SupplierName = p.Supplier?.SupplierName,
                    Note = p.Note,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    POStatusName = p.PurchaseOrderStatus.Status,
                    CreatedByName = StringExtension.ToFullName(p.User.FirstName, p.User.LastName),
                    CreatedDate = p.CreatedOn
                });
            });
            return poVm.ToList();
        }

        public PurchaseOrderModel GetPendingPOById(int userId, int poId)
        {
            var purchaseOrder = _ppsDbContext.PurchaseOrder.FirstOrDefault(x => x.Id == poId && x.IsCurrentRecord);
            var purchaseOrderDetail = purchaseOrder?.PurchaseOrderDetail.OrderBy(x => x.RawMaterialTypeId).ToList();
            var acceptedQuantity = (from entry in _ppsDbContext.StoreRawMaterial
                                    where entry.PurchaseOrderId == poId
                                    group entry by entry.RawMaterialTypeId into gDetailRm
                                    orderby gDetailRm.FirstOrDefault().RawMaterialTypeId
                                    select new PurchaseOrderDetailModel
                                    {
                                        PurchaseOrderId = gDetailRm.FirstOrDefault().PurchaseOrderId,
                                        RawMaterialTypeId = gDetailRm.FirstOrDefault().RawMaterialTypeId,
                                        Quantity = gDetailRm.Sum(x => x.Quantity)
                                    }).ToList();

            var purchaseOrderDetailVm = new List<PurchaseOrderDetailModel>();
            purchaseOrderDetail?.ForEach(p =>
            {
                int? aQuantity = 0;
                if (acceptedQuantity.Count != 0)
                {
                    var checkQuantity = acceptedQuantity.FirstOrDefault(x => x.RawMaterialTypeId == p.RawMaterialTypeId);
                    if (checkQuantity != null) aQuantity = (int)checkQuantity.Quantity;
                }

                purchaseOrderDetailVm.Add(new PurchaseOrderDetailModel
                {
                    Id = p.Id,
                    PurchaseOrderId = p.PurchaseOrderId,
                    RawMaterialTypeId = p.RawMaterialTypeId,
                    RawMaterialTypeName = p.RawMaterialType?.RawMaterialTypeName,
                    UnitTypeName = p.RawMaterialType?.UnitType.UnitTypeName,
                    Quantity = p?.Quantity,
                    AcceptedQuantity = aQuantity,
                    BalanceQuantity = p?.Quantity - aQuantity
                });
            });

            var purchaseOrderVm = new PurchaseOrderModel
            {
                Id = purchaseOrder.Id,
                PurchaseOrderNo = purchaseOrder.PurchaseOrderNo,
                PurchaseOrderDate = purchaseOrder.PurchaseOrderDate,
                SupplierId = purchaseOrder.SupplierId ?? 0,
                SupplierName = purchaseOrder.Supplier?.SupplierName,
                PurchaseOrderStatusId = purchaseOrder.PurchaseOrderStatusId,
                POStatusName = purchaseOrder.PurchaseOrderStatus.Status,
                Note = purchaseOrder.Note,
                EstimatedDeliveryDate = purchaseOrder.EstimatedDeliveryDate,
                PurchaseOrderDetail = purchaseOrderDetailVm
            };
            //purchaseOrderVm.VerifiedByOn = purchaseOrder.VerifiedBy != null ? GetUserByOn((int)purchaseOrder.VerifiedBy, (DateTime)purchaseOrder.VerifiedOn) : "-";
            //purchaseOrderVm.ApprovedByOn = purchaseOrder.ApprovedBy != null ? GetUserByOn((int)purchaseOrder.ApprovedBy, (DateTime)purchaseOrder.ApprovedOn) : "-";
            //purchaseOrderVm.RejectedByOn = purchaseOrder.RejectedBy != null ? GetUserByOn((int)purchaseOrder.RejectedBy, (DateTime)purchaseOrder.RejectedOn) : "-";
            return purchaseOrderVm;
        }

        public List<StoreRawMaterialVm> SaveAcceptedPurchaseOrder(List<StoreRawMaterialVm> storeRawMaterialVm)
        {
            using (var db = new PPSDbContext())
            {
                var poId = storeRawMaterialVm.FirstOrDefault()?.PurchaseOrderId;
                var purchaseOrder = db.PurchaseOrder.FirstOrDefault(x => x.Id == poId && x.IsCurrentRecord);
                if (purchaseOrder == null)
                {
                    throw new KeyNotFoundException($"Purchase Order Id {poId} does not exist.");
                }

                // Verify purchase order status before saving to the database
                if ((purchaseOrder.PurchaseOrderStatusId >= (int)PurchaseOrderStatusEnum.Initiated &&
                purchaseOrder.PurchaseOrderStatusId < (int)PurchaseOrderStatusEnum.Approved) ||
                purchaseOrder.PurchaseOrderStatusId == (int)PurchaseOrderStatusEnum.Accepted)
                {
                    throw new Exception("Either raw material overflow or not approved.");
                }

                var isOrderedEqualAcceptedQuantity = true;

                var storeRawMaterialList = new List<StoreRawMaterial>();

                foreach (var storeRawMaterial in storeRawMaterialVm)
                {
                    var totalOrderedQuantity = purchaseOrder?.PurchaseOrderDetail
                        .FirstOrDefault(x => x.RawMaterialTypeId == storeRawMaterial.RawMaterialTypeId)?.Quantity;
                    var totalAcceptedQuantity = db.StoreRawMaterial.Where(x =>
                            x.PurchaseOrderId == poId && x.RawMaterialTypeId == storeRawMaterial.RawMaterialTypeId).ToList()
                        .Sum(x => x.Quantity);

                    var balanceQuantity = totalOrderedQuantity - totalAcceptedQuantity;

                    if (balanceQuantity == storeRawMaterial.Quantity) { }
                    else if (balanceQuantity < storeRawMaterial.Quantity)
                    {
                        throw new Exception("Raw material quantity overflow.");
                    }
                    else
                    {
                        isOrderedEqualAcceptedQuantity = false;
                    }
                    storeRawMaterialList.Add(new StoreRawMaterial
                    {

                        PurchaseOrderId = storeRawMaterial.PurchaseOrderId,
                        RawMaterialTypeId = storeRawMaterial.RawMaterialTypeId,
                        Quantity = storeRawMaterial.Quantity,
                        ReceivedBy = storeRawMaterial.ReceivedBy,
                        ReceivedOn = storeRawMaterial.ReceivedOn
                    });
                }
                db.StoreRawMaterial.AddRange(storeRawMaterialList);
                //_ppsDbContext.SaveChanges();

                purchaseOrder.PurchaseOrderStatusId = isOrderedEqualAcceptedQuantity
                    ? (int)PurchaseOrderStatusEnum.Accepted
                    : (int)PurchaseOrderStatusEnum.PA;
                db.PurchaseOrder.Attach(purchaseOrder);
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();

                return storeRawMaterialVm;
            }
        }

        public BatchRequisitionVm GetBatchRequisitionById(int userId, int brId)
        {
            var batchRequisition = _ppsDbContext.BatchRequisition.FirstOrDefault(x => x.Id == brId);

            if (batchRequisition == null)
            {
                throw new KeyNotFoundException($"This batch requision no {brId} doesn't exist.");
            }

            var batchRequisitionDetail = batchRequisition?.BatchRequisitionDetail.OrderBy(x => x.RawMaterialTypeId).ToList();

            var batchRequisitionDetailVm = new List<BatchRequisitionDetailVm>();
            batchRequisitionDetail?.ForEach(br =>
            {
                batchRequisitionDetailVm.Add(new BatchRequisitionDetailVm
                {
                    Id = br.Id,
                    BatchRequisitionId = br.BatchRequisitionId,
                    RawMaterialTypeId = br.RawMaterialTypeId,
                    RawMaterialTypeName = br.RawMaterialType?.RawMaterialTypeName,
                    UnitTypeName = br.RawMaterialType?.UnitType.UnitTypeName,
                    Quantity = br.Quantity
                });
            });

            var batchRequisitionVm = new BatchRequisitionVm
            {
                Id = batchRequisition.Id,
                ProductionGroupIdName = batchRequisition.ProductionGroup.ProductionGroupId,
                BatchRequisitionNo = batchRequisition.BatchRequisitionNo,
                BatchRequisitionDate = batchRequisition.CreatedOn,
                CreatedByName = GetUserByOn(batchRequisition.CreatedBy, batchRequisition.CreatedOn),
                CreatedOn = batchRequisition.CreatedOn,
                DeliveredOn = batchRequisition.DeliveredOn,
                ReceivedOn = batchRequisition.ReceivedOn,
                SendToProductionOn = batchRequisition.SendToProductionOn,
                EstimatedProductionDate = batchRequisition.EstimatedProductionDate,
                BatchRequisitionDetail = batchRequisitionDetailVm
            };

            if (batchRequisition.CreatedBy > 0)
            {
                batchRequisitionVm.BRStatusName = "Initiated";
                if (batchRequisition.DeliveredBy > 0)
                {
                    batchRequisitionVm.BRStatusName = "Delivered";
                    if (batchRequisition.ReceivedBy > 0)
                    {
                        batchRequisitionVm.BRStatusName = "Received";
                        if (batchRequisition.SendToProductionBy > 0)
                        {
                            batchRequisitionVm.BRStatusName = "Send To Production";
                        }
                    }
                }
            }
            batchRequisitionVm.DeliveredByOn = batchRequisition.DeliveredBy != null ? GetUserByOn((int)batchRequisition.DeliveredBy, (DateTime)batchRequisition.DeliveredOn) : "-";
            batchRequisitionVm.ReceivedByOn = batchRequisition.ReceivedBy != null ? GetUserByOn((int)batchRequisition.ReceivedBy, (DateTime)batchRequisition.ReceivedOn) : "-";
            batchRequisitionVm.SendToProductionByOn = batchRequisition.SendToProductionBy != null ? GetUserByOn((int)batchRequisition.SendToProductionBy, (DateTime)batchRequisition.SendToProductionOn) : "-";

            var batchRequisitionProductionEstimationVmList = new ConcurrentBag<BatchRequisitionProductionEstimationVm>();

            var bRProductionEstimation = batchRequisition.BatchRequisitionProductionEstimation.ToList();

            bRProductionEstimation.ForEach(b =>
                batchRequisitionProductionEstimationVmList.Add(new BatchRequisitionProductionEstimationVm()
                {
                    ProductId = b.ProductId,
                    ProductCode = b.Product.Code,
                    ProductName = b.Product.Name,
                    Quantity = b.Quantity
                })
            );
            batchRequisitionVm.BatchRequisitionProductionEstimation =
                batchRequisitionProductionEstimationVmList.ToList();
            return batchRequisitionVm;
        }

        public async Task<bool> DeliveryBR(int userId, int id)
        {
            var batchRequisition = _ppsDbContext.BatchRequisition.FirstOrDefault(x => x.Id == id);
            if (batchRequisition == null)
            {
                throw new Exception("This batch requisition no doesn't exist.");
            }
            if (batchRequisition.DeliveredBy != null)
            {
                throw new Exception($"The batch requisition no: {id} has already been delivered.");
            }
            batchRequisition.DeliveredBy = userId;
            batchRequisition.DeliveredOn = DateTime.Now;
            _ppsDbContext.BatchRequisition.Attach(batchRequisition);
            _ppsDbContext.Entry(batchRequisition).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReceiveBR(int userId, int id)
        {
            var batchRequisition = _ppsDbContext.BatchRequisition.FirstOrDefault(x => x.Id == id);
            if (batchRequisition == null)
            {
                throw new Exception("This batch requisition no doesn't exist.");
            }
            if (batchRequisition.ReceivedBy != null)
            {
                throw new Exception($"The batch requisition no: {id} has already been received.");
            }
            batchRequisition.ReceivedBy = userId;
            batchRequisition.ReceivedOn = DateTime.Now;
            _ppsDbContext.BatchRequisition.Attach(batchRequisition);
            _ppsDbContext.Entry(batchRequisition).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendToProductionBR(int userId, BatchRequisitionVm batchRequisitionVm)
        {
            using (_ppsDbContext)
            {
                var batchRequisition = _ppsDbContext.BatchRequisition.FirstOrDefault(x => x.Id == batchRequisitionVm.Id);
                var batchRequisitionDetail = batchRequisition?.BatchRequisitionDetail;

                if (batchRequisition == null)
                {
                    throw new Exception($"This batch requisition no: {batchRequisitionVm.Id} doesn't exist.");
                }
                if (batchRequisition.SendToProductionBy != null)
                {
                    throw new Exception($"The batch requisition no: {batchRequisitionVm.Id} has already been send to production.");
                }
                batchRequisition.SendToProductionBy = userId;
                batchRequisition.SendToProductionOn = DateTime.Now;
                batchRequisition.EstimatedProductionDate = batchRequisitionVm.EstimatedProductionDate;

                _ppsDbContext.BatchRequisition.Attach(batchRequisition);
                _ppsDbContext.Entry(batchRequisition).State = EntityState.Modified;

                var newFloorStoreRawMaterial = new List<FloorStoreRawMaterial>();
                foreach (var batch in batchRequisitionDetail)
                {
                    newFloorStoreRawMaterial.Add(new FloorStoreRawMaterial
                    {
                        BatchRequisitionId = batch.BatchRequisitionId,
                        RawMaterialTypeId = batch.RawMaterialTypeId,
                        Quantity = batch.Quantity,
                        ReceivedBy = userId,
                        ReceivedOn = DateTime.Now
                    });
                }
                _ppsDbContext.FloorStoreRawMaterial.AddRange(newFloorStoreRawMaterial);
                await _ppsDbContext.SaveChangesAsync();
                return true;
            }
        }

        public List<FinishedGoodVm> SaveFinishedGood(List<FinishedGoodVm> finishedGoods, bool isClosedBatch)
        {
            using (var db = new PPSDbContext())
            {
                var productionGroupId = finishedGoods.FirstOrDefault()?.ProductionGroupId;
                var productionGroup = db.ProductionGroup.FirstOrDefault(x => x.Id == productionGroupId);

                if (productionGroup == null)
                {
                    throw new Exception("This production group doesn't exist.");
                }

                if (productionGroup.IsClosed)
                {
                    throw new Exception("This production group is already closed.");
                }

                var newFinishedGoodList = new List<FinishedGood>();

                foreach (var aFinishedGood in finishedGoods)
                {
                    newFinishedGoodList.Add(new FinishedGood
                    {
                        ProductionDate = aFinishedGood.ProductionDate,
                        ProductId = aFinishedGood.ProductId,
                        ProductionGroupId = aFinishedGood.ProductionGroupId,
                        Quantity = aFinishedGood.Quantity,
                        CreatedBy = aFinishedGood.CreatedBy,
                        CreatedOn = aFinishedGood.CreatedOn
                    });
                }
                db.FinishedGood.AddRange(newFinishedGoodList);

                productionGroup.IsClosed = isClosedBatch;
                db.ProductionGroup.Attach(productionGroup);
                db.Entry(productionGroup).State = EntityState.Modified;

                db.SaveChanges();
                return finishedGoods;
            }
        }

        //public IList<BatchRequisitionVm> GetBatchRequisitionListFromFloorStore(int userId)
        //{
        //var brList = _ppsDbContext.BatchRequisition
        //    .Where(x => x.SendToProductionBy != null && x.IsClosed == false)
        //    .Select(br => new BatchRequisitionVm { Id = br.Id })
        //    .ToList();
        //    return brList;
        //}
        public IList<ProductionGroupVm> GetProductionGroupListFromFloorStore(int userId)
        {
            var pgList = _ppsDbContext.ProductionGroup
                .Where(x => x.IsClosed == false)
                .Select(pg => new ProductionGroupVm
                {
                    Id = pg.Id,
                    ProductionGroupId = pg.ProductionGroupId
                })
                .ToList();
            return pgList;
        }

        public IList<FinishedGoodVm> GetFinishedGood(int userId)
        {
            var finishedGoodAll = _ppsDbContext.FinishedGood
                .Select(k => new
                {
                    k.ProductionGroupId
                }).Distinct().ToList();

            if (finishedGoodAll == null)
            {
                throw new Exception("No finished Goods are exist");
            }
            var finishedGoodList = new List<FinishedGoodVm>();
            var overAllFpStatusName = "Pending";
            finishedGoodAll.ForEach(i =>
            {
                var finishedGoodSubList = new List<FinishedGoodVm>();
                var finishedGood = _ppsDbContext.FinishedGood.Where(x => x.ProductionGroupId == i.ProductionGroupId).ToList();
                finishedGood.ForEach(k =>
                {
                    var fPStatusName = "Pending";
                    if (k.IsApproved)
                    {
                        fPStatusName = "Approved";
                    }

                    overAllFpStatusName = fPStatusName;

                    finishedGoodSubList.Add(new FinishedGoodVm
                    {
                        ProductionGroupIdName = k.ProductionGroup.ProductionGroupId,
                        ProductionDate = k.ProductionDate,
                        ProductId = k.ProductId,
                        ProductName = k.Product.Name,
                        Quantity = k.Quantity,
                        FPStatusName = fPStatusName
                    });
                });
                var fg = new FinishedGoodVm
                {
                    Id = finishedGood.FirstOrDefault().Id,
                    ProductionGroupId = finishedGood.FirstOrDefault().ProductionGroupId,
                    ProductionGroupIdName = finishedGood.FirstOrDefault().ProductionGroup.ProductionGroupId,
                    FPStatusName = overAllFpStatusName,
                    CreatedByName = GetUserName(userId),
                    CreatedOn = finishedGood.FirstOrDefault().CreatedOn,
                    TotalCount = finishedGoodAll.Count()
                };
                fg.FinishedGoodSubList = finishedGoodSubList;
                finishedGoodList.Add(fg);
            });

            //foreach (var afinishedGood in finishedGoodAll)
            //{
            //    var fPStatusName = "Pending";
            //    if (afinishedGood.IsApproved)
            //    {
            //        fPStatusName = "Approved";
            //    }

            //    finishedGoodList.Add(new FinishedGoodVm
            //    {
            //        Id = afinishedGood.Id,
            //        ProductionGroupId = afinishedGood.ProductionGroupId,
            //        ProductionGroupIdName = afinishedGood.ProductionGroup.ProductionGroupId,
            //        ProductionDate = afinishedGood.ProductionDate,
            //        ProductId = afinishedGood.ProductId,
            //        ProductName = afinishedGood.Product.Name,
            //        Quantity = afinishedGood.Quantity,
            //        CreatedByName = GetUserName(userId),
            //        CreatedOn = afinishedGood.CreatedOn,
            //        FPStatusName = fPStatusName
            //    });
            //}
            return finishedGoodList;
        }

        public async Task<bool> ApproveFinishedGood(int userId, int pgId)
        {
            using (var db = new PPSDbContext())
            {
                var finishedGood = db.FinishedGood.Where(x => x.ProductionGroupId == pgId && x.ApprovedBy == null).ToList();
                if (finishedGood == null)
                {
                    throw new Exception($"This finished good no: {pgId} doesn't exist.");
                }
                //if (finishedGood.IsApproved)
                //{
                //    throw new Exception($"The finished goodsno: {fgId} has already approved");
                //}

                finishedGood.ForEach(x =>
                {
                    x.ApprovedBy = userId;
                    x.ApprovedOn = DateTime.Now;
                    x.IsApproved = true;
                    var currentStock = new CurrentProductStockRepository();
                    var success = currentStock.AddProductQuantityByProductId(db, x.ProductId, x.Quantity);
                    if (success == false)
                    {
                        throw new Exception($"Available stock of product Id {x.ProductId} is overflow");
                    }
                });

                //finishedGood.ApprovedBy = userId;
                //finishedGood.ApprovedOn = DateTime.Now;
                //finishedGood.IsApproved = true;
                //db.FinishedGood.AddRange(finishedGood);
                //db.Entry(finishedGood).State = EntityState.Modified;

                //var currentStock = new CurrentProductStockRepository();
                //var success = currentStock.AddProductQuantityByProductId(db, finishedGood.ProductId, finishedGood.Quantity);
                //if (success == false)
                //{
                //    throw new Exception($"Available stock of product Id {finishedGood.ProductId} is overflow");
                //}
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> CloseBatch(int userId, int productionGroupId)
        {
            var productionGroup = _ppsDbContext.ProductionGroup.FirstOrDefault(x => x.Id == productionGroupId);
            if (productionGroup == null)
            {
                throw new Exception($"This production group no: {productionGroupId} doesn't exist.");
            }
            if (productionGroup.IsClosed)
            {
                throw new Exception($"This production group no: {productionGroupId} has already closed");
            }
            productionGroup.IsClosed = true;
            _ppsDbContext.ProductionGroup.Attach(productionGroup);
            _ppsDbContext.Entry(productionGroup).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }

        public List<BatchRequisitionProductionEstimationVm> SaveBRProductEstimation(List<BatchRequisitionProductionEstimationVm> bRProductionEstimation)
        {
            var newBrProductionEstimationList = new List<BatchRequisitionProductionEstimation>();
            foreach (var bR in bRProductionEstimation)
            {
                newBrProductionEstimationList.Add(new BatchRequisitionProductionEstimation()
                {
                    ProductId = bR.ProductId,
                    BatchRequisitionId = bR.BatchRequisitionId,
                    Quantity = bR.Quantity
                });
            }
            _ppsDbContext.BatchRequisitionProductionEstimation.AddRange(newBrProductionEstimationList);
            _ppsDbContext.SaveChanges();
            return bRProductionEstimation;
        }
        public ProductionGroupVm SaveProductionGroup(ProductionGroupVm productionGroupEntry)
        {
            using (var db = new PPSDbContext())
            {
                var lastTran = db.ProductionGroup
                .Where(x => DbFunctions.TruncateTime(x.CreatedOn) == DbFunctions.TruncateTime(productionGroupEntry.CreatedOn))
                .OrderByDescending(x => x.ProductionGroupId.Substring(12, 2))
                .FirstOrDefault();
                var lastNumber = 0;
                if (lastTran != null)
                {
                    lastNumber = int.Parse(lastTran.ProductionGroupId.Substring(12, 2));
                }
                var productionGroupId = "PG-"
                    + productionGroupEntry.CreatedOn.Year.ToString("0000")
                                + productionGroupEntry.CreatedOn.Month.ToString("00")
                                + productionGroupEntry.CreatedOn.Day.ToString("00")
                                + "-";

                if (lastNumber < 10)
                {
                    productionGroupId = productionGroupId + '0' + (lastNumber + 1);
                }
                else
                {
                    productionGroupId += (lastNumber + 1);
                }

                var pgEntry = new ProductionGroup
                {
                    ProductionGroupId = productionGroupId,
                    IsClosed = false,
                    CreatedBy = productionGroupEntry.CreatedBy,
                    CreatedOn = productionGroupEntry.CreatedOn
                };

                db.ProductionGroup.Add(pgEntry);
                db.SaveChanges();

                productionGroupEntry.ProductionGroupId = pgEntry.ProductionGroupId;

                return productionGroupEntry;
            }
        }

        public IList<ProductionGroupVm> GetProductionGroupList(int userId)
        {
            var pgList = _ppsDbContext.ProductionGroup.OrderByDescending(x => x.Id)
                .Select(pg => new ProductionGroupVm
                {
                    Id = pg.Id,
                    ProductionGroupId = pg.ProductionGroupId,
                    IsClosed = pg.IsClosed,
                    CreatedBy = pg.CreatedBy,
                    CreatedByName = pg.User.FirstName + " " + pg.User.LastName,
                    CreatedOn = pg.CreatedOn,
                })
                .ToList();
            pgList.ForEach(x =>
            {
                var brList = _ppsDbContext.BatchRequisition.Where(k => k.ProductionGroupId == x.Id).ToList();
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
                        CreatedOn = p.CreatedOn
                    });
                });
                x.BatchRequisitionList = brVm.ToList();
            });

            return pgList;
        }

        public async Task<bool> CloseProductionGroup(int userId, int productionGroupId)
        {
            var productionGroup = _ppsDbContext.ProductionGroup.FirstOrDefault(x => x.Id == productionGroupId);
            if (productionGroup == null)
            {
                throw new Exception($"This production group no: {productionGroupId} doesn't exist.");
            }
            if (productionGroup.IsClosed)
            {
                throw new Exception($"This production group no: {productionGroupId} has already closed");
            }
            productionGroup.IsClosed = true;
            _ppsDbContext.ProductionGroup.Attach(productionGroup);
            _ppsDbContext.Entry(productionGroup).State = EntityState.Modified;
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }
        public IList<FinishedGoodVm> GetFinishedGoodForFiltering(int userId, FilterVm filterVm)
        {
            var totalCount = _ppsDbContext.FinishedGood.Select(k => new
            {
                k.ProductionGroupId
            }).Distinct().Count();

            var finishedGoodAll = _ppsDbContext.FinishedGood
                .OrderByField(filterVm.SortColumn, filterVm.SortDirection.ToString().Equals(SortDirectionEnum.ASC.ToString()))
                .Skip((filterVm.PageIndex - 1) * filterVm.PageSize).Take(filterVm.PageSize)
                .Select(k => new
                {
                    k.ProductionGroupId
                }).Distinct().ToList();

            if (finishedGoodAll == null)
            {
                throw new Exception("No finished Goods are exist");
            }
            var finishedGoodList = new List<FinishedGoodVm>();
            var overAllFpStatusName = "Pending";
            finishedGoodAll.ForEach(i =>
            {
                var finishedGoodSubList = new List<FinishedGoodVm>();
                var finishedGood = _ppsDbContext.FinishedGood.Where(x => x.ProductionGroupId == i.ProductionGroupId).ToList();
                finishedGood.ForEach(k =>
                {
                    var fPStatusName = "Pending";
                    if (k.IsApproved)
                    {
                        fPStatusName = "Approved";
                    }

                    overAllFpStatusName = fPStatusName;

                    finishedGoodSubList.Add(new FinishedGoodVm
                    {
                        ProductionGroupIdName = k.ProductionGroup.ProductionGroupId,
                        ProductionDate = k.ProductionDate,
                        ProductId = k.ProductId,
                        ProductName = k.Product.Name,
                        Quantity = k.Quantity,
                        FPStatusName = fPStatusName
                    });
                });
                var fg = new FinishedGoodVm
                {
                    Id = finishedGood.FirstOrDefault().Id,
                    ProductionGroupId = finishedGood.FirstOrDefault().ProductionGroupId,
                    ProductionGroupIdName = finishedGood.FirstOrDefault().ProductionGroup.ProductionGroupId,
                    FPStatusName = overAllFpStatusName,
                    CreatedByName = GetUserName(userId),
                    CreatedOn = finishedGood.FirstOrDefault().CreatedOn,
                    TotalCount = totalCount
                };
                fg.FinishedGoodSubList = finishedGoodSubList;
                finishedGoodList.Add(fg);
            });

            //foreach (var afinishedGood in finishedGoodAll)
            //{
            //    var fPStatusName = "Pending";
            //    if (afinishedGood.IsApproved)
            //    {
            //        fPStatusName = "Approved";
            //    }

            //    finishedGoodList.Add(new FinishedGoodVm
            //    {
            //        Id = afinishedGood.Id,
            //        ProductionGroupId = afinishedGood.ProductionGroupId,
            //        ProductionGroupIdName = afinishedGood.ProductionGroup.ProductionGroupId,
            //        ProductionDate = afinishedGood.ProductionDate,
            //        ProductId = afinishedGood.ProductId,
            //        ProductName = afinishedGood.Product.Name,
            //        Quantity = afinishedGood.Quantity,
            //        CreatedByName = GetUserName(userId),
            //        CreatedOn = afinishedGood.CreatedOn,
            //        FPStatusName = fPStatusName
            //    });
            //}
            return finishedGoodList;
        }

        #region Private memebers
        private string GetUserByOn(int userId, DateTime on)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            text += " - " + on.ToString("dd/MM/yyyy");
            return text;
        }
        private string GetUserName(int userId)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            var text = StringExtension.ToFullName(user?.FirstName, user?.LastName);
            return text;
        }

        #endregion
    }
}
