'use strict';

angular.module('AtlasPPS').controller('companySalesTargetListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'sharedService','reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, sharedService, reportSettings) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.companySalesTargetList = [];
           
            var getCompanySalesTargetList = function () {
                authService.loadingOn();
                var promise = salesService.getCompanySalesTargetList();
                promise.then(function (response) {
                    _.forEach(response,
                        function (d) {
                            d.TargetDate = new Date(d.TargetDate);
                        });
                    $scope.companySalesTargetList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.companySalesTargetList = [];
                    authService.loadingOff();
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
                getCompanySalesTargetList();
            };
            pageLoad();

            $scope.addNewCompanySalesTarget = function () {
                $location.path("/Sales/CompanySalesTarget/Create");
            };
            $scope.navigateToCompanySalesTargetEdit = function (s) {
                $location.path("/Sales/CompanySalesTarget/Edit/" + s.Id);
            };
            $scope.navigateToCompanySalesTargetView = function (s) {
                $location.path("/Sales/CompanySalesTarget/View/" + s.Id);
            };
            $scope.navigateToCompanySalesTargetPrint = function (s) {
                $window.open(reportSettings.reportBaseUri + 'reports/CompanySalesTargetPrint/' + s.Id, '_blank');
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
            
            //Approve Company Sales Target
            $scope.showApproveCompanySalesTargetModal = function (companySalesTargetModel) {
                $scope.selectedCompanySalesTarget = companySalesTargetModel;
                $('#approveCompanySalesTargetModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.approveCompanySalesTargetClick = function () {
                var companySalesTargetId = $scope.selectedCompanySalesTarget.Id;
                authService.loadingOn();
                var promise = salesService.approveCompanySalesTarget(companySalesTargetId);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.closeApproveCompanySalesTargetModal();
                    var selectedSalesTarget = _.filter($scope.companySalesTargetList,
                        function(x) {
                            return x.Id === companySalesTargetId;
                        });
                    if (selectedSalesTarget.length > 0) {
                        selectedSalesTarget[0].Status = "Approved";
                    }
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            };
            $scope.closeApproveCompanySalesTargetModal = function () {
                $('#approveCompanySalesTargetModal').modal('toggle');
            };

        }]);