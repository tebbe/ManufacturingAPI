'use strict';
angular.module('AtlasPPS').factory('customerStatementRptService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var customerStatementRptServiceFactory = {};

        customerStatementRptServiceFactory.GetCustomerStatement = function (vm) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Report/GetCustomerStatement/' + vm.companyId + '/' + vm.customerId + '/' + vm.startDate + '/' + vm.endDate).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return customerStatementRptServiceFactory;
    }]);
