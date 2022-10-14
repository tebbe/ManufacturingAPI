'use strict';
angular.module('AtlasPPS').factory('storeService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
    function ($http, $q, localStorageService, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var storeServiceFactory = {};

        storeServiceFactory.getBatchRequisitionList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetBatchRequisitionList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getRawMaterialType = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetRawMaterialType/').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };


        storeServiceFactory.saveBatchRequisition = function (batchRequisition) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/SaveBatchRequisition', JSON.stringify(batchRequisition)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getPendingPOList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetPendingPOList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        storeServiceFactory.getAcceptedPOList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetAcceptedPOList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getPendingPOById = function (poId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetPendingPOById/' + poId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.saveAcceptedPurchaseOrder = function (storeRawMaterialVm) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/SaveAcceptedPurchaseOrder', JSON.stringify(storeRawMaterialVm)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getBatchRequisitionById = function (brId) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetBatchRequisitionById/' + brId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        storeServiceFactory.deliveryBR = function (brId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/DeliveryBR/' + brId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        storeServiceFactory.receiveBR = function (brId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/ReceiveBR/' + brId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        storeServiceFactory.sendToProductionBR = function (brSendToProduction) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/SendToProductionBR/', JSON.stringify(brSendToProduction)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        //storeServiceFactory.getBatchRequisitionListFromFloorStore = function () {
        //    var deferred = $q.defer();
        //    $http.get(serviceBase + 'api/Store/GetBatchRequisitionListFromFloorStore').success(function (response) {
        //        deferred.resolve(response);
        //    }).error(function (err, status) {
        //        deferred.reject(err);
        //    });
        //    return deferred.promise;
        //};
        storeServiceFactory.getProductionGroupListFromFloorStore = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetProductionGroupListFromFloorStore').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.saveFinishedGood = function (newFinisedGoods, isClosedBatch) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/SaveFinishedGood?isClosedBatch=' + isClosedBatch, JSON.stringify(newFinisedGoods)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getFinishedGood = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetFinishedGood').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getFinishedGoodForFiltering = function (filterVm) {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetFinishedGoodForFiltering?PageIndex=' + filterVm.PageIndex + '&PageSize=' + filterVm.PageSize + '&SortColumn=' + filterVm.SortColumn + '&SortDirection=' + filterVm.SortDirection).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.approveFinishedGood = function (pgId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/ApproveFinishedGood/' + pgId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };
        storeServiceFactory.closeBatchClick = function (brId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/CloseBatch/' + brId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.saveBRProductEstimation = function (bRProductionEstimation) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/SaveBRProductEstimation', JSON.stringify(bRProductionEstimation)).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.getProductionGroupList = function () {
            var deferred = $q.defer();
            $http.get(serviceBase + 'api/Store/GetProductionGroupList').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.saveProductionGroup = function () {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/SaveProductionGroup').success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        storeServiceFactory.closeProductionGroupClick = function (pgId) {
            var deferred = $q.defer();
            $http.post(serviceBase + 'api/Store/CloseProductionGroup/' + pgId).success(function (response) {
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        };

        return storeServiceFactory;
    }]);
