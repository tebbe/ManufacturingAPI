'use strict';

angular.module('AtlasPPS').controller('paymentViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService',
        'authService', 'transactionService', 'ngAuthSettings', 'DTOptionsBuilder',
        'PpsConstant', '$state', '$q', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location,
            notificationService, authService, transactionService,
            ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q,
            $window, reportSettings) {

            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            var transactionNo = "";
            $scope.moduleText = "";
            $scope.modelHeading = "Transaction";
            $scope.modelActionText = "View";
            $scope.modelDetailText = "DETAILS";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            $scope.transactionDetails = [];
            $scope.voucherDetail = [];

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

                if ($state.params && $state.params.tranNo) {
                    transactionNo = $state.params.tranNo;
                    $scope.moduleText = $state.params.text;
                }

                var getPaymentTransaction = transactionService.getPaymentTransactionDetails(transactionNo);
                getPaymentTransaction.then(function (response) {
                    $scope.transactionDetails = response;
                    $scope.voucherDetail = response.voucherDetail.VoucherDetail;
                }, function (err) {
                    $scope.transactionDetails = [];
                    $scope.voucherDetail = [];
                });

                authService.loadingOn();
                $q.all([
                    getPaymentTransaction]).then(function () {
                        authService.loadingOff();
                    });
            };

            pageLoad();
            $scope.gotoPaymentTransactionList = function () {
                if ($scope.moduleText === "journal") {
                    $location.path("/transaction/journal");
                } else if ($scope.moduleText === "receipt") {
                    $location.path("/transaction/receipt");
                }
                else if ($scope.moduleText === "contra") {
                    $location.path("/transaction/Contra");
                } else if ($scope.moduleText === "payment"){
                    $location.path("/transaction/payment");
                }
                
              
            }
           

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