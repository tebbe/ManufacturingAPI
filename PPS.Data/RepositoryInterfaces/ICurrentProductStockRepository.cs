using PPS.Data.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.RepositoryInterfaces
{
    public interface ICurrentProductStockRepository
    {
        int GetAvailableQuantityByProductId(int productId);
        bool AddProductQuantityByProductId(PPSDbContext db, int productId, int quantity);
        bool AddDeliveredQuantityByProductId(PPSDbContext db, int productId, int quantity);
        bool AddAllocatedQuantityByProductId(PPSDbContext db, int productId, int quantity);        
    }
}