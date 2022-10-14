﻿'use strict';
angular.module('AtlasPPS').factory('trailBalanceRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var ledgerRptServiceFactory = {};

        ledgerRptServiceFactory.GetTrailBalance = function (vm) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetTrailBalance/' + vm.FiscalYear + '/' + vm.CompanyId + '/' + vm.StartDate + '/' + vm.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return ledgerRptServiceFactory;
    }]);