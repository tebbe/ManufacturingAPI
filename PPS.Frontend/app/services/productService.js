'use strict';
angular.module('AtlasPPS').factory('productService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var productServiceFactory = {};

        productServiceFactory.getProductList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Product/GetProductList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        productServiceFactory.getProductById = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Product/GetProductById/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        productServiceFactory.saveProduct = function (product) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Product/SaveProduct', JSON.stringify(product)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        productServiceFactory.updateProduct = function (product) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Product/UpdateProduct', JSON.stringify(product)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        productServiceFactory.getProductHistoryByProductId = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Product/GetProductHistoryByProductId/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        productServiceFactory.getProductRelatedAllDropdownList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Product/GetProductRelatedAllDropdownList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        productServiceFactory.getProductDeliveryReportList = function (data) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Product/GetProductDeliveryReport/' + data.StartDate + '/' + data.EndDate + '/' + data.CustomerId + '/' + data.ProductId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        productServiceFactory.getProductReportList = function (data) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Report/GetProductReportList/' + data.StartDate + '/' + data.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        return productServiceFactory;
    }]);
