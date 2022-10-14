'use strict';
angular.module('AtlasPPS').factory('userRoleService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var myServiceFactory = {};

        myServiceFactory.getUserRoles = function () {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Role/GetUserRoles').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        myServiceFactory.addRole = function (roleVm) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/Role/AddRole', JSON.stringify(roleVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return myServiceFactory;
    }]);
