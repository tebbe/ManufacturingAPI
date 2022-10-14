'use strict';

angular.module('AtlasPPS').controller('deliveryChallanAddController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            $scope.processCompleted = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.modelHeading = "Delivery Challan";
            $scope.modelDetailText = "Add";
            $scope.modelActionText = "Save";
            $scope.deliveryChallan = [];
            $scope.invoiceIdList = [];
            $scope.invoice = [];
            $scope.Product = [];
            $scope.ProductDetail = [];
            $scope.selectedInvoiceId = {selected: null };
            var selectedModelDetailIndex = -1;
            var selectedModelProductDetailIndex = -1;
            $scope.invoiceProductDetail = [];
            var validateInvoiceEntry;
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;

            $scope.selectedDeliveryChallan = {

                InvoiceId: null,
                ChallanDate: new Date(),
                Note: null,
                DeliveryQuantityDetail:[]
            };

            var getInvoiceListForDelivery = function () {
                var promise = salesService.getInvoiceList();
                promise.then(function (response) {
                    $scope.invoiceIdList = response;
                }, function (err) {
                    $scope.invoiceIdList = [];
                });
            };
            $scope.getInvoiceById = function (data) {
                var promise = salesService.getInvoiceByIdForDeliveryChallan(data.selected.Id);
                promise.then(function (response) {
                    $scope.invoice = response;
                    $scope.Product = response.DeliveryChallanInvoiceDetailList;
                    $scope.invoiceProductDetail = response.DeliveryChallanInvoiceDetailList;
                }, function (err) {
                    $scope.invoice = [];
                    $scope.Product = [];
                    $scope.invoiceProductDetail = [];
                });
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
                var result = $scope.Product.filter(function (v) {
                    return v.ProductId === pd.ProductId;
                });

                if (result && result.length > 0) {
                    $scope.selectedProduct = result[0];
                }
            };
            $scope.addProductItem = function () {
                var prdDetail = {};
                if ($scope.selectedProduct) {
                    prdDetail.ProductId = $scope.selectedProduct.ProductId;
                    prdDetail.ProductName = $scope.selectedProduct.ProductName;
                    prdDetail.Quantity = $scope.Quantity;

                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedProduct.ProductName && $scope.selectedProduct.ProductName.length > 0) {
                    var result = $scope.ProductDetail.filter(function (v) {
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


                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.ProductDetail.push(prdDetail);
                } else {
                    $scope.ProductDetail[selectedModelDetailIndex] = prdDetail;
                }
                $scope.isInvoiceValidated = true;
                selectedModelDetailIndex = -1;

            };
      

            $scope.removeItemDetail = function (pd) {
                if ($scope.ProductDetail) {
                    for (var i = 0; i < $scope.ProductDetail.length; i++) {
                        if ($scope.ProductDetail[i].ProductId === pd.ProductId) {
                            $scope.ProductDetail.splice(i, 1);
                            if ($scope.ProductDetail.length === 0) {
                                $scope.isInvoiceValidated = false;
                            }
                            break;
                        }
                    }

                }
            };

            validateInvoiceEntry = function (prdDetail) {
                if (!$scope.selectedProduct.ProductName || (prdDetail.Quantity === 0)) {
                    return false;
                }
                return true;
            };
            var clearField;
            var validate = function () {
                if (!$scope.DeliveryChallanDate
                    || (!$scope.ProductDetail)) {
                    return false;
                }
                return true;
            };
            $scope.saveDeliveryChallan = function(){
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.selectedDeliveryChallan.ChallanDate = moment($scope.DeliveryChallanDate).format("DD-MM-YYYY");
                $scope.selectedDeliveryChallan.InvoiceId = $scope.invoice.InvoiceId;
                $scope.selectedDeliveryChallan.Note = $scope.Note;
                $scope.selectedDeliveryChallan.DeliveryQuantityDetail = $scope.ProductDetail;
                $scope.processCompleted = false;
                authService.loadingOn();
                var promise = salesService.saveInvoiceDeliveryChallan($scope.selectedDeliveryChallan);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $window.open(reportSettings.reportBaseUri + 'Invoice/DeliveryChalan/View/' + response, '_blank');
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            clearField = function () {
                $scope.DeliveryChallanDate = new Date();
                $scope.Note = null;
                
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
                getInvoiceListForDelivery();

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