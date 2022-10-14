//'use strict';

angular.module('AtlasPPS').controller('purchaseOrderEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'purchaseOrderService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state', '$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, purchaseOrderService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $scope.modelHeading = "Update Purchase order";
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.purchaseOrder = null;
            $scope.suppliers = null;
            $scope.PurchaseOrderDetail = [];
            $scope.rawMaterialType = [];

            $scope.selectedPurchaseOrder = {
                Id: null,
                PurchaseOrderNo: null,
                PurchaseOrderDate: new Date(),
                SupplierId: null,
                Note: null,
                PaymentType: null,
                EstimatedDeliveryDate: new Date(),
                PriceValidity: null,
                PurchaseOrderDetail: [],
                TotalAmount: null
            };
            $scope.isPurchaseOrderEditValidated = false;
            var poId;

            var getPurchaseOrderById = function (poId) {
                var promise = purchaseOrderService.getPurchaseOrderById(poId);
                promise.then(function (response) {
                    $scope.purchaseOrder = response;
                    $scope.purchaseOrder.PurchaseOrderDate = new Date($scope.purchaseOrder.PurchaseOrderDate);
                    $scope.purchaseOrder.EstimatedDeliveryDate = new Date($scope.purchaseOrder.EstimatedDeliveryDate);
                    finalAmountCalculation();
                    $scope.selectedSupplier = _.filter($scope.suppliers,
                        function (item) {
                            return item.Id === $scope.purchaseOrder.SupplierId;
                        })[0];
                }, function (err) {
                    $scope.purchaseOrder = {};
                });
            };

            var getSupplierList = function () {
                var promise = purchaseOrderService.getSupplierList();
                promise.then(function (response) {
                    $scope.suppliers = response;
                }, function (err) {
                    $scope.suppliers = [];
                });
            };
            var getRawMaterialType = function () {
                var promise = purchaseOrderService.getRawMaterialType();
                promise.then(function (response) {
                    $scope.rawMaterialType = response;
                }, function (err) {
                    $scope.rawMaterialType = [];
                });
            };

            var validateRwtEntry = function (PurchaseOrderDetail) {
                if (!$scope.selectedRawMaterialType.RawMaterialTypeName || (PurchaseOrderDetail.Quantity === 0) || (PurchaseOrderDetail.Price === 0)) {
                    return false;
                }
                return true;
            };

            var finalAmountCalculation = function () {
                var tUnitAmount = 0;
                if ($scope.purchaseOrder.PurchaseOrderDetail) {
                    for (var prd in $scope.purchaseOrder.PurchaseOrderDetail) {
                        tUnitAmount += $scope.purchaseOrder.PurchaseOrderDetail[prd].TotalUnitPrice;
                    }
                }
                $scope.purchaseOrder.TotalAmount = tUnitAmount;
            };

            $scope.addRawMaterialItem = function () {
                var rDetail = {};

                if (!$scope.purchaseOrder.Quantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                rDetail.Quantity = $scope.purchaseOrder.Quantity;

                if (!$scope.purchaseOrder.Price) {
                    rDetail.Price = 0;
                    return;
                }
                rDetail.Price = $scope.purchaseOrder.Price;

                if ($scope.selectedRawMaterialType) {
                    rDetail.RawMaterialTypeId = $scope.selectedRawMaterialType.Id;
                    rDetail.RawMaterialTypeName = $scope.selectedRawMaterialType.RawMaterialTypeName;
                    rDetail.UnitTypeName = $scope.selectedRawMaterialType.UnitTypeName;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedRawMaterialType && $scope.selectedRawMaterialType.RawMaterialTypeName && $scope.selectedRawMaterialType.RawMaterialTypeName.length > 0) {
                    var result = $scope.purchaseOrder.PurchaseOrderDetail.filter(function (v) {
                        return v.RawMaterialTypeId === rDetail.RawMaterialTypeId;
                    });
                    if (selectedModelDetailIndex === -1 && result.length > 0) {
                        notificationService.showErrorNotificatoin(PpsConstant.DuplicateProductName);
                        return;
                    }
                }

                if (!validateRwtEntry(rDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                rDetail.TotalUnitPrice = rDetail.Quantity * rDetail.Price;

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.purchaseOrder.PurchaseOrderDetail.push(rDetail);
                } else {
                    $scope.purchaseOrder.PurchaseOrderDetail[selectedModelDetailIndex] = rDetail;
                }
                $scope.isPurchaseOrderEditValidated = true;
                finalAmountCalculation();
                hasTransaction = true;
                $scope.selectedRawMaterialType = null;
                $scope.purchaseOrder.Quantity = null;
                $scope.purchaseOrder.Price = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };

            $scope.removeItemDetail = function (rm) {
                if ($scope.purchaseOrder.PurchaseOrderDetail) {
                    for (var i = 0; i < $scope.purchaseOrder.PurchaseOrderDetail.length; i++) {
                        if ($scope.purchaseOrder.PurchaseOrderDetail[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
                            $scope.purchaseOrder.PurchaseOrderDetail.splice(i, 1);
                            if ($scope.purchaseOrder.PurchaseOrderDetail.length === 0) {
                                $scope.isPurchaseOrderEditValidated = false;
                            }
                            break;
                        }
                    }
                    // Total amount calculation 
                    finalAmountCalculation();
                    $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };

            $scope.selectItemDetail = function (rm) {
                if ($scope.purchaseOrder.PurchaseOrderDetail) {
                    for (var i = 0; i < $scope.purchaseOrder.PurchaseOrderDetail.length; i++) {
                        if ($scope.purchaseOrder.PurchaseOrderDetail[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
                            selectedModelDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }
                $scope.isPurchaseOrderEditValidated = false;
                $scope.modelDetailText = "Update";
                $scope.modelDetailMode = PpsConstant.TranMode.Update;

                var result = $scope.rawMaterialType.filter(function (v) {
                    return v.Id === rm.RawMaterialTypeId;
                });

                if (result && result.length > 0) {
                    $scope.selectedRawMaterialType = result[0];
                }

                if (rm.Quantity > 0) {
                    $scope.Quantity = rm.Quantity;
                }
                if (rm.Price > 0) {
                    $scope.Price = rm.Price;
                }
            };

            $scope.cancelModelUpdate = function () {
                $scope.isPurchaseOrderEditValidated = true;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedProduct = null;
                $scope.purchaseOrder.Quantity = null;
                $scope.purchaseOrder.Price = null;
            };

            var validate = function () {
                if (!$scope.selectedPurchaseOrder.PurchaseOrderDate
                    || (!$scope.selectedSupplier)
                    || (!$scope.Note)
                    || (!$scope.PaymentType)
                    || (!$scope.EstimatedDeliveryDate)
                    || (!$scope.PriceValidity)
                    || (!$scope.purchaseOrder.PurchaseOrderDetail)) {
                    return false;
                }
                return true;
            };

            $scope.updatePurchaseOrder = function () {
                if (validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedPurchaseOrder.Id = poId;
                $scope.selectedPurchaseOrder.PurchaseOrderDate =
                    moment($scope.purchaseOrder.PurchaseOrderDate).format("MM-DD-YYYY");
                $scope.selectedPurchaseOrder.SupplierId = $scope.selectedSupplier.Id;
                $scope.selectedPurchaseOrder.Note = $scope.purchaseOrder.Note;
                $scope.selectedPurchaseOrder.PaymentType = $scope.purchaseOrder.PaymentType;
                $scope.selectedPurchaseOrder.EstimatedDeliveryDate =
                    moment($scope.purchaseOrder.EstimatedDeliveryDate).format("MM-DD-YYYY");
                $scope.selectedPurchaseOrder.PriceValidity = $scope.purchaseOrder.PriceValidity;
                $scope.selectedPurchaseOrder.PurchaseOrderDetail = $scope.purchaseOrder.PurchaseOrderDetail;

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = purchaseOrderService.updatePurchaseOrder($scope.selectedPurchaseOrder);
                
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;

                    $location.path("/Purchase/PurchaseOrder/View/" + response.Id);
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
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

                if ($state.params && $state.params.poId) {
                    poId = _.parseInt($state.params.poId);
                }
                authService.loadingOn();
                $q.all([
                    getSupplierList(), getRawMaterialType()
                ]).then(function () {
                    authService.loadingOff();
                    getPurchaseOrderById(poId);
                });
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