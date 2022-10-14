'use strict';
angular.module('AtlasPPS').factory('approvalService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var myServiceFactory = {};

        myServiceFactory.verifyTransaction = function (tran) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/Approval/TransactionVerifyAccounts', JSON.stringify(tran)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.acceptTransaction = function (tran) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/Approval/TransactionApproveAccounts', JSON.stringify(tran)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        myServiceFactory.rejectTransaction = function (tran) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/Approval/TransactionRejectAccounts', JSON.stringify(tran)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return myServiceFactory;
    }]);
