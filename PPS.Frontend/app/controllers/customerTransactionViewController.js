'use strict';

angular.module('AtlasPPS').controller('customerTransactionViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state', '$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            var transactionId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.tranMode = 1;
            $scope.customerTransaction = [];
            $scope.customerTransactionDetail = [];

            var getTransactionById = function () {
                authService.loadingOn();
                var promise = customerService.getCustomerTransactionByTransactionId(transactionId);
                promise.then(function (response) {
                    $scope.customerTransaction = response;
                    $scope.customerTransactionDetail = $scope.customerTransaction.CustomerTransactionDetail;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customerTransaction = [];
                    $scope.customerTransactionDetail = [];
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
                if ($state.params && $state.params.Id) {
                    transactionId = _.parseInt($state.params.Id);
                }
                getTransactionById();
            };
         
            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            
            $scope.gotoTransactionList = function () {
                $location.path("/Sales/CustomerTransactionList");
            };
            $scope.navigateToCustomerTransactionEdit = function (c) {
                $location.path("/Sales/CustomerTransaction/Edit/" + c.Id);
            };

            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            $scope.clickDatePicker = function () {
                $('.md-datepicker-input-container > input').attr('disabled', true);
            };

            $scope.dtOptions = DTOptionsBuilder.newOptions()
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
        }]);