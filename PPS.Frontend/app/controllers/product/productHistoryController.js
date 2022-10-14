'use strict';

angular.module('AtlasPPS').controller('productHistoryController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'productService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window','$state', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, productService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.productHistoryList = [];
            $scope.productId = null;

            var getProductHistoryList = function (id) {
                authService.loadingOn();
                var promise = productService.getProductHistoryByProductId(id);
                promise.then(function (response) {
                    $scope.productHistoryList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productHistoryList = [];
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
                    $scope.productId= _.parseInt($state.params.Id);
                }
                getProductHistoryList($scope.productId);
            };
            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
        }]);