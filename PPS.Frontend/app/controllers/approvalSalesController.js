//'use strict';

angular.module('AtlasPPS').controller('approvalSalesController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'transactionService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'approvalService',
    function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, transactionService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, approvalService) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;
        $scope.transactionRejectReasonType = [];
        $scope.isNew = false;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;
        $scope.tranMode = 1;

        $scope.modelHeading = "Add Payment";
        $scope.modelActionText = "Add Transation";
        $scope.modelDetailText = "Add to List";
        $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
        var selectedModelDetailIndex = -1;

        var selectedTransaction = null;
        $scope.selectedTransactionRejectReasonTypeId = null;

        $scope.rejectTransaction = function () {
            var vm = {
                TransactionNo: selectedTransaction.TranNo,
                TransactionTypeId: selectedTransaction.TranTypeId,
                RejectReasonTypeId: $scope.selectedTransactionRejectReasonTypeId,
                UserId: $rootScope.userId
            };
            authService.loadingOn();
            var promise = approvalService.rejectTransaction(vm);
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                $window.location.reload();
                //TODO: Instead of reload 
                //var data = $('#DataTables_Table_1').DataTable().data();
                //_.remove(data, function (x) {
                //    return x[0] == vm.TransactionNo;
                //});
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };

        $scope.approveTransaction = function () {
            var vm = {
                TransactionNo: selectedTransaction.TranNo,
                TransactionTypeId: selectedTransaction.TranTypeId,
                UserId: $rootScope.userId
            };
            authService.loadingOn();
            var promise = approvalService.acceptTransaction(vm);
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                $window.location.reload();
                //TODO: Instead of reload 
                //var data = $('#DataTables_Table_1').DataTable().data();
                //_.remove(data, function (x) {
                //    return x[0] == vm.TransactionNo;
                //});
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };
        $scope.closeApprovalModal = function () {
            selectedTransaction = null;
            $('#transactionApprovalModal').modal('toggle');
        };             
        $scope.acceptTransactionClick = function (tran) {
            selectedTransaction = tran;
            $('#transactionApprovalModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };

        $scope.rejectTransactionClick = function (tran) {
            selectedTransaction = tran;
            $('#transactionRejectionModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };
        $scope.closeRejctionModal = function () {
            selectedTransaction = null;
            $('#transactionRejectionModal').modal('toggle');
        };

        var getUnapprovedSalesTransactionList = function () {
            authService.loadingOn();
            var promise = transactionService.getUnapprovedSalesTransactionList(fiscalYear, companyId);
            promise.then(function (response) {
                $scope.transactions = response;
                authService.loadingOff();
            }, function (err) {
                $scope.transactions = [];
                authService.loadingOff();
            });
        };
        var getTransactionRejectReasonType = function () {
            if ($scope.transactionRejectReasonType.length > 0) {
                return;
            }
            authService.loadingOn();
            var promise = transactionService.getTransactionRejectReasonType();
            promise.then(function (response) {
                $scope.transactionRejectReasonType = response;
                authService.loadingOff();
            }, function (err) {
                $scope.transactionRejectReasonType = [];
                authService.loadingOff();
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
            getUnapprovedSalesTransactionList();
            getTransactionRejectReasonType();
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