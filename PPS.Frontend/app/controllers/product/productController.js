'use strict';

angular.module('AtlasPPS').controller('productController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'productService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'reportSettings',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, productService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window,reportSettings) {
            var authData = localStorageService.get('authorizationData');
            $scope.productList = [];



            var getAllProductList = function () {
                authService.loadingOn();
                var promise = productService.getProductList();
                promise.then(function (response) {
                    $scope.productList = response;
                    authService.loadingOff();
                }, function (err) {
                    $scope.productList = [];
                    authService.loadingOff();
                });
            };
            $scope.navigateToProductAdd = function (id) {
                $location.path("admin/productAdd");
            }
            $scope.navigateToProductEdit = function (id) {
                $location.path("admin/productEdit/" + id);
            }
            $scope.navigateToProductView = function (id) {
                $location.path("admin/productView/" + id);
            };
            $scope.viewProductHistory = function (id) {
                $location.path("admin/productHistory/" + id);
            }
            $scope.navigateToProductListPrint = function () {
                $location.path("admin/productPrint");
            }
            var pageLoad = function () {
                if (authData && authData.isAuth) {
                    $rootScope.userId = authData.userId;
                    $rootScope.userName = authData.userName;
                    $rootScope.userFullName = authData.fullName;  
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }
                getAllProductList();
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


            $scope.dtOptionsAll = DTOptionsBuilder.newOptions()
                .withOption('order', [0, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([]);

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withOption('order', [2, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
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