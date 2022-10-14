'use strict';
angular.module('AtlasPPS').factory('transactionService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var transactionServiceFactory = {};
        
        transactionServiceFactory.savePaymentTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/PaymentTransaction/SavePaymentTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.updatePaymentTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/PaymentTransaction/UpdatePaymentTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.saveReceiptTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/ReceiptTransaction/SaveReceiptTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.updateReceiptTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/ReceiptTransaction/UpdateReceiptTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.saveJournalTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/JournalTransaction/SaveJournalTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.updateJournalTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/JournalTransaction/UpdateJournalTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.saveContraTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/ContraTransaction/SaveContraTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.updateContraTransaction = function (transaction) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/ContraTransaction/UpdateContraTransaction', JSON.stringify(transaction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getPaymentTransactionList = function(fiscalYear, companyId, tranTypeId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/PaymentTransaction/GetPaymentTransactionList/' + fiscalYear + '/' +companyId + '/' + tranTypeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getPaymentTransactionDetails = function (tranNo) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Report/GetVoucherDetail/' + tranNo).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getReceiptTransactionList = function (fiscalYear, companyId, tranTypeId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/ReceiptTransaction/GetReceiptTransactionList/' + fiscalYear + '/' + companyId + '/' + tranTypeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getJournalTransactionList = function (fiscalYear, companyId, tranTypeId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/JournalTransaction/GetJournalTransactionList/' + fiscalYear + '/' + companyId + '/' + tranTypeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getContraTransactionList = function (fiscalYear, companyId, tranTypeId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/ContraTransaction/GetContraTransactionList/' + fiscalYear + '/' + companyId + '/' + tranTypeId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getUnapprovedAccountsTransactionList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Transaction/GetTransactionAccountsPendingList/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        transactionServiceFactory.getUnapprovedSalesTransactionList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Transaction/GetTransactionSalesPendingList/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        transactionServiceFactory.getUnapprovedPurchaseTransactionList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Transaction/GetTransactionPurchasePendingList/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        transactionServiceFactory.getTransactionRejectReasonType = function () {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Transaction/GetTransactionRejectReasonType').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        transactionServiceFactory.getTransactionAccountsRejectedList = function (fiscalYear, companyId) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Transaction/GetTransactionAccountsRejectedList/' + fiscalYear + '/' + companyId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };

        transactionServiceFactory.getTransactionByTransactionNo = function (fiscalYear, companyId, transactionNo) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/Transaction/GetTransactionByTransactionNo/' + fiscalYear + '/' + companyId + '/' + transactionNo).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        
        return transactionServiceFactory;
    }]);
