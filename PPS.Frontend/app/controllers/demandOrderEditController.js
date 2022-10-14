//'use strict';

angular.module('AtlasPPS').controller('demandOrderEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService','reportSettings', 'authService', 'salesService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state', '$q', 'employeeService',
        function ($scope, $rootScope, localStorageService, $location, notificationService, reportSettings, authService, salesService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, $q, employeeService) {
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

            $scope.modelHeading = "Update Demand order";
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.TotalAmount = null;
            $scope.SubTotal = null;
            $scope.TotalDiscountAmount = null;
            $scope.TotalGrandAmount = null;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.productDetail = [];
            $scope.customers = [];
            $scope.demandOrderType = [];
            $scope.discountType = [];
            $scope.products = [];
            $scope.saleType = [];
            $scope.salesOfficer = [];
            $scope.transactionsModel = [];


            $scope.demandOrder = [];

            $scope.selectedDemandOrder = {
                Id: null,
                DemandOrderNo: null,
                DODate: new Date(),
                SaleTypeId: null,
                CustomerId: null,
                DemandOrderTypeId: null,
                DiscountTypeId: null,
                EmployeeId: null,
                ReferenceDONo: null,
                Note: null,
                DemandOrderDetail: [],
                CreatedById: null,
                TotalAmount: null,
                RegularDiscountInPercentage: null,
                RegularDiscountAmount: null,
                SpecialDiscountInPercentage: null,
                SpecialDiscountAmount: null,
                AdditionalDiscountInPercentage: null,
                AdditionalDiscountAmount: null,
                ExtraDiscountInPercentage: null,
                ExtraDiscountAmount: null,
                CashBackAmount: null,
                TotalDiscountAmount: null,
                TotalGrandAmount: null
            };
            $scope.selectedCustomer = {
                selected: null
            };

            $scope.selectedSalesAccount = {
                selected: null
            };

            $scope.selectedProduct = {
                selected: null
            };

            $scope.isDemandOrderEditValidated = false;
            $scope.selectedProductUnitPrice = null;
            $scope.TotalQuantity = 0;
            $scope.prePrdTypeGroupId = -1;

            $scope.DemandOrderDiscountSetting = {
                RegularDiscount: PpsConstant.DemandOrderDiscountSetting.RegularDiscount,
                SpecialDiscount: PpsConstant.DemandOrderDiscountSetting.SpecialDiscount,
                AdditionalDiscount: PpsConstant.DemandOrderDiscountSetting.AdditionalDiscount,
                ExtraDiscount: PpsConstant.DemandOrderDiscountSetting.ExtraDiscount,
                CashBack: PpsConstant.DemandOrderDiscountSetting.CashBack,
                DiscountAfterDiscount: PpsConstant.DemandOrderDiscountSetting.DiscountAfterDiscount
            };

            $scope.onChangeCustomer = function (selectedCustomer) {
                if (selectedCustomer && selectedCustomer.EmployeeId) {
                    var result = $scope.salesOfficer.filter(function (v) {
                        return v.Id === selectedCustomer.EmployeeId;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesAccount.selected = result[0];
                    }
                    else {
                        $scope.selectedSalesAccount.selected = null;
                    }
                } else {
                    $scope.selectedSalesAccount.selected = null;
                }
            };

            var doId;

            var fillDataFunction = function () {
                $scope.demandOrder.DODate = new Date($scope.demandOrder.DODate);

                $scope.selectedCustomer.selected = _.filter($scope.customers,
                    function (item) {
                        return item.Id === $scope.demandOrder.CustomerId;
                    })[0];

                $scope.selectedSalesAccount.selected = _.filter($scope.salesOfficer,
                    function (item) {
                        return item.Id === $scope.demandOrder.EmployeeId;
                    })[0];
            };

            var validateDoEntry;

            var finalAmountCalculation = function () {
                var tUnitAmount = 0;
                var finalDiscount = 0;
                var totalQuantity = 0;

                var regularDiscount = parseFloat($('#txtRegularDiscount').val());
                if (isNaN(regularDiscount)) {
                    regularDiscount = 0;
                }
                var specialDiscount = parseFloat($('#txtSpecialDiscount').val());
                if (isNaN(specialDiscount)) {
                    specialDiscount = 0;
                }
                var additionDiscount = parseFloat($('#txtAdditionalDiscount').val());
                if (isNaN(additionDiscount)) {
                    additionDiscount = 0;
                }
                var extraDiscount = parseFloat($('#txtExtraDiscount').val());
                if (isNaN(extraDiscount)) {
                    extraDiscount = 0;
                }
                var cashBackAmount = parseFloat($('#txtCashBackAmount').val());
                if (isNaN(cashBackAmount)) {
                    cashBackAmount = 0;
                }

                if ($scope.demandOrder && $scope.demandOrder.DemandOrderDetail) {
                    for (var prd in $scope.demandOrder.DemandOrderDetail) {
                        if ($scope.demandOrder.DemandOrderDetail.hasOwnProperty(prd)) {
                            tUnitAmount += $scope.demandOrder.DemandOrderDetail[prd].TotalPrice;
                            totalQuantity = totalQuantity + $scope.demandOrder.DemandOrderDetail[prd].Quantity;
                        }
                    }
                }
                $scope.TotalQuantity = totalQuantity;
                $scope.demandOrder.TotalAmount = tUnitAmount;
                $scope.SubTotal = tUnitAmount;

                if (regularDiscount !== 0) {
                    var rgdiscount = regularDiscount / 100;
                    $scope.demandOrder.RegularDiscountAmount = $scope.demandOrder.TotalAmount * rgdiscount;
                    $scope.SubTotal = $scope.demandOrder.TotalAmount - $scope.demandOrder.RegularDiscountAmount;
                    finalDiscount += $scope.demandOrder.RegularDiscountAmount;
                }
                else {
                    $scope.demandOrder.RegularDiscountAmount = 0;
                }
                if (specialDiscount !== 0) {
                    var spDiscount = specialDiscount / 100;
                    $scope.demandOrder.SpecialDiscountAmount = $scope.SubTotal * spDiscount;
                    finalDiscount += $scope.demandOrder.SpecialDiscountAmount;
                }
                else {
                    $scope.demandOrder.SpecialDiscountAmount = 0;
                }
                if (additionDiscount !== 0) {
                    var addDiscount = additionDiscount / 100;
                    $scope.demandOrder.AdditionalDiscountAmount = $scope.SubTotal * addDiscount;
                    finalDiscount += $scope.demandOrder.AdditionalDiscountAmount;
                }
                else {
                    $scope.demandOrder.AdditionalDiscountAmount = 0;
                }
                if (extraDiscount !== 0) {
                    var extDiscount = extraDiscount / 100;
                    $scope.demandOrder.ExtraDiscountAmount = $scope.SubTotal * extDiscount;
                    finalDiscount += $scope.demandOrder.ExtraDiscountAmount;
                }
                else {
                    $scope.demandOrder.ExtraDiscountAmount = 0;
                }

                if (cashBackAmount !== 0) {
                    var cbAmount = cashBackAmount;
                    $scope.demandOrder.CashBackAmount = cbAmount;
                    finalDiscount += $scope.demandOrder.CashBackAmount;
                }
                else {
                    $scope.demandOrder.CashBackAmount = 0;
                }
                $scope.demandOrder.TotalDiscountAmount = finalDiscount;
                $scope.demandOrder.TotalGrandAmount = Math.round($scope.SubTotal - finalDiscount + $scope.demandOrder.RegularDiscountAmount);
            };

            $scope.addProductItem = function () {
                var prdDetail = {};

                if (!$scope.Quantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                prdDetail.Quantity = $scope.Quantity;

                if (!$scope.Discount) {
                    prdDetail.Discount = 0;
                }
                else {
                    prdDetail.Discount = $scope.Discount;
                }

                if ($scope.selectedProduct.selected) {
                    prdDetail.ProductId = $scope.selectedProduct.selected.Id;
                    prdDetail.ProductName = $scope.selectedProduct.selected.Name;
                    prdDetail.ProductCode = $scope.selectedProduct.selected.Code;
                    prdDetail.UnitPrice = $scope.selectedProduct.selected.UnitPrice;
                    prdDetail.ProductTypeGroupId = $scope.selectedProduct.selected.ProductTypeGroupId;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedProduct.selected.Name && $scope.selectedProduct.selected.Name.length > 0) {
                    var result = $scope.demandOrder.DemandOrderDetail.filter(function (v) {
                        return v.ProductId === prdDetail.ProductId;
                    });

                    if (selectedModelDetailIndex === -1 && result.length > 0) {
                        notificationService.showErrorNotificatoin(PpsConstant.DuplicateProductName);
                        return;
                    }
                }

                if (!validateDoEntry(prdDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                var totalPrice = prdDetail.UnitPrice * prdDetail.Quantity;
                var discountAmount = 0;
                if (prdDetail.Discount === 0) {
                    prdDetail.TotalPrice = totalPrice;
                }
                else {
                    discountAmount = totalPrice * (prdDetail.Discount / 100);
                    prdDetail.TotalPrice = totalPrice - discountAmount;
                }

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    if ($scope.prePrdTypeGroupId === -1 || ($scope.prePrdTypeGroupId === $scope.selectedProduct.selected.ProductTypeGroupId)) {
                        $scope.prePrdTypeGroupId = $scope.selectedProduct.selected.ProductTypeGroupId;
                        $scope.demandOrder.DemandOrderDetail.push(prdDetail);
                    } else {
                        notificationService.showErrorNotificatoin(PpsConstant.DifferentProductTypeErrorMessage);
                    }
                } else {
                    if ($scope.prePrdTypeGroupId === -1 || ($scope.prePrdTypeGroupId === $scope.selectedProduct.selected.ProductTypeGroupId)) {
                        $scope.prePrdTypeGroupId = $scope.selectedProduct.selected.ProductTypeGroupId;
                        $scope.demandOrder.DemandOrderDetail[selectedModelDetailIndex] = prdDetail;
                    } else {
                        notificationService.showErrorNotificatoin(PpsConstant.DifferentProductTypeErrorMessage);
                    }

                }

                $scope.isDemandOrderEditValidated = true;
                finalAmountCalculation();

                hasTransaction = true;
                $scope.selectedProduct.selected = null;
                $scope.tranAmountType = 1;
                $scope.Quantity = null;
                $scope.Discount = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedProductUnitPrice = null;
            };

            validateDoEntry = function (prdDetail) {
                if (!$scope.selectedProduct.selected.Name || (prdDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };

            $scope.removeItemDetail = function (pd) {
                if ($scope.demandOrder.DemandOrderDetail) {
                    for (var i = 0; i < $scope.demandOrder.DemandOrderDetail.length; i++) {
                        if ($scope.demandOrder.DemandOrderDetail[i].ProductId === pd.ProductId) {
                            $scope.demandOrder.DemandOrderDetail.splice(i, 1);
                            if ($scope.demandOrder.DemandOrderDetail.length === 0) {
                                $scope.isDemandOrderEditValidated = false;
                                $scope.prePrdTypeGroupId = -1;
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


            $scope.selectItemDetail = function (pd) {
                if ($scope.demandOrder.DemandOrderDetail) {
                    for (var i = 0; i < $scope.demandOrder.DemandOrderDetail.length; i++) {
                        if ($scope.demandOrder.DemandOrderDetail[i].ProductId === pd.ProductId) {
                            selectedModelDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }
                $scope.isDemandOrderEditValidated = false;
                $scope.modelDetailText = "Update";
                $scope.modelDetailMode = PpsConstant.TranMode.Update;

                var result = $scope.products.filter(function (v) {
                    return v.Id === pd.ProductId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct.selected = result[0];
                }

                if (pd.Quantity > 0) {
                    $scope.Quantity = pd.Quantity;
                }
                if (pd.Discount > 0) {
                    $scope.Discount = pd.Discount;
                }
                $scope.selectedProductUnitPrice = pd.UnitPrice;
            };

            $scope.cancelModelUpdate = function () {
                $scope.isDemandOrderEditValidated = true;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedProduct.selected = null;
                $scope.Quantity = null;
                $scope.Discount = null;
                $scope.selectedProductUnitPrice = null;
            };

            var validate = function () {
                if (!$scope.selectedDemandOrder.DODate
                    || (!$scope.selectedCustomer.selected)
                    || (!$scope.selectedSalesAccount.selected)
                    || (!$scope.demandOrder.DemandOrderDetail)) {
                    return false;
                }
                return true;
            };


            $scope.regularDiscountChange = function () {
                finalAmountCalculation();
            };
            $scope.specialDiscountChange = function () {
                finalAmountCalculation();
            };
            $scope.additionalDiscountChange = function () {
                finalAmountCalculation();
            };
            $scope.extraDiscountChange = function () {
                finalAmountCalculation();
            };
            $scope.cashBackAmountChange = function () {
                finalAmountCalculation();
            };

            $scope.onChangedDisplayUnitPrice = function (sp) {
                $scope.selectedProductUnitPrice = sp.UnitPrice;
                $scope.Quantity = null;
                setTimeout(function () {
                    $("#txtQuantity").focus();
                }, 1);
            };

            var clearField;
            $scope.updateDemandOrder = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedDemandOrder.Id = $scope.demandOrder.Id;
                $scope.selectedDemandOrder.DemandOrderNo = $scope.demandOrder.DemandOrderNo;
                $scope.selectedDemandOrder.DODate = moment($scope.demandOrder.DODate).format("MM-DD-YYYY");
                $scope.selectedDemandOrder.CustomerId = $scope.selectedCustomer.selected.Id;
                $scope.selectedDemandOrder.EmployeeId = $scope.selectedSalesAccount.selected.Id;
                $scope.selectedDemandOrder.ReferenceDONo = $scope.demandOrder.ReferenceNo;
                $scope.selectedDemandOrder.Note = $scope.demandOrder.Note;
                $scope.selectedDemandOrder.SaleTypeId = 1;
                $scope.selectedDemandOrder.DemandOrderTypeId = 1;
                $scope.selectedDemandOrder.TotalAmount = $scope.demandOrder.TotalAmount;

                var rgDiscount = parseFloat($('#txtRegularDiscount').val());
                if (isNaN(rgDiscount)) {
                    rgDiscount = 0;
                }
                $scope.selectedDemandOrder.RegularDiscountInPercentage = rgDiscount;
                $scope.selectedDemandOrder.RegularDiscountAmount = $scope.demandOrder.RegularDiscountAmount;

                var spDiscount = parseFloat($('#txtSpecialDiscount').val());
                if (isNaN(spDiscount)) {
                    spDiscount = 0;
                }
                $scope.selectedDemandOrder.SpecialDiscountInPercentage = spDiscount;
                $scope.selectedDemandOrder.SpecialDiscountAmount = $scope.demandOrder.SpecialDiscountAmount;

                var adDiscount = parseFloat($('#txtAdditionalDiscount').val());
                if (isNaN(adDiscount)) {
                    adDiscount = 0;
                }
                $scope.selectedDemandOrder.AdditionalDiscountInPercentage = adDiscount;
                $scope.selectedDemandOrder.AdditionalDiscountAmount = $scope.demandOrder.AdditionalDiscountAmount;

                var exDiscount = parseFloat($('#txtExtraDiscount').val());
                if (isNaN(exDiscount)) {
                    exDiscount = 0;
                }
                $scope.selectedDemandOrder.ExtraDiscountInPercentage = exDiscount;
                $scope.selectedDemandOrder.ExtraDiscountAmount = $scope.demandOrder.ExtraDiscountAmount;

                var cbAmount = parseFloat($('#txtCashBackAmount').val());
                if (isNaN(cbAmount)) {
                    cbAmount = 0;
                }
                $scope.selectedDemandOrder.CashBackAmount = $scope.demandOrder.CashBackAmount;

                $scope.selectedDemandOrder.TotalDiscountAmount = $scope.demandOrder.TotalDiscountAmount;
                $scope.selectedDemandOrder.TotalGrandAmount = $scope.demandOrder.TotalGrandAmount;
                $scope.selectedDemandOrder.DemandOrderDetail = $scope.demandOrder.DemandOrderDetail;

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.updateDemandOrder($scope.selectedDemandOrder);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    //$location.path("/Sales/DemandOrder/View/" + response.Id);
                    $window.open(reportSettings.reportBaseUri + 'Sales/DemandOrder/View/' + response.Id, '_self');
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {

                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            clearField = function () {
                $scope.selectedDemandOrder.DODate = new Date();
                $scope.selectedCustomer.selected = null;
                $scope.selectedSalesAccount.selected = null;
                $scope.referenceDONo = null;
                $scope.productDetail = null;
                $scope.TotalAmount = null;
                $scope.TotalDiscountAmount = null;
                $scope.TotalGrandAmount = null;
            }

            $scope.addNewTransaction = function () {
                $scope.selectedTransaction = {
                    TranNo: 'New',
                    TranDate: new Date(),
                    Particulars: '',
                    TransactionDetail: []
                };


                $('#transactionModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
                hasTransaction = false;
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

                if ($state.params && $state.params.doId) {
                    doId = _.parseInt($state.params.doId);
                }

                var promiseGetSaleType = salesService.GetSaleType();
                promiseGetSaleType.then(function (response) {
                    $scope.saleType = response;
                }, function (err) {
                    $scope.saleType = [];
                });

                var promiseGetCustomerList = customerService.getCustomerList();
                promiseGetCustomerList.then(function (response) {
                    $scope.customers = response;
                }, function (err) {
                    $scope.customers = [];
                });

                var promiseGetDemandOrderType = salesService.GetDemandOrderType();
                promiseGetDemandOrderType.then(function (response) {
                    $scope.demandOrderType = response;
                }, function (err) {
                    $scope.demandOrderType = [];
                });

                var promiseGetDiscountType = salesService.GetDiscountType();
                promiseGetDiscountType.then(function (response) {
                    $scope.discountType = response;
                }, function (err) {
                    $scope.discountType = [];
                });

                var promiseGetSalesOfficer = employeeService.getSalesOfficer();
                promiseGetSalesOfficer.then(function (response) {
                    $scope.salesOfficer = response;
                }, function (err) {
                    $scope.salesOfficer = [];
                });

                var promiseGetProductList = salesService.GetProductList();
                promiseGetProductList.then(function (response) {
                    $scope.products = response;
                }, function (err) {
                    $scope.products = [];
                });

                var promiseGetDemandOrderById = salesService.getDemandOrderById(doId);
                promiseGetDemandOrderById.then(function (response) {
                    $scope.isDemandOrderEditValidated = true;
                    $scope.demandOrder = response.demandOrder;
                    $scope.prePrdTypeGroupId = $scope.demandOrder.DemandOrderDetail[0].ProductTypeGroupId;

                }, function (err) {
                    $scope.demandOrder = {};
                });

                authService.loadingOn();
                $q.all([
                    promiseGetSaleType,
                    promiseGetCustomerList,
                    promiseGetDemandOrderType,
                    promiseGetDiscountType,
                    promiseGetSalesOfficer,
                    promiseGetProductList,
                    promiseGetDemandOrderById]).then(function () {
                        fillDataFunction();
                        finalAmountCalculation();
                        authService.loadingOff();
                    });
            };
            pageLoad();

            $scope.closeModal = function () {
                if (hasTransaction) {
                    pageLoad();
                }
                $scope.totalDrAmount = 0;
                $scope.totalCrAmount = 0;
                $('#transactionModal').modal('toggle');
            };

            $scope.totalDiscountChange = function () {
                var totalDiscount = $scope.demandOrder.TotalDiscountInPercentage;
                var val = parseFloat($('#txtTotalDiscount').val());
                if (isNaN(val)) {
                    val = 0;
                }
                $scope.demandOrder.TotalDiscountAmount = $scope.demandOrder.TotalAmount * (val / 100);
                $scope.demandOrder.TotalGrandAmount = Math.round($scope.demandOrder.TotalAmount - $scope.demandOrder.TotalDiscountAmount);
            };

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.addPrdToGrid = function (event) {
                if (event.which === 13) {
                    $scope.addProductItem();
                }
            }

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