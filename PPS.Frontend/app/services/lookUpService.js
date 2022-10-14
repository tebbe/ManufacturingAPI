'use strict';
angular.module('AtlasPPS').factory('lookUpService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var lookUpServiceFactory = {};
        lookUpServiceFactory.getCompanyList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Company/GetCompanyList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        lookUpServiceFactory.getEmployeeDropDownList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeDropDownList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        return lookUpServiceFactory;
    }]);
