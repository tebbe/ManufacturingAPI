'use strict';

angular.module('AtlasPPS').controller('productAddController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'productService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window','$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, productService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $q) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            
            

            $scope.modelHeading = "Add Product";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.ProductStandardTypeList = [];
            $scope.UnitTypeList = [];
            $scope.ProductTypeList = [];
            $scope.ProductTypeGroupList = [];

            $scope.Name = null;
            $scope.Code = null;
            $scope.Color = null;
            $scope.ProductStandardTypeId = null;
            $scope.Thickness = null;
            $scope.Length = null;
            $scope.UnitTypeId = null;
            $scope.UnitPrice = null;
            $scope.ProductTypeId = null;
            $scope.AccountHeadId = null;
            $scope.FiscalYear = null;

            $scope.selectedProductStandardType= {
                selected: null
            }
            $scope.selectedUnitType = {
                selected: null
            }
            $scope.selectedProductType = {
                selected: null
            }
            $scope.selectedProductTypeGroup = {
                selected: null
            }
           
            $scope.product = {
                Name: null,
                Code: null,
                Color: null,
                ProductStandardTypeId: null,
                Thickness: null,
                Length: null,
                UnitTypeId: null,
                UnitPrice: null,
                ProductTypeId: null,
                ProductTypeGroupId: null,
                AccountHeadId: null,
                FiscalYear:null
            };

            $scope.onChangeProductStandardType = function (selectedProductStandardType) {
                if (selectedProductStandardType && selectedProductStandardType.Id) {
                    var result = $scope.ProductStandardTypeList.filter(function (v) {
                        return v.Id === selectedProductStandardType.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedProductStandardType.selected = result[0];
                    }
                    else {
                        $scope.selectedProductStandardType.selected = null;
                    }
                } else {
                    $scope.selectedProductStandardType.selected = null;
                }
            }
            $scope.onChangeUnitType = function (selectedUnitType) {
                if (selectedUnitType && selectedUnitType.Id) {
                    var result = $scope.UnitTypeList.filter(function (v) {
                        return v.Id === selectedUnitType.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedUnitType.selected = result[0];
                    }
                    else {
                        $scope.selectedUnitType.selected = null;
                    }
                } else {
                    $scope.selectedUnitType.selected = null;
                }
            }
            $scope.onChangeProductType = function (selectedProductType) {
                if (selectedProductType && selectedProductType.Id) {
                    var result = $scope.ProductTypeList.filter(function (v) {
                        return v.Id === selectedProductType.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedProductType.selected = result[0];
                    }
                    else {
                        $scope.selectedProductType.selected = null;
                    }
                } else {
                    $scope.selectedProductType.selected = null;
                }
            }
            $scope.onChangeProductTypeGroup = function (selectedProductTypeGroup) {
                if (selectedProductTypeGroup && selectedProductTypeGroup.Id) {
                    var result = $scope.ProductTypeListGroup.filter(function (v) {
                        return v.Id === selectedProductTypeGroup.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedProductTypeGroup.selected = result[0];
                    }
                    else {
                        $scope.selectedProductTypeGroup.selected = null;
                    }
                } else {
                    $scope.selectedProductTypeGroup.selected = null;
                }
            }
           

            var validate = function () {
                if ($scope.selectedProductStandardType.selected.Id != null
                    && $scope.selectedUnitType.selected.Id != null
                    && $scope.selectedProductType.selected.Id != null
                    && $scope.selectedProductTypeGroup.Id != null) {
                    return false;
                }
                return true;
            };

            $scope.saveProduct = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.product.Name = $scope.Name;
                $scope.product.Code = $scope.Code;
                $scope.product.Color = $scope.Color;
                $scope.product.ProductStandardTypeId = $scope.selectedProductStandardType.selected.Id;
                $scope.product.Thickness = $scope.Thickness;
                $scope.product.Length = $scope.length;
                $scope.product.UnitTypeId = $scope.selectedUnitType.selected.Id;
                $scope.product.UnitPrice = $scope.UnitPrice;
                $scope.product.ProductTypeId = $scope.selectedProductType.selected.Id;
                $scope.product.ProductTypeGroupId = $scope.selectedProductTypeGroup.selected.Id;
                $scope.product.FiscalYear = fiscalYear; 

                authService.loadingOn();
                var promise = productService.saveProduct($scope.product);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            authService.loadingOn();
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
                //get data for dropdown list
                var getAllDropdownList = productService.getProductRelatedAllDropdownList();
                getAllDropdownList.then(function (response) {
                    $scope.ProductStandardTypeList = response.StandardType;
                    $scope.UnitTypeList = response.UnitType;
                    $scope.ProductTypeList = response.ProductType; 
                    $scope.ProductTypeGroupList = response.ProductTypeGroup;
                }, function (err) {
                    $scope.ProductStandardTypeList = [];
                    $scope.UnitTypeList = [];
                    $scope.ProductTypeList = [];
                    $scope.ProductTypeGroupList = [];
                    authService.loadingOff();
                    });
               
                $q.all([
                    getAllDropdownList]).then(function () {
                        authService.loadingOff();
                    });
            };
            pageLoad();
            $scope.gotoProductList = function () {
                $location.path("/admin/productList")
                $window.location.reload();
            }

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