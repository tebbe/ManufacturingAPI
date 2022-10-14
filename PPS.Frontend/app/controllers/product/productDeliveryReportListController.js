'use strict';

angular.module('AtlasPPS').controller('productDeliveryReportListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'authService', 'productService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, authService, productService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.productReportList = [];
            $scope.customer = [];
            $scope.product = [];

            $scope.selectedCustomer = {
                selected: null
            }
            $scope.selectedProduct = {
                selected: null
            }

            $scope.ClearDropdown = function () {
                $scope.selectedCustomer.selected = null;
                $scope.selectedProduct.selected = null;
            }
            $scope.OnChangeProduct = function (selectedProduct) {
                if (selectedProduct && selectedProduct.Id) {
                    var result = $scope.product.filter(function (v) {
                        return v.Id === selectedProduct.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedProduct.selected = result[0];
                    }
                    else {
                        $scope.selectedProduct.selected = null;
                    }
                } else {
                    $scope.selectedProduct.selected = null;
                }
            }
            $scope.OnChangeCustomer = function (selectedCustomer) {
                if (selectedCustomer && selectedCustomer.Id) {
                    var result = $scope.customer.filter(function (v) {
                        return v.Id === selectedCustomer.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedCustomer.selected = result[0];
                    } else {
                        $scope.selectedCustomer.selected = null;
                    }
                } else {
                    $scope.selectedCustomer.selected = null;
                }
            }


            $scope.GetProductReport = function () {
                var vm = {
                    //FiscalYear: fiscalYear,
                    CustomerId: $scope.selectedCustomer.selected === null ? 0 : $scope.selectedCustomer.selected.Id,
                    ProductId: $scope.selectedProduct.selected === null ? 0 : $scope.selectedProduct.selected.Id,
                    StartDate: moment($scope.reportDateRange.startDate).format('YYYY-MM-DD') != '1970-01-01' ? moment($scope.reportDateRange.startDate).format('YYYY-MM-DD') : moment(-1),
                    EndDate: moment($scope.reportDateRange.endDate).format('YYYY-MM-DD') != '1970-01-01' ? moment($scope.reportDateRange.endDate).format('YYYY-MM-DD') : moment(0)
                };
                authService.loadingOn();
                var promise = productService.getProductDeliveryReportList(vm);
                promise.then(function (response) {
                    $scope.productReportList = response;
                    var totalAmount = 0;
                    if ($scope.productReportList && $scope.productReportList.length > 0) {
                        for (var i = 0; i < $scope.productReportList.length; i++) {
                            totalAmount = totalAmount + $scope.productReportList[i].Ammount;
                        }
                        $scope.SumOfTotalAmount = totalAmount;
                    }
                   
                    authService.loadingOff();
                }, function (err) {
                    $scope.productReportList = [];
                    authService.loadingOff();
                });
            };


            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
                authService.loadingOn();
                var getCustomerList = salesService.getCustomerList();
                getCustomerList.then(function (response) {
                    $scope.customer = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customer = response;
                    authService.loadingOff();
                });
                authService.loadingOn();
                var getProductList = salesService.GetProductList();
                getProductList.then(function (response) {
                    $scope.product = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customer = [];
                    authService.loadingOff();
                });

            };
            pageLoad();




            $scope.reportDateRange = {
                startDate: moment(0).format('YYYY-MM-DD'),
                endDate: moment(0).format('YYYY-MM-DD')
            };
            $scope.singleDate = moment();

            $scope.opts = {
                locale: {
                    applyClass: 'btn-green',
                    applyLabel: "Apply",
                    fromLabel: "From",
                    format: "DD-MM-YYYY",
                    toLabel: "To",
                    cancelLabel: 'Cancel',
                    customRangeLabel: 'Custom range'
                },
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'As of Today': [moment(new Date(new Date().getFullYear() + "/1/1")), moment(new Date())]
                }
            };

            $scope.setRange = function () {
                $scope.reportDateRange = {
                    startDate: moment().format('YYYY-MM-DD'),
                    endDate: moment().format('YYYY-MM-DD')
                };
            };

            //Watch for date changes
            $scope.$watch('reportDateRange', function (newDate) {
                //console.log('New date set: ', newDate);
            }, false);

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