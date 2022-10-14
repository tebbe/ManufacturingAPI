'use strict';
angular.module('AtlasPPS').factory('purchaseOrderService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var purchaseServiceFactory = {};

        purchaseServiceFactory.getPurchaseOrderList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetPurchaseOrderList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        purchaseServiceFactory.getSupplierList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetSupplierList/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        purchaseServiceFactory.getRawMaterialType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetRawMaterialType/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        
        purchaseServiceFactory.savePurchaseOrder = function (purchaseOrder) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/SavePurchaseOrder', JSON.stringify(purchaseOrder)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        purchaseServiceFactory.updatePurchaseOrder = function (purchaseOrder) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/UpdatePurchaseOrder', JSON.stringify(purchaseOrder)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        purchaseServiceFactory.getPurchaseOrderById = function (poId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetPurchaseOrderById/' + poId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        purchaseServiceFactory.saveTransactionPO = function (purchaseOrderTransactionVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/SavePurchaseOrderTransaction', JSON.stringify(purchaseOrderTransactionVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        purchaseServiceFactory.verifyPO = function (poId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/VerifyPO/' + poId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        purchaseServiceFactory.approvePO = function (requestVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/ApprovePO', JSON.stringify(requestVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        
        //Transsection
        purchaseServiceFactory.getSupplierById = function (requestVm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetSupplierById/' + requestVm.SupplierId + '/' + requestVm.PurchaseOrderId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        purchaseServiceFactory.savePurchaseOrderTransaction = function (purchaseOrderTransaction) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/SavePurchaseOrderTransaction', JSON.stringify(purchaseOrderTransaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        purchaseServiceFactory.approvePurchaseOrderTransaction = function (purchaseOrderTransaction, fiscalYear) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Purchase/ApprovePurchaseOrderTransaction?fiscalYear=' + fiscalYear, JSON.stringify(purchaseOrderTransaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        purchaseServiceFactory.getUnapprovedPurchaseOrderTransaction = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetUnapprovedPurchaseOrderTransaction').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        purchaseServiceFactory.getApprovedPurchaseOrderTransaction = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Purchase/GetApprovedPurchaseOrderTransaction').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        return purchaseServiceFactory;
    }]);
