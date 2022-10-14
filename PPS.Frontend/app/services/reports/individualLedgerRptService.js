'use strict';
angular.module('AtlasPPS').factory('individualLedgerRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var individualLedgerRptServiceFactory = {};

        individualLedgerRptServiceFactory.GetIndividualLedger = function (vm) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetIndividualLedger/' + vm.FiscalYear + '/' + vm.CompanyId+ '/' + vm.HeadId + '/' + vm.StartDate + '/' + vm.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        individualLedgerRptServiceFactory.GetDealerAuditReport = function (vm) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/DealerAuditReport/' + vm.StartDate + '/' + vm.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return individualLedgerRptServiceFactory;
    }]);
