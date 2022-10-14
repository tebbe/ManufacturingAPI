'use strict';

angular.module('AtlasPPS').controller('productViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'productService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window','$state', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, productService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.productDetails = [];
            $scope.productId = null;



            var getProductDetails = function (id) {
                authService.loadingOn();
                var promise = productService.getProductById(id);
                promise.then(function (response) {
                    $scope.productDetails = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productDetails = [];
                    authService.loadingOff();
                });
            };

            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }


                if ($state.params && $state.params.Id) {
                    $scope.productId = _.parseInt($state.params.Id);
                }
                getProductDetails($scope.productId);
            };
            pageLoad();

            $scope.navigateToProductEdit = function () {
                $location.path("admin/productEdit/" + $scope.productId);
            }
            $scope.navigateToProductListPrint = function () {
                $location.path("admin/productPrint");
            }
            $scope.gotoProductList = function () {
                $location.path("admin/productList");
            }

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
        }]);