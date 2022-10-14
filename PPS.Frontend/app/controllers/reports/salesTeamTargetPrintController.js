//'use strict';

angular.module('AtlasPPS').controller('salesTeamTargetPrintController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'customerStatementRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'salesService', '$compile', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, customerStatementRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, salesService, $compile, $state) {
            $scope.processComplated = true;
            $scope.salesTeamTargetList = [];
            $scope.isReportGenerated = false;

            var getSalesTeamTargetList = function () {
                var year = null;
                var month = null;

                if ($state.params && $state.params.year) {
                    year = _.parseInt($state.params.year);
                }
                if ($state.params && $state.params.month) {
                    month = _.parseInt($state.params.month);
                }

                authService.loadingOn();
                var promise = salesService.getSalesTeamTargetList(year, month);
                promise.then(function (response) {
                    _.forEach(response,
                        function (d) {
                            d.TargetDate = new Date(d.TargetDate);
                        });
                    $scope.salesTeamTargetList = response;
                    
                    $('.footer').hide();
                    $scope.isReportGenerated = true;
                    
                    authService.loadingOff();
                }, function (err) {
                    $scope.salesTeamTargetList = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            getSalesTeamTargetList();


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

            // TODO: 
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);
        }]);