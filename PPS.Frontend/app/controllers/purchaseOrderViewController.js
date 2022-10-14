'use strict';

angular.module('AtlasPPS').controller('purchaseOrderViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'purchaseOrderService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', 'sharedService', 'bankService',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, purchaseOrderService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.purchaseOrderPaymentStatusColorClass = "";
            $scope.purchaseOrder = {};
            $scope.newPOTransaction = {};
            $scope.customerAvailableAmount = 0;

            $scope.supplierVm = {};
            $scope.cashBanks = [];
            $scope.newPurchaseOrderTransaction = {};

            var finalAmountCalculation = function () {
                var tUnitAmount = 0;
                if ($scope.purchaseOrder.PurchaseOrderDetail) {
                    for (var prd in $scope.purchaseOrder.PurchaseOrderDetail) {
                        tUnitAmount += $scope.purchaseOrder.PurchaseOrderDetail[prd].TotalUnitPrice;
                    }
                }
                $scope.purchaseOrder.TotalAmount = tUnitAmount;
            };

            var getPurchaseOrderById = function (poId) {
                authService.loadingOn();
                var promise = purchaseOrderService.getPurchaseOrderById(poId);
                promise.then(function (response) {
                    $scope.purchaseOrder = response;
                    var supplierId = $scope.purchaseOrder.SupplierId;
                    var purchaseOrderId = $scope.purchaseOrder.Id;
                    var requestVm = {
                        SupplierId : supplierId,
                        PurchaseOrderId: purchaseOrderId
                    };
                    getSupplierById(requestVm);
                    $scope.newPurchaseOrderTransaction = {
                        PurchaseOrderId: $scope.purchaseOrder.Id,
                        SupplierId: supplierId,
                        TransactionDate: new Date()
                    };
                    finalAmountCalculation();
                    //$scope.customerAvailableAmount = $scope.purchaseOrder.TotalBalanceAmount;
                    //getAvailableBalanceByCustomerId($scope.purchaseOrder.CustomerId);
                    //$scope.purchaseOrderPaymentStatusColorClass = sharedService.getPurchaseOrderPaymentStatusClass($scope.purchaseOrder.POPaymentStatusId);
                    authService.loadingOff();
                }, function (err) {
                    $scope.purchaseOrder = {};
                    authService.loadingOff();
                });
            };

            var getSupplierById = function (requestVm) {
                authService.loadingOn();
                var promise = purchaseOrderService.getSupplierById(requestVm);
                promise.then(function (response) {
                    $scope.supplierVm = response;
                    getBanks($scope.supplierVm.Id);
                    authService.loadingOff();
                }, function (err) {
                    $scope.supplierVm = {};
                    authService.loadingOff();
                });
            };
            var getBanks = function (supplierId) {
                authService.loadingOn();
                var promise = bankService.getBankCashAccountHeadList(supplierId);
                promise.then(function (response) {
                    $scope.cashBanks = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.cashBanks = [];
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

                var poId;
                if ($state.params && $state.params.poId) {
                    poId = _.parseInt($state.params.poId);
                }
                getPurchaseOrderById(poId);

                $scope.newPOTransaction = {
                    POId: poId,
                    TransactionDate: new Date()
                };
                
            };

            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                ]);

            $scope.verifyPOClick = function () {
                var poId = $scope.purchaseOrder.Id;
                authService.loadingOn();
                var promise = purchaseOrderService.verifyPO(poId);
                promise.then(function (response) {
                    $scope.closeVerifyPOModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getPurchaseOrderById(poId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeVerifyPOModal = function () {
                $('#verifyPOModal').modal('toggle');
            };
            $scope.showVerifyPOModal = function (poModel) {
                $scope.selectedPO = poModel;
                $('#verifyPOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.approvePOClick = function () {
                var poId = $scope.purchaseOrder.Id;

                var requestVm = {
                    PurchaseOrderId: poId,
                    FiscalYear: fiscalYear
                };

                authService.loadingOn();

                var promise = purchaseOrderService.approvePO(requestVm);
                promise.then(function (response) {
                    if (response.Success === true) {
                        notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                        getPurchaseOrderById(poId);
                        $scope.closeApprovePOModal();
                        authService.loadingOff();
                    } else {
                        notificationService.showErrorNotificatoin(response.Message);
                        $scope.closeApprovePOModal();
                        authService.loadingOff();
                    }
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeApprovePOModal = function () {
                $('#approvePOModal').modal('toggle');
            };
            $scope.showApprovePOModal = function (poModel) {
                $scope.selectedPO = poModel;
                $('#approvePOModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.gotoPOEdit = function (poId) {
                $location.path("/Purchase/PurchaseOrder/Edit/" + poId);
                $window.location.reload();
            };

            $scope.saveTransactionClick = function () {
                $scope.newPurchaseOrderTransaction.CashBankAccountHeadId = $scope.selectedAccount.Id;
                authService.loadingOn();
                var promise = purchaseOrderService.savePurchaseOrderTransaction($scope.newPurchaseOrderTransaction);
                promise.then(function (response) {
                    $scope.closeTransactionModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getPurchaseOrderById($scope.newPurchaseOrderTransaction.PurchaseOrderId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeTransactionModal = function () {
                $('#transactionModal').modal('toggle');
            };
            $scope.showTransactionModal = function (poModel) {
                $scope.selectedPO = poModel;
                $('#transactionModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };  

        }]);