'use strict';

angular.module('AtlasPPS').controller('demandOrderEarlyPaymentListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'sharedService', 'reportSettings','$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.doEarlyPaymentAllList = [];
            $scope.doEarlyPaymentPendingList = [];
            $scope.doEarlyPaymentPaidList = [];

            $scope.selectedDO = null;

            var getDemandOrderEarlyPaymentList = function () {
                authService.loadingOn();
                var promise = salesService.getDemandOrderEarlyPaymentList();
                promise.then(function (response) {
                    $scope.doEarlyPaymentAllList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doEarlyPaymentAllList = [];
                    authService.loadingOff();
                });
            };

            var demandOrderEarlyPaymentPendingListApiCalled = false;
            $scope.getDemandOrderEarlyPaymentPendingList = function () {
                if (demandOrderEarlyPaymentPendingListApiCalled) {
                    return;
                }
                demandOrderEarlyPaymentPendingListApiCalled = true;
                authService.loadingOn();
                var promise = salesService.getDemandOrderEarlyPaymentPendingList();
                promise.then(function (response) {
                    $scope.doEarlyPaymentPendingList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doEarlyPaymentPendingList = [];
                    authService.loadingOff();
                });
            };

            var demandOrderEarlyPaymentPaidListApiCalled = false;
            $scope.getDemandOrderEarlyPaymentPaidList = function () {
                if (demandOrderEarlyPaymentPaidListApiCalled) {
                    return;
                }
                demandOrderEarlyPaymentPaidListApiCalled = true;
                authService.loadingOn();
                var promise = salesService.getDemandOrderEarlyPaymentPaidList();
                promise.then(function (response) {
                    $scope.doEarlyPaymentPaidList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doEarlyPaymentPaidList = [];
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
                getDemandOrderEarlyPaymentList();
            };
            pageLoad();
            $scope.addNewDemandOrder = function () {
                $location.path("/Sales/DemandOrder/Create");
            };

            $scope.navigateToDoView = function (d) {
                $location.path("/Sales/DemandOrder/View/" + d.Id);
            };
            $scope.navigateToDoPrint = function (d) {
                //$window.open('/#/reports/demandOrderPrint/' + d.Id, '_blank');
                $window.open(reportSettings.reportBaseUri + 'reports/demandOrderPrint/' + d.Id, '_blank');
            };

            $scope.payDOEarlyPaymentDiscountToCustomerClick = function () {
                var doId = $scope.selectedDO.Id;
                authService.loadingOn();
                var promise = salesService.payDOEarlyPaymentDiscountToCustomer(doId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closePayDOEarlyPaymentDiscountToCustomerModal();
                    refeshTab();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };

            $scope.closePayDOEarlyPaymentDiscountToCustomerModal = function () {
                $('#payDOEarlyPaymentDiscountToCustomerModal').modal('toggle');
            };
            $scope.showPayDOEarlyPaymentDiscountModal = function (doModel) {
                $scope.selectedDO = doModel;
                $('#payDOEarlyPaymentDiscountToCustomerModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            var refeshTab = function () {
                var promiseAll = salesService.getDemandOrderEarlyPaymentList();
                promiseAll.then(function (response) {
                    $scope.doEarlyPaymentAllList = response;
                }, function (err) {
                    $scope.doEarlyPaymentAllList = [];
                });

                var promisePending = salesService.getDemandOrderEarlyPaymentPendingList();
                promisePending.then(function (response) {
                    $scope.doEarlyPaymentPendingList = response;
                }, function (err) {
                    $scope.doEarlyPaymentPendingList = [];
                });

                var promisePaid = salesService.getDemandOrderEarlyPaymentPaidList();
                promisePaid.then(function (response) {
                    $scope.doEarlyPaymentPaidList = response;
                }, function (err) {
                    $scope.doEarlyPaymentPaidList = [];
                });

                authService.loadingOn();

                $q.all([
                    promiseAll,
                    promisePending,
                    promisePaid]).then(function () {
                        authService.loadingOff();
                    });
            }

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