//'use strict';

angular.module('AtlasPPS').controller('customerStatementRptController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'customerStatementRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'ledgerService', 'customerService', '$compile', 'reportService','reportSettings',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, customerStatementRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, ledgerService, customerService, $compile, reportService, reportSettings) {
            $scope.processComplated = true;
            var fiscalYear = null;
            var companyId = null;
            $scope.customerStatement = {};
            $scope.customers = [];
            $scope.selectedCustomer = {};
            $scope.isReportGenerated = false;

            $scope.getCustomerStatement = function () {
                var vm = {
                    companyId: companyId,
                    customerId: $scope.selectedCustomer.selected.Id,
                    startDate: moment($scope.reportDateRange.startDate).format('YYYY-MM-DD'),
                    endDate: moment($scope.reportDateRange.endDate).format('YYYY-MM-DD')
                };
                //$window.open('/#/reports/customerStatementPrint/' + vm.companyId + '/' + vm.customerId + '/' + vm.startDate + '/' + vm.endDate, '_blank');
                $window.open(reportSettings.reportBaseUri + 'reports/customerStatementPrint/' + vm.companyId + '/' + vm.customerId + '/' + vm.startDate + '/' + vm.endDate, '_blank');
            };

            $scope.loadReport = function () {
                $scope.customerStatement = reportService.getData();
            }

            var pageLoad = function () {
                if (!authService.isValidAuth()) {
                    authService.logout();
                    $location.path('/login');
                } else {
                    var authData = localStorageService.get('authorizationData');
                    fiscalYear = authData.fiscalYear;
                    companyId = authData.companyId;
                }
                if ($rootScope.isAuthenticated('307') === false) {
                    $location.path('/UnAuthorized');
                }
                authService.loadingOn();
                var promise = customerService.getCustomerList();
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.customers = response;
                }, function (err) {
                    $scope.customers = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };

            pageLoad();

            $scope.reportDateRange = {
                startDate: moment().format('YYYY-MM-DD'),
                endDate: moment().format('YYYY-MM-DD')
            };
            $scope.singleDate = moment();

            $scope.opts = {
                locale: {
                    applyClass: 'btn-green',
                    applyLabel: "Apply",
                    fromLabel: "From",
                    format: "DD-MM-YYYY",
                    toLabel: "To",
                    cancelLabel: 'Cancel',
                    customRangeLabel: 'Custom range'
                },
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'As of Today': [moment(new Date(new Date().getFullYear() + "/1/1")), moment(new Date())]
                }
            };

            $scope.setRange = function () {
                $scope.reportDateRange = {
                    startDate: moment().format('YYYY-MM-DD'),
                    endDate: moment().format('YYYY-MM-DD')
                };
            };

            //Watch for date changes
            $scope.$watch('reportDateRange', function (newDate) {
                //console.log('New date set: ', newDate);
            }, false);

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

            // TODO: 
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);
        }]);