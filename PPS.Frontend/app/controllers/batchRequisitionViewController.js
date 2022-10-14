'use strict';

angular.module('AtlasPPS').controller('batchRequisitionViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', 'sharedService', 'bankService',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, storeService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.batchRequisition = {};

            var getBatchRequisitionById = function (brId) {
                authService.loadingOn();
                var promise = storeService.getBatchRequisitionById(brId);
                promise.then(function (response) {
                    $scope.batchRequisition = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.batchRequisition = {};
                    authService.loadingOff();
                });
            };

            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    fiscalYear = authData.fiscalYear;
                    companyId = authData.companyId;
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
                var brId;
                if ($state.params && $state.params.Id) {
                    brId = _.parseInt($state.params.Id);
                }
                getBatchRequisitionById(brId);
                $scope.EstimatedProductionDate = new Date();
            };
            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                ]);

            //delivery br
            $scope.showDeliveryBRModal = function (brModel) {
                $scope.selectedBR = brModel;
                $('#deliveryBrModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.deliveryBrClick = function () {
                var brId = $scope.batchRequisition.Id;
                authService.loadingOn();
                var promise = storeService.deliveryBR(brId);
                promise.then(function (response) {
                    $scope.closeDeliveryBRModal();
                    getBatchRequisitionById(brId);
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    //$window.location.reload();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeDeliveryBRModal = function () {
                $('#deliveryBrModal').modal('toggle');
            };

            //receive br
            $scope.showReceiveBRModal = function (brModel) {
                $scope.selectedBR = brModel;
                $('#receiveBrModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.receiveBrClick = function () {
                var brId = $scope.batchRequisition.Id;
                authService.loadingOn();
                var promise = storeService.receiveBR(brId);
                promise.then(function (response) {
                    $scope.closeReceiveBRModal();
                    getBatchRequisitionById(brId);
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    //$window.location.reload();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeReceiveBRModal = function () {
                $('#receiveBrModal').modal('toggle');
            };

            //send to product br
            $scope.showSendToProductionBRModal = function (brModel) {
                $scope.selectedBR = brModel;
                $('#sendToProductionBrModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            var brSendToProduction = {};
            $scope.sendToProductionBrClick = function () {
                brSendToProduction.Id = $scope.batchRequisition.Id;
                brSendToProduction.EstimatedProductionDate = $scope.EstimatedProductionDate;
                authService.loadingOn();
                var promise = storeService.sendToProductionBR(brSendToProduction);
                promise.then(function (response) {
                    $scope.closeSendToProductionBRModal();
                    getBatchRequisitionById(brSendToProduction.Id);
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    //$window.location.reload();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeSendToProductionBRModal = function () {
                $('#sendToProductionBrModal').modal('toggle');
            };
        }]);