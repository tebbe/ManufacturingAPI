//'use strict';

angular.module('AtlasPPS').controller('finishedGoodListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, storeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.finishedProductList = [];

            var getFinishedGood = function () {
                authService.loadingOn();
                var promise = storeService.getFinishedGood();
                promise.then(function (response) {
                    $scope.finishedProductList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.finishedProductList = [];
                    authService.loadingOff();
                });
            };

            var defaultSort = {
                SortDirection: "DESC",
                SortColumn: "ProductionGroupId"
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
                rowHeight: 40,
                enableRowSelection: true,
                expandableRowTemplate: '/views/template/expandableRowTemplate.html',
                //expandableRowScope: {
                //    subGridVariable: 'subGridScopeVariable'
                //},
                useExternalPagination: true,
                useExternalSorting: true,
                enableColumnResize: true,
                columnDefs: [
                    { name: 'ProductionGroupIdName', displayName: 'Production Group No.', enableSorting: false, width: '20%', cellClass: 'text-right' },
                    { name: 'CreatedByName', displayName: 'Created By', enableSorting: false, width: '30%', cellClass: 'text-left' },
                    { name: 'CreatedOn', displayName: 'Created Date', enableSorting: false, width: '20%', headerCellClass: 'text-center', cellClass: 'text-center', cellFilter: 'date:\'dd/MM/yyyy\'' },
                    { name: 'FPStatusName', field: 'FPStatusName', displayName: 'Status', enableSorting: false, width: '10%', headerCellClass: 'text-left', cellClass: 'text-left' },
                    {
                        name: 'buttons', field: 'buttons', displayName: '', cellClass: 'text-left', width: '20%',
                        cellTemplate: '<div class="ui-grid-cell-contents">' +
                            '<button type="button" class="btn btn-primary btn-sm" title="View" ng-if="row.entity.FPStatusName!=\'Approved\'" ng-click="grid.appScope.showApproveFgModal(row.entity)">Approve</button></div>'
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
                ////var url;
                //switch ($scope.paginationOptions.SortDirection) {
                //    case uiGridConstants.ASC:
                //        break;
                //    case uiGridConstants.DESC:
                //        break;
                //    default:
                //        break;
                //}

                storeService.getFinishedGoodForFiltering($scope.paginationOptions)
                    .then(function (response) {
                        var data = response;

                        for (i = 0; i < data.length; i++) {
                            data[i].subGridOptions = {
                                columnDefs: [
                                    { name: 'ProductionDate', field:'ProductionDate', displayName: 'Production Date', enableSorting: false, width: '20%', headerCellClass: 'text-center', cellClass: 'text-center', cellFilter: 'date:\'dd/MM/yyyy\'' },
                                    { name: 'ProductName', field:'ProductName', displayName: 'Product Name', enableSorting: false, width: '40%', headerCellClass: 'text-center', cellClass: 'text-left' },
                                    { name: 'Quantity', field: 'Quantity', displayName: 'Quantity', enableSorting: false, width: '20%', headerCellClass: 'text-center', cellClass: 'text-right' },
                                    //{ name: 'FPStatusName', field: 'FPStatusName', displayName: 'Status', enableSorting: false, width: '20%', headerCellClass: 'text-left', cellClass: 'text-left'},
                                    //{
                                    //    name: 'buttons', field: 'buttons', displayName: '', cellClass: 'text-right', width: '10%',
                                    //    cellTemplate: '<div class="ui-grid-cell-contents">' +
                                    //        '<button type="button" class="btn btn-primary btn-sm" title="View" ng-if="row.entity.FPStatusName!=\'Approved\'" ng-click="grid.appScope.showApproveFgModal(row.entity)">Approve</button></div>'
                                    //}
                                ],
                                data: data[i].FinishedGoodSubList
                            };
                        }

                        $scope.gridOptions.totalItems = response[0].TotalCount;
                        //var firstRow = ($scope.myPagination.PageIndex - 1) * $scope.myPagination.PageSize;
                        //$scope.gridOptions.data = data.slice(firstRow, firstRow + $scope.myPagination.PageSize);
                        $scope.gridOptions.data = data;
                    });
            };

            getPage();

            //$scope.expandAllRows = function () {
            //    $scope.gridApi.expandable.expandAllRows();
            //};

            //$scope.collapseAllRows = function () {
            //    $scope.gridApi.expandable.collapseAllRows();
            //};

            //$scope.toggleExpandAllBtn = function () {
            //    $scope.gridOptions.showExpandAllButton = !$scope.gridOptions.showExpandAllButton;
            //};


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
                //getFinishedGood();
            };
            pageLoad();

            $scope.addNewFinishedGood = function () {
                $location.path("/Store/FinishedGood/Create");
            }

            $scope.navigateToFpView = function (fp) {
                $location.path("/Store/FinishedGood/View/" + fp.Id);
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

            //Approve Finished Good
            $scope.showApproveFgModal = function (fgModel) {
                $scope.selectedFp = fgModel;
                $('#approveFgModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.approveFgClick = function () {
                var pgId = $scope.selectedFp.ProductionGroupId;
                authService.loadingOn();
                var promise = storeService.approveFinishedGood(pgId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $window.location.reload();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeApproveFgModal = function () {
                $('#approveFgModal').modal('toggle');
            };
        }]);