//'use strict';

angular.module('AtlasPPS').controller('userPolicyController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'userPolicyService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', '$state',
    function ($scope, $rootScope, localStorageService, $location, notificationService, authService, userPolicyService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, $state) {
        $scope.processComplated = true;
        var hasTransaction = false;
        var authData = localStorageService.get('authorizationData');
        var fiscalYear = null;
        var companyId = null;

        $rootScope.userId = null;
        $rootScope.userName = null;
        $rootScope.userFullName = null;

        $scope.rolePolicies = [];
        $scope.appPolicyStatus = 1;
        $scope.roleId;
        //$scope.addNewRole = function (user) {
        //    $('#addRoleModal').modal({
        //        backdrop: 'static',
        //        keyboard: false
        //    });
        //};
        //$scope.closeAddRoleModal = function () {
        //    $scope.newRole = null;
        //    $('#addRoleModal').modal('toggle');
        //    $(".modal-backdrop").hide();
        //};
        //$scope.addRoleClick = function () {
        //    var promise = userRoleService.addRole($scope.newRole);
        //    promise.then(function (response) {
        //        notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
        //        $scope.closeAddRoleModal();
        //        $state.reload();
        //    }, function (err) {
        //        notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
        //    });
        //};
        $scope.updateRolePolicy = function (role, rolePolicies) {
            if (role && rolePolicies) {
                var rolePolicyVm = { Id: role.Id, Policies: [] };
                _.forEach(rolePolicies, function (p) {
                    if (p.Selected) {
                        rolePolicyVm.Policies.push({ Id: p.Id, Selected: p.Selected });
                    }
                });
                authService.loadingOn();
                var promise = userPolicyService.updateRolePolicy(rolePolicyVm);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    authService.loadingOff();
                });
            }
        };
        var getUserRolePolicyList = function (roleId) {
            if (!roleId) {
                return;
            }
            authService.loadingOn();
            var promise = userPolicyService.getPolicyByRole(roleId, $scope.appPolicyStatus);
            promise.then(function (response) {
                $scope.role = response;
                $scope.rolePolicies = response.Policies;
                $(".dataTables_filter").closest('.row').hide();
                $(".dataTables_paginate").closest('.row').hide();
                if ($scope.rolePolicies.length > 0) {
                    $(".dataTables_empty").closest('tr').hide();
                }
                authService.loadingOff();
            }, function (err) {
                $scope.rolePolicies = [];
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
            
            if ($state.params && $state.params.roleId) {
                $scope.roleId = _.parseInt($state.params.roleId);
            }
            getUserRolePolicyList($scope.roleId);
        };
        pageLoad();

        $scope.goToAppPolicyType = function (status) {
            $scope.appPolicyStatus = status;
            getUserRolePolicyList($scope.roleId);
        }

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