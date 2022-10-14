'use strict';
angular.module('AtlasPPS').controller('purchaseOrderSinglePrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'purchaseOrderService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', '$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, purchaseOrderService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, $q ) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
           
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.purchaseOrder = {};
            $scope.purchaseId = null;
            var getPurchaseOrderById = function () {
                authService.loadingOn();
                var promise = purchaseOrderService.getPurchaseOrderById($scope.purchaseId);
                promise.then(function (response) {
                    $scope.purchaseOrder = response;
                    finalAmountCalculation();
                    authService.loadingOff();
                }, function (err) {
                    $scope.purchaseOrder = {};
                    authService.loadingOff();
                });
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
            var pageLoad = function () {
               
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
               
                if ($state.params && $state.params.poId) {
                    $scope.purchaseId = _.parseInt($state.params.poId);
                }

                authService.loadingOn();
                $q.all([
                ]).then(function () {
                    getPurchaseOrderById();
                        authService.loadingOff();
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

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                ]);
        }]);