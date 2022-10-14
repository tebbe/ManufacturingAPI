using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PPS.API.Shared.ViewModel.Sales;
using PPS.Data.Edmx;
using PPS.Data.Repositories;
using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPS.Data.Tests
{
    [TestClass]
    public class SalesRepositoryTests
    {
        private Mock<PPSDbContext> _mockPpsDbContext;
        private Mock<ISalesRepository> _mockSalesRepository;
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private  Mock<IEmployeeRepository> _mockEmployeeRepository;

        public SalesRepositoryTests()
        {
            _mockPpsDbContext = new Mock<PPSDbContext>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockSalesRepository = new Mock<ISalesRepository>();
        }

        [TestMethod]
        public void SaveCompanySalesTarget_CheckRecordExist()
        {
            //// Arrange
            //var companySalesTargetVm = new CompanySalesTargetVm
            //{
            //    SalesMonth = new DateTime(2018, 1, 1),
            //    SalesTarget = 2500000
            //};
            //var companySalesTargetList = new List<CompanySalesTarget>
            //{
            //    new CompanySalesTarget
            //    {
            //        SalesMonth = new DateTime(2018, 1, 1),
            //        SalesTarget = 2500000
            //    }
            //};
            ////var mockSet = new Mock<PPSDbContext>();
            ////mockSet.Setup(x => x.CompanySalesTarget.ToList()).Returns(companySalesTargetList);
            //_mockPpsDbContext.Setup(x => x.CompanySalesTarget.ToList()).Returns(companySalesTargetList);
            //// Act
            //var createdCompanySalesTarget = _mockSalesRepository.Object.SaveCompanySalesTarget(companySalesTargetVm);

            //// Assert
            //Assert.AreEqual(createdCompanySalesTarget.SalesTarget, companySalesTargetVm.SalesTarget);
        }
    }
}
