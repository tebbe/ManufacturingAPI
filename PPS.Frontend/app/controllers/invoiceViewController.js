'use strict';

angular.module('AtlasPPS').controller('invoiceViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', 'sharedService', 'bankService','reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService, reportSettings) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $scope.DemandOrderDiscountSetting = {
                RegularDiscount: PpsConstant.DemandOrderDiscountSetting.RegularDiscount,
                SpecialDiscount: PpsConstant.DemandOrderDiscountSetting.SpecialDiscount,
                AdditionalDiscount: PpsConstant.DemandOrderDiscountSetting.AdditionalDiscount,
                ExtraDiscount: PpsConstant.DemandOrderDiscountSetting.ExtraDiscount,
                CashBack: PpsConstant.DemandOrderDiscountSetting.CashBack
            };
            $scope.company = {};
            $scope.invoice = {};
            $scope.demandOrder = {};
            $scope.invoiceProductDetail = {};

            $scope.newInvoiceTransaction = {};
            $scope.customerAvailableAmount = 0;

            var getInvoiceById = function (invoiceId) {
                authService.loadingOn();
                var promise = salesService.getInvoiceById(invoiceId);
                promise.then(function (response) {
                    $scope.invoice = response.invoice;
                    $scope.customerAvailableAmount = $scope.invoice.CustomerRemainingBalance;
                    //getAvailableBalanceByCustomerId($scope.invoice.CustomerId);
                    getDemandOrderFromInvoiceById($scope.invoice.DemandOrderId, $scope.invoice.Id);
                    //finalAmountCalculation();
                    authService.loadingOff();
                }, function (err) {
                    $scope.invoice = {};
                    authService.loadingOff();
                });
            };

            var getDemandOrderFromInvoiceById = function (doId, invoiceId) {
                authService.loadingOn();
                var promise = salesService.getDemandOrderByIdFromInvoice(doId, invoiceId);
                promise.then(function (response) {
                    $scope.demandOrder = response;
                    $scope.invoiceProductDetail = $scope.demandOrder.DemandOrderDetail;
                    authService.loadingOff();
                }, function (err) {
                    $scope.demandOrder = [];
                    authService.loadingOff();
                });
            };

            //transaction start
            $scope.showTransactionInvoiceModal = function () {
                $scope.newInvoiceTransaction.TransactionAmount = $scope.customerAvailableAmount > $scope.invoice.TotalDueAmount ? $scope.invoice.TotalDueAmount : $scope.customerAvailableAmount;

                $('#transactionInvoiceModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };     
            $scope.closeTransactionInvoiceModal = function () {
                $('#transactionInvoiceModal').modal('toggle');
            };
            $scope.saveTransactionInvoiceClick = function () {
                var invoiceTransactionVm = $scope.newInvoiceTransaction;
                authService.loadingOn();
                var promise = salesService.saveTransactionInvoice(invoiceTransactionVm);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeTransactionInvoiceModal();
                    getInvoiceById(invoiceTransactionVm.InvoiceId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            //var getInvoiceByInvoiceId = function (doId) {
            //    authService.loadingOn();
            //    var promise = salesService.getDemandOrderById(doId);
            //    promise.then(function (response) {
            //        $scope.demandOrder = response;
            //        $scope.customerAvailableAmount = $scope.demandOrder.TotalCustomerBalance;
            //        getAvailableBalanceByCustomerId($scope.demandOrder.CustomerId);
            //        authService.loadingOff();
            //    }, function (err) {
            //        $scope.demandOrder = {};
            //        authService.loadingOff();
            //    });
            //};
            //var getAvailableBalanceByCustomerId = function (customerId) {
            //    var promise = salesService.getAvailableBalanceByCustomerId(customerId);
            //    authService.loadingOn();
            //    promise.then(function (response) {
            //        $scope.customerAvailableAmount = response;
            //        authService.loadingOff();
            //    }, function (err) {
            //        $scope.customerAvailableAmount = 0;
            //        authService.loadingOff();
            //    });
            //}; 
            //transaction end



            //var finalAmountCalculation = function () {
            //    var tUnitAmount = 0;
            //    var finalDiscount = 0;

            //    var totalDiscount = $scope.invoice.TotalDiscountInPercentage;
            //    if (isNaN(totalDiscount)) {
            //        totalDiscount = 0;
            //    }

            //    if ($scope.invoice.InvoiceDetail) {
            //        for (var prd in $scope.invoice.InvoiceDetail) {
            //            tUnitAmount += $scope.invoice.InvoiceDetail[prd].TotalPrice;
            //        }
            //    }

            //    $scope.invoiceTotalAmount = tUnitAmount;
            //    if (totalDiscount !== 0) {
            //        finalDiscount = totalDiscount / 100;
            //        $scope.invoiceTotalDiscountAmount = $scope.invoiceTotalAmount * finalDiscount;
            //        $scope.invoiceTotalGrandAmount = Math.round($scope.invoiceTotalAmount - $scope.invoiceTotalDiscountAmount);
            //    }
            //    else {
            //        $scope.invoiceTotalDiscountAmount = finalDiscount;
            //        $scope.invoiceTotalGrandAmount = Math.round($scope.invoiceTotalAmount - $scope.invoiceTotalDiscountAmount);
            //    }
            //};
            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    fiscalYear = authData.fiscalYear;
                    companyId = authData.companyId;
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }

                var invoiceId;
                if ($state.params && $state.params.Id) {
                    invoiceId = _.parseInt($state.params.Id);
                }
                getInvoiceById(invoiceId);
              
                $scope.newInvoiceTransaction = {
                    InvoiceId: invoiceId,
                    TransactionDate: new Date()
                };
            };

            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.navigateToInvoiceEdit = function (id) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/Edit/' + id, '_blank');
            };

            $scope.navigateToInvoicePrint = function (id) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/DeliveryPrint/' + id, '_blank');
            };
            $scope.gotoInvoiceDetailsPrint = function (id) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/InvoiceDetail/Print/' + id, '_blank');
            };

            //$scope.gotoInvoiceDetailsPrint = function (id) {
            //    $location.path("/Invoice/InvoiceDetail/Print/"+id);
            //};

            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                ]);

            $scope.approveInvoiceClick = function () {
                authService.loadingOn();
                var invoiceId = $scope.invoice.Id;

                var requestVm = {
                    InvoiceId: invoiceId,
                    FiscalYear: fiscalYear
                };

                var promise = salesService.approveInvoice(requestVm);
                promise.then(function (response) {
                    
                    $scope.closeApproveInvoiceModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getInvoiceById(invoiceId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeApproveInvoiceModal = function () {
                $('#approveInvoiceModal').modal('toggle');
            };
            $scope.showApproveInvoiceModal = function (invoiceModel) {
                $scope.selectedInvoice = invoiceModel;
                $('#approveInvoiceModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.deliveryInvoiceClick = function () {
                var invoiceId = $scope.invoice.Id;
                authService.loadingOn();
                var promise = salesService.deliveryInvoice(invoiceId);
                promise.then(function (response) {
                    $scope.closeDeliveryInvoiceModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getInvoiceById(invoiceId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeDeliveryInvoiceModal = function () {
                $('#deliveryInvoiceModal').modal('toggle');
            };
            $scope.showDeliveryInvoiceModal = function (invoiceModel) {
                $scope.selectedInvoice = invoiceModel;
                $('#deliveryInvoiceModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
        }]);