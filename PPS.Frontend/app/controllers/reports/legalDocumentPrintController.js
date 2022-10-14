'use strict';

angular.module('AtlasPPS').controller('legalDocumentPrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'legalDocumentService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$state', '$q', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, legalDocumentService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $state, $q, $window) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.legalDoc = [];
           
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            

            $scope.countExpireDate = function (date, id) {
                $scope.count++;
                var today = new Date();
                var dd = today.getDate() + 1;
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
                if (expireDate.getTime()) {
                    var timeDiff = Math.abs(currentdate.getTime() - expireDate.getTime());
                    $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
                } else {
                    $scope.dayDifference = 'N/A';
                }

                if ($scope.dayDifference <= 60 && $scope.dayDifference > 7) {
                    angular.element("#expireStyle_" + id).css("color", "#ffcc00");
                } else if ($scope.dayDifference <= 7) {
                    angular.element("#expireStyle_" + id).css("color", "red");
                }

                return $scope.dayDifference;
            }

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

               
                var getLegalDocListPrint = legalDocumentService.getLegalDocListPrint();
                getLegalDocListPrint.then(function (response) {
                    $scope.legalDoc = response;
                }, function (err) {
                    $scope.legalDoc = [];
                });
              

                authService.loadingOn();
                $q.all([
                    getLegalDocListPrint]).then(function () {
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