'use strict';
angular.module('AtlasPPS').factory('companyService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var companyServiceFactory = {};

        companyServiceFactory.getCompanyList = function () {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Company/GetCompanyList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        companyServiceFactory.getCompanyById = function (companyId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/User/GetCompanyById/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return companyServiceFactory;
    }]);
