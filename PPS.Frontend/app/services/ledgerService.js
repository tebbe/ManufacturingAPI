'use strict';
angular.module('AtlasPPS').factory('ledgerService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var ledgerServiceFactory = {};

        ledgerServiceFactory.getHeadList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Ledger/LedgerList/' + fiscalYear + '/' +companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        ledgerServiceFactory.getAccountTypeList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Ledger/LedgerAccountTypeList/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        ledgerServiceFactory.getPrimaryHeadList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Ledger/GetAccountPrimaryHeadListForLedger/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        ledgerServiceFactory.getAccountSubHeadList = function (companyId, accountPrimaryHeadId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Ledger/GetAccountSubHeadList/' + companyId + '/' + accountPrimaryHeadId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        ledgerServiceFactory.getAccountHeadList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Ledger/GetAccountHeadList/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        ledgerServiceFactory.saveAccountHead = function (ledger) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/Ledger/SaveAccountHead', JSON.stringify(ledger)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return ledgerServiceFactory;
    }]);
