'use strict';
angular.module('AtlasPPS').controller('legalDocumentListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'legalDocumentService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, legalDocumentService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.doList = [];
            $scope.doListUnPaid = [];
            $scope.doListPartiallyPaid = [];
            $scope.doListPaid = [];
            var getLegalDocumentList = function () {
                authService.loadingOn();
                var promise = legalDocumentService.getLegalDocumentList();
                promise.then(function (response) {
                    $scope.doList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.doList = [];
                    authService.loadingOff();
                });
            };
            $scope.countExpireDate = function (date, id) {
                $scope.count++;
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1;
                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }
                today = yyyy + '/' + mm + '/' + dd;
                $scope.today = today;
                var currentdate = new Date(today);
                var expireDate = new Date(date);
                var expireYear = expireDate.getFullYear();
                if (expireDate.getTime() && expireYear!=1970) {
                    var timeDiff =expireDate.getTime() - currentdate.getTime();
                    $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));

                    if ($scope.dayDifference <= 60 && $scope.dayDifference > 7) {
                        angular.element("#expireStyle_" + id).css("color", "#ffcc00");
                        angular.element("#expireStyle2_" + id).css("color", "#ffcc00");

                    } else if ($scope.dayDifference <= 7) {
                        angular.element("#expireStyle_" + id).css("color", "red");
                        angular.element("#expireStyle2_" + id).css("color", "red");
                    }

                } else {
                    $scope.dayDifference = 'N/A';
                }

                return $scope.dayDifference;
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
                getLegalDocumentList();

            };
            pageLoad();

            $scope.addNewLegalDocument = function () {
                $location.path("/Document/LegalDocument/Create");
            };
            $scope.navigateToLegalDocumentEdit = function (d) {
                $location.path("/Document/LegalDocument/Edit/" + d.Id);
            };
            $scope.navigateToLegalDocumentView = function (d) {
                $location.path("/Document/LegalDocument/View/" + d.Id);
            };
            $scope.navigateToLegalDocumentSinglePrint = function (d) {
                $window.open(reportSettings.reportBaseUri + 'reports/LegalDocumentSinglePrint/' + d.Id, '_blank');
            };
            $scope.navigateToLegalDocListPrint = function () {
                $window.open(reportSettings.reportBaseUri + 'reports/LegalDocumentListPrint', '_blank');
            };
            $scope.viewLegalDocHistory = function (d) {
                $window.open(reportSettings.reportBaseUri + 'Document/LegalDocument/History/' + d.Id, '_blank');
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

            $scope.dtOptionsAll = DTOptionsBuilder.newOptions()
                .withOption('order', [0, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withOption('order', [2, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
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