//'use strict';

angular.module('AtlasPPS').controller('demandOrderController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'employeeService', '$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, employeeService, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.tranMode = 1;

            $scope.modelHeading = "Add Demand order";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            $scope.IsCheckboxChecked = null;
            $scope.TotalAmount = null;
            $scope.SubTotal = null;
            $scope.RegularDiscountAmount = null;
            $scope.SpecialDiscountAmount = null;
            $scope.AdditionalDiscountAmount = null;
            $scope.ExtraDiscountAmount = null;
            $scope.CashBackAmount = null;
            $scope.TotalDiscountAmount = null;
            $scope.TotalGrandAmount = null;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.productDetail = [];
            $scope.customer = [];
            $scope.demandOrderType = [];
            $scope.discountType = [];
            $scope.products = [];
            $scope.saleType = [];
            $scope.transactionsModel = [];

            $scope.selectedSalesAccount = {
                selected: null
            }
            $scope.selectedCustomer = {
                selected: null
            }
            $scope.selectedProduct = {
                selected: null
            }

            $scope.selectedDemandOrder = {
                DemandOrderNo: '',
                DODate: new Date(),
                SaleTypeId: null,
                CustomerId: null,
                DemandOrderTypeId: null,
                DiscountTypeId: null,
                EmployeeId: null,
                ReferenceDONo: null,
                Note:null,
                DemandOrderDetail: [],
                CreatedById: null,
                TotalAmount: null,
                TotalDiscountInPercentage: null,
                TotalDiscountAmount: null,
                TotalGrandAmount: null
            };
            $scope.checkboxForMoreDoEntry = function () {
                $scope.IsCheckboxChecked = $scope.CheckBoxAddMore;
            }
            $scope.isdemandOrderValidated = false;
            $scope.selectedProductUnitPrice = null;
            $scope.TotalQuantity = 0;
            $scope.isProductTypeValidated = false;

            var clearDoentryForm = function () {
                $scope.selectedDemandOrder.DODate = new Date();
                $scope.referenceDONo = null;
                $scope.selectedCustomer.selected = null;
                $scope.selectedSalesAccount.selected = null;
                $scope.Note = null;
                $scope.selectedProduct.selected = null;
                $scope.Quantity = null;
                $scope.selectedProductUnitPrice = null;
                $scope.productDetail = [];
                $scope.TotalQuantity = null;
                $scope.TotalAmount = null;
                $scope.SubTotal = null;
                $scope.RegularDiscountInPercentage = null;
                $scope.RegularDiscountAmount = null;
                $scope.SpecialDiscountInPercentage = null;
                $scope.SpecialDiscountAmount = null;
                $scope.AdditionalDiscountInPercentage = null;
                $scope.AdditionalDiscountAmount = null;
                $scope.ExtraDiscountInPercentage = null;
                $scope.ExtraDiscountAmount = null;               
                $scope.CashBackAmount = null;
                $scope.TotalDiscountAmount = null;
                $scope.TotalGrandAmount = null;
            };

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
            }

            var finalAmountCalculation;
            var validateDoEntry;
            $scope.prePrdTypeGroupId = -1;

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
                    prdDetail.Name = $scope.selectedProduct.selected.Name;
                    prdDetail.Code = $scope.selectedProduct.selected.Code;
                    prdDetail.UnitPrice = $scope.selectedProduct.selected.UnitPrice;
                    prdDetail.ProductTypeGroupId = $scope.selectedProduct.selected.ProductTypeGroupId;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedProduct.selected && $scope.selectedProduct.selected.Name && $scope.selectedProduct.selected.Name.length > 0) {
                    var result = $scope.productDetail.filter(function (v) {
                        return v.ProductId === prdDetail.ProductId;
                    });
                    if (selectedModelDetailIndex === -1 && result.length > 0) {
                        $scope.Quantity = null;
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
                        $scope.productDetail.push(prdDetail);
                    } else {
                        notificationService.showErrorNotificatoin(PpsConstant.DifferentProductTypeErrorMessage);
                    }
                } else {
                    if ($scope.prePrdTypeGroupId === -1 || ($scope.prePrdTypeGroupId === $scope.selectedProduct.selected.ProductTypeGroupId)) {
                        $scope.prePrdTypeGroupId = $scope.selectedProduct.selected.ProductTypeGroupId;
                        $scope.productDetail[selectedModelDetailIndex] = prdDetail;
                    } else {
                        notificationService.showErrorNotificatoin(PpsConstant.DifferentProductTypeErrorMessage);
                    }

                }

                $scope.isdemandOrderValidated = true;
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
                setTimeout(function () {
                    $("#listProduct").focus();
                }, 1);
            };

            validateDoEntry = function (prdDetail) {
                if (!$scope.selectedProduct.selected.Name || (prdDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };

            finalAmountCalculation = function () {
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

                if ($scope.productDetail) {
                    for (var prd in $scope.productDetail) {
                        if ($scope.productDetail.hasOwnProperty(prd)) {
                            tUnitAmount += $scope.productDetail[prd].TotalPrice;
                            totalQuantity = totalQuantity + $scope.productDetail[prd].Quantity;
                        }
                    }
                }
                $scope.TotalQuantity = totalQuantity;
                $scope.TotalAmount = tUnitAmount;
                $scope.SubTotal = tUnitAmount;

                if (regularDiscount !== 0) {
                    var rgdiscount = regularDiscount / 100;
                    $scope.RegularDiscountAmount = $scope.TotalAmount * rgdiscount;
                    $scope.SubTotal = $scope.TotalAmount - $scope.RegularDiscountAmount;
                    finalDiscount += $scope.RegularDiscountAmount;
                }
                else {
                    $scope.RegularDiscountAmount = 0;
                }
                if (specialDiscount !== 0) {
                    var spDiscount = specialDiscount / 100;
                    $scope.SpecialDiscountAmount = $scope.SubTotal * spDiscount;
                    finalDiscount += $scope.SpecialDiscountAmount;
                }
                else {
                    $scope.SpecialDiscountAmount = 0;
                }
                if (additionDiscount !== 0) {
                    var addDiscount = additionDiscount / 100;
                    $scope.AdditionalDiscountAmount = $scope.SubTotal * addDiscount;
                    finalDiscount += $scope.AdditionalDiscountAmount;
                }
                else {
                    $scope.AdditionalDiscountAmount = 0;
                }
                if (extraDiscount !== 0) {
                    var extDiscount = extraDiscount / 100;
                    $scope.ExtraDiscountAmount = $scope.SubTotal * extDiscount;
                    finalDiscount += $scope.ExtraDiscountAmount;
                }
                else {
                    $scope.ExtraDiscountAmount = 0;
                }

                if (cashBackAmount !== 0) {
                    var cbAmount = cashBackAmount;
                    $scope.CashBackAmount = cbAmount;
                    finalDiscount += $scope.CashBackAmount;
                }
                else {
                    $scope.CashBackAmount = 0;
                }
                $scope.TotalDiscountAmount = finalDiscount;
                $scope.TotalGrandAmount = Math.round($scope.SubTotal - finalDiscount + $scope.RegularDiscountAmount);
            };

            $scope.removeItemDetail = function (pd) {
                if ($scope.productDetail) {
                    for (var i = 0; i < $scope.productDetail.length; i++) {
                        if ($scope.productDetail[i].ProductId === pd.ProductId) {
                            $scope.productDetail.splice(i, 1);
                            if ($scope.productDetail.length === 0) {
                                $scope.isdemandOrderValidated = false;
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
                $scope.isdemandOrderValidated = false;
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
                $scope.isdemandOrderValidated = true;
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
                    || (!$scope.selectedCustomer.selected.Id)
                    || (!$scope.selectedSalesAccount.selected.Id)
                    || (!$scope.productDetail)) {
                    return false;
                }
                return true;
            };

            var clearField;

            $scope.onChangedDisplayUnitPrice = function (sp) {
                $scope.selectedProductUnitPrice = sp.UnitPrice;
                $scope.Quantity = null;
                setTimeout(function () {
                    $("#txtQuantity").focus();
                }, 1);
            }

            $scope.saveDemandOrder = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedDemandOrder.DODate = moment($scope.selectedDemandOrder.DODate).format("MM-DD-YYYY");
                $scope.selectedDemandOrder.SaleTypeId = 1;
                $scope.selectedDemandOrder.CustomerId = $scope.selectedCustomer.selected.Id;
                $scope.selectedDemandOrder.DemandOrderTypeId = 1;
                $scope.selectedDemandOrder.Note = $scope.Note;
                $scope.selectedDemandOrder.EmployeeId = $scope.selectedSalesAccount.selected.Id;
                $scope.selectedDemandOrder.ReferenceDONo = $scope.referenceDONo;
                $scope.selectedDemandOrder.TotalAmount = $scope.TotalAmount;

                var rgDiscount = parseFloat($('#txtRegularDiscount').val());
                if (isNaN(rgDiscount)) {
                    rgDiscount = 0;
                }
                $scope.selectedDemandOrder.RegularDiscountInPercentage = rgDiscount;
                $scope.selectedDemandOrder.RegularDiscountAmount = $scope.RegularDiscountAmount;

                var spDiscount = parseFloat($('#txtSpecialDiscount').val());
                if (isNaN(spDiscount)) {
                    spDiscount = 0;
                }
                $scope.selectedDemandOrder.SpecialDiscountInPercentage = spDiscount;
                $scope.selectedDemandOrder.SpecialDiscountAmount = $scope.SpecialDiscountAmount;

                var adDiscount = parseFloat($('#txtAdditionalDiscount').val());
                if (isNaN(adDiscount)) {
                    adDiscount = 0;
                }
                $scope.selectedDemandOrder.AdditionalDiscountInPercentage = adDiscount;
                $scope.selectedDemandOrder.AdditionalDiscountAmount = $scope.AdditionalDiscountAmount;

                var exDiscount = parseFloat($('#txtExtraDiscount').val());
                if (isNaN(exDiscount)) {
                    exDiscount = 0;
                }
                $scope.selectedDemandOrder.ExtraDiscountInPercentage = exDiscount;
                $scope.selectedDemandOrder.ExtraDiscountAmount = $scope.ExtraDiscountAmount;

                var cbAmount = parseFloat($('#txtCashBackAmount').val());
                if (isNaN(cbAmount)) {
                    cbAmount = 0;
                }
                $scope.selectedDemandOrder.CashBackAmount = $scope.CashBackAmount;

                $scope.selectedDemandOrder.TotalDiscountAmount = $scope.TotalDiscountAmount;
                $scope.selectedDemandOrder.TotalGrandAmount = $scope.TotalGrandAmount;
                $scope.selectedDemandOrder.DemandOrderDetail = $scope.productDetail;

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.saveDemandOrder($scope.selectedDemandOrder);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess +" DO No." + response.DemandOrderNo);
                    $scope.processComplated = true;
                    hasTransaction = true;
                    authService.loadingOff();
                    if ($scope.IsCheckboxChecked === true) {
                        clearDoentryForm();
                    } else {
                        $location.path("/Sales/DemandOrder/View/" + response.Id);
                    }
                   
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

                var promiseGetCustomerList = salesService.getCustomerList();
                promiseGetCustomerList.then(function (response) {
                    $scope.customer = response;
                }, function (err) {
                    $scope.customer = [];
                });

                var promiseGetProductList = salesService.GetProductList();
                promiseGetProductList.then(function (response) {
                    $scope.products = response;
                }, function (err) {
                    $scope.products = [];
                });

                var promiseGetSalesOfficer = employeeService.getSalesOfficer();
                promiseGetSalesOfficer.then(function (response) {
                    $scope.salesOfficer = response;
                }, function (err) {
                    $scope.salesOfficer = [];
                });

                authService.loadingOn();
                $q.all([
                    promiseGetCustomerList,
                    promiseGetSalesOfficer,
                    promiseGetProductList]).then(function () {
                        authService.loadingOff();
                    });

                $scope.selectedDemandOrder.DODate = new Date();
            };
            pageLoad();

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


            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.addPrdToGrid = function(event) {
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