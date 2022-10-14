'use strict';
angular.module('AtlasPPS').factory('employeeLeaveService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var employeeLeaeServiceFactory = {};
        
        employeeLeaeServiceFactory.getAllEmployeeLeave = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/LeaveManagement/GetEmployeeLeaveList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status){
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeLeaeServiceFactory.getEmployeeAndEmployeeHierArchyLeaveList = function (status) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/LeaveManagement/GetEmployeeAndEmployeeHierArchyLeaveList/' + status).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeLeaeServiceFactory.GetEmployeeLeaveCategoryWithHierArchy = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/LeaveManagement/GetEmployeeLeaveCategoryWithHierArchy').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeLeaeServiceFactory.getEmployeeLeaveById = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/LeaveManagement/GetEmployeeLeaveById/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
                });
            return deferred.promise;
        };

        employeeLeaeServiceFactory.saveEmployeeLeave = function (employeeLeave) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/LeaveManagement/SaveEmployeeLeave', JSON.stringify(employeeLeave)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeLeaeServiceFactory.updateEmployeeLeave = function (employeeLeave) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/LeaveManagement/UpdateEmployeeLeave', JSON.stringify(employeeLeave)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeLeaeServiceFactory.employeeLeaveApproveOrReject = function (approveModel) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/LeaveManagement/EmployeeLeaveApproveOrReject', JSON.stringify(approveModel)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        return employeeLeaeServiceFactory;
    }]);
