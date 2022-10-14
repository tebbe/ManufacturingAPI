'use strict';

angular.module('AtlasPPS').controller('purchaseOrderTransactionApprovalController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'purchaseOrderService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', "sharedService","bankService",
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, purchaseOrderService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.suppliers = {};
            $scope.pendingPurchaseOrderTransaction = {};
            $scope.approvedPurchaseOrderTransaction = {};


            $scope.getUnapprovedPOTransaction = function () {
                authService.loadingOn();
                var promise = purchaseOrderService.getUnapprovedPurchaseOrderTransaction();
                promise.then(function (response) {
                    $scope.pendingPurchaseOrderTransaction = response; 
                    authService.loadingOff();
                }, function (err) {
                    $scope.pendingPurchaseOrderTransaction = {};
                    authService.loadingOff();
                });
            };

            

            $scope.getApprovedPOTransaction = function () {
                authService.loadingOn();
                var promise = purchaseOrderService.getApprovedPurchaseOrderTransaction();
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.approvedPurchaseOrderTransaction = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.approvedPurchaseOrderTransaction = {};
                    authService.loadingOff();
                });
            };

            $scope.navigateToPoView = function (p) {
                $location.path("/Purchase/PurchaseOrder/View/" + p.PurchaseOrderId);
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
                $scope.getUnapprovedPOTransaction();
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
                        
            $scope.approvePurchaseOrderTranClick = function () {
                authService.loadingOn();
                var promise = purchaseOrderService.approvePurchaseOrderTransaction($scope.selectedTran, fiscalYear);
                promise.then(function (response) {
                    if (response.Success === true) {
                        notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                        $scope.closePurchaseOrderTranModal();
                        $scope.getUnapprovedPOTransaction();
                        authService.loadingOff();
                    } else {
                        notificationService.showErrorNotificatoin(response.Message);
                        authService.loadingOff();
                    }
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closePurchaseOrderTranModal = function () {
                $scope.selectedTran = {};
                $('#approvePurchaseOrderTranModal').modal('toggle');
            };
            $scope.showTransactionModal = function (tranModel) {
                $scope.selectedTran = tranModel;
                $('#approvePurchaseOrderTranModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };            
        }]);