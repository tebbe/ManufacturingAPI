//'use strict';

angular.module('AtlasPPS').controller('companySalesTargetEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'employeeService', '$q', '$timeout', '$state','$filter',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, employeeService, $q, $timeout, $state, $filter) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelHeading = "Update Company Sales Target";
            $scope.modelActionText = "Update";

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            var companySalesTargetId;

            function buildLocaleProvider(formatString) {
                return {
                    formatDate: function (date) {
                        if (date) return $filter('date')(date, formatString);
                        else return null;
                    },
                    parseDate: function (dateString) {
                        if (dateString) {
                            var m = $filter('date')(date, formatString);
                            return m.isValid() ? m.toDate() : new Date(NaN);
                        } else return null;
                    }
                };
            }

            ////10 seconds delay
            //$timeout(function () {
            //    $scope.dateFieldsLocale = buildLocaleProvider("MMM-YYYY");
            //}, 5000);

            $scope.dateFieldsLocale = buildLocaleProvider("MMM-yyyy");

            $scope.selectedCompanySalesTarget = {
                SalesTarget: null,
                SalesMonth: new Date()
            };

            var validate = function () {
                if (!$scope.selectedCompanySalesTarget.SalesTarget
                    || !$scope.selectedCompanySalesTarget.SalesMonth) {
                    return false;
                }
                return true;
            };

            var clearField;

            $scope.updateCompanySalesTarget = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedCompanySalesTarget.SalesMonth = moment("1-" + moment($scope.selectedCompanySalesTarget.SalesMonth).format("MMM-YYYY")).format("MM-DD-YYYY");

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.updateCompanySalesTarget($scope.selectedCompanySalesTarget);

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

                if ($state.params && $state.params.companySalesTargetId) {
                    companySalesTargetId = _.parseInt($state.params.companySalesTargetId);
                }

                authService.loadingOn();
                var promiseGetCompanySalesTargetById = salesService.getCompanySalesTargetById(companySalesTargetId);
                promiseGetCompanySalesTargetById.then(function (response) {
                    //response.SalesMonth = $filter('date')(response.SalesMonth, "dd-MMM-yyyy");
                    $scope.selectedCompanySalesTarget = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.selectedCompanySalesTarget = {};
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