'use strict';
angular.module('AtlasPPS').factory('voucherRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var voucherRptServiceFactory = {};

        voucherRptServiceFactory.getVoucherDetail = function (tranNo) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetVoucherDetail/' + tranNo).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        voucherRptServiceFactory.getTransactionHistoryByTransactionNo = function (tranNo) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetTransactionHistoryByTransactionNo/' + tranNo).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return voucherRptServiceFactory;
    }]);
