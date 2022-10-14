'use strict';
angular.module('AtlasPPS').controller('paymentAddController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'transactionService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings','$q',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, transactionService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, reportSettings, $q) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $scope.isNew = false;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.tranMode = 1;
            $scope.modelActionText = "Add Transation";
            $scope.modelDetailText = "Add to List";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            $scope.tranAmountType = 1;
            $scope.tranAmount = 0;
            $scope.totalDrAmount = 0;
            $scope.totalCrAmount = 0;
            $scope.Note = "";
            var transactionType = PpsConstant.TransactionType.Payment;
            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;
            $scope.accountsHead = [];
            $scope.transactionsModel = [];
            
            $scope.selectedTransaction = {
                TranNo: 'New',
                TranDate: new Date(),
                TransactionDetail: [],
                Particulars: null
            };
            $scope.transactions = [];
            $scope.selectedHead = {
                selected: null
            };

            $scope.selectTransactionDetail = function (tran) {
                if ($scope.selectedTransaction && $scope.selectedTransaction.TransactionDetail) {
                    for (var i = 0; i < $scope.selectedTransaction.TransactionDetail.length; i++) {
                        if ($scope.selectedTransaction.TransactionDetail[i].TranHeadId === tran.TranHeadId) {
                            selectedModelDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }

              

                var result = $scope.accountsHead.filter(function (v) {
                    return v.HeadId === tran.TranHeadId;
                });

                if (result && result.length > 0) {
                    $scope.selectedHead.selected = result[0];
                }

                if (tran.DrAmount > 0) {
                    $scope.tranAmountType = PpsConstant.TranAmountType.Debit;
                    $scope.tranAmount = tran.DrAmount;
                } else {
                    $scope.tranAmountType = PpsConstant.TranAmountType.Credit;
                    $scope.tranAmount = tran.CrAmount;
                }
                if (tran.Note && tran.Note.length > 0) {
                    $scope.Note = tran.Note;
                } else {
                    $scope.Note = "";
                }
            };
            $scope.removeTransactionDetailItem = function (tran, index) {
                if ($scope.selectedTransaction && $scope.selectedTransaction.TransactionDetail) {
                    for (var i = 0; i < $scope.selectedTransaction.TransactionDetail.length; i++) {
                        if ($scope.selectedTransaction.TransactionDetail[i].TranHeadId === tran.TranHeadId) {
                            $scope.selectedTransaction.TransactionDetail.splice(i, 1);
                            break;
                        }
                    }
                    // Total amount calculation 
                    drcrAmountCalculation();
                    $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };
            $scope.addToTransactionDetailList = function () {
                var tranDetail = {};

                if (!$scope.tranAmount) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.tranAmountType === PpsConstant.TranAmountType.Debit) {
                    tranDetail.DrAmount = $scope.tranAmount;
                    tranDetail.CrAmount = 0;
                } else {
                    tranDetail.DrAmount = 0;
                    tranDetail.CrAmount = $scope.tranAmount;
                }

                if ($scope.Note && $scope.Note.length > 0) {
                    tranDetail.Note = $scope.Note.trim();
                } else {
                    tranDetail.Note = "";
                }


                if ($scope.selectedHead.selected) {
                    tranDetail.TranHeadId = $scope.selectedHead.selected.HeadId;
                    tranDetail.TranHead = $scope.selectedHead.selected.HeadName;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }


                if (!validateTranEntry(tranDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if ($scope.modelDetailMode === PpsConstant.TranMode.AddNew) {
                    $scope.selectedTransaction.TransactionDetail.push(tranDetail);
                } else {
                    $scope.selectedTransaction.TransactionDetail[selectedModelDetailIndex] = tranDetail;
                }
                hasTransaction = true;
                // Total amount calculation 
                drcrAmountCalculation();
                // Reset form
                $scope.selectedHead.selected = null;
                $scope.tranAmountType = 1;
                $scope.tranAmount = 0;
                $scope.Note = "";
                $scope.modelDetailText = "Add to List";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
            };
            $scope.cancelModelUpdate = function () {
                $scope.modelDetailText = "Add to List";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.tranAmountType = PpsConstant.TranAmountType.Debit;
                $scope.tranAmount = 0;
                $scope.selectedHead.selected = null;
            };
            var drcrAmountCalculation = function () {
                $scope.totalDrAmount = 0;
                $scope.totalCrAmount = 0;
                if ($scope.selectedTransaction && $scope.selectedTransaction.TransactionDetail) {
                    for (var d in $scope.selectedTransaction.TransactionDetail) {
                        $scope.totalDrAmount += $scope.selectedTransaction.TransactionDetail[d].DrAmount;
                        $scope.totalCrAmount += $scope.selectedTransaction.TransactionDetail[d].CrAmount;
                    }
                }
            };
            var validateTranEntry = function (tranDetail) {
                if (!$scope.selectedTransaction.TranDate
                    || (tranDetail.DrAmount === 0 && tranDetail.CrAmount === 0)
                    || tranDetail.TranHeadId === null) {
                    return false;
                }
                return true;
            };
            var getAccountHeadList = function () {
                authService.loadingOn();
                var promise = ledgerService.getAccountHeadList(fiscalYear, companyId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.accountsHead = response;
                }, function (err) {
                    $scope.accountsHead = [];
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
                $scope.tranAmount = "";

                authService.loadingOn();

                $q.all([]).then(function () {
                    getAccountHeadList();
                    drcrAmountCalculation();
                    authService.loadingOff();
                });

               
            };
            pageLoad();
            var validate = function () {
                if (!$scope.selectedTransaction.TranDate
                    || !$scope.totalDrAmount
                    || ($scope.totalDrAmount !== $scope.totalCrAmount)
                    || !$scope.selectedTransaction.Particulars) {
                    return false;
                }
                return true;
            };
            var filterZeroAmountItem = function () {
                for (var i = 0; i < $scope.selectedTransaction.TransactionDetail.length; i++) {
                    if (!$scope.selectedTransaction.TransactionDetail[i].TranHeadId || ($scope.selectedTransaction.TransactionDetail[i].DrAmount === 0 || $scope.selectedTransaction.TransactionDetail[i].DrAmount === undefined) && ($scope.selectedTransaction.TransactionDetail[i].CrAmount === 0 || $scope.selectedTransaction.TransactionDetail[i].CrAmount === undefined)) {
                        $scope.selectedTransaction.TransactionDetail.splice(i, 1);
                        i--;
                    }
                }
            };
            $scope.SaveTransaction = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                $scope.selectedTransaction.TranTypeId = transactionType;
                $scope.selectedTransaction.CompanyId = companyId;
                $scope.selectedTransaction.FiscalYear = fiscalYear;
                $scope.selectedTransaction.UpdatedById = $rootScope.userId;
                $scope.selectedTransaction.CreatedById = $rootScope.userId;
               
                // Remove 0 amount 
                filterZeroAmountItem();
                // remove timezone difference 
                $scope.selectedTransaction.TranDate = moment($scope.selectedTransaction.TranDate).format("MM-DD-YYYY");

                $scope.processComplated = false;
                authService.loadingOn();
                var promise = transactionService.savePaymentTransaction($scope.selectedTransaction);
                promise.then(function (response) {
                    authService.loadingOff();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $scope.selectedTransaction = {
                        TranNo: 'New',
                        TranDate: new Date(),
                        TransactionDetail: [],
                        Particulars: null
                    };
                    $scope.totalDrAmount = 0;
                    $scope.totalCrAmount = 0;
                    hasTransaction = true;
                    getAccountHeadList();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                });
            };
            $scope.closeModal = function () {
                $location.path("/transaction/payment");
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

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withOption('order', [])
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