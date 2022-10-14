'use strict';

angular.module('AtlasPPS').controller('logoutController',
    ['$scope', '$location', 'authService',
    function ($scope, $location, authService) {
        $scope.processComplated = true;
               
        $scope.logout = function () {
            authService.logout();
            $location.path('/login');
        };

        var pageLoad = function () {
            $scope.logout();
        };
        pageLoad();
    }]);