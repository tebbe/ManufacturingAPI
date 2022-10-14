'use strict';

angular.module('AtlasPPS').controller('productionForecastListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'sharedService', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.productionForecastList = [];

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

            var currentYear = new Date().getFullYear();

            $scope.years = [];
            $scope.years.push((currentYear - 1).toString());
            $scope.years.push((currentYear).toString());
            $scope.years.push((currentYear + 1).toString());

            $scope.selectedYear = {
                selected: $scope.years[1]
            };

            var totalPriceCalculation = function () {
                var totalAmount = 0;
                for (var i = 0; i < $scope.productionForecastList.length; i++) {
                    totalAmount = totalAmount +
                        $scope.productionForecastList[i].TotalUnitPrice;
                }
                $scope.TotalPrice = totalAmount;
            };

            $scope.getProductionForecastList = function () {
                authService.loadingOn();
                var promise = salesService.getProductionForecastList($scope.selectedYear.selected, $scope.selectedMonth.selected.Id);
                promise.then(function (response) {
                    _.forEach(response,
                        function (d) {
                            d.TargetDate = new Date(d.TargetDate);
                        });
                    $scope.productionForecastList = response;
                    totalPriceCalculation();
                    authService.loadingOff();
                }, function (err) {
                    $scope.productionForecastList = [];
                    authService.loadingOff();
                });
            };

            $scope.printProductionForecastList = function () {
                var year = $scope.selectedYear.selected;
                var month = $scope.selectedMonth.selected.Id;
                $window.open(reportSettings.reportBaseUri + 'reports/ProductionForecastPrint/' + year +'/'+month);
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
            };
            pageLoad();

            $scope.addNewProductionForecast = function () {
                $location.path("/Sales/ProductionForecast/Create");
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
            //$scope.$on('$viewContentLoaded', function () {
            //    $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            //});
        }]);