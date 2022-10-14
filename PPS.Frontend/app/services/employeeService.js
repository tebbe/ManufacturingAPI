'use strict';
angular.module('AtlasPPS').factory('employeeService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var employeeServiceFactory = {};

        employeeServiceFactory.getSalesOfficer = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetSalesEmployee').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        employeeServiceFactory.getEmployee = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetAllEmployee').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        employeeServiceFactory.getSalesEmployeeWithSalesTargetByMonth = function(employeeRequestVm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetSalesEmployeeWithSalesTargetByMonth/'+employeeRequestVm.Year+'/'+employeeRequestVm.Month).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        employeeServiceFactory.getActiveEmployeeList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetActiveEmployeeList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        
        employeeServiceFactory.getInactiveEmployeeList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetInactiveEmployeeList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
                });
            return deferred.promise;
        };
        employeeServiceFactory.addNewEmployee = function (employee) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Employee/AddNewEmployee', JSON.stringify(employee)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeServiceFactory.getEmployeeById = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeById/'+Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeServiceFactory.updateEmployee = function (employee) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Employee/EmployeeUpdate', JSON.stringify(employee)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeServiceFactory.getEmployeeHisoryList = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/EmployeeHistoryList/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeServiceFactory.employeeSinglePrint = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeById/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeServiceFactory.employeeListPrint = function (status) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/EmployeeListPrint/' + status).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        employeeServiceFactory.getEmployeeAreaList = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeAreaList/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        }; 
        employeeServiceFactory.getEmployeeBaseList = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeBaseList/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        employeeServiceFactory.getEmployeeSalesLocationByEmployeeId = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Employee/GetEmployeeSalesLocationByEmployeeId/' + id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        return employeeServiceFactory;

    }]);
