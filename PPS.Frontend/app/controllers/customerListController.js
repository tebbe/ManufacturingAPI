//'use strict';

angular.module('AtlasPPS').controller('customerListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
    function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
        $scope.processComplated = true;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;
        
        $scope.customerList = [];
        $scope.pendingDeactivatedCustomerList = [];
        var pendingDeactivatedApiCalled = false;
        $scope.getPendingDeactivatedCustomerList = function () {
            if (pendingDeactivatedApiCalled) {
                return;
            }
            pendingDeactivatedApiCalled = true;
            authService.loadingOn();
            var promise = customerService.getPendingDeactivatedCustomerList();
            promise.then(function (response) {
                $scope.pendingDeactivatedCustomerList = response;
                authService.loadingOff();
            }, function (err) {
                $scope.pendingDeactivatedCustomerList = [];
                authService.loadingOff();
            });
        };
        var getCustomerList = function () { 
            authService.loadingOn();
            var promise = customerService.getCustomerList();
            promise.then(function (response) {
                $scope.customerList = response;
                authService.loadingOff();
            }, function (err) {
                $scope.customerList = [];
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
            getCustomerList();
        };
        pageLoad();
        $scope.addNewCustomer = function () {
            $location.path("/Sales/Customer/Create");
        };
        $scope.navigateToCustomerEdit = function (c) {
            $location.path("/Sales/Customer/Edit/" + c.Id);
        };
        $scope.navigateToCustomerView = function (c) {
            $location.path("/Sales/Customer/View/" + c.Id);
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

        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDOM('<"html5buttons"B>lTfgitp')
            .withPaginationType('full_numbers').withDisplayLength(25)
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
    }]);