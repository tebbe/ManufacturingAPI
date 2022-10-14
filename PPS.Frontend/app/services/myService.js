'use strict';
angular.module('AtlasPPS').factory('myService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var myServiceFactory = {};

        myServiceFactory.updatePassword = function (user) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/My/UpdatePassword', JSON.stringify(user)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return myServiceFactory;
    }]);
