'use strict';
angular.module('AtlasPPS').factory('demandOrderRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var demandOrderRptServiceFactory = {};

        demandOrderRptServiceFactory.getDemandOrderById = function (doId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetDemandOrder/' + doId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        return demandOrderRptServiceFactory;
    }]);
