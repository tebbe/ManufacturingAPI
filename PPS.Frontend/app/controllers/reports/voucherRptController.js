'use strict';

angular.module('AtlasPPS').controller('voucherRptController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'voucherRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'ledgerService', '$compile', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, voucherRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, ledgerService, $compile, $state) {

            $scope.company = null;
            $scope.voucher = null;

            var totalTransactionAmount = null;

            $scope.ReportHeaderSetting = {
                CompanyLogoIncludeInReport: PpsConstant.ReportHeaderSetting.CompanyLogoIncludeInReport
            };

            var getVoucherDetail = function () {
                var tranNo = null;

                if ($state.params && $state.params.tranNo) {
                    tranNo = $state.params.tranNo;
                }
                authService.loadingOn();

                var promise = voucherRptService.getVoucherDetail(tranNo);
                promise.then(function (response) {
                    authService.loadingOff();
                    $('.footer').hide();
                    $scope.company = response.company;
                    $scope.voucher = response.voucherDetail;
                    totalTransactionAmount = $scope.voucher.TransactionAmount;
                }, function (err) {
                    $scope.voucherDetail = {};
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };
            getVoucherDetail();

            $scope.convertAmountInWord = function () {
                var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
                var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];
                var number = parseFloat(totalTransactionAmount).toFixed(2).split(".");
                var num = parseInt(number[0]);
                var digit = parseInt(number[1]);
                if ((num.toString()).length > 9) return 'overflow';
                var n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
                var d = ('00' + digit).substr(-2).match(/^(\d{2})$/);;
                if (!n) return; var str = 'Taka ';
                str += n[1] != 0 ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
                str += n[2] != 0 ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
                str += n[3] != 0 ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
                str += n[4] != 0 ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
                str += n[5] != 0 ? (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + ' ' : '';
                str += d[1] != 0 ? str !== '' ? "and " : '' + (a[Number(d[1])] || b[d[1][0]] + ' ' + a[d[1][1]]) + ' ' : 'only';
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