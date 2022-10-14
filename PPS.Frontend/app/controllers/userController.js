'use strict';

angular.module('AtlasPPS').controller('userController',
    ['$scope', '$q', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'userService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state', 'companyService', 'employeeService',
        function ($scope, $q, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, userService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state, companyService, employeeService) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;
               
        $scope.SelectedUser = null;
        $scope.newUser = {
            Email: "",
            FirstName: "",
            LastName: "",
            CompanyId: ""
        };
        $scope.users = [];
        $scope.company = [];
        $scope.employees = [];
        $scope.selectedCompany = null;

        var getCompany = function () {
            authService.loadingOn();
            var promise = companyService.getCompanyList();
            promise.then(function (response) {
                $scope.company = response;
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };
        var getEmployee = function () {
            authService.loadingOn();
            var promise = employeeService.getEmployee();
            promise.then(function (response) {
                $scope.employees = response;
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };
        $scope.navigateToUserDetail = function (user) {
            $location.path("/admin/user/" + user.Id);
        };

        $scope.addNewUser = function (user) {
            $('#addUserModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };
        $scope.closeAddUserModal = function () {
            $scope.newUser = null;
            $('#addUserModal').modal('toggle');
            $(".modal-backdrop").hide();
            $("body").removeClass("modal-open");
        };
        $scope.addNewUserClick = function () {
            authService.loadingOn();
            $scope.newUser.FirstName = $scope.selectedEmployee.FirstName;
            $scope.newUser.LastName = $scope.selectedEmployee.LastName;
            $scope.newUser.CompanyId = $scope.selectedCompany.Id;
            $scope.newUser.EmployeeId = $scope.selectedEmployee.Id;
            $scope.newUser.Email = $scope.selectedEmployee.Email;
            var promise = userService.userRegister($scope.newUser);
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                $scope.users.push(response);                
                $scope.selectedCompany = null;
                $scope.closeAddUserModal();
                $state.reload();
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };
        
        $scope.resetPasswordClick = function (user) {
            $scope.SelectedUser = user;
            $('#resetPasswordUserModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };        
        $scope.closeResetPasswordUserModal = function () {
            $scope.SelectedUser = null;
            $('#resetPasswordUserModal').modal('toggle');
            $(".modal-backdrop").hide();
            $("body").removeClass("modal-open");
        };
        $scope.resetUserClick = function () {
            authService.loadingOn();
            var userId = $scope.SelectedUser.AspNetUserId;
            var promise = userService.resetUser(userId);
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                $scope.SelectedUser = null;
                $scope.closeResetPasswordUserModal();
                $state.reload();
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };

        $scope.userStatusClick = function (user) {
            $scope.SelectedUser = user;
            $('#activeInactiveUserModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };
        $scope.closeActiveInactiveUserModal = function () {
            $scope.SelectedUser = null;
            $('#activeInactiveUserModal').modal('toggle');
            $(".modal-backdrop").hide();
            $("body").removeClass("modal-open");
        };
        $scope.activeInactiveUserClick = function () {
            authService.loadingOn();
            var userId = $scope.SelectedUser.Id;
            var promise = null;
            if ($scope.SelectedUser.Status === 'Active') {
                promise = userService.userDeactivate(userId);
            } else {
                promise = userService.userActivate(userId);
            }
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                var user = _.filter($scope.users, function (u) { return u.Id === userId });
                user[0].Status = user[0].Status === 'Active' ? 'Inactive' : 'Active';
                $scope.SelectedUser = null;
                $scope.closeActiveInactiveUserModal();
                $state.reload();
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };

        $scope.userLockedClick = function (user) {
            $scope.SelectedUser = user;
            $('#lockUnlockUserModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };
        $scope.closeLockUnlockUserModal = function () {
            $scope.SelectedUser = null;
            $('#lockUnlockUserModal').modal('toggle');
            $(".modal-backdrop").hide();
            $("body").removeClass("modal-open");
        };
        $scope.lockUnlockUserClick = function () {
            authService.loadingOn();
            var userId = $scope.SelectedUser.Id;
            var promise = null;
            if ($scope.SelectedUser.Locked === 'Locked') {
                promise = userService.userUnlock(userId);
            } else {
                promise = userService.userLock(userId);
            }            
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                var user = _.filter($scope.users, function (u) { return u.Id === userId });
                user[0].Locked = user[0].Locked === 'Locked' ? '' : 'Locked';
                $scope.SelectedUser = null;
                $scope.closeLockUnlockUserModal();
                $state.reload();                
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };

        var getUserList = function () {
            authService.loadingOn();
            var promise = userService.getUsers();
            promise.then(function (response) {
                $scope.users = response;
                $q.all([
                    getCompany(),
                    getEmployee()]).then(function () {
                        authService.loadingOff();
                    });
            }, function (err) {
                $scope.users = [];
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
            getUserList();
            //getTransactionRejectReasonType();
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
            //.withPaginationType('full_numbers')
            .withPaginationType('full_numbers').withDisplayLength(25)
            .withDisplayLength(500)
            .withOption('lengthChange', false);

        $('.inner-table .html5buttons').css({ 'display': 'none' });
        $('.inner-table .dataTables_length').css({ 'display': 'none' });
        $('.inner-table .DataTables_Table_0_filter').css({ 'display': 'none' });
        
        // TODO: 
        $scope.$on('$viewContentLoaded', function () {
            $('.ibox-content .dataTables_wrapper').css({ 'overflow': 'auto' });
        });
    }]);