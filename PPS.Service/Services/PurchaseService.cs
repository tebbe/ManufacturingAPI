using PPS.Service.ServiceInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ReturnVm;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.API.Shared.ViewModel.User;

namespace PPS.Service.Services
{
    public class PurchaseService : IPurchaseInterface
    {
        private IPurchaseRepository _purchaseRepository;
        public PurchaseService()
        {
            _purchaseRepository = new PurchaseRepository();
        }

        public IList<PurchaseOrderVm> GetPurchaseOrderList(int userId)
        {
            return _purchaseRepository.GetPurchaseOrderList(userId);
        }

        public IList<SupplierVm> GetSupplierList(int userId)
        {
            return _purchaseRepository.GetSupplierList(userId);
        }

        public IList<RawMaterialTypeVm> GetRawMaterialType(int userId)
        {
            return _purchaseRepository.GetRawMaterialType(userId);
        }

        public PurchaseOrderModel SavePurchaseOrder(PurchaseOrderModel purchaseOrderModel)
        {
            var PurchaseDto = _purchaseRepository.SavePurchaseOrder(purchaseOrderModel);
            return PurchaseDto;
        }

        public PurchaseOrderModel GetPurchaseOrderById(int userId, int poId)
        {
            var vm = _purchaseRepository.GetPurchaseOrderById(userId, poId);
            return vm;
        }

        public PurchaseOrderModel UpdatePurchaseOrder(PurchaseOrderModel purchaseOrderModel)
        {
            var PurchaseDto = _purchaseRepository.UpdatePurchaseOrder(purchaseOrderModel);
            return PurchaseDto;
        }

        public async Task<bool> VerifyPO(int poId, int userId)
        {
            return await _purchaseRepository.VerifyPO(poId, userId);
        }

        public ReturnVm ApprovePO(PurchaseOrderWithSupplierRequestVm purchaseOrderWithSupplierRequestVm)
        {
            return _purchaseRepository.ApprovePO(purchaseOrderWithSupplierRequestVm);
        }

        public SupplierVm GetSupplierById(PurchaseOrderWithSupplierRequestVm purchaseOrderWithSupplierRequestVm)
        {
            return _purchaseRepository.GetSupplierById(purchaseOrderWithSupplierRequestVm);
        }

        public async Task<bool> SavePurchaseOrderTransaction(int userId, PurchaseOrderTransactionVm purchaseOrderTransactionVm)
        {
            return await _purchaseRepository.SavePurchaseOrderTransaction(userId, purchaseOrderTransactionVm);
        }

        public ReturnVm ApprovePurchaseOrderTransaction(UserVm userVm, PurchaseOrderTransactionVm purchaseOrderTransactionVm, int fiscalYear)
        {
            return _purchaseRepository.ApprovePurchaseOrderTransaction(userVm, purchaseOrderTransactionVm, fiscalYear);
        }

        public List<PurchaseOrderTransactionVm> GetUnapprovedPurchaseOrderTransaction()
        {
            return _purchaseRepository.GetUnapprovedPurchaseOrderTransaction();
        }
        public List<PurchaseOrderTransactionVm> GetApprovedPurchaseOrderTransaction()
        {
            return _purchaseRepository.GetApprovedPurchaseOrderTransaction();
        }
    }
}