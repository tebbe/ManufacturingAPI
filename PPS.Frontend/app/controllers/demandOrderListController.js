'use strict';

angular.module('AtlasPPS').controller('demandOrderListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'sharedService', 'reportSettings', 'uiGridConstants',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings, uiGridConstants) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.doList = [];
            $scope.doListUnPaid = [];
            $scope.doListPartiallyPaid = [];
            $scope.doListPaid = [];
            $scope.selectedDO = null;
            $scope.customers = [];
            $scope.selectedCustomer = {};

            //paging start

            var defaultSort = {
                SortDirection: "DESC",
                SortColumn: "DemandOrderNo"
            };

            $scope.paginationOptions = {
                PageIndex: 1,
                PageSize: 25,
                SortDirection: defaultSort.SortDirection,
                SortColumn: defaultSort.SortColumn,
                StartDate: null,
                EndDate: null,
                CustomerId: null
            };

            $scope.gridOptions = {
                paginationPageSizes: [25, 50, 75, 100],
                paginationPageSize: 25,
                rowHeight: 40,
                enableRowSelection: true,
                expandableRowScope: {
                    navigateToInvoiceView: function (d) {
                        $window.open(reportSettings.reportBaseUri + 'Invoice/View/' + d.Id + '/' + d.DemandOrderId, '_blank');
                    }
                    //,
                    //navigateToInvoiceView1: function (d) {
                    //    $window.open(reportSettings.reportBaseUri + 'Invoice/View/' + d.Id, '_blank');
                    //}
                },
                expandableRowTemplate: '/views/template/expandableRowTemplate.html',
                useExternalPagination: true,
                useExternalSorting: true,
                enableColumnResize: true,
                columnDefs: [
                    { name: 'DemandOrderNo', displayName: 'DO No.', enableSorting: true, width: '90', cellClass: 'text-right' },
                    { name: 'DODate', displayName: 'DO Date', enableSorting: true, width: '100', headerCellClass: 'text-center', cellClass: 'text-center', cellFilter: 'date:\'dd/MM/yyyy\'' },
                    {
                        name: 'MaturityDays', displayName: 'Maturity Days', enableSorting: false, width: '120', cellClass: 'text-center',
                        //cellTemplate: '<span class="label" ng-class="{\'label-success\': row.entity.MaturityLabel===4, \'label-default\': row.entity.MaturityLabel===1, \'label-warning\': row.entity.MaturityLabel===2, \'label-danger\': row.entity.MaturityLabel===3}" style="text-align: left; padding-left: 20px; padding-right: 20px;">{{ row.entity.MaturityDays }}</span>',
                        cellTemplate: '<span ng-if="row.entity.MaturityLabel===4" class="label" ng-class="{\'label-success\': row.entity.MaturityLabel===4}" style="text-align: left; padding-left: 20px; padding-right: 20px; background-color: forestgreen; color: white;">{{ row.entity.MaturityDays }}</span>'
                            + '<span ng-if="row.entity.MaturityLabel!==4" class="label" ng-class="{\'label-default\': row.entity.MaturityLabel===1, \'label-warning\': row.entity.MaturityLabel===2, \'label-danger\': row.entity.MaturityLabel===3}" style="text-align: left; padding-left: 20px; padding-right: 20px;">{{ row.entity.MaturityDays }}</span>'

                    },
                    { name: 'CustomerName', displayName: 'Customer Name', enableSorting: false, width: '200', cellClass: 'text-left' },
                    { name: 'TotalGrandAmount', displayName: 'Amount', enableSorting: true, width: '100', headerCellClass: 'text-right', cellClass: 'text-right', cellFilter: 'currency : \'\'' },
                    { name: 'CreatedByName', displayName: 'Created By', enableSorting: false, width: '150', cellClass: 'text-left' },
                    { name: 'DOStatusName', displayName: 'DO Status', enableSorting: false, width: '100', cellClass: 'text-left' },
                    { name: 'ProductTypeGroupName', displayName: 'Prd Type', enableSorting: false, width: '110', cellClass: 'text-left' },
                    { name: 'DOPaymentStatus', displayName: 'Payment', enableSorting: false, width: '100', cellClass: 'text-left' },
                    {
                        name: 'buttons', displayName: '', cellClass: 'text-right', width: '200',
                        cellTemplate: '<div class="ui-grid-cell-contents">' +
                            '<button type="button" class="btn btn-default btn-sm" style="padding-right=10px;" title="Edit" ng-if="((!row.entity.Submitted || (row.entity.Submitted && row.entity.DOStatusName !==\'Verified\')) && row.entity.DOStatusName !== \'Approved\' ) && grid.appScope.isAuthenticated(\'324\')" ng-click="grid.appScope.navigateToDoEdit(row.entity)"> Edit</button>' +
                            '<button type="button" class="btn btn-default btn-sm" title="View" ng-if="grid.appScope.isAuthenticated(\'323\')" ng-click="grid.appScope.navigateToDoView(row.entity)">View</button>' +
                            '<button type="button" class="btn btn-default btn-sm" title="Print" ng-if="grid.appScope.isAuthenticated(\'323\')" ng-click="grid.appScope.navigateToDoPrint(row.entity)">Print</button></div>'
                    }
                    //{
                    //    name: 'buttons', displayName: '', cellClass: 'text-right',
                    //    cellTemplate: '<div class="ui-grid-cell-contents">' +
                    //        '<button type="button" class="btn btn-default btn-sm" ng-if="((!row.Submitted || (row.Submitted && row.DOStatusName !==\'Verified\')) && row.DOStatusName !== \'Approved\' ) && isAuthenticated(\'324\')" title="Edit" ng-click="navigateToDoEdit(row)"> Edit</button>' +
                    //        '<button type="button" class="btn btn-default btn-sm" title="View" ng-if="isAuthenticated(\'323\')" ng-click="navigateToDoView(row)">View</button>' +
                    //        '<button type="button" class="btn btn-default btn-sm" title="View" ng-if="isAuthenticated(\'323\')" ng-click="navigateToDoPrint(row)">Print</button></div>'
                    //}

                ],
                
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;

                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (sortColumns.length > 1) {
                            var column = null;
                            for (var j = 0; j < grid.columns.length; j++) {
                                if (grid.columns[j].name === sortColumns[0].field) {
                                    column = grid.columns[j];
                                    break;
                                }
                            }
                            if (column) {
                                sortColumns[1].sort.priority = 1; // have to do this otherwise the priority keeps going up.                           
                                column.unsort();
                            }
                        }

                        if (sortColumns.length === 0) {
                            $scope.paginationOptions.SortDirection = defaultSort.SortDirection.toUpperCase();
                            $scope.paginationOptions.SortColumn = defaultSort.SortColumn;
                            $scope.gridApi.grid.columns[0].sort = { direction: uiGridConstants.DESC, priority: 0 };
                            //sortColumns[0].sort = { direction: uiGridConstants.DESC, priority: 0 };
                        } else {
                            $scope.paginationOptions.SortDirection = sortColumns[0].sort.direction.toUpperCase();
                            $scope.paginationOptions.SortColumn = sortColumns[0].name;
                        }
                        getPage();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (newPage, PageSize) {
                        $scope.paginationOptions.PageIndex = newPage;
                        $scope.paginationOptions.PageSize = PageSize;
                        $scope.paginationOptions.SortDirection = defaultSort.SortDirection.toUpperCase();
                        $scope.paginationOptions.SortColumn = defaultSort.SortColumn;
                        
                            getPage();
                    });
                }
              
            };

            var getPage = function () {
                ////var url;
                //switch ($scope.paginationOptions.SortDirection) {
                //    case uiGridConstants.ASC:
                //        break;
                //    case uiGridConstants.DESC:
                //        break;
                //    default:
                //        break;
                //}

                salesService.getDemandOrderListForFiltering($scope.paginationOptions)
                    .then(function (response) {
                        var data = response;
                        $scope.totalAmount = 0;
                        for (var i = 0; i < data.length; i++) {
                            $scope.totalAmount += data[i].TotalGrandAmount;
                            data[i].subGridOptions = {
                                columnDefs: [
                                    { name: 'InvoiceNo', field: 'InvoiceNo', displayName: 'Invoice No', enableSorting: false, width: '10%', headerCellClass: 'text-left', cellClass: 'text-right' },
                                    { name: 'TotalGrandAmount', field: 'TotalGrandAmount', displayName: 'Total Amount', enableSorting: false, width: '10%', headerCellClass: 'text-left', cellClass: 'text-right', cellFilter: 'currency : \'\'' },
                                    { name: 'InvoiceDate', field: 'InvoiceDate', displayName: 'Invoice Date', enableSorting: false, width: '10%', headerCellClass: 'text-left', cellClass: 'text-right', cellFilter: 'date:\'dd/MM/yyyy\'' },
                                    {
                                        name: 'buttons', displayName: '', cellClass: 'text-center', width: '10%', padding: '0px 12px',
                                        cellTemplate: '<div class="ui-grid-cell-contents">' +
                                            '<a type="button" class="btn btn-default" style="padding: 0 20px;" ng-click="grid.appScope.navigateToInvoiceView(row.entity);">View</a></div>'
                                    }
                                    //,{
                                    //    name: 'buttons1', displayName: '', cellClass: 'text-center', width: '10%', padding: '0px 12px',
                                    //    cellTemplate: '<div class="ui-grid-cell-contents">' +
                                    //        '<a type="button" class="btn btn-default" style="padding: 0 20px;" ng-click="grid.appScope.navigateToInvoiceView1(row.entity);">View</a></div>'
                                    //}
                                ],
                                data: data[i].InvoiceList
                            };
                        }

                        if (response && response.length > 0) {
                            $scope.gridOptions.totalItems = response[0].TotalCount;
                        }
                        else {
                            $scope.gridOptions.totalItems = 0;
                        }
                        //var firstRow = ($scope.myPagination.PageIndex - 1) * $scope.myPagination.PageSize;
                        //$scope.gridOptions.data = data.slice(firstRow, firstRow + $scope.myPagination.PageSize);
                        $scope.gridOptions.data = data;
                    });
            };

            getPage();

            //paging end


            var getDemandOrderList = function () {
                authService.loadingOn();
                var promise = salesService.getDemandOrderList();
                promise.then(function (response) {
                    $scope.doList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doList = [];
                    authService.loadingOff();
                });
            };

            var getDemandOrderListForFiltering = function () {
                authService.loadingOn();
                var promise = salesService.GetDemandOrderListForFiltering($scope.myPagination);
                promise.then(function (response) {
                    $scope.doList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doList = [];
                    authService.loadingOff();
                });
            };

            var demandOrderListUnPaidApiCalled = false;
            $scope.getDemandOrderUnPaidList = function () {
                if (demandOrderListUnPaidApiCalled) {
                    return;
                }
                demandOrderListUnPaidApiCalled = true;
                authService.loadingOn();
                var promise = salesService.getDemandOrderUnPaidList();
                promise.then(function (response) {

                    $scope.doListUnPaid = response;
                    authService.loadingOff();
                }, function (err) {
                    authService.loadingOff();
                    $scope.doListUnPaid = [];
                    authService.loadingOff();
                });
            };

            var demandOrderListPartiallyPaidApiCalled = false;
            $scope.getDemandOrderPartiallyPaidList = function () {
                if (demandOrderListPartiallyPaidApiCalled) {
                    return;
                }
                demandOrderListPartiallyPaidApiCalled = true;
                authService.loadingOn();
                var promise = salesService.getDemandOrderPartiallyPaidList();
                promise.then(function (response) {
                    $scope.doListPartiallyPaid = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doListPartiallyPaid = [];
                    authService.loadingOff();
                });
            };

            var demandOrderListPaidApiCalled = false;
            $scope.getDemandOrderPaidList = function () {
                if (demandOrderListPaidApiCalled) {
                    return;
                }
                demandOrderListPaidApiCalled = true;
                authService.loadingOn();
                var promise = salesService.getDemandOrderPaidList();
                promise.then(function (response) {
                    $scope.doListPaid = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doListPaid = [];
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
                var promise = salesService.getCustomerList();
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.customers = response;
                }, function (err) {
                    $scope.customers = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };
            pageLoad();
            //Date fielter start

            $scope.reportDateRange = {
                startDate: moment().format('YYYY-MM-DD'),
                endDate: moment().format('YYYY-MM-DD')
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


            //Date Fielter end


            $scope.addNewDemandOrder = function () {
                $location.path("/Sales/DemandOrder/Create");
            };
            $scope.getDOByFielter = function () {
                $scope.paginationOptions.StartDate = moment($scope.reportDateRange.startDate).format('YYYY-MM-DD');
                $scope.paginationOptions.EndDate = moment($scope.reportDateRange.endDate).format('YYYY-MM-DD');
                $scope.paginationOptions.CustomerId = $scope.selectedCustomer.selected !== null ? $scope.selectedCustomer.selected.Id:0;
                getPage();
            };
            $scope.navigateToDoEdit = function (d) {
                //$location.path("/Sales/DemandOrder/Edit/" + d.Id);
                $window.open(reportSettings.reportBaseUri + 'Sales/DemandOrder/Edit/' + d.Id, '_blank');
            };
            $scope.navigateToDoView = function (d) {
                $window.open(reportSettings.reportBaseUri + 'Sales/DemandOrder/View/' + d.Id, '_blank');
            };

            $scope.navigateToDoPrint = function (d) {
                //$window.open('/#/reports/demandOrderPrint/' + d.Id, '_blank');
                $window.open(reportSettings.reportBaseUri + 'reports/demandOrderPrint/' + d.Id, '_blank');
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
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });

            $scope.getPaymentStatusColorClass = function (demandOrder) {
                return sharedService.getDemandOrderPaymentStatusClass(demandOrder.DOPaymentStatusId);
            };
        }]);