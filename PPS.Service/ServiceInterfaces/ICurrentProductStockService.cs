using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.ServiceInterfaces
{
    public interface ICurrentProductStockService
    {
        int GetAvailableQuantityByProductId(int productId);
        bool AddProductQuantityByProductId(int productId, int quantity);
        bool AddDeliveredQuantityByProductId(int productId, int quantity);
        bool AddAllocatedQuantityByProductId(int productId, int quantity);
    }
}