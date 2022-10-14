'use strict';

angular.module('AtlasPPS').controller('salesReportListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'sharedService', 'reportSettings', 'customerService', 'employeeService', '$q', '$filter',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings, customerService, employeeService, $q, $filter) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.salesReportList = [];
            $scope.salesAreaList = [];
            $scope.salesOfficerList = [];
            $scope.customerList = [];

            $scope.filteredSalesArea = [];
            $scope.filteredSalesOfficer = [];
            $scope.filteredCustomer = [];
            $scope.isValidated = false;

            $scope.salesDivision = [{
                Id: -1, SalesDivisionName: 'All'
            }];
            //$scope.salesArea = [{
            //    Id: -1, SalesAreaName: 'All'
            //}];
            //$scope.salesOfficer = [{
            //    Id: -1, FullName: 'All', Designation: null, EmployeeCode: null
            //}];
            //$scope.customer = [{
            //    Id: -1, CustomerName: 'All', CustomerCode: null
            //}];

            $scope.salesArea = [];
            $scope.salesOfficer = [];
            $scope.customer = [];

            $scope.selectedSalesDivision = {
                selected: null
            };
            $scope.selectedSalesArea = {
                selected: null
            };
            $scope.selectedSalesOfficer = {
                selected: null
            };
            $scope.selectedCustomer = {
                selected: null
            };

            $scope.getSalesReportList = function () {
                authService.loadingOn();
                var salesDivisionId = $scope.selectedSalesDivision.selected.Id;
                var startDate = moment($scope.date.startDate).format('YYYY-MM-DD');
                var endDate = moment($scope.date.endDate).format('YYYY-MM-DD');
                var salesAreaId = $scope.selectedSalesArea.selected.Id;
                var employeeId = $scope.selectedSalesOfficer.selected.Id;
                var customerId = $scope.selectedCustomer.selected.Id;
                var promise = salesService.getSalesReportList(startDate, endDate, salesDivisionId, salesAreaId, employeeId, customerId);
                promise.then(function (response) {
                    $scope.salesReportList = response.salesReportList;
                    CalculateInvoiceAmount();
                    authService.loadingOff();
                }, function (err) {
                    $scope.salesReportList = [];
                    authService.loadingOff();
                });
            };

            $scope.printSalesReportList = function () {
                var startDate = moment($scope.date.startDate).format('YYYY-MM-DD');
                var endDate = moment($scope.date.endDate).format('YYYY-MM-DD');
                var salesDivisionId = $scope.selectedSalesDivision.selected.Id;
                var salesAreaId = $scope.selectedSalesArea.selected.Id;
                var employeeId = $scope.selectedSalesOfficer.selected.Id;
                var customerId = $scope.selectedCustomer.selected.Id;
                $window.open(reportSettings.reportBaseUri + 'reports/SalesReportPrint/' + startDate + '/' + endDate + '/' + salesDivisionId + '/' + salesAreaId + '/' + employeeId + '/' + customerId);
            };

            $scope.onChangeSalesDivision = function (salesDivision) {
                $scope.isValidated = true;
                $scope.salesReportList = [];
                CalculateInvoiceAmount();
                $scope.salesAreaList = [];
                $scope.salesOfficerList = [];
                $scope.customerList = [];
                $scope.salesArea = [{
                    Id: -1, SalesAreaName: 'All'
                }];
                $scope.salesOfficer = [{
                    Id: -1, FullName: 'All', Designation: null, EmployeeCode: null
                }];
                $scope.customer = [{
                    Id: -1, CustomerName: 'All', CustomerCode: null
                }];
                if (salesDivision.Id !== -1) {
                    authService.loadingOn();
                    var promise = salesService.getSalesAreaWithSalesOfficerWithCustomerList(salesDivision.Id);
                    promise.then(function (response) {
                        $scope.salesAreaList = response.SA;
                        Array.prototype.push.apply($scope.salesArea, $scope.salesAreaList);
                        $scope.selectedSalesArea.selected = $scope.salesArea[0];
                        $scope.salesOfficerList = response.SO;
                        Array.prototype.push.apply($scope.salesOfficer, $scope.salesOfficerList);
                        $scope.selectedSalesOfficer.selected = $scope.salesOfficer[0];
                        $scope.customerList = response.Customers;
                        Array.prototype.push.apply($scope.customer, $scope.customerList);
                        $scope.selectedCustomer.selected = $scope.customer[0];
                        authService.loadingOff();
                    },
                        function (err) {
                            $scope.salesArea = [{
                                Id: -1, SalesAreaName: 'All'
                            }];
                            $scope.salesOfficer = [{
                                Id: -1, FullName: 'All', Designation: null, EmployeeCode: null
                            }];
                            $scope.customer = [{
                                Id: -1, CustomerName: 'All', CustomerCode: null
                            }];
                            authService.loadingOff();
                        });

                } else {
                    authService.loadingOn();
                    promise = salesService.getSalesAreaWithSalesOfficerWithCustomerList(salesDivision.Id);
                    promise.then(function (response) {
                        $scope.salesAreaList = response.SA;
                        Array.prototype.push.apply($scope.salesArea, $scope.salesAreaList);
                        $scope.selectedSalesArea.selected = $scope.salesArea[0];
                        $scope.salesOfficerList = response.SO;
                        Array.prototype.push.apply($scope.salesOfficer, $scope.salesOfficerList);
                        $scope.selectedSalesOfficer.selected = $scope.salesOfficer[0];
                        $scope.customerList = response.Customers;
                        Array.prototype.push.apply($scope.customer, $scope.customerList);
                        $scope.selectedCustomer.selected = $scope.customer[0];
                        authService.loadingOff();
                    },
                        function (err) {
                            $scope.salesArea = [{
                                Id: -1, SalesAreaName: 'All'
                            }];
                            $scope.salesOfficer = [{
                                Id: -1, FullName: 'All', Designation: null, EmployeeCode: null
                            }];
                            $scope.customer = [{
                                Id: -1, CustomerName: 'All', CustomerCode: null
                            }];
                            authService.loadingOff();
                        });
                }
            };


            $scope.onChangeSalesArea = function (salesArea) {
                if (salesArea.Id !== -1) {
                    authService.loadingOn();
                    $scope.salesReportList = [];
                    CalculateInvoiceAmount();
                    $scope.salesOfficer = [{
                        Id: -1, FullName: 'All', Designation: null, EmployeeCode: null
                    }];
                    $scope.filteredSalesOfficer = _.filter($scope.salesOfficerList, function (item) {
                        return item.SalesAreaId === salesArea.Id;
                    });
                    Array.prototype.push.apply($scope.salesOfficer, $scope.filteredSalesOfficer);
                    $scope.selectedSalesOfficer.selected = $scope.salesOfficer[0];

                    $scope.customer = [{
                        Id: -1, CustomerName: 'All', CustomerCode: null
                    }];

                    $scope.newCustomer = [];
                    $scope.filteredSalesOfficer.forEach(function (item) {
                        $scope.newCustomer = $scope.newCustomer.concat(_.filter($scope.customerList,
                            function (cs) {
                                return item.Id === cs.EmployeeId;
                            }));
                    });
                    Array.prototype.push.apply($scope.customer, $scope.newCustomer);
                    $scope.selectedCustomer.selected = $scope.customer[0];
                    authService.loadingOff();
                }
            };

            $scope.onChangeSalesOfficer = function (salesOfficer) {
                if (salesOfficer.Id !== -1) {
                    authService.loadingOn();
                    $scope.salesReportList = [];
                    CalculateInvoiceAmount();

                    $scope.customer = [{
                        Id: -1, CustomerName: 'All', CustomerCode: null
                    }];
                    $scope.filteredCustomer = _.filter($scope.customerList, function (item) {
                        return item.EmployeeId === salesOfficer.Id;
                    });
                    Array.prototype.push.apply($scope.customer, $scope.filteredCustomer);
                    $scope.selectedCustomer.selected = $scope.customer[0];
                    authService.loadingOff();
                }
            };
            $scope.onChangeCustomer = function (customer) {
                if (customer.Id !== -1) {
                    $scope.salesReportList = [];
                    CalculateInvoiceAmount();
                }
            };

            $scope.TotalInvoiceAmount = 0;
            $scope.TotalPaidAmount = 0;
            $scope.TotalBalanceAmount = 0;

            var CalculateInvoiceAmount = function () {
                var totalInvoiceAmount = 0;
                var totalPaidAmount = 0;
                var totalBalanceAmount = 0;

                if ($scope.salesReportList) {
                    for (var item in $scope.salesReportList) {
                        if ($scope.salesReportList.hasOwnProperty(item)) {
                            totalInvoiceAmount += $scope.salesReportList[item].DOAmount;
                            totalPaidAmount += $scope.salesReportList[item].DOPaid;
                        }
                    }
                }
                totalBalanceAmount = totalInvoiceAmount - totalPaidAmount;
                $scope.TotalInvoiceAmount = totalInvoiceAmount;
                $scope.TotalPaidAmount = totalPaidAmount;
                $scope.TotalBalanceAmount = totalBalanceAmount;
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

                var promiseGetSalesDivisionList = salesService.getSalesDivisionList();
                promiseGetSalesDivisionList.then(function (response) {
                    Array.prototype.push.apply($scope.salesDivision, response);
                }, function (err) {
                    $scope.salesDivision = [{
                        Id: -1, SalesDivisionName: 'All'
                    }];
                });

                //var promiseGetSalesAreaList = salesService.getSalesAreaList();
                //promiseGetSalesAreaList.then(function (response) {
                //    Array.prototype.push.apply($scope.salesArea, response);
                //}, function (err) {
                //    $scope.salesArea = [{
                //        Id: -1, SalesAreaName: 'All'
                //    }];
                //});

                //var promiseGetSalesOfficer = employeeService.getSalesOfficer();
                //promiseGetSalesOfficer.then(function (response) {
                //    Array.prototype.push.apply($scope.salesOfficer, response);
                //}, function (err) {
                //    $scope.salesOfficer = [{
                //        Id: -1, FullName: 'All', Designation: null, EmployeeCode: null
                //    }];
                //});

                //var promiseGetCustomerList = salesService.getCustomerList();
                //promiseGetCustomerList.then(function (response) {
                //    Array.prototype.push.apply($scope.customer, response);
                //}, function (err) {
                //    $scope.customer = [{
                //        Id: -1, CustomerName: 'All', CustomerCode: null
                //    }];
                //});

                authService.loadingOn();
                $q.all([
                    promiseGetSalesDivisionList,
                    //promiseGetSalesAreaList,
                    //promiseGetSalesOfficer,
                    //promiseGetCustomerList
                ]).then(function () {
                    authService.loadingOff();
                });
            };
            pageLoad();

            $scope.date = {
                startDate: moment(),
                endDate: moment()
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
                    //'This Year': [moment(new Date(new Date().getFullYear() + "/1/1")), moment(new Date(new Date().getFullYear() + "/12/31"))]
                }
            };

            $scope.setRange = function () {
                $scope.date = {
                    startDate: moment("en"),
                    endDate: moment("en")
                };
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
                .withOption('order', [1, 'asc'])
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