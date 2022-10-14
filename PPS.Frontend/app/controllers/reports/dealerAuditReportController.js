'use strict';

angular.module('AtlasPPS').controller('dealerAuditReportController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'individualLedgerRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'ledgerService', '$compile', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, individualLedgerRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, ledgerService, $compile, $state) {
            $scope.company = null;
            $scope.demandOrder = null;
            $scope.TotalQuantity = 0;
            $scope.finalAmount = 0;
            $scope.subTotal = 0;
            $scope.companyId = 0;
            $scope.delarAuditReportList = [];
            $scope.dateRange = {};


            $scope.GetDealerAuditReport = function () {
                var vm = {
                    StartDate: moment($scope.reportDateRange.startDate).format('YYYY-MM-DD'),
                    EndDate: moment($scope.reportDateRange.endDate).format('YYYY-MM-DD')
                };
                $scope.dateRange = vm;
                authService.loadingOn();
                var promise = individualLedgerRptService.GetDealerAuditReport(vm);
                promise.then(function (response) {
                    $scope.delarAuditReportList = response;
                    authService.loadingOff();
                   
                }, function (err) {
                    $scope.delarAuditReportList = [];
                    authService.loadingOff();
                    
                });
            };

            

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

            var pageLoad = function () {
                if (!authService.isValidAuth()) {
                    authService.logout();
                    $location.path('/login');
                } else {
                    var authData = localStorageService.get('authorizationData');
                    $scope.companyId = authData.companyId;
                    $rootScope.userFullName = authData.fullName;
                    $rootScope.companyName = authData.companyName;
                    $rootScope.companyAddress = authData.companyAddress;
                    $rootScope.companyEmail = authData.companyEmail;
                    $rootScope.companyContact = authData.companyContact;
                    $rootScope.companyLogoPath = authData.companyLogoPath;
                }
                authService.loadingOff();
            };
            pageLoad();
            
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