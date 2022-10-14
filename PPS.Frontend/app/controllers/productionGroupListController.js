//'use strict';

angular.module('AtlasPPS').controller('productionGroupListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, storeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.productionGroupList = [];

            var getProductionGroupList = function () {
                authService.loadingOn();
                var promise = storeService.getProductionGroupList();
                promise.then(function (response) {
                    $scope.productionGroupList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productionGroupList = [];
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
                getProductionGroupList();
            };
            pageLoad();

            //$scope.addNewProductionGroup = function () {
            //    $location.path("/Store/ProductionGroup/Create");

            //};
            //$scope.navigateToPgEdit = function (b) {
            //    $location.path("/Store/ProductionGroup/Edit/" + b.Id);
            //};
            //$scope.navigateToPgView = function (b) {
            //    $location.path("/Store/ProductionGroup/View/" + b.Id);
            //};
            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });

            $scope.clickDatePicker = function () {
                $('.md-datepicker-input-container > input').attr('disabled', true);
            };

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withOption('order', [])
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $('.inner-table .html5buttons').css({ 'display': 'none' });
            $('.inner-table .dataTables_length').css({ 'display': 'none' });
            $('.inner-table .DataTables_Table_0_filter').css({ 'display': 'none' });

            // TODO: 
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });

            $scope.addNewProductionGroup = function () {
                $('#addProductionGroupModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            
            $scope.confirmAddProductionGroupClick = function () {
                authService.loadingOn();
                var promise = storeService.saveProductionGroup();
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.cancelAddProductionGroupModal();
                    getProductionGroupList();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.cancelAddProductionGroupModal = function () {
                $('#addProductionGroupModal').modal('toggle');
            };

            //Close Production Group
            $scope.showCloseProductionGroupModal = function (pgModel) {
                $scope.selectedProductionGroup = pgModel;
                $('#closeProductionGroupModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            //finished good entry close for this production group
            $scope.closeProductionGroupClick = function () {
                var pgId = $scope.selectedProductionGroup.Id;
                authService.loadingOn();
                var promise = storeService.closeProductionGroupClick(pgId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeCloseProductionGroupModal();
                    getProductionGroupList();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeCloseProductionGroupModal = function () {
                $('#closeProductionGroupModal').modal('toggle');
            };
        }]);