//'use strict';

angular.module('AtlasPPS').controller('myController',
    ['$scope', '$rootScope', 'localStorageService', 'notificationService', '$location', 'authService', 'myService', 'ngAuthSettings', 'PpsConstant', '$window', 'DTOptionsBuilder',
    function ($scope, $rootScope, localStorageService, notificationService, $location, authService, myService, ngAuthSettings, PpsConstant, $window, DTOptionsBuilder) {
        $scope.processComplated = true;
        var hasTransaction = false;        
        var userId = null;
        var userName = null;
        var fiscalYear = null;
        var companyId = null;
        $scope.user = {
            email: null,
            currentPassword: null,
            newPassword: null,
            confirmPassword: null
        };

        var validate = function () {
            if (!$scope.user.email || !$scope.user.currentPassword || !$scope.user.newPassword || !$scope.user.confirmPassword) {
                notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                return false;
            }
            if ($scope.user.newPassword != $scope.user.confirmPassword) {
                notificationService.showErrorNotificatoin(PpsConstant.PasswordMismatch);
                return false;
            }
            if ($scope.user.newPassword.length < 8) {
                notificationService.showErrorNotificatoin(PpsConstant.ShortPassword);
                return false;
            }
            return true;
        };

        $scope.updatePassword = function () {
            if (!validate()) {                
                return;
            }
            authService.loadingOn();
            var promise = myService.updatePassword($scope.user);
            promise.then(function (response) {
                authService.loadingOff();
                notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                $scope.processComplated = true;
                $scope.user.currentPassword = null;
                $scope.user.newPassword = null;
                $scope.user.confirmPassword = null;
                authService.logout();
                $location.path('/login');
            }, function (err) {
                notificationService.showErrorNotificatoin(err.Message);
                $scope.processComplated = true;
            });            
        };

        $scope.logout = function () {
            authService.logout();
            $location.path('/login');
        };

        var pageLoad = function () {
            if (!authService.isValidAuth()) {
                authService.logout();
                $location.path('/login');
            } else {
                var authData = localStorageService.get('authorizationData');
                fiscalYear = authData.fiscalYear;
                companyId = authData.companyId;
                userId = authData.userId;
                userName = authData.userName;                
                $scope.user = {
                    email: userName
                };
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
                //'This Year': [moment(new Date(new Date().getFullYear() + "/1/1")), moment(new Date(new Date().getFullYear() + "/12/31"))]
            }
        };

        //$scope.setStartDate = function () {
        //    $scope.date.startDate = moment().subtract(4, "days").toDate();
        //};

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