//'use strict';

angular.module('AtlasPPS').controller('productionForecastController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'employeeService', '$q', '$timeout', '$filter',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, employeeService, $q, $timeout, $filter) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelHeading = "Add Production Forecast";
            $scope.modelActionText = "Save";

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.productList = [];
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

            $scope.selectedProductionForecast = [];

            $scope.TotalPrice = 0;
            var totalPriceCalculation;

            $scope.addSalesForecast = function () {
                authService.loadingOn();
                var promiseGetProductionForecast = salesService.getProductionForecastList($scope.selectedYear.selected, $scope.selectedMonth.selected.Id);
                promiseGetProductionForecast.then(function (response) {
                    if (response.length > 0) {
                        _.forEach(response,
                            function (d) {
                                _.forEach($scope.productList,
                                    function(item) {
                                        if (d.ProductId === item.ProductId) {
                                            item.Quantity = d.Quantity;
                                            item.TotalUnitPrice = d.Quantity * item.UnitPrice;
                                            item.IsExist = 1;
                                        }
                                    });
                            });
                        $scope.productionForecastList = $scope.productList;
                        totalPriceCalculation();
                    } else {
                        $scope.productionForecastList = $scope.productList;
                        totalPriceCalculation();
                    }
                    authService.loadingOff();
                }, function (err) {
                    $scope.productionForecastList = [];
                    authService.loadingOff();
                });
            }

            
            $scope.onChangeQuantity = function (productionForecast) {
                productionForecast.TotalUnitPrice = productionForecast.Quantity * productionForecast.UnitPrice;
                totalPriceCalculation();
            }

            var validate = function () {
                if (!$scope.selectedProductionForecast
                    || !$scope.selectedYear.selected
                    || !$scope.selectedMonth.selected.Id
                    || (!$scope.productionForecastList && $scope.productionForecastList.length > 0)) {
                    return false;
                }
                return true;
            };

            totalPriceCalculation = function () {
                var totalAmount = 0;
                for (var i = 0; i < $scope.productionForecastList.length; i++) {
                    totalAmount = totalAmount +
                        $scope.productionForecastList[i].TotalUnitPrice;
                }
                $scope.TotalPrice = totalAmount;
            };

            var clearField;

            $scope.saveProductionForecast = function () {

                $scope.productionForecastList = _.filter($scope.productionForecastList,
                    function (d) {
                        return d.Quantity > 0 || d.IsExist === 1;
                    });

                if ($scope.productionForecastList && $scope.productionForecastList.length > 0) {
                    for (var i = 0; i < $scope.productionForecastList.length; i++) {
                        var newProductionForecast = {
                            ProductId: $scope.productionForecastList[i].Id,
                            Quantity: $scope.productionForecastList[i].Quantity,
                            SalesYear: $scope.selectedYear.selected,
                            SalesMonth: $scope.selectedMonth.selected.Id
                        }
                        $scope.selectedProductionForecast.push(newProductionForecast);
                    }
                }

                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.saveProductionForecast($scope.selectedProductionForecast);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $location.path("/Sales/ProductionForecastList");
                    hasTransaction = true;
                    $scope.selectedProductionForecast = [];
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.selectedProductionForecast = [];
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
                var promiseGetProductList = salesService.GetProductList();
                promiseGetProductList.then(function (response) {
                    $scope.productList = _.forEach(response,
                        function (d) {
                            d.ProductId = d.Id;
                            d.Quantity = 0;
                            d.TotalUnitPrice = 0;
                        });
                    authService.loadingOff();
                }, function (err) {
                    $scope.productList = [];
                    authService.loadingOff();
                });

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