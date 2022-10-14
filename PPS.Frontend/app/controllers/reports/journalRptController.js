
'use strict';

angular.module('AtlasPPS').controller('journalRptController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'journalRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder',
    function ($scope, $rootScope, localStorageService, notificationService, $location, authService, journalRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var fiscalYear = null;
        var companyId = null;
        $scope.journalList = [];

        $scope.GetJournal = function () {
            var vm = {
                FiscalYear: fiscalYear,
                CompanyId: companyId,
                StartDate: moment($scope.date.startDate).format('YYYY-MM-DD'),
                EndDate: moment($scope.date.endDate).format('YYYY-MM-DD')
            };
            authService.loadingOn();
            var promise = journalRptService.GetJournal(vm);
            promise.then(function (response) {
                authService.loadingOff();
                $scope.journalList = response;
                makeGroupedTable();
            }, function (err) {
                $scope.journalList = [];
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
            });
        };

        var makeGroupedTable = function () {
            //var list = $('#journalEntryList table tr');
            if ($scope.journalList.JournalEntry && $scope.journalList.JournalEntry.length > 1) {
                var tempTransactionNumber = $scope.journalList.JournalEntry[0].TransactionNo;
                var count = 1;
                var countIndex = 0;
                for (var i = 1 ; i < $scope.journalList.JournalEntry.length ; i++) {
                    var prevTransactionNumber = $scope.journalList.JournalEntry[i].TransactionNo;
                    if (tempTransactionNumber === $scope.journalList.JournalEntry[i].TransactionNo) {
                        $scope.journalList.JournalEntry[i].TransactionDate = '';
                        $scope.journalList.JournalEntry[i].TransactionNo = '';
                        $scope.journalList.JournalEntry[i].TransactionType = '';
                    }
                    if (tempTransactionNumber != prevTransactionNumber) {
                        $scope.journalList.JournalEntry[countIndex].Count = count;
                        count = 1;
                        countIndex = i;
                    } else {
                        count++;
                    }
                    tempTransactionNumber = prevTransactionNumber;
                }
                if (tempTransactionNumber === prevTransactionNumber) {
                    $scope.journalList.JournalEntry[countIndex].Count = count;
                }
            }
        };

        $scope.PrintJurnalReport = function (id) {
            var printContents = document.getElementById(id).innerHTML;
            var popupWin = window.open('', '_blank','');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="../../css/bootstrap.min.css" />' +
                '<body onload ="window.print()"><div class="text-center">' +
                '<h2>' + $rootScope.companyName + '</h2>' +
                '<p>' + $rootScope.companyAddress + '</p>' +
                '<p>Email: ' + $rootScope.companyEmail + ', Contact: ' + $rootScope.companyContact + '</p>' +
                '<h3 class="font-bold"><u>Day Book</u></h3></div></div>' +
                '<table class="table table-striped table-bordered table-hover">' + printContents + '</table></body></html> ');
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
                $rootScope.userFullName = authData.fullName;
                $rootScope.companyName = authData.companyName;
                $rootScope.companyAddress = authData.companyAddress;
                $rootScope.companyEmail = authData.companyEmail;
                $rootScope.companyContact = authData.companyContact;
                $rootScope.companyLogoPath = authData.companyLogoPath;
            }
            if ($rootScope.isAuthenticated('301') === false) {
                $location.path('/UnAuthorized');
            }
        };
        pageLoad();

        $scope.date = {
            startDate: moment(),
            endDate: moment()
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
            $scope.date = {
                startDate: moment("en"),
                endDate: moment("en")
            };
        };

        //Watch for date changes
        $scope.$watch('date', function (newDate) {
            console.log('New date set: ', newDate);
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