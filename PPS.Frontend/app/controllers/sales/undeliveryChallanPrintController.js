'use strict';

angular.module('AtlasPPS').controller('undeliveryChallanPrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, salesService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.modelHeading = "Delivery Challan";
            $scope.unDeliveryChallan = [];
            $scope.company = null;
            $scope.TotalQuantity = null;

            var ChallanId = null;
            $scope.ReportHeaderSetting = {
                CompanyLogoIncludeInReport: PpsConstant.ReportHeaderSetting.CompanyLogoIncludeInReport
            }

            var getUnDeliveryChallanById = function () {
                authService.loadingOn();
                var promise = salesService.getInvoiceUnDeliveryChallanById(ChallanId);
                promise.then(function (response) {
                    $scope.unDeliveryChallan = response.undeliveryChallan;
                    $scope.company = response.company;
                    $('.footer').hide();
                    authService.loadingOff();
                }, function (err) {
                    $scope.unDeliveryChallan = [];
                    authService.loadingOff();
                });
            };

            var calculateTotal = function () {
                var totalQty = 0;
                if ($scope.unDeliveryChallan.UndeliveryQuantityDetailVm) {
                    for (var item in $scope.unDeliveryChallan.UndeliveryQuantityDetailVm) {
                        if ($scope.unDeliveryChallan.UndeliveryQuantityDetailVm.hasOwnProperty(item)) {
                            totalQty += $scope.unDeliveryChallan.UndeliveryQuantityDetailVm[item].Quantity;
                        }
                    }
                }
                $scope.TotalQuantity = totalQty;
            };

            if ($state.params && $state.params.Id) {
                ChallanId = _.parseInt($state.params.Id);
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
                getUnDeliveryChallanById();
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