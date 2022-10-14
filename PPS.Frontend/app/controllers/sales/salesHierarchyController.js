'use strict';

angular.module('AtlasPPS').controller('salesHierarchyController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.salesHierarchy = {};
            var fiscalYear = null;
            var companyId = null;

            var getMySalesHierarchy = function () {
                authService.loadingOn();
                var promise = salesService.getMySalesHierarchy();
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.salesHierarchy = response;
                }, function (err) {
                    authService.loadingOff();
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };
            var pageLoad = function () {
                var authData = localStorageService.get('authorizationData');
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
                getMySalesHierarchy();
            };
            pageLoad();
        }]);