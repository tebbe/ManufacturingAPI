using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.Product;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.Services
{
    public class ProductService : IProductInterface
    {
        private readonly IProductRepository _productRepository;
        private PPSDbContext _ppsDbContext;
        public ProductService()
        {
            _productRepository = new ProductRepository();
            _ppsDbContext = new PPSDbContext();
        }
        public List<ProductVm> GetProductList()
        {
            return _productRepository.GetProductList().Select(d => new ProductVm
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code != null ? d.Code : "N/A",
                Color = d.Color,
                ProductStandardTypeName = d.ProductStandardTypeId > 0 ? d.ProductStandardType.ProductStandardTypeName : "N/A",
                Thickness = d.Thickness,
                UnitPrice = d.UnitPrice,
                Length = d.Length,
                UnitTypeName = d.UnitTypeId > 0 ? d.UnitType.UnitTypeName : "N/A",
                ProductTypeName = d.ProductTypeId > 0 ? d.ProductType.ProductTypeName : "N/A",
                AccountHeadId = d.AccountHeadId,
                CreatedOn = d.UpdatedOn!=null ? d.UpdatedOn : null,
                UpdatedOn = d.UpdatedOn != null ? d.UpdatedOn : null,

            }).ToList();
        }

        public List<AccountSubHeadVm> GetAccountSubHeadCategory()
        {
            var salesAccountPrimaryHeadId = _ppsDbContext.ReferenceTable.Where(m => m.ReferenceName == ReferenceTableEnum.SalesAccountPrimaryHeadId.ToString()).Select(m => m.ReferenceValue).FirstOrDefault();
            return _productRepository.GetAccountSubHeadCategory(Convert.ToInt32(salesAccountPrimaryHeadId)).Select(d => new AccountSubHeadVm
            {
                Id = d.Id,
                AccountSubHeadName = d.AccountSubHeadName,

            }).ToList();
        }
        public ProductVm GetProductById(int Id)
        {
            var productDetails = _productRepository.GetProductById(Id);
            if (productDetails == null)
            {
                throw new KeyNotFoundException($"Product Id: {Id} not found.");
            }
            ProductVm model = new ProductVm
            {
                Id = productDetails.Id,
                Name = productDetails.Name,
                Code = productDetails.Code != null ? productDetails.Code : "N/A",
                Color = productDetails.Color,
                ProductStandardTypeName = productDetails.ProductStandardTypeId > 0 ? productDetails.ProductStandardType.ProductStandardTypeName : "N/A",
                Thickness = productDetails.Thickness,
                UnitPrice = productDetails.UnitPrice>0?productDetails.UnitPrice:0,
                Length = productDetails.Length>0? productDetails.Length:0,
                UnitTypeName = productDetails.UnitTypeId > 0 ? productDetails.UnitType.UnitTypeName : "N/A",
                ProductTypeName = productDetails.ProductTypeId > 0 ? productDetails.ProductType.ProductTypeName : "N/A",
                AccountHeadId = productDetails.AccountHeadId,
                CreatedOn =productDetails.CreatedBy>0?productDetails.CreatedOn:null,
                UpdatedOn = productDetails.UpdatedBy>0? productDetails.UpdatedOn:null,
                ProductTypeGroupId = productDetails.AccountHead.AccountSubHeadId,
                UnitTypeId=productDetails.UnitTypeId>0? productDetails.UnitTypeId:0,
                ProductTypeId = productDetails.ProductTypeId,
                ProductStandardTypeId = productDetails.ProductStandardTypeId>0? productDetails.ProductStandardTypeId:0,

            };
            return model;
        }
        public Product SaveProduct(ProductVm productVm)
        {
            var unitTypeName = _ppsDbContext.UnitType.Where(m => m.Id == productVm.UnitTypeId).Select(m => m.UnitTypeName).FirstOrDefault(); 
            Product model = new Product
            {
                Name = productVm.Name,
                Code = productVm.Code,
                Color = productVm.Color,
                UnitPrice = productVm.UnitPrice,
                ProductStandardTypeId = productVm.ProductStandardTypeId,
                Thickness = productVm.Thickness,
                Length = productVm.Length,
                UnitTypeId = productVm.UnitTypeId,
                ProductTypeId = productVm.ProductTypeId,
                CreatedBy = productVm.CreatedBy,
                CreatedOn = productVm.CreatedOn,
            };

            AccountHead accountHead = new AccountHead
            {
                AccountHeadName = Convert.ToString(productVm.Name + "(" + productVm.Color + " " + productVm.Thickness + " " + productVm.Length + " " + unitTypeName + ")"),
                AccountSubHeadId = productVm.ProductTypeGroupId,
                Active = true,
                CompanyId = productVm.CompanyId,
                CreatedById = productVm.CreatedBy,
                CreatedDate = Convert.ToDateTime(productVm.CreatedOn)
            };
            AccountHeadOpening accountHeadOpening = new AccountHeadOpening
            {
                CompanyId = productVm.CompanyId,
                CreatedById = productVm.CreatedBy,
                FiscalYear = productVm.FiscalYear,
                CreatedDate = Convert.ToDateTime(productVm.CreatedOn)
            };
            var addProduct = _productRepository.SaveProduct(model, accountHead, accountHeadOpening);
            return addProduct;
        }
        public Product UpdateProduct(ProductVm productVm)
        {
            AccountHead accountHead = new AccountHead();
            var productDetails = _ppsDbContext.Product.Where(m => m.Id == productVm.Id).FirstOrDefault();
            var unitTypeName = _ppsDbContext.UnitType.Where(m => m.Id == productDetails.UnitTypeId).Select(m => m.UnitTypeName).FirstOrDefault();
            if (productDetails == null)
            {
                throw new KeyNotFoundException($"Product Id: {productVm.Id} does not exist.");
            }
            Product model = new Product
            {
                Id = productDetails.Id,
                Name = productVm.Name,
                Code = productVm.Code != null ? productVm.Code : null,
                Color = productVm.Color != null ? productVm.Color : null,
                UnitPrice = productVm.UnitPrice,
                ProductStandardTypeId = productVm.ProductStandardTypeId > 0 ? productVm.ProductStandardTypeId : null,
                Thickness = productVm.Thickness != null ? productVm.Thickness : null,
                Length = productVm.Length > 0 ? productVm.Length : null,
                UnitTypeId = productVm.UnitTypeId > 0 ? productVm.UnitTypeId : null,
                ProductTypeId = productVm.ProductTypeId,
                AccountHeadId = productDetails.AccountHeadId,
                CreatedBy = productDetails.CreatedBy,
                CreatedOn = productDetails.CreatedOn.Value.Date,
                UpdatedBy = productVm.UpdatedBy,
                UpdatedOn = productVm.UpdatedOn,
            };
            if(productDetails.Color!=productVm.Color||productDetails.Thickness!=productVm.Thickness || productDetails.Length != productVm.Length || productDetails.UnitTypeId != productVm.UnitTypeId)
            {
                var accountHeadDetails = _ppsDbContext.AccountHead.Where(m => m.Id == productDetails.AccountHeadId).FirstOrDefault();
                if (accountHeadDetails == null)
                {
                    throw new KeyNotFoundException($"AccountHead Id: {accountHeadDetails.Id} does not exist.");
                }

                accountHead.Id = accountHeadDetails.Id;
                accountHead.AccountHeadName = Convert.ToString(productVm.Name + "(" + productVm.Color + " " + productVm.Thickness + " " + productVm.Length + " " + unitTypeName + ")");
                accountHead.AccountSubHeadId = productVm.ProductTypeGroupId;
                accountHead.Active = accountHeadDetails.Active;
                accountHead.CompanyId = accountHeadDetails.CompanyId;
                accountHead.CreatedById = accountHeadDetails.CreatedById;
                accountHead.CreatedDate = Convert.ToDateTime(accountHeadDetails.CreatedDate);
                accountHead.UpdatedById = productVm.UpdatedBy;
                accountHead.UpdatedDate = productVm.UpdatedOn;

            }
            ProductHistory history = new ProductHistory
            {
                ProductId = productDetails.Id,
                Name = productDetails.Name,
                Code = productDetails.Code != null ? productDetails.Code : null,
                Color = productDetails.Color != null ? productDetails.Color : null,
                UnitPrice = productDetails.UnitPrice,
                ProductStandardTypeId = productDetails.ProductStandardTypeId > 0 ? productDetails.ProductStandardTypeId : null,
                Thickness = productDetails.Thickness != null ? productDetails.Thickness : null,
                Length = productDetails.Length > 0 ? productDetails.Length : null,
                UnitTypeId = productDetails.UnitTypeId > 0 ? productDetails.UnitTypeId : null,
                ProductTypeId = productDetails.ProductTypeId,
                AccountHeadId = productDetails.AccountHeadId,
               
                HistoryById = productVm.UpdatedBy,
                HistoryDate =(DateTime) productVm.UpdatedOn,
            };

            var editProduct = _productRepository.UpdateProduct(model, accountHead, history);
            return editProduct;
        }
        public List<ProductHistoryVm> GetProductHistoryByProductId(int Id)
        {
            return _productRepository.GetProductHistoryByProductId(Id).Select(d => new ProductHistoryVm
            {
                Name = d.Name,
                Code = d.Code != null ? d.Code : null,
                Color = d.Color,
                ProductStandardTypeName = d.ProductStandardTypeId > 0 ? d.ProductStandardType.ProductStandardTypeName : null,
                Thickness = d.Thickness,
                UnitPrice = d.UnitPrice,
                Length = d.Length,
                UnitTypeName = d.UnitTypeId > 0 ? d.UnitType.UnitTypeName : null,
                ProductTypeName = d.ProductTypeId > 0 ? d.ProductType.ProductTypeName : null,
                AccountHeadId = d.AccountHeadId,
                BatchId=d.BatchId,
                HistoryById= d.HistoryById,
                HistoryDate = d.HistoryDate

            }).ToList();
        }
        public List<ProductStandardTypeVm> GetProductStandardTypeList()
        {
            return _productRepository.GetProductStandardTypeList().Select(d => new ProductStandardTypeVm
            {
                Id = d.Id,
                ProductStandardTypeName = d.ProductStandardTypeName
            }).ToList();

        }
        public List<UnitTypeVm> GetUnitTypeList()
        {
            return _productRepository.GetUnitTypeList().Select(d => new UnitTypeVm
            {
                Id = d.Id,
                UnitTypeName = d.UnitTypeName
            }).ToList();
        }
        public List<ProductTypeVm> GetProductTypeList()
        {
            return _productRepository.GetProductTypeList().Select(d => new ProductTypeVm
            {
                Id = d.Id,
                ProductTypeName = d.ProductTypeName
            }).ToList();
        }

        public List<ProductDeliveryListVm> GetProductDeliveryReport(DatePickerVm datePickerVm)
        {
            var productDetails = _productRepository.GetProductDeliveryReport(datePickerVm);

            return productDetails;
        }
    }
}