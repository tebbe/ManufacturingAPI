//'use strict';

angular.module('AtlasPPS').controller('userRoleController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'userRoleService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state',
    function ($scope, $rootScope, localStorageService, $location, notificationService, authService, userRoleService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;

        //$scope.SelectedUser = null;
        $scope.newRole = {
            RoleName: "",
            Description: ""
        };
        $scope.roles = [];

        $scope.navigateToRole = function (roleId) {
            $location.path("/admin/role/" + roleId);
        };

        $scope.addNewRole = function (user) {
            $('#addRoleModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        };
        $scope.closeAddRoleModal = function () {
            $scope.newRole = null;
            $('#addRoleModal').modal('toggle');
            $(".modal-backdrop").hide();
        };
        $scope.addRoleClick = function () {
            authService.loadingOn();
            var promise = userRoleService.addRole($scope.newRole);
            promise.then(function (response) {
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                $scope.closeAddRoleModal();
                $state.reload();
                authService.loadingOff();
            }, function (err) {
                notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                authService.loadingOff();
            });
        };
        
        var getUserRoleList = function () {
            authService.loadingOn();
            var promise = userRoleService.getUserRoles();
            promise.then(function (response) {
                $scope.roles = response;    
                $(".dataTables_filter").closest('.row').hide();
                $(".dataTables_paginate").closest('.row').hide();
                if ($scope.roles.length > 0) {
                    $(".dataTables_empty").closest('tr').hide();
                }
                authService.loadingOff();
            }, function (err) {
                $scope.roles = [];
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
            getUserRoleList();
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
            .withPaginationType('full_numbers')
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