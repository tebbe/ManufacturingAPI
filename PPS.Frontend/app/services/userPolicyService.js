'use strict';
angular.module('AtlasPPS').factory('userPolicyService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var myServiceFactory = {};

        myServiceFactory.getPolicyByRole = function (roleId,appTypeStatus) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Role/GetPolicyByRole/' + roleId + "/" + appTypeStatus).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        myServiceFactory.updateRolePolicy = function (rolePolicyVm) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/Role/updateRolePolicy', JSON.stringify(rolePolicyVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return myServiceFactory;
    }]);
