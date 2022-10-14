'use strict';

angular
    .module('PPSAtlas').controller('accountController', ['$scope', '$location', 'authService', 'ngAuthSettings', function ($scope, $location, authService, ngAuthSettings) {
    $scope.processComplated = false;
    $scope.authentication = authService.authentication;

    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };
    $scope.registration = {
        userName: "",
        password: "",
        confirmPassword: ""
    };

    $scope.message = "";

    $scope.login = function () {
        authService.login($scope.loginData).then(function (response) {
            $location.path('/orders');
        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Failed to register user due to:" + errors.join(' ');
         });
    };

    $scope.logOut = function () {
        authService.logOut(); 
        $location.path('/home');
    }

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 2000);
    }
}]);
