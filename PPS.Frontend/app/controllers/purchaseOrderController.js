//'use strict';

angular.module('AtlasPPS').controller('purchaseOrderController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'purchaseOrderService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$q', 'bankService', '$filter',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, purchaseOrderService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $q, bankService, $filter) {
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

            $scope.modelHeading = "Add Purchase order";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.TotalAmount = null;
            $scope.totalDiscount = null;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.rmDetail = [];
            $scope.rawMaterialType = [];
            $scope.PurchaseOrderType = [];
            $scope.discountType = [];
            $scope.products = [];
            $scope.supplier = [];
            $scope.transactionsModel = [];
            $scope.selectedCustomer = null;

            $scope.selectedPurchaseOrder = {
                PurchaseOrderNo: null,
                PurchaseOrderDate: new Date(),
                IsCashPurchase: null,
                IsCreditPurchase: null,
                IsLcPurchase: null,
                Note: null,
                PaymentType: null,
                EstimatedDeliveryDate: new Date(),
                PriceValidity: null,
                TotalAmount: 0,
                SupplierId: null,
                SupplierAccountHeadId: null,
                SupplierAmount: 0,
                CashAccountHeadId: null,
                CashAmount: 0,
                BankAccountHeadId: null,
                BankAmount: 0,
                LCNo: null,
                LCAccountHeadId: null,
                LCAmount: 0,
                PurchaseOrderDetail: []
            };

            $scope.isPurchaseOrderValidated = false;
            $scope.selectedCashAccount = {
                selected: null
            }
            $scope.selectedBankAccount = {
                selected: null
            }
            $scope.selectedSupplier = {
                selected: null
            }
            $scope.selectedRawMaterialType = {
                selected: null
            }
            $scope.selectedLCAccount = {
                selected: null
            }

            $scope.isRequiredPOTypeCash = false;
            $scope.isRequiredPOTypeCredit = false;
            $scope.isRequiredPOTypeLC = false;

            $scope.onPurchaseOrderTypeChange = function (purchaseOrderType) {
                if ($scope.purchaseOrderType == 1) {
                    $scope.isRequiredPOTypeCash = true;
                }
                else {
                    $scope.isRequiredPOTypeCash = false;
                }
                if ($scope.purchaseOrderType == 2) {
                    $scope.isRequiredPOTypeCredit = true;
                }
                else {
                    $scope.isRequiredPOTypeCredit = false;
                }
                if ($scope.purchaseOrderType == 3) {
                    $scope.isRequiredPOTypeLC = true;
                }
                else {
                    $scope.isRequiredPOTypeLC = false;
                }
            }

            var finalAmountCalculation;
            var validateRwtEntry;
            $scope.addRawMaterialItem = function () {
                var rDetail = {};

                if (!$scope.Quantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                rDetail.Quantity = $scope.Quantity;

                if (!$scope.Price) {
                    rDetail.Price = 0;
                }
                rDetail.Price = $scope.Price;

                if ($scope.selectedRawMaterialType.selected) {
                    rDetail.RawMaterialTypeId = $scope.selectedRawMaterialType.selected.Id;
                    rDetail.RawMaterialTypeName = $scope.selectedRawMaterialType.selected.RawMaterialTypeName;
                    rDetail.UnitTypeName = $scope.selectedRawMaterialType.selected.UnitTypeName;
                    rDetail.AccountHeadId = $scope.selectedRawMaterialType.selected.AccountHeadId;
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

                if (!validateRwtEntry(rDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                //rDetail.totalUnitPrice = rDetail.Quantity * rDetail.Price;
                rDetail.totalUnitPrice = parseFloat((rDetail.Quantity * rDetail.Price).toFixed(2));

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.rmDetail.push(rDetail);
                } else {
                    $scope.rmDetail[selectedModelDetailIndex] = rDetail;
                }

                $scope.isPurchaseOrderValidated = true;

                finalAmountCalculation();

                hasTransaction = true;

                $scope.selectedRawMaterialType.selected = null;

                $scope.Quantity = null;
                $scope.Price = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };

            validateRwtEntry = function (rmDetail) {
                if (!$scope.selectedRawMaterialType.selected.RawMaterialTypeName || (rmDetail.Quantity === 0) || (rmDetail.Price === 0)) {
                    return false;
                }
                return true;
            };

            finalAmountCalculation = function () {
                var tunitAmount = 0;
                if ($scope.rmDetail) {
                    for (var prd in $scope.rmDetail) {
                        tunitAmount += $scope.rmDetail[prd].totalUnitPrice;
                    }
                }
                $scope.TotalAmount = Math.round(tunitAmount);
            };

            $scope.removeItemDetail = function (rm) {
                if ($scope.rmDetail) {
                    for (var i = 0; i < $scope.rmDetail.length; i++) {
                        if ($scope.rmDetail[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
                            $scope.rmDetail.splice(i, 1);
                            if ($scope.rmDetail.length === 0) {
                                $scope.isPurchaseOrderValidated = false;
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

                $scope.isPurchaseOrderValidated = false;
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
                if (rm.Price > 0) {
                    $scope.Price = rm.Price;
                }
            };

            $scope.cancelModelUpdate = function () {
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedRawMaterialType = null;
                $scope.Quantity = null;
                $scope.Price = null;
            };

            var validate;
            $scope.savePurchaseOrder = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                if ($scope.purchaseOrderType == 1) {
                    if ($scope.selectedSupplier.selected && $scope.SupplierAmount) {
                        $scope.selectedPurchaseOrder.SupplierId = $scope.selectedSupplier.selected.Id;
                        $scope.selectedPurchaseOrder.SupplierAccountHeadId = $scope.selectedSupplier.selected.AccountHeadId;
                        $scope.selectedPurchaseOrder.SupplierAmount = $scope.SupplierAmount;
                    }
                    if ($scope.selectedCashAccount.selected && $scope.CashAmount) {
                        $scope.selectedPurchaseOrder.CashAccountHeadId = $scope.selectedCashAccount.selected.Id;
                        $scope.selectedPurchaseOrder.CashAmount = $scope.CashAmount;
                    }
                    if ($scope.selectedBankAccount.selected && $scope.BankAmount) {
                        $scope.selectedPurchaseOrder.BankAccountHeadId = $scope.selectedBankAccount.selected.Id;
                        $scope.selectedPurchaseOrder.BankAmount = $scope.BankAmount;
                    }
                    $scope.selectedPurchaseOrder.IsCashPurchase = 1;
                    $scope.selectedPurchaseOrder.IsCreditPurchase = 0;
                    $scope.selectedPurchaseOrder.IsLcPurchase = 0;
                }

                else if ($scope.purchaseOrderType == 2) {
                    $scope.selectedPurchaseOrder.SupplierId = $scope.selectedSupplier.selected.Id;
                    $scope.selectedPurchaseOrder.SupplierAccountHeadId = $scope.selectedSupplier.selected.AccountHeadId;
                    $scope.selectedPurchaseOrder.SupplierAmount = $scope.SupplierAmount;

                    $scope.selectedPurchaseOrder.IsCashPurchase = 0;
                    $scope.selectedPurchaseOrder.IsCreditPurchase = 1;
                    $scope.selectedPurchaseOrder.IsLcPurchase = 0;
                }
                else {
                    $scope.selectedPurchaseOrder.LCNo = $scope.LCNO;
                    $scope.selectedPurchaseOrder.LCAccountHeadId = $scope.selectedLCAccount.selected.Id;
                    $scope.selectedPurchaseOrder.LCAmount = $scope.LCAmount;

                    $scope.selectedPurchaseOrder.IsCashPurchase = 0;
                    $scope.selectedPurchaseOrder.IsCreditPurchase = 0;
                    $scope.selectedPurchaseOrder.IsLcPurchase = 1;
                }

                $scope.selectedPurchaseOrder.PurchaseOrderDate =
                    moment($scope.selectedPurchaseOrder.PurchaseOrderDate).format("MM-DD-YYYY");
                $scope.selectedPurchaseOrder.Note = $scope.Note;
                $scope.selectedPurchaseOrder.PaymentType = $scope.PaymentType;
                $scope.selectedPurchaseOrder.EstimatedDeliveryDate =
                    moment($scope.selectedPurchaseOrder.EstimatedDeliveryDate).format("MM-DD-YYYY");
                $scope.selectedPurchaseOrder.PriceValidity = $scope.PriceValidity;
                $scope.selectedPurchaseOrder.TotalAmount = $scope.TotalAmount;
                $scope.selectedPurchaseOrder.PurchaseOrderDetail = $scope.rmDetail;

                var totalDrAmount = $scope.TotalAmount;
                var totalCrAmount = 0;
                if ($scope.purchaseOrderType == 1) {
                    totalCrAmount = $scope.selectedPurchaseOrder.CashAmount + $scope.selectedPurchaseOrder.BankAmount + $scope.selectedPurchaseOrder.SupplierAmount;
                }
                if ($scope.purchaseOrderType == 2) {
                    totalCrAmount = $scope.selectedPurchaseOrder.SupplierAmount;
                }
                if ($scope.purchaseOrderType == 3) {
                    totalCrAmount = $scope.selectedPurchaseOrder.LCAmount;
                }

                if (totalDrAmount != totalCrAmount) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = purchaseOrderService.savePurchaseOrder($scope.selectedPurchaseOrder);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;

                    $location.path("/Purchase/PurchaseOrder/View/" + response.Id);

                    $scope.TotalAmount = null;

                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            validate = function () {
                if ($scope.purchaseOrderType == 1) {
                    if (($scope.selectedSupplier.selected && $scope.SupplierAmount)
                        || ($scope.selectedCashAccount.selected && $scope.CashAmount)
                        || ($scope.selectedBankAccount.selected && $scope.BankAmount)) {
                        return true;
                    }
                }
                else if ($scope.purchaseOrderType == 2) {
                    if ($scope.selectedSupplier.selected && $scope.SupplierAmount) {
                        return true;
                    }
                }
                else {
                    if ($scope.LCNo && $scope.selectedLCAccount.selected && $scope.LCAmount) {
                        return true;
                    }
                }
                //if (!$scope.selectedPurchaseOrder.PurchaseOrderDate
                //    || (!$scope.selectedSupplier.selected)
                //    || (!$scope.Note)
                //    || (!$scope.PaymentType)
                //    || (!$scope.EstimatedDeliveryDate)
                //    || (!$scope.PriceValidity)
                //    || (!$scope.selectedRawMaterialType.selected)
                //    || (!$scope.Quantity)
                //    || (!$scope.Price)
                //    || (!$scope.isNew)) {
                //    return false;
                //}
                return false;
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

                var promiseBankCashAccountHeadList = bankService.getBankCashAccountHeadList();
                promiseBankCashAccountHeadList.then(function (response) {
                    authService.loadingOn();
                    $scope.cashBanks = response;
                    $scope.cashAccount = $filter("filter")(response, { AccountType: 'Cash' });
                    $scope.bankAccount = $filter("filter")(response, { AccountType: 'Bank' });
                }, function (err) {
                    $scope.cashBanks = [];
                });

                var promiseGetSupplierList = purchaseOrderService.getSupplierList();
                promiseGetSupplierList.then(function (response) {
                    $scope.supplier = response;
                }, function (err) {
                    $scope.supplier = [];
                });

                var promiseLCAccountHeadList = bankService.getLCAccountHeadList();
                promiseLCAccountHeadList.then(function (response) {
                    $scope.lcAccount = response;
                }, function (err) {
                    $scope.lcAccount = [];
                });

                var promiseGetRawMaterialType = purchaseOrderService.getRawMaterialType();
                promiseGetRawMaterialType.then(function (response) {
                    $scope.rawMaterialType = response;
                }, function (err) {
                    $scope.rawMaterialType = [];
                });

                authService.loadingOn();
                $q.all([
                    promiseBankCashAccountHeadList,
                    promiseGetSupplierList,
                    promiseGetRawMaterialType,
                    promiseLCAccountHeadList]).then(function () {
                        authService.loadingOff();
                    });

                $scope.selectedPurchaseOrder.PurchaseOrderDate = new Date();
                $scope.selectedPurchaseOrder.EstimatedDeliveryDate = new Date();
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