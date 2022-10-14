//'use strict';

angular.module('AtlasPPS').controller('reportController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'customerStatementRptService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder', 'ledgerService', 'customerService', '$compile', 'reportService', '$state',
        function ($scope, $rootScope, localStorageService, notificationService, $location, authService, customerStatementRptService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder, ledgerService, customerService, $compile, reportService, $state) {
            $scope.processComplated = true;
            $scope.company = [];
            $scope.customerStatement = [];
            $scope.customers = [];
            $scope.selectedCustomer = {};
            $scope.isReportGenerated = false;
            
            var getCustomerStatement = function () {
                var companyId = null;
                var customerId = null;
                var startDate = null;
                var endDate = null;

                if ($state.params && $state.params.companyId) {
                    companyId = _.parseInt($state.params.companyId);
                }
                if ($state.params && $state.params.customerId) {
                    customerId = _.parseInt($state.params.customerId);
                }
                if ($state.params && $state.params.startDate) {
                    startDate = $state.params.startDate;
                }
                if ($state.params && $state.params.endDate) {
                    endDate = $state.params.endDate;
                }

                var vm = {
                    companyId: companyId,
                    customerId: customerId,
                    startDate: moment(startDate).format('YYYY-MM-DD'),
                    endDate: moment(endDate).format('YYYY-MM-DD')
                };
                authService.loadingOn();
                var promise = customerStatementRptService.GetCustomerStatement(vm);
                promise.then(function (response) {
                    authService.loadingOff();
                    $('.footer').hide();
                    $scope.isReportGenerated = true;
                    $scope.company = response.company;
                    $scope.customerStatement = response.customerStatement;
                }, function (err) {
                    $scope.customerStatement = [];
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };
            getCustomerStatement();

            //$scope.loadReport = function () {
            //    $scope.customerStatement = reportService.getData();
            //}
            //$scope.loadReport();

            $scope.printStatement = function () {
                var printContent = $('#customerStatement').html();
                var originalContent = $('#customerStatementPageContent').html();
                $('.footer').hide();
                $('.navbar').hide();
                $('#customerStatementPageContent').html(printContent);
                window.print();
                $scope.$apply(function () {
                    $('#customerStatementPageContent').html($compile(originalContent));
                });

                var pdf = new jsPDF('p', 'mm', 'a4');
                var printContent = $('#customerStatement').html();
                var specialElementHandlers = {
                    '#bypassme': function (element, renderer) {
                        return true;
                    }
                };

                var margins = {
                    top: 50,
                    bottom: 50,
                    left: 50,
                    width: 522
                };

                pdf.fromHTML(
                    printContent,
                    margins.left,
                    margins.top, {
                        'width': margins.width,
                        'elementHandlers': specialElementHandlers
                    },
                    function (dispose) {
                        pdf.save('Test.pdf');
                    },
                    margins
                );

                var page = document.getElementById('customerStatement');

                html2canvas(page, {
                    onrendered: function (canvas) {
                        var img = canvas.toDataURL("image/png");
                        var doc = new jsPDF();
                        doc.addImage(img, 'JPEG', 5, 5);
                        doc.save('test.pdf');
                    }

                });
            }



            //var pageLoad = function () {
            //    if (!authService.isValidAuth()) {
            //        authService.logout();
            //        $location.path('/login');
            //    } else {
            //        var authData = localStorageService.get('authorizationData');
            //        fiscalYear = authData.fiscalYear;
            //        companyId = authData.companyId;
            //    }
            //    if ($rootScope.isAuthenticated('307') === false) {
            //        $location.path('/UnAuthorized');
            //    }
            //    authService.loadingOn();
            //    var promise = customerService.getCustomerList();
            //    promise.then(function (response) {
            //        authService.loadingOff();
            //        $scope.customers = response;
            //    }, function (err) {
            //        $scope.customers = [];
            //        notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
            //    });
            //};

            //pageLoad();

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