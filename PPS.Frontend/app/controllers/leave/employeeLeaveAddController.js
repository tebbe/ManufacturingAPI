'use strict';

angular.module('AtlasPPS').controller('employeeLeaveAddController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', '$q', 'authService', 'employeeLeaveService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$state', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, $q, authService, employeeLeaveService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelHeading = "Add New Leave";
            $scope.modelActionText = "Save";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            $scope.Address = null;
            $scope.EmployeeId = null;
            $scope.LeaveCategoryId = null;
            $scope.LeaveDays = null;
            $scope.FromDate = null;
            $scope.ToDate = null;
            $scope.ReasonOfLeave = null;
            $scope.Address = null;
            $scope.MobileNo = null;
            $scope.leaveCatregory = [];
            $scope.employeeHierArchy = [];
            $scope.selectedLeaveCategory = {
                selected: null
            }
            $scope.selectedEmployeeHierArchy = {
                selected: null
            }
            $scope.employeeLeaveModel = {
                LeaveCategoryId: null,
                EmployeeId: null,
                LeaveDays : null,
                FromDate : null,
                ToDate : null,
                ReasonOfLeave : null,
                Address : null,
                MobileNo : null
            };

            $scope.onChangeLeaveCategory = function (selectedLeaveCategory){
                if (selectedLeaveCategory && selectedLeaveCategory.Id) {
                    var result = $scope.leaveCatregory.filter(function (v) {
                        return v.Id === selectedLeaveCategory.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedLeaveCategory.selected = result[0];
                    }
                    else {
                        $scope.selectedLeaveCategory.selected = null;
                    }
                } else {
                    $scope.selectedLeaveCategory.selected = null;
                }
            }
            $scope.onChangeEmployeeHierArchy = function (selectedEmployeeHierArchy) {
                if (selectedEmployeeHierArchy && selectedEmployeeHierArchy.Id) {
                    var result = $scope.employeeHierArchy.filter(function (v) {
                        return v.Id === selectedEmployeeHierArchy.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedEmployeeHierArchy.selected = result[0];
                    }
                    else {
                        $scope.selectedEmployeeHierArchy.selected = null;
                    }
                } else {
                    $scope.selectedEmployeeHierArchy.selected = null;
                }
            }
            var validate = function () {
                if ($scope.selectedLeaveCategory.selected != null && $scope.selectedEmployeeHierArchy.selected != null) {
                    return true;
                }
                return false;
            };
            var validateDateRangeWithLeaveDays = function () {
                var fromDay = moment($scope.FromDate).format("MM-DD-YYYY").split("-")[1];
                fromDay = parseInt(fromDay);
                var toDay = moment($scope.ToDate).format("MM-DD-YYYY").split("-")[1];
                toDay = parseInt(toDay);
                var leaveDay = parseInt($scope.LeaveDays);

                var totalDateDifference = toDay - fromDay;
                if ((totalDateDifference + 1) === leaveDay) {
                    return true;
                }
                return false;
            };
            $scope.saveNewEmployeeLeave = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                if (!validateDateRangeWithLeaveDays()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput + " Correct difference between Leave Dates & Days ");
                    return;
                }
                $scope.employeeLeaveModel.LeaveCategoryId = $scope.selectedLeaveCategory.selected === null ? "" : $scope.selectedLeaveCategory.selected.Id;
                $scope.employeeLeaveModel.EmployeeId = $scope.selectedEmployeeHierArchy.selected === null ? "" : $scope.selectedEmployeeHierArchy.selected.Id;
                $scope.employeeLeaveModel.Address = $scope.Address;
                $scope.employeeLeaveModel.LeaveDays = $scope.LeaveDays;
                $scope.employeeLeaveModel.FromDate = moment($scope.FromDate).format("MM-DD-YYYY");
                $scope.employeeLeaveModel.ToDate = moment($scope.ToDate).format("MM-DD-YYYY");
                $scope.employeeLeaveModel.ReasonOfLeave = $scope.ReasonOfLeave;
                $scope.employeeLeaveModel.MobileNo = $scope.MobileNo;

                authService.loadingOn();
                var promise = employeeLeaveService.saveEmployeeLeave($scope.employeeLeaveModel);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            var getLeaveCategoryWithHierArchy = function () {
                authService.loadingOn();
                var promise = employeeLeaveService.GetEmployeeLeaveCategoryWithHierArchy();
                promise.then(function (response) {
                    $scope.leaveCatregory = response.LeaveCategory;
                    $scope.employeeHierArchy = response.EmployeeHierArchy;
                    authService.loadingOff();
                }, function (err) {
                    $scope.leaveCatregory = [];
                    $scope.employeeHierArchy = [];
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
               
                authService.loadingOn();
                if ($state.params && $state.params.Id) {
                  
                }

                $q.all([]).then(function () {
                    getLeaveCategoryWithHierArchy();
                        authService.loadingOff();
                    });
            };
            pageLoad();

 

            $scope.gotoEmployeeLeaveList = function () {
               $location.path("employeeLeave/employeeAndEmployeeHierArchyleaveList");
            }
         

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
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