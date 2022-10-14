'use strict';

angular.module('AtlasPPS').controller('employeeLeaveEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', '$q', '$state', 'authService', 'employeeLeaveService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, $q,$state, authService, employeeLeaveService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.modelHeading = "Edit New Leave";
            $scope.modelActionText = "Update";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            var leaveId = null;
            $scope.text = null;
            $scope.Address = null;
            $scope.LeaveCategoryId = null;
            $scope.EmployeeId = null;
            $scope.LeaveDays = null;
            $scope.FromDate = null;
            $scope.ToDate = null;
            $scope.ReasonOfLeave = null;
            $scope.Address = null;
            $scope.MobileNo = null;
            $scope.leaveCatregory = [];
            $scope.leaveDetails = [];
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
                LeaveDays: null,
                FromDate : null,
                ToDate : null,
                ReasonOfLeave : null,
                Address : null,
                MobileNo : null
            };
            var fillDataFunction = function () {
                $scope.leaveDetails.FromDate = new Date($scope.leaveDetails.FromDate);
                $scope.leaveDetails.ToDate= new Date($scope.leaveDetails.ToDate);

                $scope.selectedLeaveCategory.selected = _.filter($scope.leaveCatregory,
                    function (item) {
                        return item.Id === $scope.leaveDetails.LeaveCategoryId;
                    })[0];
                $scope.selectedEmployeeHierArchy.selected = _.filter($scope.employeeHierArchy,
                    function (item) {
                        return item.Id === $scope.leaveDetails.EmployeeId;
                    })[0];
            };
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
            $scope.onChangeLeaveCategory = function (selectedLeaveCategory) {
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

            var validate = function () {
                if ($scope.selectedLeaveCategory.selected != null) {
                    return true;
                }
                return false;
            };
            var validateDateRangeWithLeaveDays = function () {
                var fromDay = moment($scope.leaveDetails.FromDate).format("MM-DD-YYYY").split("-")[1];
                fromDay = parseInt(fromDay);
                var toDay = moment($scope.leaveDetails.ToDate).format("MM-DD-YYYY").split("-")[1];
                toDay = parseInt(toDay);
                var paidDay = parseInt($scope.leaveDetails.LeaveDays);
                var unpaidDay = parseInt($scope.leaveDetails.UnpaidLeaveDays);
                var totalLeave = paidDay + unpaidDay;
                var totalDateDifference = toDay - fromDay;
                if ((totalDateDifference + 1) === totalLeave) {
                    return true;
                }
                return false;
            };
            $scope.updateNewEmployeeLeave = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                if (!validateDateRangeWithLeaveDays()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput +" Correct difference between Leave Dates & Days ");
                    return;
                }
                $scope.employeeLeaveModel.Id = $scope.leaveDetails.Id;
                $scope.employeeLeaveModel.LeaveCategoryId = $scope.selectedLeaveCategory.selected === null ? "" : $scope.selectedLeaveCategory.selected.Id;
                $scope.employeeLeaveModel.EmployeeId = $scope.selectedEmployeeHierArchy.selected === null ? "" : $scope.selectedEmployeeHierArchy.selected.Id;
                $scope.employeeLeaveModel.Address = $scope.leaveDetails.Address;
                if ($scope.leaveDetails.LeaveDays > 0 && $scope.leaveDetails.UnpaidLeaveDays > 0) {
                    var totalLeaveDays = parseInt($scope.leaveDetails.LeaveDays) + parseInt($scope.leaveDetails.UnpaidLeaveDays)
                    $scope.employeeLeaveModel.LeaveDays = totalLeaveDays
                } else {
                    $scope.employeeLeaveModel.LeaveDays = $scope.leaveDetails.LeaveDays > 0 ? $scope.leaveDetails.LeaveDays : $scope.leaveDetails.UnpaidLeaveDays;
                }
               
                $scope.employeeLeaveModel.FromDate = moment($scope.leaveDetails.FromDate).format("MM-DD-YYYY");
                $scope.employeeLeaveModel.ToDate = moment($scope.leaveDetails.ToDate).format("MM-DD-YYYY");
                $scope.employeeLeaveModel.ReasonOfLeave = $scope.leaveDetails.ReasonOfLeave;
                $scope.employeeLeaveModel.MobileNo = $scope.leaveDetails.MobileNo;

                authService.loadingOn();
                var promise = employeeLeaveService.updateEmployeeLeave($scope.employeeLeaveModel);
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
                    leaveId = _.parseInt($state.params.Id);
                    $scope.text = $state.params.text;
                }

                var getEmployeeLeaveDetails = employeeLeaveService.getEmployeeLeaveById(leaveId);
                getEmployeeLeaveDetails.then(function (response) {
                    $scope.leaveDetails = response.LeaveById;
                    $scope.leaveCatregory = response.LeaveCategory;
                    $scope.employeeHierArchy = response.EmployeeHierArchy;
                }, function (err) {
                    $scope.leaveDetails = [];
                    $scope.leaveCatregory = [];
                    $scope.employeeHierArchy = [];
                    authService.loadingOff();
                 });

                $q.all([getEmployeeLeaveDetails]).then(function () {
                    fillDataFunction();
                        authService.loadingOff();
                    });
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