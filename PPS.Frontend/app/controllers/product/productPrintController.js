'use strict';

angular.module('AtlasPPS').controller('productPrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'productService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, productService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.productList = [];

            var getAllProductList = function () {
                authService.loadingOn();
                var promise = productService.getProductList();
                promise.then(function (response) {
                    $scope.productList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productList = [];
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
                getAllProductList();
            };
            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
        }]);