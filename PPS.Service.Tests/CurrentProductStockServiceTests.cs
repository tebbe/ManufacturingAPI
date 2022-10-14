using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PPS.Data.RepositoryInterfaces;
using Moq;
using PPS.Data.Edmx;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System.Collections.Generic;
using System.Linq;

namespace PPS.Service.Tests
{
    [TestClass]
    public class CurrentProductStockServiceTests
    {
        //[TestMethod]
        //public void GetAvailableQuantityByProductId()
        //{
        //    // Arrange
        //    var productId = 1;
        //    var currentStock = new CurrentProductStock
        //    {
        //        Id = productId,
        //        TotalQuantity = 50,
        //        AllocatedQuantity = 40,
        //        SoldQuantity = 6,
        //        AvailableQuantity = 4
        //    };
        //    var db = new FakeDbSet<CurrentProductStock>();
        //    db.Add(currentStock);

        //    // Act
        //    db.Setup(x => x.CurrentProductStock).Returns(db);
        //    var mockCurrentProductStockRepository = new Mock<ICurrentProductStockRepository>(db.Object);
        //    mockCurrentProductStockRepository
        //        .Setup(x => x.GetAvailableQuantityByProductId(productId))
        //        .Returns(currentStocks[0].AvailableQuantity);
        //    var service = new CurrentProductStockService(mockCurrentProductStockRepository.Object);
        //    var availableQuantity = service.GetAvailableQuantityByProductId(productId);
        //    // Assert
        //    Assert.AreEqual(currentStocks[0].AvailableQuantity, availableQuantity);
        //}
    }
}
