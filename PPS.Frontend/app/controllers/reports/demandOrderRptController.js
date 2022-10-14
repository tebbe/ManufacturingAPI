'use strict';

angular.module('AtlasPPS').controller('demandOrderRptController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'demandOrderRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'ledgerService', '$compile', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, demandOrderRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, ledgerService, $compile, $state) {
            $scope.company = null;
            $scope.demandOrder = null;
            $scope.TotalQuantity = 0;
            $scope.finalAmount = 0;
            $scope.subTotal = 0;

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

            var getDemandOrderById = function () {
                var doId = null;
                if ($state.params && $state.params.doId) {
                    doId = $state.params.doId;
                }
                authService.loadingOn();

                var promise = demandOrderRptService.getDemandOrderById(doId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $('.footer').hide();
                    $scope.demandOrder = response.demandOrder;
                    $scope.company = response.company;
                    finalAmountCalculation();
                }, function (err) {
                    $scope.demandOrder = {};
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };

            getDemandOrderById();
            
            var finalAmountCalculation = function () {
                var tUnitAmount = 0;
                var finalDiscount = 0;
                var totalQuantity = 0;

                var regularDiscount = $scope.demandOrder.RegularDiscountInPercentage;
                if (isNaN(regularDiscount)) {
                    regularDiscount = 0;
                }
                var specialDiscount = $scope.demandOrder.SpecialDiscountInPercentage;
                if (isNaN(specialDiscount)) {
                    specialDiscount = 0;
                }
                var additionDiscount = $scope.demandOrder.AdditionalDiscountInPercentage;
                if (isNaN(additionDiscount)) {
                    additionDiscount = 0;
                }
                var extraDiscount = $scope.demandOrder.ExtraDiscountInPercentage;
                if (isNaN(extraDiscount)) {
                    extraDiscount = 0;
                }
                var cashBackAmount = $scope.demandOrder.CashBackAmount;
                if (isNaN(cashBackAmount)) {
                    cashBackAmount = 0;
                }

                if ($scope.demandOrder && $scope.demandOrder.DemandOrderDetail) {
                    for (var prd in $scope.demandOrder.DemandOrderDetail) {
                        if ($scope.demandOrder.DemandOrderDetail.hasOwnProperty(prd)) {
                            tUnitAmount += $scope.demandOrder.DemandOrderDetail[prd].TotalPrice;
                            totalQuantity = totalQuantity + $scope.demandOrder.DemandOrderDetail[prd].Quantity;
                        }
                    }
                }
                $scope.TotalQuantity = totalQuantity;
                $scope.demandOrder.TotalAmount = tUnitAmount;
                $scope.subTotal = tUnitAmount;

                if (regularDiscount !== 0) {
                    var rgdiscount = regularDiscount / 100;
                    $scope.demandOrder.RegularDiscountAmount = $scope.demandOrder.TotalAmount * rgdiscount;
                    $scope.subTotal = $scope.demandOrder.TotalAmount - $scope.demandOrder.RegularDiscountAmount;
                    finalDiscount += $scope.demandOrder.RegularDiscountAmount;
                }
                else {
                    $scope.demandOrder.RegularDiscountAmount = 0;
                }
                if (specialDiscount !== 0) {
                    var spDiscount = specialDiscount / 100;
                    $scope.demandOrder.SpecialDiscountAmount = $scope.subTotal * spDiscount;
                    finalDiscount += $scope.demandOrder.SpecialDiscountAmount;
                }
                else {
                    $scope.demandOrder.SpecialDiscountAmount = 0;
                }
                if (additionDiscount !== 0) {
                    var addDiscount = additionDiscount / 100;
                    $scope.demandOrder.AdditionalDiscountAmount = $scope.subTotal * addDiscount;
                    finalDiscount += $scope.demandOrder.AdditionalDiscountAmount;
                }
                else {
                    $scope.demandOrder.AdditionalDiscountAmount = 0;
                }
                if (extraDiscount !== 0) {
                    var extDiscount = extraDiscount / 100;
                    $scope.demandOrder.ExtraDiscountAmount = $scope.subTotal * extDiscount;
                    finalDiscount += $scope.demandOrder.ExtraDiscountAmount;
                }
                else {
                    $scope.demandOrder.ExtraDiscountAmount = 0;
                }

                if (cashBackAmount !== 0) {
                    var cbAmount = cashBackAmount;
                    $scope.demandOrder.CashBackAmount = cbAmount;
                    finalDiscount += $scope.demandOrder.CashBackAmount;
                }
                else {
                    $scope.demandOrder.CashBackAmount = 0;
                }
                $scope.demandOrder.TotalDiscountAmount = finalDiscount;
                $scope.demandOrder.TotalGrandAmount = Math.round($scope.subTotal - finalDiscount + $scope.demandOrder.RegularDiscountAmount);
                $scope.finalAmount = $scope.demandOrder.TotalGrandAmount;
            };

            $scope.convertAmountInWord = function () {
                var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
                var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];
                var number = parseFloat($scope.finalAmount).toFixed(2).split(".");
                var num = parseInt(number[0]);
                var digit = parseInt(number[1]);
                if ((num.toString()).length > 9) return 'overflow';
                var n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
                var d = ('00' + digit).substr(-2).match(/^(\d{2})$/);;
                if (!n) return; var str = 'Taka ';
                str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
                str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
                str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
                str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
                str += (n[5] != 0) ? (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + ' ' : '';
                str += (d[1] != 0) ? ((str != '') ? "and " : '') + (a[Number(d[1])] || b[d[1][0]] + ' ' + a[d[1][1]]) + ' ' : 'only';
                return str;
            }
            //$scope.convertAmountInWord();
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