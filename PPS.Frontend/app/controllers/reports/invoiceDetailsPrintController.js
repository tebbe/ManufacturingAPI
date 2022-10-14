'use strict';

angular.module('AtlasPPS').controller('invoiceDetailsPrintController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'salesService', 'ngAuthSettings', 'DTOptionsBuilder', '$state', 'PpsConstant', '$window', 'sharedService', 'bankService',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, salesService, ngAuthSettings, DTOptionsBuilder, $state, PpsConstant, $window, sharedService, bankService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $scope.company = null;
            $scope.invoice = null;
            $scope.TotalQuantity = 0;

            $scope.DemandOrderDiscountSetting = {
                RegularDiscount: PpsConstant.DemandOrderDiscountSetting.RegularDiscount,
                SpecialDiscount: PpsConstant.DemandOrderDiscountSetting.SpecialDiscount,
                AdditionalDiscount: PpsConstant.DemandOrderDiscountSetting.AdditionalDiscount,
                ExtraDiscount: PpsConstant.DemandOrderDiscountSetting.ExtraDiscount,
                CashBack: PpsConstant.DemandOrderDiscountSetting.CashBack
            };
            $scope.ReportHeaderSetting = {
                CompanyLogoIncludeInReport: PpsConstant.ReportHeaderSetting.CompanyLogoIncludeInReport
            };
                
            $scope.invoice = [];

            var getInvoiceById = function (invoiceId) {
                authService.loadingOn();
                var promise = salesService.getInvoiceById(invoiceId);
                promise.then(function (response) {
                    $('.footer').hide();
                    $scope.company = response.company;
                    $scope.invoice = response.invoice;
                    calculateTotal();
                    authService.loadingOff();
                }, function (err) {
                    $scope.invoice = [];
                    authService.loadingOff();
                });
            };

            var calculateTotal = function () {
                var totalQty = 0;
                if ($scope.invoice.InvoiceDetail) {
                    for (var item in $scope.invoice.InvoiceDetail) {
                        if ($scope.invoice.InvoiceDetail.hasOwnProperty(item)) {
                            totalQty += $scope.invoice.InvoiceDetail[item].AllocatedQuantity;
                        }
                    }
                }
                $scope.TotalQuantity = totalQty;
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

                var invoiceId;
                if ($state.params && $state.params.Id) {
                    invoiceId = _.parseInt($state.params.Id);
                }
                getInvoiceById(invoiceId);
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

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                ]);

        }]);