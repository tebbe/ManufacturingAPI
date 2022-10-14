//'use strict';

angular.module('AtlasPPS').controller('salesTeamTargetController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'employeeService', '$q', '$timeout', '$filter',
        function ($scope,
            $rootScope,
            localStorageService,
            $location,
            notificationService,
            authService,
            salesService,
            ngAuthSettings,
            DTOptionsBuilder,
            PpsConstant,
            $window,
            employeeService,
            $q,
            $timeout,
            $filter) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelHeading = "Add Sales Team Target";
            $scope.modelActionText = "Save";

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;
            $scope.salesOfficer = [];
            $scope.employeeList = [];
            $scope.SalesTeamTargetDetail = [];
            $scope.isDisable = false;

            $scope.selectedSalesAccount = {
                selected: null
            }

            $scope.months = [
                { Id: 1, monthName: 'Jan' },
                { Id: 2, monthName: 'Feb' },
                { Id: 3, monthName: 'Mar' },
                { Id: 4, monthName: 'Apr' },
                { Id: 5, monthName: 'May' },
                { Id: 6, monthName: 'Jun' },
                { Id: 7, monthName: 'Jul' },
                { Id: 8, monthName: 'Aug' },
                { Id: 9, monthName: 'Sept' },
                { Id: 10, monthName: 'Oct' },
                { Id: 11, monthName: 'Nov' },
                { Id: 12, monthName: 'Dec' }
            ];

            $scope.salesStack = [];

            $scope.selectedMonth = {
                selected: $scope.months[new Date().getMonth()]
            };

            var currentYear = new Date().getFullYear();

            $scope.years = [];
            $scope.years.push((currentYear - 1).toString());
            $scope.years.push((currentYear).toString());
            $scope.years.push((currentYear + 1).toString());

            $scope.selectedYear = {
                selected: $scope.years[1]
            };

            function buildLocaleProvider(formatString) {
                return {
                    formatDate: function (date) {
                        if (date) return moment(date).format(formatString);
                        else return null;
                    },
                    parseDate: function (dateString) {
                        if (dateString) {
                            var m = moment(dateString, formatString, true);
                            return m.isValid() ? m.toDate() : new Date(NaN);
                        } else return null;
                    }
                };
            }

            $scope.dateFieldsLocale = buildLocaleProvider("MMM-YYYY");
            $scope.selectedSalesTeamTarget = [];

            $scope.onChangedEmployee = function (selectedSalesAccount, fromList) {
                if (selectedSalesAccount.hasChild === false) {
                    return;
                }
                if (selectedSalesAccount && selectedSalesAccount.Id) {
                    var result = $scope.salesOfficer.filter(function (v) {
                        return v.Id === selectedSalesAccount.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.employeeList = _.filter($scope.salesOfficer,
                            function (x) {
                                return x.ManagerId === selectedSalesAccount.Id;
                            });
                        if ($scope.employeeList.length > 0) {
                            $scope.selectedSalesAccount.selected = result[0];
                            $scope.SalesTeamTargetDetail = $scope.employeeList;
                            $scope.SalesTarget = result[0].SalesTarget;
                            $scope.TeamTarget = result[0].TeamTarget;
                        } else {
                            $scope.SalesTeamTargetDetail = [];
                        }
                    } else {
                        $scope.SalesTeamTargetDetail = [];
                    }
                } else {
                    $scope.SalesTeamTargetDetail = [];
                }
                if ($scope.SalesTeamTargetDetail && $scope.SalesTeamTargetDetail.length > 0) {
                    if (fromList === true) {
                        $scope.salesStack.push(selectedSalesAccount);
                    } else if (fromList === false) {
                        $scope.salesStack = [];
                        $scope.salesStack.push(selectedSalesAccount);
                    }
                }
            }

            var getSalesEmployeeWithSalesTargetByMonth;

            $scope.onChangedMonth = function (selectedMonth) {
                $scope.selectedMonth.selected.Id = selectedMonth.Id;
                $scope.selectedYear.selected = selectedYear;
                $scope.SalesTarget = null;
                $scope.TeamTarget = null;
                $scope.selectedSalesAccount.selected = null;
                $scope.SalesTeamTargetDetail = null;
                getSalesEmployeeWithSalesTargetByMonth();
            }

            $scope.onChangedYear = function (selectedYear) {
                $scope.selectedMonth.selected.Id = selectedMonth.Id;
                $scope.selectedYear.selected = selectedYear;
                $scope.SalesTarget = null;
                $scope.TeamTarget = null;
                $scope.selectedSalesAccount.selected = null;
                $scope.SalesTeamTargetDetail = null;
                getSalesEmployeeWithSalesTargetByMonth();
            }

            $scope.navigateToFillEmployee = function (salesTeam) {
                $scope.onChangedEmployee(salesTeam, true);
            }

            $scope.backToPreviousData = function () {
                if ($scope.salesStack.length > 1) {
                    $scope.salesStack.pop();
                    var salesPerson = $scope.salesStack[$scope.salesStack.length - 1];
                    $scope.onChangedEmployee(salesPerson, null);
                }
            }

            var validate = function () {
                if (!$scope.selectedSalesTeamTarget
                    || !$scope.currentYear
                    || !$scope.selectedMonth.selected.Id
                    || (!$scope.SalesTeamTargetDetail && $scope.SalesTeamTargetDetail.length > 0)) {
                    return false;
                }
                return true;
            };

            var equalityCheckCalculation = function () {
                var totalTarget = 0;
                for (var i = 0; i < $scope.selectedSalesTeamTarget.length; i++) {
                    totalTarget = totalTarget +
                        $scope.selectedSalesTeamTarget[i].SalesTarget +
                        $scope.selectedSalesTeamTarget[i].TeamTarget;
                }
                if (totalTarget === ($scope.SalesTarget + $scope.TeamTarget)) {
                    return true;
                }
                return false;
            }

            var clearField;

            $scope.saveSalesTeamTarget = function () {
                if ($scope.SalesTeamTargetDetail && $scope.SalesTeamTargetDetail.length > 0) {
                    for (var i = 0; i < $scope.SalesTeamTargetDetail.length; i++) {

                        var newSalesTeamTarget = {
                            EmployeeId: $scope.SalesTeamTargetDetail[i].Id,
                            SalesTarget: $scope.SalesTeamTargetDetail[i].SalesTarget,
                            TeamTarget: $scope.SalesTeamTargetDetail[i].TeamTarget,
                            SalesYear: $scope.currentYear,
                            SalesMonth: $scope.selectedMonth.selected.Id
                        }

                        $scope.selectedSalesTeamTarget.push(newSalesTeamTarget);
                    }
                }

                if (equalityCheckCalculation() === false) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    $scope.selectedSalesTeamTarget = [];
                    return;
                }

                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.saveSalesTeamTarget($scope.selectedSalesTeamTarget);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    //$location.path("/Sales/SalesTeamTarget");
                    clearField();
                    hasTransaction = true;
                    $scope.selectedSalesTeamTarget = [];
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.selectedSalesTeamTarget = [];
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            clearField = function () {
                $scope.selectedSalesTeamTarget.SalesTarget = null;
                $scope.selectedSalesTeamTarget.TeamTarget = null;
                $scope.selectedSalesTeamTarget.SalesMonth = new Date();
            }


            getSalesEmployeeWithSalesTargetByMonth = function () {
                authService.loadingOn();

                var employeeRequestVm = {
                    Month: $scope.selectedMonth.selected.Id,
                    Year: $scope.selectedYear.selected
                }

                var promiseGetSalesOfficer = employeeService.getSalesEmployeeWithSalesTargetByMonth(employeeRequestVm);
                promiseGetSalesOfficer.then(function (response) {
                    _.forEach(response,
                        function (item) {
                            var manager = _.filter(response,
                                function (x) {
                                    return x.ManagerId === item.Id;
                                });
                            if (manager.length > 0) {
                                item.hasChild = true;
                            } else {
                                item.hasChild = false;
                            }
                        });
                    $scope.salesOfficer = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.salesOfficer = [];
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

                getSalesEmployeeWithSalesTargetByMonth();

            };
            pageLoad();


            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.$on('$viewContentLoaded', function () {
                $('.inner-table .dataTables_info').css({ 'display': 'none' });
            });

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
                .withOption('paging', false)
                .withOption('searching', false)
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $('.inner-table .html5buttons').css({ 'display': 'none' });
            $('.inner-table .dataTables_length').css({ 'display': 'none' });
            $('.inner-table .dataTables_info').css({ 'display': 'none !important' });
            $('.inner-table .dataTables_filter').css({ 'display': 'none' });
        }]);