using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Product;
using PPS.Data.Edmx;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProductList();
        IQueryable<AccountSubHead> GetAccountSubHeadCategory(int Id);
        Product GetProductById(int id);
        Product SaveProduct(Product model,AccountHead accountHead,AccountHeadOpening accountHeadOpening);
        Product UpdateProduct(Product model,AccountHead accountHead, ProductHistory history);
        IQueryable<ProductHistory> GetProductHistoryByProductId(int id);
        IQueryable<ProductStandardType> GetProductStandardTypeList();
        IQueryable<UnitType> GetUnitTypeList();
        IQueryable<ProductType> GetProductTypeList();
        List<ProductDeliveryListVm> GetProductDeliveryReport(DatePickerVm datePickerVm);
    }
}