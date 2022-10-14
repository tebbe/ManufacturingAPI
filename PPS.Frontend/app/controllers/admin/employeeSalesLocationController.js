'use strict';
angular.module('AtlasPPS').controller('employeeSalesLocationController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'employeeSalesLocationService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, employeeSalesLocationService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            var companyId = null;
            var optPromise = null;
            var areaId = null;
            var baseId = null;
            $scope.btnCheckId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.Id= null,
            $scope.SalesDivisionId=null;
            $scope.SalesAreaName= "";
            $scope.SalesAreaId= null;
            $scope.SalesBaseName= "";

            $scope.salesDivision = [];
            $scope.salesArea = [];
            $scope.getAreaById = [];
            $scope.getBaseById = [];
            $scope.salesBase = [];
            $scope.salesAreaList = [];
            $scope.salesBaseList = [];

            $scope.SalesAreaModel = {
                Id: null,
                SalesDivisionId: "",
                SalesAreaName: ""
            };
            $scope.SalesBaseModel = {
                Id: null,
                SalesAreaId: "",
                SalesBaseName: ""
            };


            $scope.selectedSalesDivision = {
                selected: null
            }
            $scope.selectedSalesArea = {
                selected: null
            }
            var fillAreaFunction = function () {
                $scope.selectedSalesDivision.selected = _.filter($scope.divisionList,
                    function (item) {
                        return item.Id === $scope.getAreaById[0].SalesDivisionId;
                    })[0];
               
            }
            var fillBaseFunction = function () {
                $scope.selectedSalesArea.selected = _.filter($scope.salesAreaList,
                    function (item) {
                        return item.Id === $scope.getBaseById[0].SalesAreaId;
                    })[0];

            }
            $scope.onChangeSalesDivision = function (selectedSalesDivision) {
                if (selectedSalesDivision && selectedSalesDivision.Id) {
                    var result = $scope.divisionList.filter(function (v) {
                        return v.Id === selectedSalesDivision.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesDivision.selected = result[0];

                        var area = employeeSalesLocationService.getEmployeeAreaList(result[0].Id);
                        area.then(function (response) {
                            $scope.salesArea = response;
                            $scope.selectedSalesArea.selected = null;
                        }, function (err) {
                            $scope.salesArea = [];
                        });

                    } else {
                        $scope.selectedSalesDivision.selected = null;
                    }
                } else {
                    $scope.selectedSalesDivision.selected = null;
                }
            }
            $scope.onChangesalesArea = function (selectedSalesArea) {
                if (selectedSalesArea && selectedSalesArea.Id) {
                    var result = $scope.salesArea.filter(function (v) {
                        return v.Id === selectedSalesArea.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesArea.selected = result[0];
                    }
                    else {
                        $scope.selectedSalesArea.selected = null;
                    }

                } else {
                    $scope.selectedSalesArea.selected = null;
                }
            }
            //popup model on click open
            $scope.addSalesAreaModel = function () {
                $scope.btnCheckId = null;
                $('#addSalesArea').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.addSalesBase = function () {
                $scope.btnCheckId = null;
                $('#addSalesBase').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            //popup model on click close
            //popup Edit model on click open
            $scope.navigateToSalesAreaEdit = function (area) {
                areaId = area.Id;
                $scope.btnCheckId = area.Id;
                getSaleArea(areaId);
                $('#addSalesArea').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.navigateToSalesBaseEdit = function (base) {    
                baseId = base.Id;
                $scope.btnCheckId = base.Id;
                getSaleBase(baseId);
                $('#addSalesBase').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            //popup Edit model on click close
            $scope.closeAddAreaModal = function () {
                $scope.selectedSalesDivision.selected = null;
                $scope.SalesAreaName = null;
                $('#addSalesArea').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            };
            $scope.closeAddBaseModal = function () {
                $scope.selectedSalesDivision.selected = null;
                $scope.selectedSalesArea.selected = null;
                $scope.SalesBaseName = null;
                $('#addSalesBase').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            };
            var getSaleZone = function () {
                authService.loadingOn();
                var promise = employeeSalesLocationService.getEmployeeSalesLocation();
                promise.then(function (response) {
                    $scope.divisionList = response.div;
                    $scope.salesAreaList = response.area;
                    $scope.salesBaseList = response.sBase;
                    authService.loadingOff();
                }, function (err) {
                    $scope.divisionList = [];
                    $scope.salesAreaList = [];
                    $scope.salesBaseList = [];
                    authService.loadingOff();
                });
            }
            var getSaleArea = function (areaId) {
                authService.loadingOn();
                var promise = employeeSalesLocationService.getSalesAreaById(areaId);
                promise.then(function (response) {
                    $scope.getAreaById = response;
                    $scope.Id = $scope.getAreaById[0].Id;
                    $scope.SalesAreaName = $scope.getAreaById[0].SalesAreaName;
                    fillAreaFunction();
                    authService.loadingOff();
                }, function (err) {
                    $scope.getAreaById = [];
                    authService.loadingOff();
                });
            }
            var getSaleBase = function (baseId) {
                authService.loadingOn();
                var promise = employeeSalesLocationService.getSalesBaseById(baseId);
                promise.then(function (response) {
                    $scope.getBaseById = response;
                    $scope.Id = $scope.getBaseById[0].Id;
                    $scope.SalesBaseName = $scope.getBaseById[0].SalesBaseName;
                    fillBaseFunction();
                    authService.loadingOff();
                }, function (err) {
                    $scope.getBaseById = [];
                    authService.loadingOff();
                });
            }
            //save code for sales Area & Sales Base start
            $scope.SalesAreaClick = function (edit) {
                if ($scope.selectedSalesDivision.selected === null || $scope.SalesArea === null || $scope.SalesArea === "") {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                authService.loadingOn();            
                $scope.SalesAreaModel.SalesDivisionId = $scope.selectedSalesDivision.selected.Id;
                $scope.SalesAreaModel.SalesAreaName = $scope.SalesAreaName;
                if (edit === 1) {
                    $scope.SalesAreaModel.Id = areaId;
                    optPromise = employeeSalesLocationService.updateSalesArea($scope.SalesAreaModel);
                } else {
                    optPromise = employeeSalesLocationService.salesAreaAdd($scope.SalesAreaModel);
                }
                
                optPromise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $window.location.reload();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };
            $scope.SalesBaseClick = function (edit) {
                if ($scope.selectedSalesArea.selected === null || $scope.SalesBase === "" || $scope.SalesBase === null) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                authService.loadingOn();
                
                $scope.SalesBaseModel.SalesAreaId = $scope.selectedSalesArea.selected.Id;
                $scope.SalesBaseModel.SalesBaseName = $scope.SalesBaseName;
                if (edit === 1) {
                    $scope.SalesBaseModel.Id = baseId;
                    optPromise = employeeSalesLocationService.updateSalesBase($scope.SalesBaseModel);
                } else {
                    optPromise = employeeSalesLocationService.salesBaseAdd($scope.SalesBaseModel);
                }
                
                optPromise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };
            //save code for sales Area & Sales Base End



            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    companyId = authData.companyId;
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
                getSaleZone();
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


        }]);