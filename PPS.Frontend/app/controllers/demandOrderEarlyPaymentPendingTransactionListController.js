'use strict';

angular.module('AtlasPPS').controller('demandOrderEarlyPaymentPendingTransactionListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'sharedService','reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.doEarlyPaymentPendingTransactionList = [];
            $scope.doEarlyPaymentApprovedTransactionList = [];
            
            $scope.selectedDO = null;

            var getDemandOrderEarlyPaymentPendingTransactionList = function () {
                authService.loadingOn();
                var promise = salesService.getDemandOrderEarlyPaymentPendingTransactionList();
                promise.then(function (response) {
                    $scope.doEarlyPaymentPendingTransactionList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doEarlyPaymentPendingTransactionList = [];
                    authService.loadingOff();
                });
            };

            var demandOrderEarlyPaymentApprovedTransactionListApiCalled = false;
            $scope.getDemandOrderEarlyPaymentApprovedTransactionList = function () {
                if (demandOrderEarlyPaymentApprovedTransactionListApiCalled) {
                    return;
                }
                demandOrderEarlyPaymentApprovedTransactionListApiCalled = true;
                authService.loadingOn();
                var promise = salesService.getDemandOrderEarlyPaymentApprovedTransactionList();
                promise.then(function (response) {
                    $scope.doEarlyPaymentApprovedTransactionList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doEarlyPaymentApprovedTransactionList = [];
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
                getDemandOrderEarlyPaymentPendingTransactionList();
            };
            pageLoad();
            
            $scope.navigateToDoView = function (d) {
                $location.path("/Sales/DemandOrder/View/" + d.Id);
            };
            $scope.navigateToDoPrint = function (d) {
                //$window.open('/#/reports/demandOrderPrint/' + d.Id, '_blank');
                $window.open(reportSettings.reportBaseUri + 'reports/demandOrderPrint/' + d.Id, '_blank');
            };

            $scope.verifyDOEarlyPaymentDiscountToCustomerClick = function () {
                var doId = $scope.selectedDO.Id;

                var requestVm = {
                    DemandOrderId: doId,
                    FiscalYear: fiscalYear
                };
                authService.loadingOn();
                var promise = salesService.verifyDOEarlyPaymentDiscountToCustomer(requestVm);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeVerifyDOEarlyPaymentDiscountToCustomerModal();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeVerifyDOEarlyPaymentDiscountToCustomerModal = function () {
                $('#verifyDOEarlyPaymentDiscountToCustomerModal').modal('toggle');
            };
            $scope.showVerifyDOEarlyPaymentDiscountModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#verifyDOEarlyPaymentDiscountToCustomerModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            $scope.clickDatePicker = function () {
                $('.md-datepicker-input-container > input').attr('disabled', true);
            };

            $scope.dtOptionsAll = DTOptionsBuilder.newOptions()
                .withOption('order', [0, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withOption('order', [2, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $('.inner-table .html5buttons').css({ 'display': 'none' });
            $('.inner-table .dataTables_length').css({ 'display': 'none' });
            $('.inner-table .DataTables_Table_0_filter').css({ 'display': 'none' });

            // TODO: 
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });

            $scope.getPaymentStatusColorClass = function (demandOrder) {
                return sharedService.getDemandOrderPaymentStatusClass(demandOrder.DOPaymentStatusId);
            };
        }]);