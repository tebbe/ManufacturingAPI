'use strict';
angular.module('AtlasPPS').factory('journalRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var journalRptServiceFactory = {};

        journalRptServiceFactory.GetJournal = function (vm) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetJournal/' + vm.FiscalYear + '/' + vm.CompanyId + '/' + vm.StartDate + '/' + vm.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return journalRptServiceFactory;
    }]);
