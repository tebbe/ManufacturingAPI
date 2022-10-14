//'use strict';

angular.module('AtlasPPS').controller('salesReportPrintController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'salesService', '$compile', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, salesService, $compile, $state) {
            $scope.processComplated = true;
            $scope.salesReportList = [];
            $scope.company = null;
            $scope.isReportGenerated = false;

            $scope.startDate = null;
            $scope.endDate = null;

            $scope.ReportHeaderSetting = {
                CompanyLogoIncludeInReport: PpsConstant.ReportHeaderSetting.CompanyLogoIncludeInReport
            };

            var getSalesReportList = function () {
                var startDate = null;
                var endDate = null;
                var salesDivisionId = null;
                var salesAreaId = null;
                var employeeId = null;
                var customerId = null;

                if ($state.params && $state.params.startDate) {
                    startDate = moment($state.params.startDate).format('YYYY-MM-DD');
                    $scope.startDate = startDate;
                }
                if ($state.params && $state.params.endDate) {
                    endDate = moment($state.params.endDate).format('YYYY-MM-DD');
                    $scope.endDate = endDate;
                }
                if ($state.params && $state.params.salesDivisionId) {
                    salesDivisionId = _.parseInt($state.params.salesDivisionId);
                }
                if ($state.params && $state.params.salesAreaId) {
                    salesAreaId = _.parseInt($state.params.salesAreaId);
                }
                if ($state.params && $state.params.employeeId) {
                    employeeId = _.parseInt($state.params.employeeId);
                }
                if ($state.params && $state.params.customerId) {
                    customerId = _.parseInt($state.params.customerId);
                }

                authService.loadingOn();
                var promise = salesService.getSalesReportList(startDate, endDate, salesDivisionId, salesAreaId, employeeId, customerId);
                promise.then(function (response) {
                    $scope.salesReportList = response.salesReportList;
                    $scope.company = response.company;
                    CalculateInvoiceAmount();
                    $('.footer').hide();
                    $scope.isReportGenerated = true;
                    authService.loadingOff();
                }, function (err) {
                    $scope.salesReportList = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            getSalesReportList();

            $scope.TotalInvoiceAmount = 0;
            $scope.TotalPaidAmount = 0;
            $scope.TotalBalanceAmount = 0;

            var CalculateInvoiceAmount = function () {
                var totalInvoiceAmount = 0;
                var totalPaidAmount = 0;
                var totalBalanceAmount = 0;

                if ($scope.salesReportList) {
                    for (var item in $scope.salesReportList) {
                        if ($scope.salesReportList.hasOwnProperty(item)) {
                            totalInvoiceAmount += $scope.salesReportList[item].DOAmount;
                            totalPaidAmount += $scope.salesReportList[item].DOPaid;
                        }
                    }
                }
                totalBalanceAmount = totalInvoiceAmount - totalPaidAmount;
                $scope.TotalInvoiceAmount = totalInvoiceAmount;
                $scope.TotalPaidAmount = totalPaidAmount;
                $scope.TotalBalanceAmount = totalBalanceAmount;
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