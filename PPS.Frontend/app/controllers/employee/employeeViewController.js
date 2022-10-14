'use strict';

angular.module('AtlasPPS').controller('employeeViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService',
        'authService', 'employeeService', 'ngAuthSettings', 'DTOptionsBuilder',
        'PpsConstant', '$state', '$q', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location,
            notificationService, authService, employeeService,
            ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q,
            $window, reportSettings) {

            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
      

            $scope.modelHeading = "View Employee Details";
            $scope.modelActionText = "View";
            $scope.modelDetailText = "Details";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            var employeeId;
            $scope.employeeList = [];
            $scope.employeeLocationList = [];
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
                    employeeId = _.parseInt($state.params.Id);
                }

                var getEmployeeById = employeeService.getEmployeeById(employeeId);
                getEmployeeById.then(function (response) {
                    $scope.employeeList = response.Employee;
                    $scope.employeeLocationList = response.sLocation;
                }, function (err) {
                    $scope.employeeList = [];
                    $scope.employeeLocationList = [];
                });

                authService.loadingOn();
                $q.all([
                    getEmployeeById]).then(function () {
                        authService.loadingOff();
                    });
            };

            pageLoad();
            $scope.gotoEmployeeEdit = function () {
                $location.path("/Employee/Employee/Edit/" + employeeId);
                $window.location.reload();
            };
            $scope.gotoEmployeeList = function () {
                $location.path("/Employee/EmployeeList");
                $window.location.reload();
            }
            $scope.navigateToEmployeeSinglePrint = function () {
                $window.open(reportSettings.reportBaseUri + 'Employee/EmployeeSinglePrint/' + employeeId, '_blank');
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