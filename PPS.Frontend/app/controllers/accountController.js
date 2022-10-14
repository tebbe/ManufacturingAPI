'use strict';

angular.module('AtlasPPS').controller('accountController', ['$rootScope', '$scope', '$location', 'localStorageService', 'authService', 'ngAuthSettings',
        function ($rootScope, $scope, $location, localStorageService, authService, ngAuthSettings) {
            $scope.processComplated = true;
            $scope.loginButtonText = "Login"; //Logging in
            var pageLoad = function () {
                var authData = localStorageService.get('authorizationData');
                if (authData && authData.isAuth) {
                    $location.path('/dashboard');
                    //$rootScope.idleStart();
                } else {
                    $location.path('/login');
                }
            };
            pageLoad();

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
                if (!$scope.loginData.userName || !$scope.loginData.password) {
                    $scope.message = "Please enter email and password.";
                    return;
                }
                $scope.processComplated = false;
                authService.loadingOn();
                var promise = authService.login($scope.loginData);
                promise.then(function (response) {
                    $location.path('/dashboard');
                    //$rootScope.idleStart();
                    $scope.processComplated = true;
                    authService.loadingOff();
                }, function (err) {
                    $scope.message = err.error_description;
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            $scope.signUp = function () {
                console.log("from register");
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

            $scope.logout = function () {
                authService.logout();
                $location.path('/login');
            }

            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            }
        }]);
