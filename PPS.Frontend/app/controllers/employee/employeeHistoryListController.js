'use strict';
angular.module('AtlasPPS').controller('employeeHistoryListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', '$state', 'notificationService', 'authService', 'employeeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$q', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, $state, notificationService, authService, employeeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $q, $window, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.employeeList = [];
            $scope.historyList = [];
           
            var employeeId;
            var employeeHistoryList = function () {
                authService.loadingOn();
                var promise = employeeService.getEmployeeHisoryList(employeeId);
                promise.then(function (response) {
                    $scope.employeeList = response.EmployeeDetails;
                    $scope.historyList = response.empHistory;
                    authService.loadingOff();
                }, function (err) {
                    $scope.employeeList = [];
                    $scope.historyList = [];
                    authService.loadingOff();
                });
            };
            if ($state.params && $state.params.Id) {
                employeeId = _.parseInt($state.params.Id);
            }
            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
                employeeHistoryList();
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
        }]);