//'use strict';

angular.module('AtlasPPS').controller('companySalesTargetController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'employeeService', '$q', '$timeout',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, employeeService, $q, $timeout) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelHeading = "Add Company Sales Target";
            $scope.modelActionText = "Save";

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

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

            $scope.selectedMonth = {
                selected: $scope.months[new Date().getMonth()]
            };

            $scope.currentYear = new Date().getFullYear();


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

            ////10 seconds delay
            //$timeout(function () {
            //    $scope.dateFieldsLocale = buildLocaleProvider("MMM-YYYY");
            //}, 5000);

            $scope.dateFieldsLocale = buildLocaleProvider("MMM-YYYY");

            $scope.selectedCompanySalesTarget = {
                SalesTarget: null,
                SalesYear: null,
                SalesMonth: null
            };

            var validate = function () {
                if (!$scope.selectedCompanySalesTarget.SalesTarget
                    || !$scope.selectedCompanySalesTarget.SalesMonth) {
                    return false;
                }
                return true;
            };

            var clearField;

            $scope.saveCompanySalesTarget = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                //$scope.selectedCompanySalesTarget.SalesMonth = moment("1-" + moment($scope.selectedCompanySalesTarget.SalesMonth).format("MMM-YYYY")).format("MM-DD-YYYY");;
                $scope.selectedCompanySalesTarget.SalesYear = $scope.currentYear;
                $scope.selectedCompanySalesTarget.SalesMonth = $scope.selectedMonth.selected.Id;

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.saveCompanySalesTarget($scope.selectedCompanySalesTarget);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $location.path("/Sales/CompanySalesTarget");
                    clearField();
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            clearField = function () {
                $scope.selectedCompanySalesTarget.SalesTarget = null;
                $scope.selectedCompanySalesTarget.SalesMonth = new Date();
            }

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

                $scope.selectedCompanySalesTarget.SalesMonth = new Date();
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