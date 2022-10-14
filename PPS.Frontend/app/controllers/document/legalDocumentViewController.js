'use strict';

angular.module('AtlasPPS').controller('legalDocumentViewController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService',
        'authService', 'legalDocumentService', 'ngAuthSettings', 'DTOptionsBuilder',
        'PpsConstant', '$state', '$q', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location,
            notificationService, authService, legalDocumentService,
            ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q,
            $window, reportSettings) {

            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
        
            $scope.company = [];
            $scope.legalDocumentType = [];
            $scope.legalDocumentRenewalCategory = [];
            $scope.legalDocumentStatus = [];

            $scope.modelHeading = "View Legal Document";
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Edit";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            var leDocId;

            $scope.selectedDate = {
                selected: null
            }
          
           
            $scope.legalDocument = [];

           

            var fillDataFunction = function () {
                $scope.legalDocument.IssueDate = new Date($scope.legalDocument.IssueDate)
                $scope.legalDocument.ExpireDate = new Date($scope.legalDocument.ExpireDate);
            }

            var clearField;

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
              
                var getLegalDocById = legalDocumentService.getLegalDocumentById(leDocId);
                getLegalDocById.then(function (response) {
                    $scope.legalDocument = response;
                }, function (err) {
                    $scope.legalDocument = [];
                });

               

                authService.loadingOn();
                $q.all([                        
                    getLegalDocById]).then(function () {
                        fillDataFunction();
                        authService.loadingOff();
                    });


            };
         
            pageLoad();
            $scope.gotoLegalDocEdit = function () {
                $location.path("/Document/LegalDocument/Edit/" + leDocId);
                $window.location.reload();
            };
            $scope.gotoLegalDocList = function () {
                $location.path("/Document/LegalDocumentList")
                $window.location.reload();
            }
            $scope.navigateToLegalDocumentSinglePrint = function () {
                $window.open(reportSettings.reportBaseUri + 'reports/LegalDocumentSinglePrint/' + leDocId, '_blank');
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
        }]);