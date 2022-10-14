//'use strict';

angular.module('AtlasPPS').controller('individualLedgerRptController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'individualLedgerRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'ledgerService',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, individualLedgerRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, ledgerService) {
            $scope.processComplated = true;
            var makeGroupedTable;
            var hasTransaction = false;
            var fiscalYear = null;
            var companyId = null;
            var headId = null;
            $scope.individualLedgerList = [];
            $scope.accountsHead = [];
            //$scope.selectedHead = {};

            $scope.selectedHead = {
                selected: null
            }

            $scope.GetIndividualLedger = function () {
                var vm = {
                    FiscalYear: fiscalYear,
                    CompanyId: companyId,
                    HeadId: $scope.selectedHead.selected.HeadId,
                    StartDate: moment($scope.reportDateRange.startDate).format('YYYY-MM-DD'),
                    EndDate: moment($scope.reportDateRange.endDate).format('YYYY-MM-DD')
                };
                authService.loadingOn();
                var promise = individualLedgerRptService.GetIndividualLedger(vm);

                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.individualLedgerList = response;
                    makeGroupedTable();
                }, function (err) {
                    $scope.individualLedgerList = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };

            var makeGroupedTable = function () {
                if ($scope.individualLedgerList.Detail.length > 1) {
                    var tempTransactionNumber = $scope.individualLedgerList.Detail[0].TransactionNo;
                    var count = 1;
                    var countIndex = 0;
                    for (var i = 1; i < $scope.individualLedgerList.Detail.length; i++) {
                        var prevTransactionNumber = $scope.individualLedgerList.Detail[i].TransactionNo;
                        if (tempTransactionNumber === $scope.individualLedgerList.Detail[i].TransactionNo) {
                            $scope.individualLedgerList.Detail[i].TransactionNo = '';
                            $scope.individualLedgerList.Detail[i].TransactionDate = '';
                           
                        }
                        if (tempTransactionNumber != prevTransactionNumber) {
                            $scope.individualLedgerList.Detail[countIndex].Count = count;
                            count = 1;
                            countIndex = i;
                        } else {
                            count++;
                        }
                        tempTransactionNumber = prevTransactionNumber;
                    }
                    if (tempTransactionNumber === prevTransactionNumber) {
                        $scope.individualLedgerList.Detail[countIndex].Count = count;
                    }
                }
            };
            $scope.IndividualLedgerReport = function (id) {
                var printContents = document.getElementById(id).innerHTML;
                var popupWin = window.open('', '_blank', 'width=900,height=900');
                popupWin.document.open();
                popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="../../css/bootstrap.css" /></head>' +
                    '<body onload ="window.print()"><div class="text-center">' +
                    '<h2>' + $rootScope.companyName + '</h2>' +
                    '<p>' + $rootScope.companyAddress + '</p>' +
                    '<p>Email: ' + $rootScope.companyEmail + ', Contact: ' + $rootScope.companyContact + '</p>' +
                    '<h3 class="font-bold"><u>Individual Ledger Report</u></h3></div></div>' +
                    '<table class="table table-striped table-bordered">' + printContents + '</table></body></html> ');
                popupWin.document.close();
            }   
            var pageLoad = function () {
                if (!authService.isValidAuth()) {
                    authService.logout();
                    $location.path('/login');
                } else {
                    var authData = localStorageService.get('authorizationData');
                    fiscalYear = authData.fiscalYear;
                    companyId = authData.companyId;
                    $rootScope.companyName = authData.companyName;
                    $rootScope.companyAddress = authData.companyAddress;
                    $rootScope.companyEmail = authData.companyEmail;
                    $rootScope.companyContact = authData.companyContact;
                    $rootScope.companyLogoPath = authData.companyLogoPath;

                }
                if ($rootScope.isAuthenticated('303') === false) {
                    $location.path('/UnAuthorized');
                }
                authService.loadingOn();
                var promise = ledgerService.getAccountHeadList(fiscalYear, companyId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.accountsHead = response;
                    //console.log(response);
                    
                }, function (err) {
                    $scope.accountsHead = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };

            pageLoad();

            $scope.reportDateRange = {
                startDate: moment().format('YYYY-MM-DD'),
                endDate: moment().format('YYYY-MM-DD')
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
                //console.log('New date set: ', newDate);
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