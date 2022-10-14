'use strict';

angular.module('AtlasPPS').controller('rawMatrialOpeningController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService',
        'authService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder',
        'PpsConstant', '$state', '$q', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location,
            notificationService, authService, storeService,
            ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q,
            $window, reportSettings) {

            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.rawMatrialOpeningReportList = [];

            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }

               
                var getRawMatrialOpeningReportList = storeService.getRawMaterialType();
                getRawMatrialOpeningReportList.then(function (response) {
                    $scope.rawMatrialOpeningReportList = response;
                  
                }, function (err) {
                    $scope.rawMatrialOpeningReportList = [];
                });

                authService.loadingOn();
                $q.all([
                    getRawMatrialOpeningReportList]).then(function () {
                        authService.loadingOff();
                    });
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