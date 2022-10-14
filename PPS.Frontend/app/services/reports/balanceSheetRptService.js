'use strict';
angular.module('AtlasPPS').factory('balanceSheetRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var ledgerRptServiceFactory = {};

        ledgerRptServiceFactory.GetBalanceSheet = function (vm) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetBalanceSheet/' + vm.FiscalYear + '/' + vm.CompanyId + '/' + vm.StartDate + '/' + vm.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return ledgerRptServiceFactory;
    }]);
