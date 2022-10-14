using PPS.API.Shared.ViewModel.Product;
using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.ServiceInterfaces
{
    public interface IProductInterface
    {
        List<ProductVm> GetProductList();
        ProductVm GetProductById(int id);
        Product SaveProduct(ProductVm productVm);
        Product UpdateProduct(ProductVm productVm);
        List<ProductHistoryVm> GetProductHistoryByProductId(int productId);
        List<ProductStandardTypeVm> GetProductStandardTypeList();
        List<UnitTypeVm> GetUnitTypeList();
        List<ProductTypeVm> GetProductTypeList();
        List<AccountSubHeadVm> GetAccountSubHeadCategory();
        List<ProductDeliveryListVm> GetProductDeliveryReport(DatePickerVm datePickerVm);
    }
}