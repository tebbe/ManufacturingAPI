using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using PPS.Service.ServiceInterfaces;

namespace PPS.Service.Services
{
    public class CurrentProductStockService : ICurrentProductStockService
    {
        private ICurrentProductStockRepository _currentProductStockRepo;
        public CurrentProductStockService(ICurrentProductStockRepository currentProductStockRepo = null)
        {
            if (_currentProductStockRepo == null)
            {
                _currentProductStockRepo = new CurrentProductStockRepository();
            }
            else
            {
                _currentProductStockRepo = currentProductStockRepo;
            }
        }
        public bool AddAllocatedQuantityByProductId(int productId, int quantity)
        {
            return _currentProductStockRepo.AddAllocatedQuantityByProductId(null, productId, quantity);
        }

        public bool AddProductQuantityByProductId(int productId, int quantity)
        {
            return _currentProductStockRepo.AddProductQuantityByProductId(productId, quantity);
        }

        public bool AddDeliveredQuantityByProductId(int productId, int quantity)
        {
            return _currentProductStockRepo.AddDeliveredQuantityByProductId(productId, quantity);
        }

        public int GetAvailableQuantityByProductId(int productId)
        {
            return _currentProductStockRepo.GetAvailableQuantityByProductId(productId);
        }
    }
}