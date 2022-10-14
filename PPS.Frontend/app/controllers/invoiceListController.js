'use strict';

angular.module('AtlasPPS').controller('invoiceListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', "sharedService",'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings) {

            var authData = localStorageService.get('authorizationData');
            var companyId = null;
            var fiscalYear = null;

            $scope.invoiceList = [];

            var getInvoiceList = function () {
                authService.loadingOn();
                var promise = salesService.getInvoiceList();
                promise.then(function (response) {
                    $scope.invoiceList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.invoiceList = [];
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
                getInvoiceList();
            };

            pageLoad();

            $scope.addNewInvoice = function () {
                //$location.path("/Invoice/Create");
                $window.open(reportSettings.reportBaseUri + 'Invoice/Create', '_blank');
            };
            $scope.navigateToInvoiceEdit = function (i) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/Edit/' + i.Id, '_blank');
            };
            $scope.navigateToInvoiceView = function (i) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/View/' + i.Id + '/' + i.DemandOrderId, '_blank');
            };
            $scope.navigateToInvoicePrint = function (i) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/DeliveryPrint/' + i.Id, '_blank');
            };
            $scope.gotoInvoiceDetailsPrint = function (i) {
                $window.open(reportSettings.reportBaseUri + 'Invoice/InvoiceDetail/Print/' + i.Id, '_blank');
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
                .withOption('order', [0, 'desc'])
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