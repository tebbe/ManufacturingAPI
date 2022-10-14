'use strict';

angular.module('AtlasPPS').controller('legalDocumentSinglePrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'legalDocumentService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$state', '$q', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, legalDocumentService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.legalDocument = [];
            var leDocId;
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
          

           

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

                if ($state.params && $state.params.Id) {
                    leDocId = _.parseInt($state.params.Id);
                }

                var getLegalDocSinglePrint = legalDocumentService.getLegalDocSinglePrint(leDocId);
                getLegalDocSinglePrint.then(function (response) {
                    $scope.legalDocument = response;
                }, function (err) {
                    $scope.legalDocument = [];
                });

                authService.loadingOn();
                $q.all([
                    getLegalDocSinglePrint]).then(function () {
                        authService.loadingOff();
                    });


            };

            pageLoad();

            $scope.navigateToLegalDocumentPrint = function (d) {
                $window.open(reportSettings.reportBaseUri + 'reports/LegalDocumentSinglePrint/' + d.Id, '_blank');
            };
            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.addPrdToGrid = function (event) {
                if (event.which === 13) {
                    $scope.addProductItem();
                }
            }

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