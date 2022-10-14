//'use strict';

angular.module('AtlasPPS').controller('finishedGoodEntryController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'storeService', 'salesService','ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, storeService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelHeading = "Add Finished Good";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.productDetail = [];
            
            //$scope.batchRequisitionList = [];
            $scope.productionGroupList = [];
            $scope.productList = [];
            $scope.isFinishedGoodValidated = false;

            $scope.selectedProductionGroup = {
                selected: null
            }
            $scope.selectedProduct = {
                selected: null
            }

            //var getBatchRequisitionListFromFloorStore = function () {
            //    authService.loadingOn();
            //    var promise = storeService.getBatchRequisitionListFromFloorStore();
            //    promise.then(function (response) {
            //        $scope.batchRequisitionList = response;
            //        authService.loadingOff();
            //    }, function (err) {
            //        $scope.batchRequisitionList = [];
            //        authService.loadingOff();
            //    });
            //};

            var getProductionGroupListFromFloorStore = function () {
                authService.loadingOn();
                var promise = storeService.getProductionGroupListFromFloorStore();
                promise.then(function (response) {
                    $scope.productionGroupList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productionGroupList = [];
                    authService.loadingOff();
                });
            };
            var getProductList = function () {
                authService.loadingOn();
                var promise = salesService.GetProductList();
                promise.then(function (response) {
                    $scope.productList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productList = [];
                    authService.loadingOff();
                });
            };

            $scope.addProductItem = function () {
                var prdDetail = {};
                prdDetail.ProductionGroupId = $scope.selectedProductionGroup.selected.Id;
                prdDetail.ProductionGroupIdName = $scope.selectedProductionGroup.selected.ProductionGroupId;
                prdDetail.ProductionDate = $scope.ProductionDate;

                if (!$scope.Quantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                prdDetail.Quantity = $scope.Quantity;

                if ($scope.selectedProduct.selected) {
                    prdDetail.ProductId = $scope.selectedProduct.selected.Id;
                    prdDetail.Name = $scope.selectedProduct.selected.Name;
                    prdDetail.Code = $scope.selectedProduct.selected.Code;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedProduct.selected.Name && $scope.selectedProduct.selected.Name.length > 0) {
                    var result = $scope.productDetail.filter(function (v) {
                        return v.ProductId === prdDetail.ProductId;
                    });
                    if (selectedModelDetailIndex === -1 && result.length > 0) {
                        notificationService.showErrorNotificatoin(PpsConstant.DuplicateProductName);
                        return;
                    }
                }

                if (!validateFpEntry(prdDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.productDetail.push(prdDetail);
                } else {
                    $scope.productDetail[selectedModelDetailIndex] = prdDetail;
                }
                $scope.isFinishedGoodValidated = true;
                hasTransaction = true;

                $scope.selectedProduct.selected = null;
                $scope.Quantity = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };

            validateFpEntry = function (prdDetail) {
                if (!$scope.selectedProduct.selected.Name || (prdDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };

            $scope.removeItemDetail = function (pd) {
                if ($scope.productDetail) {
                    for (var i = 0; i < $scope.productDetail.length; i++) {
                        if ($scope.productDetail[i].ProductId === pd.ProductId) {
                            $scope.productDetail.splice(i, 1);
                            if ($scope.productDetail.length === 0) {
                                $scope.isFinishedGoodValidated = false;
                            }
                            break;
                        }
                    }
                    $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };


            $scope.selectItemDetail = function (pd) {
                if ($scope.productDetail) {
                    for (var i = 0; i < $scope.productDetail.length; i++) {
                        if ($scope.productDetail[i].ProductId === pd.ProductId) {
                            selectedModelDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }

                $scope.isFinishedGoodValidated = false;
                $scope.modelDetailText = "Update";
                $scope.modelDetailMode = PpsConstant.TranMode.Update;

                var result = $scope.productList.filter(function (v) {
                    return v.Id === pd.ProductId;
                });

                var resultProductionGroup = $scope.productionGroupList.filter(function (v) {
                    return v.Id === pd.ProductionGroupId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct.selected = result[0];
                }

                if (resultProductionGroup && resultProductionGroup.length > 0) {
                    $scope.selectedProductionGroup.selected = resultProductionGroup[0];
                }

                if (pd.Quantity > 0) {
                    $scope.Quantity = pd.Quantity;
                }
            };

            $scope.cancelModelUpdate = function () {
                $scope.isFinishedGoodValidated = true;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedProduct.selected = null;
                $scope.Quantity = null;
            };

            var validate = function () {
                if (!$scope.productDetail) {
                    return false;
                }
                return true;
            };

            var clearField;

            $scope.saveFinishedGood = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.processComplated = false;

                authService.loadingOn();
                var promise = storeService.saveFinishedGood($scope.productDetail, $scope.IsClosedProductionGroup);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $location.path("/Store/finishedGoodList");
                    //$window.location.reload();
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            clearField = function () {
                $scope.productDetail = null;
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
                //getBatchRequisitionListFromFloorStore();
                getProductionGroupListFromFloorStore();
                getProductList();
                $scope.ProductionDate = new Date();
                $scope.IsClosedProductionGroup = false;
                $scope.isFinishedGoodValidated = false;
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