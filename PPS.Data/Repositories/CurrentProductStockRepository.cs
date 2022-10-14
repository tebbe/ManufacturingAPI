using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using System.EnterpriseServices;
using System.Data;
using System.Transactions;
using System.Data.Entity;

namespace PPS.Data.Repositories
{
    public class CurrentProductStockRepository : ICurrentProductStockRepository
    {
        private PPSDbContext _ppsDbContext;

        public CurrentProductStockRepository(PPSDbContext ppsDbContext = null)
        {
            if (ppsDbContext == null)
            {
                _ppsDbContext = new PPSDbContext();
            }
            else
            {
                _ppsDbContext = ppsDbContext;
            }
        }

        public virtual bool AddAllocatedQuantityByProductId(PPSDbContext db, int productId, int quantity)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
            {
                var currentStock = db.CurrentProductStock.FirstOrDefault(x => x.ProductId == productId);
                if (currentStock == null)
                {
                    var newCurrentStock = new CurrentProductStock
                    {
                        ProductId = productId,
                        TotalQuantity = 0,
                        DeliveredQuantity = 0,
                        AllocatedQuantity = quantity,
                        AvailableQuantity = 0 - quantity
                    };
                    db.CurrentProductStock.Add(newCurrentStock);
                }
                else
                {
                    currentStock.AllocatedQuantity = currentStock.AllocatedQuantity + quantity;
                    currentStock.AvailableQuantity = currentStock.AvailableQuantity - quantity;
                    db.CurrentProductStock.Attach(currentStock);
                    db.Entry(currentStock).State = EntityState.Modified;
                }
                scope.Complete();
            }
            return true;
        }

        public virtual bool AddProductQuantityByProductId(PPSDbContext db, int productId, int quantity)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
            {
                var currentStock = db.CurrentProductStock.FirstOrDefault(x => x.ProductId == productId);
                if (currentStock == null)
                {
                    var newCurrentStock = new CurrentProductStock
                    {
                        ProductId = productId,
                        TotalQuantity = quantity,
                        DeliveredQuantity = 0,
                        AllocatedQuantity = 0,
                        AvailableQuantity = quantity
                    };
                    db.CurrentProductStock.Add(newCurrentStock);
                }
                else
                {
                    currentStock.TotalQuantity = currentStock.TotalQuantity + quantity;
                    currentStock.AvailableQuantity = currentStock.AvailableQuantity + quantity;
                    db.CurrentProductStock.Attach(currentStock);
                    db.Entry(currentStock).State = EntityState.Modified;
                }
                scope.Complete();
            }
            return true;
        }

        public virtual bool AddDeliveredQuantityByProductId(PPSDbContext db, int productId, int quantity)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
            {
                var currentStock = db.CurrentProductStock.FirstOrDefault(x => x.ProductId == productId);
                if (currentStock == null || currentStock.AllocatedQuantity < quantity)
                {
                    scope.Complete();
                    return false;
                }
                currentStock.DeliveredQuantity = currentStock.DeliveredQuantity + quantity;
                currentStock.AllocatedQuantity = currentStock.AllocatedQuantity - quantity;
                db.CurrentProductStock.Attach(currentStock);
                db.Entry(currentStock).State = EntityState.Modified;
                scope.Complete();
            }
            return true;
        }

        public virtual int GetAvailableQuantityByProductId(int productId)
        {
            var currentStockQuantity = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
            {
                var currentStock = _ppsDbContext.CurrentProductStock.FirstOrDefault(x => x.ProductId == productId);
                if (currentStock != null)
                {
                    currentStockQuantity = currentStock.AvailableQuantity;
                }
            }
            return currentStockQuantity;
        }
        //public CurrentProductStock GetCurrentProductStockByProductId(int productId)
        //{
        //    var currentProductStock = new CurrentProductStock();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
        //    {
        //        currentProductStock = _ppsDbContext.CurrentProductStock.FirstOrDefault(x => x.ProductId == productId);
        //        scope.Complete();
        //    }
        //    return currentProductStock;
        //}
    }
}