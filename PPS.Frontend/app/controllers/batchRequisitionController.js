//'use strict';

angular.module('AtlasPPS').controller('batchRequisitionController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'salesService','$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, storeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, salesService, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $scope.isNew = false;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.tranMode = 1;


            $scope.modelHeading = "Add Batch Requisition";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelProductDetailActionText = "Save";
            $scope.modelProductDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            $scope.modelProductDetailMode = PpsConstant.TranMode.AddNew;

            var selectedModelDetailIndex = -1;
            var selectedModelProductDetailIndex = -1;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.rmDetail = [];
            $scope.rawMaterialType = [];
            $scope.isBatchRequisitionValidated = false;
            $scope.productList = [];
            $scope.productDetail = [];
            $scope.isBRProductionEstimationValidated = false;
            var batchRequisitionId = null;

            $scope.selectedBatchRequisition = {
                BatchRequisitionDetail: [],
                BatchRequisitionProductionEstimation: []
            };
            $scope.selectedRawMaterialType = {
                selected: null
            }
            $scope.selectedProductionGroup = {
                selected: null
            }
            $scope.selectedProduct = {
                selected: null
            }

            var getRawMaterialType = function () {
                authService.loadingOn();
                var promise = storeService.getRawMaterialType();
                promise.then(function (response) {
                    $scope.rawMaterialType = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.rawMaterialType = [];
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

            var validateBrEntry;

            $scope.addRawMaterialItem = function () {
                var rDetail = {};

                if (!$scope.Quantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                rDetail.Quantity = $scope.Quantity;

                if ($scope.selectedRawMaterialType.selected) {
                    rDetail.RawMaterialTypeId = $scope.selectedRawMaterialType.selected.Id;
                    rDetail.RawMaterialTypeName = $scope.selectedRawMaterialType.selected.RawMaterialTypeName;
                    rDetail.UnitTypeName = $scope.selectedRawMaterialType.selected.UnitTypeName;
                    rDetail.AvailableQty = $scope.selectedRawMaterialType.selected.AvailableQty;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedRawMaterialType.selected.RawMaterialTypeName && $scope.selectedRawMaterialType.selected.RawMaterialTypeName.length > 0) {
                    var result = $scope.rmDetail.filter(function (v) {
                        return v.RawMaterialTypeId === rDetail.RawMaterialTypeId;
                    });
                    if (selectedModelDetailIndex === -1 && result.length > 0) {
                        notificationService.showErrorNotificatoin(PpsConstant.DuplicateProductName);
                        return;
                    }
                }

                if (!validateBrEntry(rDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.rmDetail.push(rDetail);
                } else {
                    $scope.rmDetail[selectedModelDetailIndex] = rDetail;
                }

                batchRequisitionValidationChecker();

                hasTransaction = true;
                $scope.selectedRawMaterialType.selected = null;
                $scope.Quantity = null;

                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };

            validateBrEntry = function (rmDetail) {
                if (!$scope.selectedRawMaterialType.selected.RawMaterialTypeName || (rmDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };


            $scope.removeItemDetail = function (rm) {
                if ($scope.rmDetail) {
                    for (var i = 0; i < $scope.rmDetail.length; i++) {
                        if ($scope.rmDetail[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
                            $scope.rmDetail.splice(i, 1);
                            if ($scope.rmDetail.length === 0) {
                                $scope.isBatchRequisitionValidated = false;
                            }
                            break;
                        }
                    }
                    $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };

            $scope.selectItemDetail = function (rm) {
                $scope.isBatchRequisitionValidated = false;
                if ($scope.rmDetail) {
                    for (var i = 0; i < $scope.rmDetail.length; i++) {
                        if ($scope.rmDetail[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
                            selectedModelDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }

                $scope.modelDetailText = "Update";
                $scope.modelDetailMode = PpsConstant.TranMode.Update;

                var result = $scope.rawMaterialType.filter(function (v) {
                    return v.Id === rm.RawMaterialTypeId;
                });

                if (result && result.length > 0) {
                    $scope.selectedRawMaterialType.selected = result[0];
                }

                if (rm.Quantity > 0) {
                    $scope.Quantity = rm.Quantity;
                }
            };

            $scope.cancelModelUpdate = function () {
                batchRequisitionValidationChecker();
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedRawMaterialType.selected = null;
                $scope.Quantity = null;
            };

            $scope.saveBatchRequisition = function () {
                if (!$scope.selectedProductionGroup.selected) {
                    return;
                }

                $scope.selectedBatchRequisition.ProductionGroupId = $scope.selectedProductionGroup.selected.Id;
                $scope.selectedBatchRequisition.BatchRequisitionDetail = $scope.rmDetail;
                $scope.selectedBatchRequisition.BatchRequisitionProductionEstimation = $scope.productDetail;

                $scope.processComplated = false;

                authService.loadingOn();

                var promise = storeService.saveBatchRequisition($scope.selectedBatchRequisition);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $location.path("/Store/BatchRequisition/View/" + response.Id);
                    batchRequisitionId = response.Id;
                    $scope.selectedBatchRequisition = {
                        BatchRequisitionDetail: [],
                        BatchRequisitionProductionEstimation: []
                    };
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            $scope.addProductItem = function () {
                var prdDetail = {};
                prdDetail.BatchRequisitionId = batchRequisitionId;

                if (!$scope.ProductQuantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                prdDetail.Quantity = $scope.ProductQuantity;

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
                    if (selectedModelProductDetailIndex === -1 && result.length > 0) {
                        notificationService.showErrorNotificatoin(PpsConstant.DuplicateProductName);
                        return;
                    }
                }

                if (!validateBRProductionEstimationEntry(prdDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.modelProductDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.productDetail.push(prdDetail);
                } else {
                    $scope.productDetail[selectedModelProductDetailIndex] = prdDetail;
                }
                batchRequisitionValidationChecker();
                hasTransaction = true;

                $scope.selectedProduct.selected = null;
                $scope.ProductQuantity = null;
                $scope.modelProductDetailText = "Add";
                $scope.modelProductDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelProductDetailIndex = -1;
            };

            validateBRProductionEstimationEntry = function (prdDetail) {
                if (!$scope.selectedProduct.selected.Name || (prdDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };

            $scope.removeProductItemDetail = function (pd) {
                if ($scope.productDetail) {
                    for (var i = 0; i < $scope.productDetail.length; i++) {
                        if ($scope.productDetail[i].ProductId === pd.ProductId) {
                            $scope.productDetail.splice(i, 1);
                            if ($scope.productDetail.length === 0) {
                                $scope.isBatchRequisitionValidated = false;
                            }
                            break;
                        }
                    }
                    $scope.modelProductDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };


            $scope.selectProductItemDetail = function (pd) {
                if ($scope.productDetail) {
                    for (var i = 0; i < $scope.productDetail.length; i++) {
                        if ($scope.productDetail[i].ProductId === pd.ProductId) {
                            selectedModelProductDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }

                $scope.isBatchRequisitionValidated = false;
                $scope.modelProductDetailText = "Update";
                $scope.modelProductDetailMode = PpsConstant.TranMode.Update;

                var result = $scope.productList.filter(function (v) {
                    return v.Id === pd.ProductId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct.selected = result[0];
                }

                if (pd.Quantity > 0) {
                    $scope.ProductQuantity = pd.Quantity;
                }
            };

            $scope.cancelProductDetailModelUpdate = function () {
                $scope.isBatchRequisitionValidated = true;
                $scope.modelProductDetailText = "Add";
                $scope.modelProductDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelProductDetailIndex = -1;
                $scope.selectedProduct.selected = null;
                $scope.ProductQuantity = null;
            };

            var batchRequisitionValidationChecker = function () {
                if ($scope.productDetail.length > 0 && $scope.rmDetail.length > 0) {
                    $scope.isBatchRequisitionValidated = true;
                } else {
                    $scope.isBatchRequisitionValidated = false;
                }
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

                var promiseProductionGroupList = storeService.getProductionGroupListFromFloorStore();
                promiseProductionGroupList.then(function (response) {
                    $scope.productionGroupList = response;
                }, function (err) {
                    $scope.productionGroupList = [];
                });

                var promiseProductList = salesService.GetProductList();
                promiseProductList.then(function (response) {
                    $scope.productList = response;
                }, function (err) {
                    $scope.productList = [];
                });

                var promiseRawMaterialType = storeService.getRawMaterialType();
                promiseRawMaterialType.then(function (response) {
                    $scope.rawMaterialType = response;
                }, function (err) {
                    $scope.rawMaterialType = [];
                });

                authService.loadingOn();
                $q.all([
                    promiseProductionGroupList,
                    promiseProductList,
                    promiseRawMaterialType]).then(function () {
                        authService.loadingOff();
                    });

                $scope.selectedBatchRequisition.BatchRequisitionDate = new Date();
                $scope.isNew = true;
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