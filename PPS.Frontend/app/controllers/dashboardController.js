'use strict';

angular
    .module('AtlasPPS').controller('dashboardController', ['$scope', '$rootScope', 'localStorageService', '$location', 'authService', 'ngAuthSettings',
function ($scope, $rootScope, localStorageService, $location, authService, ngAuthSettings) {
    $scope.processComplated = false;
    $rootScope.userId = null;
    $rootScope.userName = null;
    $rootScope.userFullName = null;

    var pageLoad = function () {
        var authData = localStorageService.get('authorizationData');
        
        if (authData && authData.isAuth) {
            $rootScope.userId = authData.userId;
            $rootScope.userName = authData.userName;
            $rootScope.userFullName = authData.fullName;
        } else {
            $location.path('/login');
        }
    };
    pageLoad();

    $rootScope.logout = function () {
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
