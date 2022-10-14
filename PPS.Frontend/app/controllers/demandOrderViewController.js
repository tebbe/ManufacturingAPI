//'use strict';

angular.module('AtlasPPS').controller('demandOrderViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', 'sharedService','reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, reportSettings) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.DemandOrderDiscountSetting = {
                RegularDiscount: PpsConstant.DemandOrderDiscountSetting.RegularDiscount,
                SpecialDiscount: PpsConstant.DemandOrderDiscountSetting.SpecialDiscount,
                AdditionalDiscount: PpsConstant.DemandOrderDiscountSetting.AdditionalDiscount,
                ExtraDiscount: PpsConstant.DemandOrderDiscountSetting.ExtraDiscount,
                CashBack: PpsConstant.DemandOrderDiscountSetting.CashBack
            }

            $scope.demandOrderPaymentStatusColorClass = "";
            $scope.demandOrder = {};
            $scope.newDOTransaction = {};
            $scope.customerAvailableAmount = 0;
            $scope.CustomerTransactionHistory = {};

            var getDemandOrderById = function (doId) {
                authService.loadingOn();
                var promise = salesService.getDemandOrderById(doId);
                promise.then(function (response) {
                    $scope.demandOrder = response.demandOrder;
                    $scope.customerAvailableAmount = $scope.demandOrder.TotalBalanceAmount;
                    getAvailableBalanceByCustomerId($scope.demandOrder.CustomerId);
                    getCustomerTransactionHistoryByCustomerId(doId);
                    $scope.demandOrderPaymentStatusColorClass = sharedService.getDemandOrderPaymentStatusClass($scope.demandOrder.DOPaymentStatusId);
                    authService.loadingOff();
                }, function (err) {
                    $scope.demandOrder = {};
                    authService.loadingOff();
                });
            };
            var getAvailableBalanceByCustomerId = function (customerId) {
                var promise = salesService.getAvailableBalanceByCustomerId(customerId);
                authService.loadingOn();
                promise.then(function (response) {
                    $scope.customerAvailableAmount = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customerAvailableAmount = 0;
                    authService.loadingOff();
                });
            };          

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

                var doId;
                if ($state.params && $state.params.doId) {
                    doId = _.parseInt($state.params.doId);
                }
                getDemandOrderById(doId);
                $scope.newDOTransaction = {
                    DemandOrderId: doId,
                    TransactionDate: new Date()
                };
                //var locationVm = $location.path('/Invoice/Invoice/View/');
            };

            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.goToInvoiceView = function (Id) {           
                $window.open(reportSettings.reportBaseUri + 'Invoice/View/' +Id, '_blank');
                
            }

            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            //$scope.clickDatePicker = function () {
            //    $('.md-datepicker-input-container > input').attr('disabled', true);
            //};

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $scope.dtOptionsDemandOrderDetail = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withPaginationType('full_numbers').withDisplayLength(100)
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $scope.submitDOClick = function () {
                var doId = $scope.selectedDO.Id;
                authService.loadingOn();
                var promise = salesService.submitDO(doId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeDOModal();
                    getDemandOrderById(doId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeDOModal = function () {
                $('#submitDOModal').modal('toggle');
            };
            $scope.showSubmitDOModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#submitDOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.verifyDOClick = function () {
                var doId = $scope.selectedDO.Id;
                authService.loadingOn();
                var promise = salesService.verifyDO(doId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeVerifyDOModal();
                    getDemandOrderById(doId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeVerifyDOModal = function () {
                $('#verifyDOModal').modal('toggle');
            };
            $scope.showVerifyDOModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#verifyDOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.approveDOClick = function () {
                var doId = $scope.selectedDO.Id;
                authService.loadingOn();
                var promise = salesService.approveDO(doId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeApproveDOModal();
                    getDemandOrderById(doId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeApproveDOModal = function () {
                $('#approveDOModal').modal('toggle');
            };
            $scope.showApproveDOModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#approveDOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.deliveryConfirmedDOClick = function () {
                var doId = $scope.selectedDO.Id;
                authService.loadingOn();
                var promise = salesService.deliveryConfirmedDO(doId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeDeliveryConfirmedDOModal();
                    getDemandOrderById(doId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeDeliveryConfirmedDOModal = function () {
                $('#deliveryConfirmedDOModal').modal('toggle');
            };
            $scope.showDeliveryConfirmedDOModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#deliveryConfirmedDOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.saveTransactionDOClick = function () {
                var demandOrderTransactionVm = $scope.newDOTransaction;
                authService.loadingOn();
                var promise = salesService.saveTransactionDO(demandOrderTransactionVm);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeTransactionDOModal();
                    getDemandOrderById(demandOrderTransactionVm.DemandOrderId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeTransactionDOModal = function () {
                $('#transactionDOModal').modal('toggle');
            };
            $scope.showTransactionDOModal = function (doModel) {
                $scope.newDOTransaction.TransactionAmount = $scope.customerAvailableAmount > $scope.demandOrder.TotalDueAmount ? $scope.demandOrder.TotalDueAmount : $scope.customerAvailableAmount;
                $scope.selectedDO = doModel;
                $('#transactionDOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };     

            $scope.gotoDOEdit = function (doId) {
                $window.open(reportSettings.reportBaseUri + "Sales/DemandOrder/Edit/" + doId, '_self');
                //$location.path("/Sales/DemandOrder/Edit/" + doId);
                location.reload();
            };

            $scope.navigateToDoPrint = function (d) {
                $window.open(reportSettings.reportBaseUri + 'reports/demandOrderPrint/' + d.Id, '_blank');
            };

            var getCustomerTransactionHistoryByCustomerId = function (doId) {
                authService.loadingOn();
                var promise = salesService.getCustomerTransactionHistoryByCustomerId(doId);
                promise.then(function (response) {
                    $scope.CustomerTransactionHistory = response;
                    calculationTotalAmount();
                    authService.loadingOff();
                }, function (err) {
                    $scope.CustomerTransactionHistory = {};
                    authService.loadingOff();
                });
            };
            var calculationTotalAmount = function () {
                var totalDoAmount = 0;
                var totalDoPaidAmount = 0;
                var totalDoBalanceAmount = 0;
                var totalDoInvoiceAmount = 0;
                var totalDoInvoiceBalance = 0;
                for (var i = 0; i < $scope.CustomerTransactionHistory.length; i++) {
                    totalDoAmount += $scope.CustomerTransactionHistory[i].DOAmount;
                    totalDoPaidAmount += $scope.CustomerTransactionHistory[i].DOPaidAmount;
                    totalDoBalanceAmount += $scope.CustomerTransactionHistory[i].DOBalanceAmount;
                    totalDoInvoiceAmount += $scope.CustomerTransactionHistory[i].DOInvoiceAmount;
                    totalDoInvoiceBalance += $scope.CustomerTransactionHistory[i].DOInvoiceBalance;
                }
                $scope.TotalDOAmount = totalDoAmount;
                $scope.TotalDOPaidAmount = totalDoPaidAmount;
                $scope.TotalDOBalanceAmount = totalDoBalanceAmount;
                $scope.TotalDOInvoiceAmount = totalDoInvoiceAmount;
                $scope.TotalDOInvoiceBalance = totalDoInvoiceBalance;
            }
        }]);