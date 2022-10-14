'use strict';
angular.module('AtlasPPS').controller('employeeAndEmployeeHierArchyLeaveController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'employeeLeaveService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings', '$q', 
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, employeeLeaveService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSetting, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            var status = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            
            $scope.employeeLeaveList = [];
            $scope.employeeLeaveDetails = [];
            $scope.employeeHierArchyLeaveList = [];
          

            $scope.getLeaveList = function (status) {
                var promise = employeeLeaveService.getEmployeeAndEmployeeHierArchyLeaveList(status);
                promise.then(function (response) {
                    $scope.employeeHierArchyLeaveList = [];
                    $scope.employeeHierArchyLeaveList = response.EmployeeHierArchyLeaveList;
                }, function (err) {
                    $scope.employeeHierArchyLeaveList = [];
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

                authService.loadingOn();
                var getEmployeeLeaveDetails = employeeLeaveService.getEmployeeAndEmployeeHierArchyLeaveList(status);
                getEmployeeLeaveDetails.then(function (response) {
                    $scope.employeeLeaveDetails = response.EmployeeDetails;
                    $scope.employeeLeaveList = response.EmployeeLeaveList;
                    $scope.employeeHierArchyLeaveList = response.EmployeeHierArchyLeaveList;
                    authService.loadingOff();
                }, function (err) {
                    $scope.employeeLeaveDetails = [];
                    $scope.employeeLeaveList = [];
                    $scope.employeeHierArchyLeaveList = [];
                    authService.loadingOff();
                });


           

                $q.all([
                    getEmployeeLeaveDetails
                ]).then(function () {
                        authService.loadingOff();
                    });
            };
            pageLoad();

            $scope.addNewEmployeeLeave = function (text) {
                $location.path("/employeeLeave/EmployeeLeaveAdd/" + text);
            };
            $scope.navigateToEmployeeLeaveEdit = function (d, text) {

                $location.path("/employeeLeave/EmployeeLeaveEdit/" + d.Id + "/" + text);
            };
            $scope.navigateToEmployeeLeaveView = function (d, text) {
                
                $location.path("/employeeLeave/EmployeeLeaveView/" + d.Id + "/" + text);
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


            $scope.dtOptionsAll = DTOptionsBuilder.newOptions()
                .withOption('order', [0, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([]);

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