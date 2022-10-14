'use strict';

angular.module('AtlasPPS').controller('productReportListController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'authService', 'productService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, authService, productService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.productReportList = [];



            $scope.GetProductReport = function () {
                var vm = {
                    StartDate: moment($scope.reportDateRange.startDate).format('YYYY-MM-DD') != '1970-01-01' ? moment($scope.reportDateRange.startDate).format('YYYY-MM-DD') : moment(-1),
                    EndDate: moment($scope.reportDateRange.endDate).format('YYYY-MM-DD') != '1970-01-01' ? moment($scope.reportDateRange.endDate).format('YYYY-MM-DD') : moment(0)
                };
                authService.loadingOn();
                var promise = productService.getProductReportList(vm);
                promise.then(function (response) {
                    $scope.productReportList = response;
                    var totalAmount = 0;
                    if ($scope.productReportList && $scope.productReportList.length > 0) {
                        for (var i = 0; i < $scope.productReportList.length; i++) {
                            totalAmount = totalAmount + $scope.productReportList[i].Ammount;
                        }
                        $scope.SumOfTotalAmount = totalAmount;
                    }

                    authService.loadingOff();
                }, function (err) {
                    $scope.productReportList = [];
                    authService.loadingOff();
                });
            };


            $scope.PrintProductReport = function (id) {
                var printContents = document.getElementById(id).innerHTML;
                var popupWin = window.open('', '_blank', 'width=900,height=900');
                popupWin.document.open();
                popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="../../css/bootstrap.css" /></head>' +
                    '<body onload ="window.print()"><div class="text-center">' +
                    '<h2>' + $rootScope.companyName + '</h2>' +
                    '<p>' + $rootScope.companyAddress + '</p>' +
                    '<p>Email: ' + $rootScope.companyEmail + ', Contact: ' + $rootScope.companyContact + '</p>' +
                    '<h3 class="font-bold"><u>Product Report</u></h3></div></div>' +
                    '<table class="table table-striped table-bordered">' + printContents + '</table></body></html> ');
                popupWin.document.close();
            }

            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;
                    $rootScope.companyName = authData.companyName;
                    $rootScope.companyAddress = authData.companyAddress;
                    $rootScope.companyEmail = authData.companyEmail;
                    $rootScope.companyContact = authData.companyContact;
                    $rootScope.companyLogoPath = authData.companyLogoPath;

                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
            };
            pageLoad();

            $scope.reportDateRange = {
                startDate: moment(0).format('YYYY-MM-DD'),
                endDate: moment(0).format('YYYY-MM-DD')
            };
            $scope.singleDate = moment();

            $scope.opts = {
                locale: {
                    applyClass: 'btn-green',
                    applyLabel: "Apply",
                    fromLabel: "From",
                    format: "DD-MM-YYYY",
                    toLabel: "To",
                    cancelLabel: 'Cancel',
                    customRangeLabel: 'Custom range'
                },
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'As of Today': [moment(new Date(new Date().getFullYear() + "/1/1")), moment(new Date())]
                }
            };

            $scope.setRange = function () {
                $scope.reportDateRange = {
                    startDate: moment().format('YYYY-MM-DD'),
                    endDate: moment().format('YYYY-MM-DD')
                };
            };

            //Watch for date changes
            $scope.$watch('reportDateRange', function (newDate) {
            }, false);

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $('.md-datepicker-calendar-pane').css({ 'z-index': '2200' });
            $('.md-datepicker-button').css({ 'display': 'none' });
            $('.md-datepicker-input-container').css({ 'margin': '0', 'border-bottom-width': '0' });
            $('.md-datepicker-input-container > input').attr('disabled', true);
            $('.md-datepicker-input-container.md-datepicker-invalid').css({ 'border-bottom-color': 'none' });


            $scope.dtOptionsAll = DTOptionsBuilder.newOptions()
                .withOption('order', [0, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([]);

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