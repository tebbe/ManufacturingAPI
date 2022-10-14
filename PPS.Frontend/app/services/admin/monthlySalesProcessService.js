'use strict';
angular.module('AtlasPPS').factory('monthlySalesProcessService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var monthlySalesProcessFactory = {};

        monthlySalesProcessFactory.getMonthlySalesProcessingList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/MonthlyProcess/GetMonthlySalesProcess').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        monthlySalesProcessFactory.saveMonthlySalesProcessing = function (year, month) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/MonthlyProcess/SaveMonthlySalesProcessing/' + year + "/" + month).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        monthlySalesProcessFactory.reprocessMonthlySalesProcessing = function (year, month) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/MonthlyProcess/ReprocessMonthlySalesProcessing/' + year + "/" + month).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        monthlySalesProcessFactory.updateCustomer = function (customer) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/UpdateCustomer', JSON.stringify(customer)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        
        return monthlySalesProcessFactory;
    }]);
