'use strict';

angular.module('AtlasPPS').controller('pendingPOViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', 'sharedService', 'bankService',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, storeService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            var hasTransaction = false;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.purchaseOrderPaymentStatusColorClass = "";
            $scope.purchaseOrder = {};
            $scope.storeRawMaterial = [];
            $scope.newPOTransaction = {};
            $scope.customerAvailableAmount = 0;

            $scope.purchaseOrderVm = {};
            $scope.modelHeading = "Add Accepting Raw Material";
            $scope.modelDetailText = "Add";
            $scope.modelActionText = "Save";
            $scope.isPendingPOValidated = false;

            var getPendingPOById = function (poId) {
                authService.loadingOn();
                var promise = storeService.getPendingPOById(poId);
                promise.then(function (response) {
                    $scope.purchaseOrder = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.purchaseOrder = {};
                    authService.loadingOff();
                });
            };

            var getRawMaterialType = function () {
                $scope.rawMaterialType = $scope.purchaseOrder.PurchaseOrderDetail;
            };

            var validateRwtEntry = function (storeRawMaterial) {
                if (!$scope.selectedRawMaterialType.RawMaterialTypeName || (storeRawMaterial.Quantity === 0)) {
                    return false;
                }
                return true;
            };

            $scope.addRawMaterialItem = function () {
                var rDetail = {};

                if (!$scope.purchaseOrder.ReceivedQuantity) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                rDetail.Quantity = $scope.purchaseOrder.ReceivedQuantity;

                if ($scope.selectedRawMaterialType) {
                    rDetail.PurchaseOrderId = $scope.selectedRawMaterialType.PurchaseOrderId;
                    rDetail.RawMaterialTypeId = $scope.selectedRawMaterialType.RawMaterialTypeId;
                    rDetail.RawMaterialTypeName = $scope.selectedRawMaterialType.RawMaterialTypeName;
                    rDetail.UnitTypeName = $scope.selectedRawMaterialType.UnitTypeName;
                    rDetail.OrderedQuantity = $scope.selectedRawMaterialType.Quantity;
                    rDetail.AcceptedQuantity = $scope.selectedRawMaterialType.AcceptedQuantity + rDetail.Quantity;
                    rDetail.BalanceQuantity = rDetail.OrderedQuantity - rDetail.AcceptedQuantity;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.selectedRawMaterialType && $scope.selectedRawMaterialType.RawMaterialTypeName && $scope.selectedRawMaterialType.RawMaterialTypeName.length > 0) {
                    var result = $scope.storeRawMaterial.filter(function (v) {
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

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.storeRawMaterial.push(rDetail);
                    $scope.isPendingPOValidated = true;
                } else {
                    $scope.storeRawMaterial[selectedModelDetailIndex] = rDetail;
                }

                $scope.selectedRawMaterialType = null;
                $scope.purchaseOrder.OrderedQuantity = null;
                $scope.purchaseOrder.BalanceQuantity = null;
                $scope.purchaseOrder.ReceivedQuantity = null;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            };

            $scope.removeItemDetail = function (rm) {
                if ($scope.storeRawMaterial) {
                    for (var i = 0; i < $scope.storeRawMaterial.length; i++) {
                        if ($scope.storeRawMaterial[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
                            $scope.storeRawMaterial.splice(i, 1);
                            if ($scope.storeRawMaterial.length === 0) {
                                $scope.isPendingPOValidated = false;
                            }
                            break;
                        }
                    }
                    $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };

            $scope.selectItemDetail = function (rm) {
                if ($scope.storeRawMaterial) {
                    for (var i = 0; i < $scope.storeRawMaterial.length; i++) {
                        if ($scope.storeRawMaterial[i].RawMaterialTypeId === rm.RawMaterialTypeId) {
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
                    $scope.selectedRawMaterialType = result[0];
                }

                if (rm.Quantity > 0) {
                    $scope.purchaseOrder.ReceivedQuantity = rm.Quantity;
                }

            };

            $scope.cancelModelUpdate = function () {
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedRawMaterialType = null;
                $scope.purchaseOrder.ReceivedQuantity = null;
            };

            $scope.saveAcceptedPurchaseOrder = function () {
                $scope.processComplated = false;
                authService.loadingOn();
                var promise = storeService.saveAcceptedPurchaseOrder($scope.storeRawMaterial);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $('#transactionModal').modal('toggle');
                    $location.path("/Store/PendingPOList/View/" + response[0].PurchaseOrderId);
                    $window.location.reload();
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            $scope.clearReceivingQuantityField = function () {
                $scope.purchaseOrder.ReceivedQuantity = null;
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

                var poId;
                if ($state.params && $state.params.poId) {
                    poId = _.parseInt($state.params.poId);
                }
                getPendingPOById(poId);
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

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                ]);

            $scope.closeTransactionModal = function () {
                $('#transactionModal').modal('toggle');
                $scope.selectedRawMaterialType = null;
            };
            $scope.gotoAcceptPO = function (poId) {
                $('#transactionModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
                getRawMaterialType(poId);
            };

        }]);