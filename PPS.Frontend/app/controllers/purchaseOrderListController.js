//'use strict';

angular.module('AtlasPPS').controller('purchaseOrderListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'purchaseOrderService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window','reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, purchaseOrderService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;
        
        $scope.poList = [];
        var getPurchaseOrderList = function () {
            authService.loadingOn();
            var promise = purchaseOrderService.getPurchaseOrderList();
            promise.then(function (response) {
                $scope.poList = response;
                authService.loadingOff();
            }, function (err) {
                $scope.poList = [];
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
            getPurchaseOrderList();
        };
        pageLoad();
        $scope.addNewPurchaseOrder = function () {
            $location.path("/Purchase/PurchaseOrder/Create");
        };
        $scope.navigateToPoEdit = function (p) {
            $location.path("/Purchase/PurchaseOrder/Edit/" + p.POId);
        };
        $scope.navigateToPoView = function (p) {
            $location.path("/Purchase/PurchaseOrder/View/" + p.POId);
        };
        $scope.navigateToPurchaseOrderPrint = function (p) {
            $window.open(reportSettings.reportBaseUri + 'reports/PurchaseOrderSinglePrint/' + p.POId, '_blank');
          
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
    }]);