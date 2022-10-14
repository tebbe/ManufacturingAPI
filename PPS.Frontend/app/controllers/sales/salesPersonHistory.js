'use strict';

angular.module('AtlasPPS').controller('salesPersonHistoryController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'employeeService',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, $state, employeeService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            var employeeId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.hasLoaded = false;

            $scope.salesOfficer = [];
            $scope.salesPersonHistory = null;
            $scope.salesPerson = {};
            $scope.selectedSalesPerson = {
                selected: null
            };

            var getSalesOfficer = function () {
                authService.loadingOn();
                var promise = employeeService.getSalesOfficer();
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.salesOfficer = response;
                }, function (err) {
                    $scope.salesOfficer = [];
                });
            };

            $scope.getSalesPersonHistory = function () {
                authService.loadingOn();
                employeeId = $scope.selectedSalesPerson.selected.Id;
                var promise = salesService.getSalesPersonHistory(employeeId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.salesPersonHistory = response;
                }, function (err) {
                    $scope.salesPersonHistory = {};
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
                getSalesOfficer();
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

            //$scope.clickDatePicker = function () {
            //    $('.md-datepicker-input-container > input').attr('disabled', true);
            //};

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withOption('order', [])
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);


            var calculationTotalAmount = function () {
                var totalDoAmount = 0;
                var totalPaidAmount = 0;
                var percentage = 0;
            }
        }]);