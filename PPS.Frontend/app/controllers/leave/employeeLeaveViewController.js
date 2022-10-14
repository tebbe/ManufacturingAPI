'use strict';
angular.module('AtlasPPS').controller('employeeLeaveViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'employeeLeaveService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window','$state', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, employeeLeaveService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            var leaveId = null;
            $scope.employeeLeaveDetails = [];
            $scope.employeeLeaveList = [];
            $scope.text = null;
            $scope.ApprovedByDepartmentHead = null;
            $scope.ApprovedByMD = null;
            $scope.IsApproved = null;
            $scope.Id = null;

            $scope.ApprovedModel = {
               ApprovedByDepartmentHead : null,
               ApprovedByMD: null,
               IsApproved: null,
               Id: null
            }

            var getallLeave = function () {
                authService.loadingOn();
                var promise = employeeLeaveService.getEmployeeLeaveById(leaveId);
                promise.then(function (response) {
                    $scope.employeeLeaveDetails = response.LeaveById;
                    authService.loadingOff();
                }, function (err) {
                    $scope.employeeLeaveDetails = [];
                    authService.loadingOff();
                });
            };

            $scope.navigateToApprovedBy = function (data, text) {
                if (text === "head") { $scope.ApprovedModel.ApprovedByDepartmentHead = true; } else if (text === "MD") { $scope.ApprovedModel.ApprovedByMD = true; }
                $scope.ApprovedModel.IsApproved = 1;
                $scope.ApprovedModel.Id = data.Id;
                var promise = employeeLeaveService.employeeLeaveApproveOrReject($scope.ApprovedModel);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $window.location.reload();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };
            $scope.navigateToRejectedBy = function (data, text) {
                
                if (text === "head") { $scope.ApprovedModel.ApprovedByDepartmentHead = false; } else if (text === "MD") { $scope.ApprovedModel.ApprovedByMD = false; }
                $scope.ApprovedModel.IsApproved = -1;
                $scope.ApprovedModel.Id = data.Id;
                var promise = employeeLeaveService.employeeLeaveApproveOrReject($scope.ApprovedModel);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $window.location.reload();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
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

                if ($state.params && $state.params.Id) {
                    leaveId = _.parseInt($state.params.Id);
                    $scope.text = $state.params.text;
                }
               
                getallLeave();
            };
            pageLoad();

            $scope.gotoEmployeeLeaveList = function () {
                if ($scope.text === "personal") {
                    $location.path("employeeLeave/employeeAndEmployeeHierArchyleaveList");
                } else {
                    $location.path('employeeLeave/employeeLeaveList');
                }
            }

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