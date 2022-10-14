'use strict';

angular.module('AtlasPPS').controller('rejectedAccountsTransactionListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'transactionService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'approvalService',
    function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, transactionService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, approvalService) {
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;

        var getTransactionAccountsRejectedList = function () {
            authService.loadingOn();
            var promise = transactionService.getTransactionAccountsRejectedList(fiscalYear, companyId);
            promise.then(function (response) {
                $scope.transactions = response;
                authService.loadingOff();
            }, function (err) {
                $scope.transactions = [];
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
            getTransactionAccountsRejectedList();
        };
        pageLoad();

        $scope.getVoucherDetail = function (tranNo) {
            $window.open('/#/reports/voucherPrint/' + tranNo, '_blank');
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
            .withOption('order', [])
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
    }]);