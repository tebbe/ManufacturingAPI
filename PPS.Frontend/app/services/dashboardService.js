'use strict';
angular.module('AtlasPPS').factory('dashboardService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var myServiceFactory = {};

        myServiceFactory.getLastNDOs = function (n) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Sales/LastNDOs/' + n).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

       

        return myServiceFactory;
    }]);
