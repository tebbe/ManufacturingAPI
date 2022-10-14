//'use strict';

angular.module('AtlasPPS').controller('customerTransactionListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.customerTransactionList = [];

            var getCustomerTransactionList = function () {
                authService.loadingOn();
                var promise = customerService.getCustomerTransactionList();
                promise.then(function (response) {
                    $scope.customerTransactionList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customerTransactionList = [];
                    authService.loadingOff();
                });
            };

            var defaultSort = {
                SortDirection: "DESC",
                SortColumn: "TransactionDate"
            };

            $scope.paginationOptions = {
                PageIndex: 1,
                PageSize: 25,
                SortDirection: defaultSort.SortDirection,
                SortColumn: defaultSort.SortColumn
            };

            $scope.gridOptions = {
                paginationPageSizes: [25, 50, 75, 100],
                paginationPageSize: 25,
                rowHeight: 30,
                enableRowSelection: true,
                expandableRowTemplate: '/views/template/expandableRowTemplate.html',
                expandableRowScope: {
                    
                    //,
                    //navigateToInvoiceView1: function (d) {
                    //    $window.open(reportSettings.reportBaseUri + 'Invoice/View/' + d.Id, '_blank');
                    //}
                },
                useExternalPagination: true,
                useExternalSorting: true,
                enableColumnResize: true,
                columnDefs: [
                    { name: 'Id', displayName: 'Tx No.', enableSorting: false, width: '8%', cellClass: 'text-right' },
                    { name: 'AccountName', displayName: 'Account Head', enableSorting: false, width: '24%', headerCellClass: 'text-left', cellClass: 'text-left' },
                    { name: 'TransactionDate', displayName: 'Tx Date', enableSorting: false, width: '8%', headerCellClass: 'text-center', cellClass: 'text-center', cellFilter: 'date:\'dd/MM/yyyy\'' },
                    { name: 'TransactionAmount', displayName: 'Tx Amount', enableSorting: false, width: '10%', headerCellClass: 'text-right', cellClass: 'text-right', cellFilter: 'currency:\'\'' },
                    {
                        name: 'BankChargeAmount', displayName: 'BC', enableSorting: false, width: '6%',
                        headerCellClass: 'text-right', cellClass: 'text-right',
                        cellTeamplate: 'ng-if="row.entity.BankChargeAmount>0"'
                    },
                    { name: 'CreatedByName', displayName: 'Created By', enableSorting: false, width: '15%', cellClass: 'text-left' },
                    { name: 'CreatedOn', displayName: 'Created On', enableSorting: false, width: '10%', headerCellClass: 'text-center', cellClass: 'text-center', cellFilter: 'date:\'dd/MM/yyyy\'' },
                    {
                        name: 'Status', field: 'Status', displayName: 'Status',
                        enableSorting: false, width: '10%', headerCellClass: 'text-center', cellClass: 'text-center',
                        cellTemplate: '<span class="label label-warning" style="text-align: center; margin-right: 10px;" ng-if="row.entity.Status==\'Pending\'">{{ row.entity.Status }}</span>' +
                            '<span class="label label-info" style="text-align: center; margin-right: 10px;" ng-if="row.entity.Status==\'Approved\'">{{ row.entity.Status }}</span>'
                    },
                    {
                        name: 'buttons', field: 'buttons', displayName: '', cellClass: 'text-left', width: '20%',
                        cellTemplate: '<div class="ui-grid-cell-contents">' +
                            '<button type="button" class="btn btn-primary btn-sm" title="Edit" style="padding-right=15px;" ng-if="grid.appScope.isAuthenticated(\'424\')&&row.entity.Status==\'Pending\'" ng-click="grid.appScope.navigateToCustomerTransactionEdit(row.entity)">Edit</button>' +
                            '<button type="button" class="btn btn-primary btn-sm" title="View" ng-if="grid.appScope.isAuthenticated(\'423\')" ng-click="grid.appScope.navigateToCustomerTransactionView(row.entity)">View</button></div>'
                    }
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

                        if (sortColumns.length == 0) {
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
                customerService.getCustomerTransactionListForFiltering($scope.paginationOptions)
                    .then(function (response) {
                        var data = response;

                        for (i = 0; i < data.length; i++) {
                            data[i].subGridOptions = {
                                columnDefs: [
                                    { name: 'CustomerName', field: 'CustomerName', displayName: 'Customer Name', enableSorting: false, width: '30%', headerCellClass: 'text-left', cellClass: 'text-left' },
                                    { name: 'CustomerCode', field: 'CustomerCode', displayName: 'Customer Code', enableSorting: false, width: '15%', headerCellClass: 'text-right', cellClass: 'text-right' },
                                    { name: 'TransactionAmount', field: 'TransactionAmount', displayName: 'Tx Amount', enableSorting: false, width: '15%', headerCellClass: 'text-right', cellClass: 'text-right', cellFilter: 'currency:\'\'' },
                                    { name: 'BookNo', field: 'BookNo', displayName: 'Book No', enableSorting: false, width: '10%', headerCellClass: 'text-right', cellClass: 'text-right' },
                                    { name: 'BookSerialNo', field: 'BookSerialNo', displayName: 'Book SL No', enableSorting: false, width: '10%', headerCellClass: 'text-right', cellClass: 'text-right' }
                                ],
                                data: data[i].CustomerTransactionDetail
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
                //getCustomerTransactionList();
            };
            pageLoad();
            $scope.addCustomerTransaction = function () {
                $location.path("/Sales/CustomerTransaction/Create");
            };
            $scope.navigateToCustomerTransactionEdit = function (c) {
                $location.path("/Sales/CustomerTransaction/Edit/" + c.Id);
            };
            $scope.navigateToCustomerTransactionView = function (c) {
                $location.path("/Sales/CustomerTransaction/View/" + c.Id);
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

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withOption('order', [])
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