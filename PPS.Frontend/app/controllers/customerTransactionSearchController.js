//'use strict';

angular.module('AtlasPPS').controller('customerTransactionSearchController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'ledgerRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'customerService',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, ledgerRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, customerService) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var fiscalYear = null;
            var companyId = null;
        
            $scope.customerTransactionSearch = function () {
                var vm = {
                    StartDate: moment($scope.date.startDate).format('YYYY-MM-DD'),
                    EndDate: moment($scope.date.endDate).format('YYYY-MM-DD')
                };
                $scope.dateList = vm;
                authService.loadingOn();
                var promise = customerService.customerTransactionSearch(vm);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.customerTransactionList = response;
                }, function (err) {
                    authService.loadingOff();
                    $scope.customerTransactionList = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };

            var pageLoad = function () {
                if (!authService.isValidAuth()) {
                    authService.logout();
                    $location.path('/login');
                } else {
                    var authData = localStorageService.get('authorizationData');
                    fiscalYear = authData.fiscalYear;
                    companyId = authData.companyId;
                }
                if ($rootScope.isAuthenticated('302') === false) {
                    $location.path('/UnAuthorized');
                }
            };
            pageLoad();

            $scope.date = {
                startDate: moment(),
                endDate: moment()
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
                    //'This Year': [moment(new Date(new Date().getFullYear() + "/1/1")), moment(new Date(new Date().getFullYear() + "/12/31"))]
                }
            };

            //$scope.setStartDate = function () {
            //    $scope.date.startDate = moment().subtract(4, "days").toDate();
            //};

            $scope.setRange = function () {
                $scope.date = {
                    startDate: moment("en"),
                    endDate: moment("en")
                };
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