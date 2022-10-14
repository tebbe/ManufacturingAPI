'use strict';
angular.module('AtlasPPS').factory('bankService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var customerServiceFactory = {};

        customerServiceFactory.getBankCashAccountHeadList = function (customerId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Ledger/GetBankCashAccountHeadList/' + customerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getBankCashAccountHeadList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Ledger/GetBankCashAccountHeadList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.getLCAccountHeadList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Ledger/GetLCAccountHeadList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        //customerServiceFactory.saveCustomerTransaction = function (customerTransaction) {
        //    var deferred = $q.defer();
        //    $http.post(serviceBase + 'api/Customer/SaveCustomerTransaction', JSON.stringify(customerTransaction)).success(function (response) {
        //        deferred.resolve(response);
        //    }).error(function (err, status) {
        //        deferred.reject(err);
        //    });
        //    return deferred.promise;
        //};
        return customerServiceFactory;
    }]);
