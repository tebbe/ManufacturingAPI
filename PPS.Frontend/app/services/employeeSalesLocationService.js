'use strict';
angular.module('AtlasPPS').factory('employeeSalesLocationService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var employeeSalesLocationServiceFactory = {};

        employeeSalesLocationServiceFactory.getEmployeeSalesLocation = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeSalesLocation').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeSalesLocationServiceFactory.getEmployeeAreaList = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeAreaList/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }; 
        
        employeeSalesLocationServiceFactory.salesAreaAdd = function (salesArea) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Employee/AddSalesArea', JSON.stringify(salesArea)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }; 
        employeeSalesLocationServiceFactory.salesBaseAdd = function (salesBase) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Employee/AddSalesBase', JSON.stringify(salesBase)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeSalesLocationServiceFactory.getSalesAreaById = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetSalesAreaById/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }; 
        employeeSalesLocationServiceFactory.getSalesBaseById = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetSalesBaseById/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeSalesLocationServiceFactory.updateSalesArea = function (salesArea) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Employee/UpdateSalesArea', JSON.stringify(salesArea)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeSalesLocationServiceFactory.updateSalesBase = function (salesBase) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Employee/UpdateSalesBase', JSON.stringify(salesBase)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        return employeeSalesLocationServiceFactory;
    }]);
