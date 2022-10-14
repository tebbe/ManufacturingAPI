'use strict';

angular.module('AtlasPPS').controller('invoiceReturnController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.tranMode = 1;

            $scope.modelHeading = "Add Invoice Return";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            var selectedModelProductDetailIndex = -1;

            $scope.TotalAmount = null;
            $scope.TotalDiscountAmount = null;
            $scope.TotalGrandAmount = null;


            $scope.invoiceTotalAmount = null;
            $scope.invoiceTotalDiscountAmount = null;
            $scope.invoiceTotalGrandAmount = null;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.productDetail = [];
            $scope.invoiceProductDetail = [];
            $scope.customer = [];
            $scope.invoiceType = [];
            $scope.discountType = [];
            $scope.products = [];
            $scope.Quantity = null;
            $scope.Discount = null;
            $scope.saleType = [];
            $scope.transactionsModel = [];
            $scope.selectedCustomer = null;
            $scope.InvoiceIdList = { selected: null };
            $scope.selectedInvoice = {
                InvoiceNo: null,
                ReturnDate: new Date(),
                invoiceId: null,
                Note: null,
                InvoiceReturnDetail: [],
                TotalAmount: null,
                RegularDiscountInPercentage: null,
                RegularDiscountAmount: null,
                TotalDiscountAmount: null,
                TotalGrandAmount: null
            };

            $scope.isInvoiceValidated = false;

            $scope.invoice = [];
            $scope.InvoiceIdList = [];

            var getAllInvoiceList = function () {
                authService.loadingOn();
                var promise = salesService.getAllInvoiceList();
                promise.then(function (response) {
                    $scope.InvoiceIdList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.InvoiceIdList = [];
                    authService.loadingOff();
                });
            }

            $scope.getInvoiceById = function (invoice) {
                authService.loadingOn();
                var promise = salesService.getInvoiceById(invoice.Id);
                promise.then(function (response) {
                    $scope.invoice = response;
                    $scope.products = response.InvoiceDetail;
                    $scope.invoiceProductDetail = response.InvoiceDetail;
                    $scope.productDetail = [];
                    finalAmountCalculation();
                    authService.loadingOff();
                }, function (err) {
                    $scope.invoice = [];
                    authService.loadingOff();
                });
            }

            var finalAmountCalculation;
            var validateInvoiceEntry;

            $scope.addProductItem = function () {
                var prdDetail = {};
               
                if (!($scope.ReturnQuantity)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                if (!$scope.selectedProduct.Discount) {
                    prdDetail.Discount = 0;
                }
                else {
                    prdDetail.Discount = $scope.selectedProduct.Discount;
                }

                if ($scope.selectedProduct) {
                    prdDetail.ProductId = $scope.selectedProduct.ProductId;
                    prdDetail.ProductName = $scope.selectedProduct.ProductName;
                    prdDetail.ProductCode = $scope.selectedProduct.ProductCode;
                    prdDetail.UnitPrice = $scope.selectedProduct.UnitPrice;
                    prdDetail.ReturnQuantity = $scope.ReturnQuantity;
                    
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

                var totalPrice = prdDetail.UnitPrice * (prdDetail.ReturnQuantity);
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
                $scope.Discount = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };

            validateInvoiceEntry = function (prdDetail) {
                if (!$scope.selectedProduct.ProductName || ((prdDetail.ReturnQuantity) === 0)) {
                    return false;
                }
                return true;
            };


            finalAmountCalculation = function () {
                var tUnitAmount = 0;
                var finalDiscount = 0;

                var invoiceRegularDiscount = $scope.invoice.RegularDiscountInPercentage;
                if (isNaN(invoiceRegularDiscount)) {
                    invoiceRegularDiscount = 0;
                }
            
                if ($scope.productDetail) {
                    for (var prd in $scope.productDetail) {
                        if ($scope.productDetail.hasOwnProperty(prd)) {
                            tUnitAmount += $scope.productDetail[prd].TotalPrice;
                        }
                    }
                }

                $scope.invoiceTotalAmount = tUnitAmount;

                if (invoiceRegularDiscount !== 0) {
                    var rgdiscount = invoiceRegularDiscount / 100;
                    $scope.invoiceRegularDiscountAmount = Math.round($scope.invoiceTotalAmount * rgdiscount);
                    finalDiscount += $scope.invoiceRegularDiscountAmount;
                }
                else {
                    $scope.invoiceRegularDiscountAmount = 0;
                }
                $scope.invoiceTotalDiscountAmount = finalDiscount;
                $scope.invoiceTotalGrandAmount = Math.round($scope.invoiceTotalAmount - finalDiscount);
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


            //$scope.selectItemDetail = function (pd) {
            //    if ($scope.productDetail) {
            //        for (var i = 0; i < $scope.productDetail.length; i++) {
            //            if ($scope.productDetail[i].ProductId === pd.ProductId) {
            //                selectedModelDetailIndex = i;
            //                break;
            //            }
            //        }
            //    } else {
            //        return;
            //    }
            //    $scope.isInvoiceValidated = false;
            //    $scope.modelDetailText = "Update";
            //    $scope.modelDetailMode = PpsConstant.TranMode.Update;

            //    var result = $scope.products.filter(function (v) {
            //        return v.ProductId === pd.ProductId;
            //    });

            //    if (result && result.length > 0) {
            //        $scope.selectedProduct = result[0];
            //        $scope.DeliveredQuantity = $scope.selectedProduct.DeliveredQuantity
            //    }
            //};

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
                $scope.modelDetailText = "Add";
                var result = $scope.products.filter(function (v) {
                    return v.ProductId === pd.ProductId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct = result[0];
                    $scope.ReturnQuantity = $scope.selectedProduct.DeliveredQuantity
                }

            };

            $scope.cancelModelUpdate = function () {
                $scope.isInvoiceValidated = true;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedProduct = null;

            };

            var validate = function () {
                if (!$scope.selectedInvoice.ReturnDate
                    || (!$scope.InvoiceIdList.selected)
                    || (!$scope.productDetail)) {
                    return false;
                }
                return true;
            };

          

            $scope.saveReturnInvoice = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedInvoice.ReturnDate = moment($scope.ReturnDate).format("MM-DD-YYYY");
                $scope.selectedInvoice.InvoiceId = $scope.InvoiceIdList.selected.Id;
                $scope.selectedInvoice.Note = $scope.Note;
                $scope.selectedInvoice.TotalAmount = $scope.invoiceTotalAmount;
                $scope.selectedInvoice.TotalGrandAmount = $scope.invoiceTotalGrandAmount;
                $scope.selectedInvoice.InvoiceReturnDetail = $scope.productDetail;

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = salesService.saveReturnInvoice($scope.selectedInvoice);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
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
                getAllInvoiceList();
                $scope.InvoiceDate = new Date();
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

            $scope.navigateToReturnInvoiceView = function (data) {
                $location.path('/Invoice/InvoiceReturn/View/' + data.Id);
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