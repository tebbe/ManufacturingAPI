'use strict';

angular.module('AtlasPPS').controller('userDetailController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'ledgerService', 'userService', 'userRoleService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, ledgerService, userService, userRoleService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;
               
        $scope.SelectedUser = null;
       
        $scope.user = {};

        $scope.navigateToRole = function (roleId) {
            $location.path("/admin/role/" + roleId);
        };

        $scope.updateUser = function (user) {
            if (user && user.Roles) {
                var userVm = { Id: user.Id, Roles: [] };
                _.forEach(user.Roles, function (r) {
                    if (r.Selected) {
                        userVm.Roles.push({ Id: r.Id, Selected: r.Selected });
                    }
                });
                authService.loadingOn();
                var promise = userService.updateUser(userVm);
                promise.then(function (response) {                    
                    authService.loadingOff();
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                }, function (err) {
                    authService.loadingOff();
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            }
        };

        var getUserDetail = function (userId) {
            authService.loadingOn();
            var promise = userService.getUserRoleDetailById(userId);
            promise.then(function (response) {
                authService.loadingOff();
                $scope.user = response;
            }, function (err) {
                authService.loadingOff();
                $scope.user = {};
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
            var userId;
            if ($state.params && $state.params.userId) {
                userId = _.parseInt($state.params.userId);
            }
            getUserDetail(userId);
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