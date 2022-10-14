//'use strict';

angular.module('AtlasPPS').controller('customerViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', "sharedService", "bankService",
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, customerService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.customerVm = {};
            $scope.cashBanks = [];

            var getCustomerById = function (customerId) {
                authService.loadingOn();
                $scope.customerVm = {};
                var promise = customerService.getCustomerById(customerId);
                promise.then(function (response) {
                    $scope.customerVm = response;
                    getBanks($scope.customerVm.Id);
                    authService.loadingOff();
                }, function (err) {
                    $scope.customerVm = {};
                    authService.loadingOff();
                });
            };
            var getBanks = function (customerId) {
                $scope.cashBanks = [];
                var promise = bankService.getBankCashAccountHeadList(customerId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.cashBanks = response;
                }, function (err) {
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

                var customerId;
                if ($state.params && $state.params.customerId) {
                    customerId = _.parseInt($state.params.customerId);
                }
                if (!customerId) {
                    $location.path('/customerList');
                }
                getCustomerById(customerId);
                $scope.newCustomerTransaction = {
                    CustomerId: customerId,
                    TransactionDate: new Date()
                };
            };

            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.gotoCustomerEdit = function (id) {
                $location.path("/Sales/Customer/Edit/" + id);
                $window.location.reload();
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
                .withOption('order', [])
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $scope.saveTransactionClick = function () {
                var custId = $scope.customerVm.Id;
                $scope.newCustomerTransaction.CashBankAccountHeadId = $scope.selectedAccount.Id;
                authService.loadingOn();
                var promise = customerService.saveCustomerTransaction($scope.newCustomerTransaction);
                promise.then(function (response) {
                    $scope.closeTransactionModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getCustomerById(custId);
                    authService.loadingOff();
                }, function (err) {
                    $scope.closeTransactionModal();
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeTransactionModal = function () {
                $('#transactionModal').modal('toggle');
            };
            $scope.showTransactionModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#transactionModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.deactivateCustomerModal = function () {
                var custId = $scope.customerVm.Id;
                authService.loadingOn();
                var promise = customerService.deactivateCustomer(custId);
                promise.then(function (response) {
                    $scope.closeDeactivateCustomerModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getCustomerById(custId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeDeactivateCustomerModal = function () {
                $('#deactivateCustomerModal').modal('toggle');
            };
            $scope.showDeactiveCustomerModal = function () {
                $('#deactivateCustomerModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.activateCustomerModal = function () {
                var custId = $scope.customerVm.Id;
                authService.loadingOn();
                var promise = customerService.activateCustomer(custId);
                promise.then(function (response) {
                    $scope.closeActivateCustomerModal();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    getCustomerById(custId);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeActivateCustomerModal = function () {
                $('#activateCustomerModal').modal('toggle');
            };
            $scope.showActivateCustomerModal = function () {
                $('#activateCustomerModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
        }]);