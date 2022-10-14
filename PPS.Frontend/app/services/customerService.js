'use strict';
angular.module('AtlasPPS').factory('customerService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var customerServiceFactory = {};

        customerServiceFactory.getCustomerList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getPendingDeactivatedCustomerList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetPendingDeactivatedCustomerList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getCustomerById = function (customerId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerById/' + customerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getPostOfficeList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetPostOfficeList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.getAreaList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Sales/GetAreaList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.getCustomerType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerType').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.getAttachmentType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetAttachmentType').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.SaveCustomer = function (customer) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/SaveCustomer', JSON.stringify(customer)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.updateCustomer = function (customer) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/UpdateCustomer', JSON.stringify(customer)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.deactivateCustomer = function (customerId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/DeactivateCustomer/' + customerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.activateCustomer = function (customerId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/ActivateCustomer/' + customerId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.saveCustomerTransaction = function (customerTransaction) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/SaveCustomerTransaction', JSON.stringify(customerTransaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.approveCustomerTransaction = function (customerTransaction, fiscalYear, companyId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/ApproveCustomerTransaction?fiscalYear=' + fiscalYear + '&companyId=' + companyId, JSON.stringify(customerTransaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getUnapprovedCustomerTransaction = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetUnapprovedCustomerTransaction').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getCustomerTransactionList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerTransactionList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.getCustomerTransactionListForFiltering = function (filterVm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerTransactionListForFiltering?PageIndex=' + filterVm.PageIndex + '&PageSize=' + filterVm.PageSize + '&SortColumn=' + filterVm.SortColumn + '&SortDirection=' + filterVm.SortDirection).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.getUnapprovedCustomerTransactionForFiltering = function (filterVm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetUnapprovedCustomerTransactionForFiltering?PageIndex=' + filterVm.PageIndex + '&PageSize=' + filterVm.PageSize + '&SortColumn=' + filterVm.SortColumn + '&SortDirection=' + filterVm.SortDirection).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        customerServiceFactory.customerTransactionSearch = function (vm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/CustomerTransactionSearch/' + vm.StartDate + '/' + vm.EndDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        customerServiceFactory.getCustomerTransactionByTransactionId = function (id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Customer/GetCustomerTransactionByTransactionId/' +id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        customerServiceFactory.updateCustomerTransaction = function (customerTransaction) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Customer/UpdateCustomerTransaction', JSON.stringify(customerTransaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        return customerServiceFactory;
    }]);
