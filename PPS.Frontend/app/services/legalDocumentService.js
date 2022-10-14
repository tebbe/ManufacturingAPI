'use strict';
angular.module('AtlasPPS').factory('legalDocumentService', ['$http', '$q',
    'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var legalDocumentServiceFactory = {};

        legalDocumentServiceFactory.getLegalDocumentList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/GetLegalDocumentList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.getCompanyList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Company/GetCompanyList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.getLegalDocumentType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/GetLegalDocumentType').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.getLegalDocumentRenewalCategory = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/GetRenewalCategory').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.getLeglDocumentStatus = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/GetLeglDocumentStatus').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
       
        legalDocumentServiceFactory.saveLegalDocument = function (legalDocument) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Document/SaveLegalDocument', JSON.stringify(legalDocument)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.getLegalDocumentById = function (ldocId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/GetLegalDocumentById/' + ldocId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.updateLegalDocument = function (legalDocument) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Document/UpdateLegalDocument', JSON.stringify(legalDocument)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;
        };
        legalDocumentServiceFactory.getLegalDocSinglePrint = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/LegalDocumentSinglePrint/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        legalDocumentServiceFactory.getLegalDocListPrint = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/LegalDocumentListPrint').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };   
        legalDocumentServiceFactory.legalDocumentHistoryList = function (Id) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Document/LegalDocumentHistoryList/' + Id).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        return legalDocumentServiceFactory;
    }]);