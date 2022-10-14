'use strict';
angular.module('AtlasPPS').factory('userService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var myServiceFactory = {};

        myServiceFactory.getUsers = function () {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/User/GetUsers').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.getUserRoleDetailById = function (userId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/User/GetUserRoleById/' + userId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.userRegister = function (user) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/Register', JSON.stringify(user)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        myServiceFactory.resetUser = function (userId) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/ResetUser/' + userId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        myServiceFactory.userLock = function (userId) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/UserLock/' + userId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.userUnlock = function (userId) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/UserUnlock/' + userId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.userActivate = function (userId) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/UserActivate/' + userId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.userDeactivate = function (userId) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/UserDeactivate/' + userId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        myServiceFactory.updateUser = function (userVm) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/User/UpdateUser', JSON.stringify(userVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        //myServiceFactory.rejectTransaction = function (tran) {
        //    var deferred = $q.defer();

        //    $http.post(serviceBase + 'api/user/RejectTransaction', JSON.stringify(tran)).success(function (response) {
        //        deferred.resolve(response);
        //    }).error(function (err, status) {
        //        deferred.reject(err);
        //    });

        //    return deferred.promise;
        //};

        return myServiceFactory;
    }]);
