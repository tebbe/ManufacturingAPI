//'use strict';

angular.module('AtlasPPS').controller('batchRequisitionListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, storeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.batchRequisitionList = [];

            var getBatchRequisitionList = function () {
                authService.loadingOn();
                var promise = storeService.getBatchRequisitionList();
                promise.then(function (response) {
                    $scope.batchRequisitionList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.batchRequisitionList = [];
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
                getBatchRequisitionList();
            };
            pageLoad();
            $scope.addNewBatchRequisition = function () {
                $location.path("/Store/BatchRequisition/Create");
            };
            $scope.navigateToBrEdit = function (b) {
                $location.path("/Store/BatchRequisition/Edit/" + b.Id);
            };
            $scope.navigateToBrView = function (b) {
                $location.path("/Store/BatchRequisition/View/" + b.Id);
            };
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

            //Close Batch
            $scope.showCloseBatchModal = function (brModel) {
                $scope.selectedBr = brModel;
                $('#closeBatchModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            //finished good entry close for this batch
            $scope.closeBatchClick = function () {
                var brId = $scope.selectedBr.Id;
                authService.loadingOn();
                var promise = storeService.closeBatchClick(brId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $window.location.reload();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeCloseBatchModal = function () {
                $('#closeBatchModal').modal('toggle');
            };
        }]);