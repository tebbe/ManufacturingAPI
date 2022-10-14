//'use strict';

angular.module('AtlasPPS').controller('customerReportPrintController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'customerStatementRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'customerService', '$compile', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, customerStatementRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, customerService, $compile, $state) {
            $scope.processComplated = true;
            $scope.customerReportList = [];
            $scope.isReportGenerated = false;

            $scope.startDate = null;
            $scope.endDate = null;

            var getCustomerReportList = function () {
                var startDate = null;
                var endDate = null;
                var salesDivisionId = null;
                var salesAreaId = null;
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
                if ($state.params && $state.params.endDate) {
                    customerId = _.parseInt($state.params.customerId);
                }

                authService.loadingOn();
                var promise = customerService.getCustomerReportList(startDate, endDate, salesDivisionId, salesAreaId, customerId);
                promise.then(function (response) {
                    $scope.customerReportList = response;
                    
                    $('.footer').hide();
                    $scope.isReportGenerated = true;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customerReportList = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            getCustomerReportList();


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