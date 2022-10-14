'use strict';

angular.module('AtlasPPS').controller('customerTransactionEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'bankService', 'salesService', '$q','$state',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, bankService, salesService, $q, $state) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            var transactionId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.modelHeading = "Edit Customer";
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.customerTransactionList = [];
            $scope.cashBanks = [];
            $scope.customer = [];
            $scope.transaction = [];

            $scope.selectedAccount = {
                selected: null
            }
            $scope.selectedCustomer = {
                selected: null
            }


            var fillDataFunction = function () {

                $scope.transactionDate = new Date($scope.transaction.TransactionDate);
                $scope.selectedAccount.selected = _.filter($scope.cashBanks,
                    function (item) {
                        return item.Id === $scope.transaction.CashBankAccountHeadId;
                    })[0];
                $scope.selectedCustomer.selected = _.filter($scope.customer,
                    function (item) {
                        return item.Id === $scope.CustomerTransaction.CustomerTransactionDetail[0].CustomerId;
                    })[0];
            };

            $scope.onChangeAccount = function (selectedAccount) {
                if (selectedAccount && selectedAccount.Id) {
                    var result = $scope.cashBanks.filter(function (v) {
                        return v.Id === selectedAccount.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedAccount.selected = result[0];
                    }
                    else {
                        $scope.selectedAccount.selected = null;
                    }
                } else {
                    $scope.selectedAccount.selected = null;
                }
            }

            $scope.onChangeCustomer = function (selectedCustomer) {
                if (selectedCustomer && selectedCustomer.Id) {
                    var result = $scope.customer.filter(function (v) {
                        return v.Id === selectedCustomer.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedCustomer.selected = result[0];
                    }
                    else {
                        $scope.selectedCustomer.selected = null;
                    }
                } else {
                    $scope.selectedCustomer.selected = null;
                }
            }
            $scope.CustomerTransaction = {
                CashBankAccountHeadId: null,
                TransactionDate: new Date(),
                TransactionAmount: 0,
                BankChargeAmount: 0,
                TransactionReference: null,
                CustomerTransactionDetail: []
            };

            $scope.isCustomerTransactionValidated = false;
            $scope.TransactionAmount = 0;
            $scope.totalAmountInGrid = 0;
            $scope.editedAmount = 0;
            $scope.BookNo = null;
            $scope.BookSerialNo = null;

            var validateCustomerTransactionEntry;
            var calculateTotalAmountInGrid;
            $scope.addTransaction = function () {
                var ctDetail = {};

                if (!$scope.TransactionAmount) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                ctDetail.TransactionAmount = $scope.TransactionAmount;


                if ($scope.selectedCustomer.selected) {
                    ctDetail.CustomerId = $scope.selectedCustomer.selected.Id;
                    ctDetail.CustomerName = $scope.selectedCustomer.selected.CustomerName + " (" + $scope.selectedCustomer.selected.CustomerCode + ")";
                    ctDetail.BookNo = $scope.BookNo;
                    ctDetail.BookSerialNo = $scope.BookSerialNo;
                    ctDetail.TransactionAmount = $scope.TransactionAmount;
                } else {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }

                if (!validateCustomerTransactionEntry(ctDetail)) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                } else {
                    if ($scope.modelDetailMode === 2) {
                        $scope.CustomerTransaction.CustomerTransactionDetail[selectedModelDetailIndex] = ctDetail;
                    } else {
                        $scope.CustomerTransaction.CustomerTransactionDetail[$scope.CustomerTransaction.CustomerTransactionDetail.length] = ctDetail;
                    }
                   
                }

                $scope.totalAmountInGrid = calculateTotalAmountInGrid();

                //if ($scope.totalAmountInGrid === ($scope.CustomerTransaction.TransactionAmount + $scope.CustomerTransaction.BankChargeAmount)) {
                //    $scope.isCustomerTransactionValidated = true;
                //} else {
                //    $scope.isCustomerTransactionValidated = false;
                //}

                $scope.isCustomerTransactionValidated = true;
                hasTransaction = true;
                $scope.selectedCustomer.selected = null;

                $scope.BookNo = null;
                $scope.BookSerialNo = null;
                $scope.TransactionAmount = null;

                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;


                //$scope.TransactionAmount = ($scope.CustomerTransaction.TransactionAmount + $scope.CustomerTransaction.BankChargeAmount) - $scope.totalAmountInGrid;
                setTimeout(function () {
                    $("#listCustomer").focus();
                }, 1);
            };

            validateCustomerTransactionEntry = function (ctDetail) {
                if (!$scope.selectedCustomer.selected.CustomerName || (ctDetail.TransactionAmount === 0)) {
                    return false;
                }
                return true;
            };
            calculateTotalAmountInGrid = function () {
                var totalAmount = 0;
                if ($scope.CustomerTransaction && $scope.CustomerTransaction.CustomerTransactionDetail && $scope.CustomerTransaction.CustomerTransactionDetail.length > 0) {
                    for (var i = 0; i < $scope.CustomerTransaction.CustomerTransactionDetail.length; i++) {
                        totalAmount = totalAmount +
                            $scope.CustomerTransaction.CustomerTransactionDetail[i].TransactionAmount;
                    }
                }
                return totalAmount;
            };


            $scope.removeItemDetail = function (tran) {
                if ($scope.CustomerTransaction.CustomerTransactionDetail) {
                    for (var i = 0; i < $scope.CustomerTransaction.CustomerTransactionDetail.length; i++) {
                        if ($scope.CustomerTransaction.CustomerTransactionDetail[i].CustomerId === tran.CustomerId) {
                            $scope.CustomerTransaction.CustomerTransactionDetail.splice(i, 1);
                            if ($scope.CustomerTransaction.CustomerTransactionDetail.length === 0) {
                                $scope.isCustomerTransactionValidated = false;
                            }
                            break;
                        }
                    }
                    //$scope.totalAmountInGrid = calculateTotalAmountInGrid();
                    //if ($scope.totalAmountInGrid === ($scope.CustomerTransaction.TransactionAmount + $scope.CustomerTransaction.BankChargeAmount)) {
                    //    $scope.isCustomerTransactionValidated = true;
                    //} else {
                    //    $scope.isCustomerTransactionValidated = false;
                    //}
                    setTransactionAmount();
                    $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                    hasTransaction = true;
                }
            };

            $scope.selectItemDetail = function (tran) {
                if ($scope.CustomerTransaction.CustomerTransactionDetail) {
                    for (var i = 0; i < $scope.CustomerTransaction.CustomerTransactionDetail.length; i++) {
                        if ($scope.CustomerTransaction.CustomerTransactionDetail[i].CustomerId === tran.CustomerId) {
                            selectedModelDetailIndex = i;
                            break;
                        }
                    }
                } else {
                    return;
                }
                $scope.isCustomerTransactionValidated = false;
                $scope.modelDetailText = "Update";
                $scope.modelDetailMode = PpsConstant.TranMode.Update;

                $scope.editedAmount = tran.TransactionAmount;

                var result = $scope.customer.filter(function (v) {
                    return v.Id === tran.CustomerId;
                });

                if (result && result.length > 0) {
                    $scope.selectedCustomer.selected = result[0];
                }
                $scope.BookNo = tran.BookNo;
                $scope.BookSerialNo = tran.BookSerialNo;
                $scope.TransactionAmount = tran.TransactionAmount;
                $scope.CustomerTransaction.TransactionAmount = tran.TransactionAmount;
                
            };

            $scope.cancelModelUpdate = function () {
                $scope.isCustomerTransactionValidated = true;
                $scope.modelDetailText = "Add";
                $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
                selectedModelDetailIndex = -1;
                $scope.selectedCustomer.selected = null;
                $scope.BookNo = null;
                $scope.BookSerialNo = null;
                $scope.TransactionAmount = null;
                $scope.editedAmount = 0;
            };

            var validate = function () {
                if (!$scope.CustomerTransaction.TransactionDate
                    || (!$scope.CustomerTransaction.TransactionAmount)
                    || (!$scope.selectedAccount.selected.Id)
                    || (!$scope.CustomerTransaction.CustomerTransactionDetail)) {
                    return false;
                }
                return true;
            };

            var setTransactionAmount = function () {
                var txAmount = 0;
                var bankChargeAmount = 0;
                if ($scope.CustomerTransaction.TransactionAmount) {
                    txAmount = $scope.CustomerTransaction.TransactionAmount;
                }
                if ($scope.CustomerTransaction.BankChargeAmount) {
                    bankChargeAmount = $scope.CustomerTransaction.BankChargeAmount;
                }
                //$scope.TransactionAmount = (txAmount + bankChargeAmount) - $scope.totalAmountInGrid;
                $scope.TransactionAmount = (txAmount + bankChargeAmount);
               
            }

            $scope.$watch("CustomerTransaction.TransactionAmount", function (newValue, oldValue) {

                setTransactionAmount();
            });

            $scope.$watch("CustomerTransaction.BankChargeAmount", function (newValue, oldValue) {
                setTransactionAmount();
            });

            var clearField;
            $scope.updateCustomerTransaction = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.CustomerTransaction.TransactionDate =
                    moment($scope.transaction.TransactionDate).format("MM-DD-YYYY");
                $scope.CustomerTransaction.CashBankAccountHeadId = $scope.transaction.CashBankAccountHeadId;
                $scope.CustomerTransaction.Id = $scope.transaction.Id;
                $scope.processComplated = false;
                authService.loadingOn();
                var promise = customerService.updateCustomerTransaction($scope.CustomerTransaction);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    clearField();
                    $location.path("/Sales/CustomerTransaction/View/" + response.Id);
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
                    authService.loadingOff();
                });
            };

            clearField = function () {
                $scope.CustomerTransaction.TransactionDate = new Date();
                $scope.CustomerTransaction.TransactionAmount = null;
                $scope.CustomerTransaction.BankChargeAmount = 0;
                $scope.CustomerTransaction.TransactionReference = null;
                $scope.selectedAccount.selected = null;
                $scope.CustomerTransaction.CustomerTransactionDetail = [];
                $scope.isCustomerTransactionValidated = false;
                $scope.totalAmountInGrid = 0;
                $scope.editedAmount = 0;
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

                if ($state.params && $state.params.Id) {
                    transactionId = _.parseInt($state.params.Id);
                }

                var getTransactionById = customerService.getCustomerTransactionByTransactionId(transactionId);
                getTransactionById.then(function (response) {
                    authService.loadingOn();
                    $scope.transaction = response;
                    //$scope.transaction.TransactionDate=moment($scope.transaction.TransactionDate).format("DD-MM-YYYY");
                    $scope.CustomerTransaction.CustomerTransactionDetail = response.CustomerTransactionDetail;
                }, function (err) {
                    $scope.transaction = [];
                });

                var promiseBankCashAccountHeadList = bankService.getBankCashAccountHeadList();
                promiseBankCashAccountHeadList.then(function (response) {
                    authService.loadingOn();
                    $scope.cashBanks = response;
                }, function (err) {
                    $scope.cashBanks = [];
                });

                var promiseGetCustomerList = salesService.getCustomerList();
                promiseGetCustomerList.then(function (response) {
                    $scope.customer = response;
                }, function (err) {
                    $scope.customer = [];
                });

                authService.loadingOn();
                $q.all([
                    getTransactionById,
                    promiseGetCustomerList,
                    promiseBankCashAccountHeadList]).then(function () {
                        fillDataFunction();
                        authService.loadingOff();
                    });

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

            // TODO: 
            $scope.$on('$viewContentLoaded', function () {
                $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
            });
        }]);