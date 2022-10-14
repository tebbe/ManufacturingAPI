'use strict';
angular.module('AtlasPPS').factory('salesService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var salesServiceFactory = {};

        salesServiceFactory.GetSaleType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSaleType/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getCustomerList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerList/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.GetDemandOrderType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderType/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.GetDiscountType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDiscountType/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.GetProductList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetProductList/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.saveDemandOrder = function (demandOrder) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveDemandOrder', JSON.stringify(demandOrder)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.updateDemandOrder = function (demandOrder) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/UpdateDemandOrder', JSON.stringify(demandOrder)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        salesServiceFactory.saveTransactionDO = function (demandOrderTransactionVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveDemandOrderTransaction', JSON.stringify(demandOrderTransactionVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.saveTransactionInvoice = function (invoiceTransactionVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveInvoiceTransaction', JSON.stringify(invoiceTransactionVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.submitDO = function (doId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SubmitDO/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.verifyDO = function (doId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/VerifyDO/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.approveDO = function (doId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/ApproveDO/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.deliveryConfirmedDO = function (doId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/DeliveryConfirmedDO/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getDemandOrderList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandOrderListForFiltering = function (filterVm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderListForFiltering?PageIndex=' + filterVm.PageIndex + '&PageSize=' + filterVm.PageSize + '&SortColumn=' + filterVm.SortColumn + '&SortDirection=' + filterVm.SortDirection + '&StartDate=' + filterVm.StartDate + '&EndDate=' + filterVm.EndDate + '&CustomerId=' + filterVm.CustomerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandOrderUnPaidList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderUnPaidList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getDemandOrderPartiallyPaidList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderPartiallyPaidList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getDemandOrderPaidList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderPaidList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getDemandOrderEarlyPaymentList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderEarlyPaymentList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getDemandOrderEarlyPaymentPendingList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderEarlyPaymentPendingList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getDemandOrderEarlyPaymentPaidList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderEarlyPaymentPaidList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandOrderById = function (doId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrder/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getAvailableBalanceByCustomerId = function (customerId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetAvailableBalanceByCustomerId/' + customerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        //Get Purchase Order List
        salesServiceFactory.getPurchaseOrderList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetPurchaseOrderList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getInvoiceList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetInvoiceList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandIdOrderListForInvoice = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderIdListForInvoice').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandOrderByIdFromInvoice = function (doId, invoiceId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderByIdFromInvoice/' + doId + '/' + invoiceId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getMySalesHierarchy = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetSalesHierarchyById/0').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.saveInvoice = function (invoice) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveInvoice', JSON.stringify(invoice)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.updateInvoice = function (invoice) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/UpdateInvoice', JSON.stringify(invoice)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getInvoiceById = function (invoiceId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetInvoiceById/' + invoiceId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.approveInvoice = function (requestVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/ApproveInvoice', JSON.stringify(requestVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.deliveryInvoice = function (invoiceId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/DeliveryInvoice/' + invoiceId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getCustomerTransactionHistoryByCustomerId = function (doId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetCustomerTransactionHistoryByCustomerId/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getSalesPersonHistory = function (employeeId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSalesPersonHistoryByEmployeeId/' + employeeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.payDOEarlyPaymentDiscountToCustomer = function (doId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/PayDOEarlyPaymentDiscountToCustomer/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandOrderEarlyPaymentPendingTransactionList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderEarlyPaymentPendingTransactionList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getDemandOrderEarlyPaymentApprovedTransactionList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrderEarlyPaymentApprovedTransactionList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.verifyDOEarlyPaymentDiscountToCustomer = function (requestVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/VerifyDOEarlyPaymentDiscountToCustomer', JSON.stringify(requestVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getCompanySalesTargetList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetCompanySalesTargetList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.saveCompanySalesTarget = function (salesTarget) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveCompanySalesTarget', JSON.stringify(salesTarget)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getCompanySalesTargetById = function (companySalesTargetId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetCompanySalesTargetById/' + companySalesTargetId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.updateCompanySalesTarget = function (salesTarget) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/UpdateCompanySalesTarget', JSON.stringify(salesTarget)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.approveCompanySalesTarget = function (companySalesTargetId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/ApproveCompanySalesTarget/' + companySalesTargetId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getSalesTeamTargetList = function (year, month) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSalesTeamTargetList/' + year + '/' + month).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.saveSalesTeamTarget = function (salesTeamTarget) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveSalesTeamTarget', JSON.stringify(salesTeamTarget)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getSalesDivisionList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSalesDivisionList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getSalesAreaList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSalesAreaList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getSalesReportList = function (startDate, endDate, salesDivisionId, salesAreaId, employeeId, customerId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSalesReportList/' + startDate + '/' + endDate + '/' + salesDivisionId + '/' + salesAreaId + '/' + employeeId + '/' + customerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getProductionForecastList = function (year, month) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetProductionForecastList/' + year + '/' + month).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.saveProductionForecast = function (productionForecast) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveProductionForecast', JSON.stringify(productionForecast)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getSalesAreaWithSalesOfficerWithCustomerList = function (salesDivisionId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetSalesAreaWithSalesOfficerWithCustomerList/' + salesDivisionId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        //return invoice list
        salesServiceFactory.getInvoiceReturnList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/InvoiceReturnList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.getAllInvoiceList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetAllInvoiceList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.saveReturnInvoice = function (returnInvoice) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/SaveReturnInvoice', JSON.stringify(returnInvoice)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getInvoiceReturnById = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetInvoiceReturnById/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.updateReturnInvoice = function (returnInvoice) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/UpdateReturnInvoice', JSON.stringify(returnInvoice)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        salesServiceFactory.approveReturnInvoice = function (Id) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/ApproveReturnInvoice/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        //return invoice end

        // Invoice Delivery Challan Start

        salesServiceFactory.getInvoiceDeliveryChallanList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/InvoiceDeliveryQuantityList/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getInvoiceByIdForDeliveryChallan = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/InvoiceDetailsByIdForDeliveryChallan/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.saveInvoiceDeliveryChallan = function (deliveryQuantity) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/DeliveryQuantitySave', JSON.stringify(deliveryQuantity)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getInvoiceDeliveryChallanById = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDeliveryQuantityById/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.invoiceDeliveryChallanApprove = function (id) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/ApproveDeliveryQuantityById/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.getInvoiceUnDeliveryChallanById = function (id) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/GetUndeliveryQuantityById/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        salesServiceFactory.updateInvoiceDeliveryChallan = function (deliveryQuantity) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Sales/DeliveryQuantityUpdate', JSON.stringify(deliveryQuantity)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        //Invoice Delivery Challan End
        //sales report list service start

        salesServiceFactory.getTotalSalesReportList = function (vm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetTotalSalesReportList/' + vm.StartDate + '/' + vm.EndDate ).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };



        return salesServiceFactory;
    }]);
