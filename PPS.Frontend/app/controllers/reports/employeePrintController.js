'use strict';

angular.module('AtlasPPS').controller('employeePrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'employeeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$state', '$q', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, employeeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.employeeList = [];
            var listStatus = true;
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

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
                if ($state.params && $state.params.Id) {
                    listStatus = _.parseInt($state.params.Id);
                }
                var getEmployeeListPrint = employeeService.employeeListPrint(listStatus);
                getEmployeeListPrint.then(function (response) {
                    $scope.employeeList = response;
                }, function (err) {
                    $scope.employeeList = [];
                });

                authService.loadingOn();
                $q.all([
                    getEmployeeListPrint]).then(function () {
                        authService.loadingOff();
                    });


            };
            pageLoad();
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