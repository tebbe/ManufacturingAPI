'use strict';
angular.module('AtlasPPS').factory('fileService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var fileServiceFactory = {};

        fileServiceFactory.fileUpload = function (request) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/File/UploadFile',  request ).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return fileServiceFactory;
    }]);
