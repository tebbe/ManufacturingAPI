'use strict';
angular.module('AtlasPPS').controller('employeeListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'employeeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, employeeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            var listStatus = true;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.employeeList = [];
            $scope.employeeInactiveList = [];
            $scope.employeeSalesLocationList = [];
            var isInactiveListLoaded = false;
            $scope.getInactiveEmployee = function () {
                if (isInactiveListLoaded === false) {
                    isInactiveListLoaded = true;
                    authService.loadingOn();
                    var promise = employeeService.getInactiveEmployeeList();
                    promise.then(function (response) {
                        $scope.employeeInactiveList = response;
                        authService.loadingOff();
                    }, function (err) {
                        $scope.employeeInactiveList = [];
                        authService.loadingOff();
                    });
                }
            };

            var getEmployeeList = function () {
                authService.loadingOn();
                var promise = employeeService.getActiveEmployeeList();
                promise.then(function (response) {
                    $scope.employeeList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.employeeList = [];
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
                getEmployeeList();
               
            };
            pageLoad();
           
            $scope.addNewEmployeeDocument = function () {
                $location.path("/Employee/Employee/Create");
            };
            $scope.navigateToEmployeeEdit = function (d) {
                $location.path("/Employee/Employee/Edit/" + d.Id);
            };
            $scope.navigateToEmployeeView = function (d) {
                $location.path("/Employee/Employee/View/" + d.Id);
            };
            $scope.navigateToEmployeeSinglePrint = function (d) {
                $window.open(reportSettings.reportBaseUri + 'Employee/EmployeeSinglePrint/' + d.Id, '_blank');
            };
            $scope.navigateToEmployeeListPrint = function () {
                $window.open(reportSettings.reportBaseUri + 'Employee/EmployeePrint/' + listStatus, '_blank');
            };
            $scope.viewEmployeeHistory = function (d) {
                $window.open(reportSettings.reportBaseUri + 'Employee/EmployeeHistory/' + d.Id, '_blank');
            }

            //PopUp btn click for sales location history start
            $scope.viewEmployeeSalesLocation= function (data) {
                var promise = employeeService.getEmployeeSalesLocationByEmployeeId(data.Id);
                promise.then(function (response) {
                    $scope.employeeSalesLocationList = response.SalesLocationByEmployeeId;
                }, function (err) {
                    $scope.employeeSalesLocationList = [];
                });
                $('#showSalesLocation').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.closeSalesLocation= function () {
                $('#showSalesLocation').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            };
            //PopUp btn click for sales location history end
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

            // TODO: 
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });


        }]);