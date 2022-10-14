//'use strict';

angular.module('AtlasPPS').controller('invoiceEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, reportSettings) {
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
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            var selectedModelProductDetailIndex = -1;

            $scope.TotalAmount = null;
            $scope.TotalDiscountAmount = null;
            $scope.TotalGrandAmount = null;
            $scope.availableQuantity = null;

            $scope.invoiceTotalAmount = null;
            $scope.subTotal = null;
            $scope.invoiceTotalDiscountAmount = null;
            $scope.invoiceTotalGrandAmount = null;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.productDetail = [];
            $scope.invoiceProductDetail = [];
            $scope.customer = [];
            $scope.demandOrderType = [];
            $scope.discountType = [];
            $scope.products = [];
            $scope.saleType = [];
            $scope.transactionsModel = [];
            $scope.selectedCustomer = null;
            $scope.selectedDemandOrderId = { selected: null };
            $scope.selectedInvoice = {
                Id: null,
                InvoiceNo: null,
                InvoiceDate: new Date(),
                DemandOrderId: null,
                Note: null,
                InvoiceDetail: [],
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

            $scope.DemandOrderDiscountSetting = {
                RegularDiscount: PpsConstant.DemandOrderDiscountSetting.RegularDiscount,
                SpecialDiscount: PpsConstant.DemandOrderDiscountSetting.SpecialDiscount,
                AdditionalDiscount: PpsConstant.DemandOrderDiscountSetting.AdditionalDiscount,
                ExtraDiscount: PpsConstant.DemandOrderDiscountSetting.ExtraDiscount,
                CashBack: PpsConstant.DemandOrderDiscountSetting.CashBack
            };

            $scope.isInvoiceValidated = false;

            $scope.demandOrder = [];
            $scope.demandOrderIdList = [];

            $scope.company = {};
            $scope.invoice = {};

            $scope.customerAvailableAmount = 0;
            var invDetail = null;
            var getInvoiceById = function (invoiceId) {
                authService.loadingOn();
                var promise = salesService.getInvoiceById(invoiceId);
                promise.then(function (response) {
                    $scope.invoice = response.invoice;
                    invDetail = response.invoice.InvoiceDetail;
                    $scope.invoice.InvoiceDate = new Date($scope.invoice.InvoiceDate);
                    $scope.customerAvailableAmount = $scope.invoice.TotalCustomerBalance;
                    getAvailableBalanceByCustomerId($scope.invoice.CustomerId);
                    getDemandOrderFromInvoiceById($scope.invoice.DemandOrderId, $scope.invoice.Id);
                    authService.loadingOff();
                }, function (err) {
                    $scope.invoice = {};
                    authService.loadingOff();
                });
            };

            var getDemandOrderFromInvoiceById = function (doId, invoiceId) {
                authService.loadingOn();
                var promise = salesService.getDemandOrderByIdFromInvoice(doId, invoiceId);
                promise.then(function (response) {
                    $scope.demandOrder = response;
                    $scope.products = $scope.demandOrder.DemandOrderDetail;
                    $scope.invoiceProductDetail = $scope.demandOrder.DemandOrderDetail;
                    $scope.productDetail = invDetail;
                    finalAmountCalculation();
                    authService.loadingOff();
                }, function (err) {
                    $scope.demandOrder = [];
                    authService.loadingOff();
                });
            };

            var getAvailableBalanceByCustomerId = function (customerId) {
                var promise = salesService.getAvailableBalanceByCustomerId(customerId);
                authService.loadingOn();
                promise.then(function (response) {
                    $scope.customerAvailableAmount = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.customerAvailableAmount = 0;
                    authService.loadingOff();
                });
            };

            var finalAmountCalculation;
            var validateInvoiceEntry;

            $scope.addProductItem = function () {
                var prdDetail = {};

                if (!$scope.Quantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                prdDetail.NewAllocatedQuantity = $scope.Quantity;

                if (!$scope.Discount) {
                    prdDetail.Discount = 0;
                }
                else {
                    prdDetail.Discount = $scope.Discount;
                }

                if ($scope.selectedProduct) {
                    prdDetail.ProductId = $scope.selectedProduct.ProductId;
                    prdDetail.ProductName = $scope.selectedProduct.ProductName;
                    prdDetail.ProductCode = $scope.selectedProduct.ProductCode;
                    prdDetail.UnitPrice = $scope.selectedProduct.UnitPrice;
                    prdDetail.Quantity = $scope.selectedProduct.Quantity;
                    prdDetail.PreAllocatedQuantity = $scope.selectedProduct.AllocatedQuantity - $scope.selectedProduct.AllocatedQuantity;
                    prdDetail.AllocatedQuantity = prdDetail.NewAllocatedQuantity;
                    prdDetail.DeliveredQuantity = $scope.selectedProduct.DeliveredQuantity;
                    prdDetail.AvailableQuantity = prdDetail.Quantity - (prdDetail.AllocatedQuantity + prdDetail.DeliveredQuantity);
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedProduct.ProductName && $scope.selectedProduct.ProductName.length > 0) {
                    var result = $scope.productDetail.filter(function (v) {
                        return v.ProductId === prdDetail.ProductId;
                    });
                    if (selectedModelDetailIndex === -1 && result.length > 0) {
                        notificationService.showErrorNotificatoin(PpsConstant.DuplicateProductName);
                        return;
                    }
                }

                if (!validateInvoiceEntry(prdDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                var totalPrice = prdDetail.UnitPrice * prdDetail.NewAllocatedQuantity;
                var discountAmount = 0;
                if (prdDetail.Discount === 0) {
                    prdDetail.TotalPrice = totalPrice;
                }
                else {
                    discountAmount = totalPrice * (prdDetail.Discount / 100);
                    prdDetail.TotalPrice = totalPrice - discountAmount;
                }

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.productDetail.push(prdDetail);
                } else {
                    $scope.productDetail[selectedModelDetailIndex] = prdDetail;
                }

                $scope.isInvoiceValidated = true;

                finalAmountCalculation();

                hasTransaction = true;
                $scope.selectedProduct = null;
                $scope.tranAmountType = 1;
                $scope.Quantity = null;
                $scope.Discount = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };

            validateInvoiceEntry = function (prdDetail) {
                if (!$scope.selectedProduct.ProductName || (prdDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };

            finalAmountCalculation = function () {
                var tUnitAmount = 0;
                var finalDiscount = 0;

                var invoiceRegularDiscount = $scope.demandOrder.RegularDiscountInPercentage;
                if (isNaN(invoiceRegularDiscount)) {
                    invoiceRegularDiscount = 0;
                }
                var invoiceSpecialDiscount = $scope.demandOrder.SpecialDiscountInPercentage;
                if (isNaN(invoiceSpecialDiscount)) {
                    invoiceSpecialDiscount = 0;
                }
                var invoiceAdditionDiscount = $scope.demandOrder.AdditionalDiscountInPercentage;
                if (isNaN(invoiceAdditionDiscount)) {
                    invoiceAdditionDiscount = 0;
                }
                var invoiceExtraDiscount = $scope.demandOrder.ExtraDiscountInPercentage;
                if (isNaN(invoiceExtraDiscount)) {
                    invoiceExtraDiscount = 0;
                }
                var invoiceCashBackAmount = $scope.demandOrder.CashBackAmount;
                if (isNaN(invoiceCashBackAmount)) {
                    invoiceCashBackAmount = 0;
                }

                if ($scope.productDetail) {
                    for (var prd in $scope.productDetail) {
                        if ($scope.productDetail.hasOwnProperty(prd)) {
                            tUnitAmount += $scope.productDetail[prd].TotalPrice;
                        }
                    }
                }

                $scope.invoiceTotalAmount = tUnitAmount;
                $scope.subTotal = tUnitAmount;

                if (invoiceRegularDiscount !== 0) {
                    var rgdiscount = invoiceRegularDiscount / 100;
                    $scope.invoiceRegularDiscountAmount = Math.round($scope.invoiceTotalAmount * rgdiscount);
                    $scope.subTotal = $scope.invoiceTotalAmount - $scope.invoiceRegularDiscountAmount;
                    finalDiscount += $scope.invoiceRegularDiscountAmount;
                }
                else {
                    $scope.invoiceRegularDiscountAmount = 0;
                }
                if (invoiceSpecialDiscount !== 0) {
                    var spDiscount = invoiceSpecialDiscount / 100;
                    $scope.invoiceSpecialDiscountAmount = Math.round($scope.subTotal * spDiscount);
                    finalDiscount += $scope.invoiceSpecialDiscountAmount;
                }
                else {
                    $scope.invoiceSpecialDiscountAmount = 0;
                }
                if (invoiceAdditionDiscount !== 0) {
                    var addDiscount = invoiceAdditionDiscount / 100;
                    $scope.invoiceAdditionalDiscountAmount = Math.round($scope.subTotal * addDiscount);
                    finalDiscount += $scope.invoiceAdditionalDiscountAmount;
                }
                else {
                    $scope.invoiceAdditionalDiscountAmount = 0;
                }
                if (invoiceExtraDiscount !== 0) {
                    var extDiscount = invoiceExtraDiscount / 100;
                    $scope.invoiceExtraDiscountAmount = Math.round($scope.subTotal * extDiscount);
                    finalDiscount += $scope.invoiceExtraDiscountAmount;
                }
                else {
                    $scope.invoiceExtraDiscountAmount = 0;
                }

                if (invoiceCashBackAmount !== 0) {
                    var cbAmount = invoiceCashBackAmount;
                    $scope.invoiceCashBackAmount = invoiceCashBackAmount;
                    finalDiscount += $scope.invoiceCashBackAmount;
                }
                else {
                    $scope.invoiceCashBackAmount = 0;
                }
                $scope.invoiceTotalDiscountAmount = finalDiscount;
                $scope.invoiceTotalGrandAmount = Math.round($scope.subTotal - finalDiscount + $scope.invoiceRegularDiscountAmount);
            };


            $scope.removeItemDetail = function (pd) {
                if ($scope.productDetail) {
                    for (var i = 0; i < $scope.productDetail.length; i++) {
                        if ($scope.productDetail[i].ProductId === pd.ProductId) {
                            $scope.productDetail.splice(i, 1);
                            if ($scope.productDetail.length === 0) {
                                $scope.isInvoiceValidated = false;
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
                $scope.isInvoiceValidated = false;
                $scope.modelDetailText = "Update";
                $scope.modelDetailMode = PpsConstant.TranMode.Update;

                var result = $scope.products.filter(function (v) {
                    return v.ProductId === pd.ProductId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct = result[0];
                }

                if (pd.Quantity > 0) {
                    //$scope.Quantity = pd.Quantity;
                    $scope.Quantity = pd.Quantity - pd.DeliveredQuantity - pd.PreAllocatedQuantity;
                }
                if (pd.Discount > 0) {
                    $scope.Discount = pd.Discount;
                }
                $scope.availableQuantity = pd.Quantity - pd.DeliveredQuantity - pd.PreAllocatedQuantity;
            };

            $scope.selectProductItemDetail = function (pd) {
                if ($scope.invoiceProductDetail) {
                    for (var i = 0; i < $scope.invoiceProductDetail.length; i++) {
                        if ($scope.invoiceProductDetail[i].ProductId === pd.ProductId) {
                            selectedModelProductDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }
                //$scope.isInvoiceValidated = false;
                $scope.modelDetailText = "Add";
                //$scope.modelDetailMode = PpsConstant.TranMode.AddNew;

                var result = $scope.products.filter(function (v) {
                    return v.ProductId === pd.ProductId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct = result[0];
                }

                if (pd.Quantity > 0) {
                    //$scope.Quantity = pd.ApprovedQuantity;
                    $scope.Quantity = pd.Quantity - pd.DeliveredQuantity - pd.PreAllocatedQuantity;
                }
                if (pd.Discount > 0) {
                    $scope.Discount = pd.Discount;
                }
                //$scope.availableQuantity = pd.AvailableQuantity;
                //$scope.availableQuantity = pd.Quantity - pd.DeliveredQuantity;
                $scope.availableQuantity = pd.Quantity - pd.DeliveredQuantity - pd.PreAllocatedQuantity;
            };

            $scope.cancelModelUpdate = function () {
                $scope.isInvoiceValidated = true;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedProduct = null;
                $scope.Quantity = null;
                $scope.Discount = null;
            };

            var validate = function () {
                if (!$scope.invoice.InvoiceDate || !$scope.invoice.DemandOrderId || !$scope.productDetail) {
                    return false;
                }
                return true;
            };

            var clearField;

            $scope.updateInvoice = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedInvoice.InvoiceDate = moment($scope.invoice.InvoiceDate).format("MM-DD-YYYY");
                $scope.selectedInvoice.DemandOrderId = $scope.invoice.DemandOrderId;
                $scope.selectedInvoice.Note = $scope.invoice.Note;
                $scope.selectedInvoice.TotalAmount = $scope.invoiceTotalAmount;
                $scope.selectedInvoice.RegularDiscountInPercentage = $scope.demandOrder.RegularDiscountInPercentage;
                $scope.selectedInvoice.RegularDiscountAmount = $scope.invoiceRegularDiscountAmount;
                $scope.selectedInvoice.SpecialDiscountInPercentage = $scope.demandOrder.SpecialDiscountInPercentage;
                $scope.selectedInvoice.SpecialDiscountAmount = $scope.invoiceSpecialDiscountAmount;
                $scope.selectedInvoice.AdditionalDiscountInPercentage = $scope.demandOrder.AdditionalDiscountInPercentage;
                $scope.selectedInvoice.AdditionalDiscountAmount = $scope.invoiceAdditionalDiscountAmount;
                $scope.selectedInvoice.ExtraDiscountInPercentage = $scope.demandOrder.ExtraDiscountInPercentage;
                $scope.selectedInvoice.ExtraDiscountAmount = $scope.invoiceExtraDiscountAmount;
                $scope.selectedInvoice.CashBackAmount = $scope.invoiceCashBackAmount;
                $scope.selectedInvoice.TotalDiscountAmount = $scope.invoiceTotalDiscountAmount;
                $scope.selectedInvoice.TotalGrandAmount = $scope.invoiceTotalGrandAmount;
                $scope.selectedInvoice.InvoiceDetail = $scope.productDetail;

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.updateInvoice($scope.selectedInvoice);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $location.path("/Invoice/View/" + response.Id + '/' + response.DemandOrderId);
                    //$window.open(reportSettings.reportBaseUri + 'Invoice/View/' + response.Id, '_blank');
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
                $scope.selectedSaleType = null;
                $scope.selectedCustomer = null;
                $scope.selectedDemandOrderType = null;
                $scope.selectedDiscountType = null;
                $scope.referenceDONo = null;
                $scope.productDetail = null;
                $scope.TotalAmount = null;
                $scope.TotalDiscountAmount = null;
                $scope.TotalGrandAmount = null;
                $scope.invoiceTotalAmount = null;
                $scope.invoiceTotalDiscountAmount = null;
                $scope.invoiceTotalGrandAmount = null;
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

                var invoiceId;
                if ($state.params && $state.params.Id) {
                    invoiceId = _.parseInt($state.params.Id);
                    $scope.selectedInvoice.Id = invoiceId;
                }
                getInvoiceById(invoiceId);
            };
            pageLoad();

            $scope.totalDiscountChange = function () {
                var totalDiscount = $scope.invoiceTotalDiscountInPercentage;
                var val = parseFloat($('#txtTotalDiscount').val());
                if (isNaN(val)) {
                    val = 0;
                }
                $scope.invoiceTotalDiscountAmount = $scope.invoiceTotalAmount * (val / 100);
                $scope.invoiceTotalGrandAmount = Math.round($scope.invoiceTotalAmount - $scope.invoiceTotalDiscountAmount);
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
        }]);