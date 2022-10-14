using PPS.API.Shared.ViewModel.Purchase;
using System.Collections.Generic;
using System.Threading.Tasks;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ReturnVm;
using PPS.API.Shared.ViewModel.User;

namespace PPS.Service.ServiceInterfaces
{
    public interface IPurchaseInterface
    {
        IList<PurchaseOrderVm> GetPurchaseOrderList(int userId);
        IList<SupplierVm> GetSupplierList(int userId);
        IList<RawMaterialTypeVm> GetRawMaterialType(int userId);
        PurchaseOrderModel SavePurchaseOrder(PurchaseOrderModel purchaseOrderModel);
        PurchaseOrderModel GetPurchaseOrderById(int userId, int poId);
        PurchaseOrderModel UpdatePurchaseOrder(PurchaseOrderModel purchaseOrderModel);
        Task<bool> VerifyPO(int poId, int userId);
        ReturnVm ApprovePO(PurchaseOrderWithSupplierRequestVm purchaseOrderWithSupplierRequestVm);
        SupplierVm GetSupplierById(PurchaseOrderWithSupplierRequestVm purchaseOrderWithSupplierRequestVm);
        Task<bool> SavePurchaseOrderTransaction(int userId, PurchaseOrderTransactionVm purchaseOrderTransactionVm);
        ReturnVm ApprovePurchaseOrderTransaction(UserVm userVm, PurchaseOrderTransactionVm purchaseOrderTransactionVm, int fiscalYear);
        List<PurchaseOrderTransactionVm> GetUnapprovedPurchaseOrderTransaction();
        List<PurchaseOrderTransactionVm> GetApprovedPurchaseOrderTransaction();
    }
}