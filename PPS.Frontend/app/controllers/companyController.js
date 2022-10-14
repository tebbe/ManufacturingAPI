'use strict';

angular
    .module('AtlasPPS').controller('companyController', ['$scope', '$rootScope', 'localStorageService', '$location', 'authService', 'ngAuthSettings',
function ($scope, $rootScope, localStorageService, $location, authService, ngAuthSettings) {
    $scope.processComplated = false;
    $scope.userId = null;
    $scope.userName = null;
    $scope.userFullName = null;
    $scope.companyName = '';

    var pageLoad = function () {
        var authData = localStorageService.get('authorizationData');

        if (authData && authData.isAuth && authData.expires_in) {
            $scope.userId = authData.userId;
            $scope.userName = authData.userName;
            $scope.userFullName = authData.fullName;
            $scope.fiscalYear = authData.fiscalYear;
            $scope.companyName = authData.companyName;
        } else {
            $location.path('/login');
        }
    };
    pageLoad();

    $scope.logout = function () {
        authService.logout();
        $location.path('/login');
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 2000);
    }
}]);
