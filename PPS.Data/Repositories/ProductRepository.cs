using PPS.API.Shared.ViewModel.Product;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PPS.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PPSDbContext _ppsDbContext;
        public ProductRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public IQueryable<Product> GetProductList()
        {
            return _ppsDbContext.Product;
        }
        public IQueryable<AccountSubHead> GetAccountSubHeadCategory(int id)
        {
            return _ppsDbContext.AccountSubHead.Where(m => m.AccountPrimaryHeadId == id);
        }
        public Product GetProductById(int Id)
        {
            return _ppsDbContext.Product.Where(m => m.Id == Id).FirstOrDefault();
        }
        public Product SaveProduct(Product product, AccountHead accountHead, AccountHeadOpening accountHeadOpening)
        {

            using (var db = new PPSDbContext())
            {
                db.AccountHead.Add(accountHead);
                product.AccountHeadId = accountHead.Id;
                db.Product.Add(product);
                accountHeadOpening.AccountHeadId = accountHead.Id;
                db.AccountHeadOpening.Add(accountHeadOpening);
                db.SaveChanges();
            }
            return product;
        }

        public Product UpdateProduct(Product model, AccountHead accountHead, ProductHistory history)
        {
            using (var db = new PPSDbContext())
            {
                db.ProductHistory.Add(history);
                if (accountHead != null)
                {
                    db.AccountHead.Attach(accountHead);
                }
                db.Product.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            return model;
        }
        public IQueryable<ProductHistory> GetProductHistoryByProductId(int id)
        {
            return _ppsDbContext.ProductHistory.Where(m => m.ProductId == id);
        }
        public IQueryable<ProductStandardType> GetProductStandardTypeList()
        {
            return _ppsDbContext.ProductStandardType;
        }
        public IQueryable<UnitType> GetUnitTypeList()
        {
            return _ppsDbContext.UnitType;
        }
        public IQueryable<ProductType> GetProductTypeList()
        {
            return _ppsDbContext.ProductType;
        }

        public List<ProductDeliveryListVm> GetProductDeliveryReport(DatePickerVm datePickerVm)
        {
            var data = new List<InvoiceDetail>();
            var productDeliverydetails = new List<ProductDeliveryListVm>();
            if ((datePickerVm.StartDate != null && datePickerVm.EndDate != null) && (datePickerVm.CustomerId > 0 && datePickerVm.ProductId > 0))
            {
                data = _ppsDbContext.InvoiceDetail.Where(m => m.Invoice.CreatedOn >= datePickerVm.StartDate && m.Invoice.CreatedOn <= datePickerVm.EndDate).ToList();
                data = data.Count > 0 ? data.Where(m => m.Invoice.DemandOrder.CustomerId == datePickerVm.CustomerId && m.ProductId == datePickerVm.ProductId).ToList() : data;
            }
            else if ((datePickerVm.StartDate != null && datePickerVm.EndDate != null) && (datePickerVm.CustomerId > 0 || datePickerVm.ProductId > 0))
            {
                data = _ppsDbContext.InvoiceDetail.Where(m => m.Invoice.CreatedOn >= datePickerVm.StartDate && m.Invoice.CreatedOn <= datePickerVm.EndDate).ToList();
                data = datePickerVm.CustomerId > 0 ? data.Where(m => m.Invoice.DemandOrder.CustomerId == datePickerVm.CustomerId).ToList() : data.Where(m => m.ProductId == datePickerVm.ProductId).ToList();
            }
            else if (datePickerVm.CustomerId > 0 && datePickerVm.ProductId > 0)
            {
                data = _ppsDbContext.InvoiceDetail.Where(m => m.Invoice.DemandOrder.CustomerId == datePickerVm.CustomerId && m.ProductId == datePickerVm.ProductId).ToList();
            }
            else if (datePickerVm.CustomerId > 0 || datePickerVm.ProductId > 0)
            {
                data = datePickerVm.CustomerId > 0 ? _ppsDbContext.InvoiceDetail.Where(m => m.Invoice.DemandOrder.CustomerId == datePickerVm.CustomerId).ToList() : _ppsDbContext.InvoiceDetail.Where(m => m.ProductId == datePickerVm.ProductId).ToList();
            }
            else if (datePickerVm.StartDate != null && datePickerVm.EndDate != null)
            {
                data = _ppsDbContext.InvoiceDetail.ToList().Where(m => m.Invoice.CreatedOn >= datePickerVm.StartDate && m.Invoice.CreatedOn <= datePickerVm.EndDate).ToList();
            }
            data.GroupBy(m => new { m.ProductId }).ToList().ForEach(d =>
              {
                  productDeliverydetails.Add(new ProductDeliveryListVm
                  {
                      ProductId = d.FirstOrDefault().ProductId,
                      Name = d.FirstOrDefault().Product.Name,
                      Code = d.FirstOrDefault().Product.Code,
                      Length=d.FirstOrDefault().Product.Length,
                      Thickness= d.FirstOrDefault().Product.Thickness,
                      Color = d.FirstOrDefault().Product.Color,
                      Ammount = (double)d.Sum(m => m.TotalAmount),
                      Quantity = d.Sum(m => m.Quantity),
                  });

              });

            return productDeliverydetails;
        }
    }
}