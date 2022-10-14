'use strict';

angular.module('AtlasPPS').controller('monthlySalesProcessController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'monthlySalesProcessService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'approvalService', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, monthlySalesProcessService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, approvalService, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.monthList = ["Jan", "Feb", "Mar", "Apr", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            $scope.yearList = [];
            var reprocessYear = null;
            var reprocessMonth = null;

            $scope.monthlySalesProcessClick = function () {                
                authService.loadingOn();
                var monthNumber = _.findIndex($scope.monthList, function (m) {
                    return $scope.selectedMonth === m;
                });
                var promise = monthlySalesProcessService.saveMonthlySalesProcessing($scope.selectedYear, monthNumber + 1);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeAddMonthlySalesProcessModal();
                    getMonthlySalesProcessingList();
                }, function (err) {
                    authService.loadingOff();
                });
            };
            
            $scope.closeAddMonthlySalesProcessModal = function () {
                $('#addMonthlySalesProcess').modal('toggle');
            };
            $scope.showMonthlySalesProcessModal = function (tran) {
                var date = new Date();
                var monthNumber = date.getMonth();
                $scope.selectedMonth = $scope.monthList[monthNumber];
                $scope.selectedYear = date.getFullYear();
                $('#addMonthlySalesProcess').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };

            $scope.reprocessMonthlySalesProcessingClick = function () {
                authService.loadingOn();
                var promise = monthlySalesProcessService.reprocessMonthlySalesProcessing(reprocessYear, reprocessMonth);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeReprocessMonthlySalesProcessingModal();
                    getMonthlySalesProcessingList();
                }, function (err) {
                    authService.loadingOff();
                });
            };

            $scope.closeReprocessMonthlySalesProcessingModal = function () {
                $('#reprocessMonthlySalesProcessingModal').modal('toggle');
            };
            $scope.showReprocessMonthlySalesProcessingModal = function (process) {
                reprocessYear = process.Year;
                reprocessMonth = process.Month;
                $('#reprocessMonthlySalesProcessingModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            
            var getMonthlySalesProcessingList = function () {
                authService.loadingOn();
                var promise = monthlySalesProcessService.getMonthlySalesProcessingList();
                promise.then(function (response) {
                    $scope.monthlySalesProcessingList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.monthlySalesProcessingList = [];
                    authService.loadingOff();
                });
            };

            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    //fiscalYear = authData.fiscalYear;
                    //companyId = authData.companyId;
                    //$rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
                var date = new Date();
                var currentYear = date.getFullYear();
                for (var i = currentYear - 1; i < currentYear + 2; i++) {
                    $scope.yearList.push(i);
                }
                //$scope.yearList.push(currentYear);
                //$scope.yearList.push(currentYear + 1);
                getMonthlySalesProcessingList();
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
                .withOption('order', [])
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