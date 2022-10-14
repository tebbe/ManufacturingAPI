'use strict';

angular.module('AtlasPPS').controller('deliveryChallanViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state','reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.modelHeading = "Delivery Challan";
            $scope.deliveryChallan = [];

            var ChallanId = null;

            var getDeliveryChallanById = function () {
                authService.loadingOn();
                var promise = salesService.getInvoiceDeliveryChallanById(ChallanId);
                promise.then(function (response) {
                    $scope.deliveryChallan = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.deliveryChallan = [];
                    authService.loadingOff();
                });
            };

        
         
            $scope.navigateToDeliveryChallanEdit = function (data) {
                //$location.path("/Invoice/DeliveryChalan/Edit/" + data.Id);
                $window.open(reportSettings.reportBaseUri + 'Invoice/DeliveryChalan/Edit/' + data.Id, '_blank');
            };
            $scope.navigateToDeliveryChallanPrint = function (data) {
                //$location.path("/Invoice/DeliveryChalan/Print/" + data.Id);
                $window.open(reportSettings.reportBaseUri + 'Invoice/DeliveryChalan/Print/' + data.Id, '_blank');
            };
           
            $scope.approveDeliveryChallan = function () {
                authService.loadingOn();
                var promise = salesService.invoiceDeliveryChallanApprove(ChallanId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    //$location.path("/Invoice/DeliveryChalan/View/" + response.Id);
                    $window.open(reportSettings.reportBaseUri + 'Invoice/DeliveryChalan/View/' + response.Id, '_blank');
                }, function (err) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };


            if ($state.params && $state.params.Id) {
                ChallanId = _.parseInt($state.params.Id);
            }
            

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
                getDeliveryChallanById();
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
        }]);