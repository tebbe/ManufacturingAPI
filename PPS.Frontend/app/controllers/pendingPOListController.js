//'use strict';

angular.module('AtlasPPS').controller('pendingPOListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'storeService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, storeService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var isAcceptedPOListLoaded = false;
            var isPendingPOListLoaded = false;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.pendingPOList = [];
            $scope.acceptedPOList = [];

            function getPendingPOList() {
                if (isPendingPOListLoaded === false) {
                    authService.loadingOn();
                    var promise = storeService.getPendingPOList();
                    promise.then(function (response) {
                        $scope.pendingPOList = response;
                            authService.loadingOff();
                    },
                        function (err) {
                            $scope.pendingPOList = [];
                            authService.loadingOff();
                        });
                    isPendingPOListLoaded = true;
                }
            };

            var getAcceptedPOList = function () {
                if (isAcceptedPOListLoaded === false) {
                    authService.loadingOn();
                    var promise = storeService.getAcceptedPOList();
                    promise.then(function (response) {
                        $scope.acceptedPOList = response;
                        $("#DataTables_Table_1 th:first-child").addClass("sorting_desc");
                        authService.loadingOff();
                    }, function (err) {
                        $scope.acceptedPOList = [];
                        authService.loadingOff();
                    });
                    isAcceptedPOListLoaded = true;
                }
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
                getPendingPOList();
            };
            pageLoad();

            $scope.getAcceptedPOList = function () {
                getAcceptedPOList();
            };
            $scope.getPendingPOList = function () {
                getPendingPOList();
            };

            $scope.navigateToPendingPoView = function (p) {
                $location.path("/Store/PendingPOList/View/" + p.POId);
            };

            $scope.navigateToAcceptedPoView = function (p) {
                $location.path("/Store/PendingPOList/View/" + p.POId);
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