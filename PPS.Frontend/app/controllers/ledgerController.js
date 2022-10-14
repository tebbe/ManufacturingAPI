'use strict';

angular.module('AtlasPPS').controller('ledgerController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'authService', 'ledgerService', 'ngAuthSettings', 'DTOptionsBuilder', 'toaster', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, authService, ledgerSvc, ngAuthSettings, DTOptionsBuilder, toaster, PpsConstant, $window) {
            $scope.processComplated = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;

            $scope.ledgerList = [];
            $scope.accountTypeList = [];
            //$scope.accountPrimaryHeadList = [{ PrimaryHead: 'Cash', AccountType: 1, AccountPrimaryHeadId: 1 }, { PrimaryHead: 'Bank', AccountType: 1, AccountPrimaryHeadId: 2}];
            $scope.accountPrimaryHeadList = [];
            $scope.accountSubHeadList = [];
            $scope.selectedMainGroup = null;
            $scope.selectedSubGroup = null;
            $scope.accountHead = {
                SubHeadId: null,
                HeadCode: null,
                HeadName: null,
                DrAmount: 0,
                CrAmount: 0,
                CompanyId: null,
                Active: null,
                FiscalYear: null
            };

            var getHeadList = function () {
                authService.loadingOn();
                var promise = ledgerSvc.getHeadList(fiscalYear, companyId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.ledgerList = response.ResponseData;
                }, function (err) {
                    $scope.ledgerList = [];
                });
            };

            var getAccountTypeList = function () {
                authService.loadingOn();
                var promise = ledgerSvc.getAccountTypeList(fiscalYear, companyId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.accountTypeList = response.ResponseData;
                }, function (err) {
                    $scope.accountTypeList = [];
                });
            };

            var getPrimaryHeadList = function () {
                authService.loadingOn();
                var promise = ledgerSvc.getPrimaryHeadList(fiscalYear, companyId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.accountPrimaryHeadList = response.ResponseData;
                }, function (err) {
                    $scope.accountPrimaryHeadList = [];
                });
            };

            $scope.getAccountSubHeadList = function (selectedPrimaryHeadId) {
                $scope.accountSubHeadList = [];
                if (!selectedPrimaryHeadId)
                    return;
                authService.loadingOn();
                var promise = ledgerSvc.getAccountSubHeadList(companyId, selectedPrimaryHeadId);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.accountSubHeadList = response.ResponseData;
                }, function (err) {
                    $scope.accountSubHeadList = [];
                });
            };

            $scope.updateSubHeadId = function (selectedSubHead) {
                if (!!selectedSubHead)
                    $scope.accountHead.SubHeadId = selectedSubHead.SubHeadId;
                else
                    $scope.accountHead.SubHeadId = null;
            };

            $scope.saveAccountHead = function () {
                if (!validate()) {
                    $scope.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                // TODO: 
                $scope.accountHead.Active = true;
                $scope.accountHead.FiscalYear = fiscalYear;
                $scope.accountHead.CompanyId = companyId;
                authService.loadingOn();
                var promise = ledgerSvc.saveAccountHead($scope.accountHead);
                promise.then(function (response) {
                    authService.loadingOff();
                    $scope.accountHead = response.ResponseData;
                    $scope.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.modalDismiss();
                    //$scope.ledgerList.push({});
                    $window.location.reload();
                }, function (err) {
                    $scope.showErrorNotificatoin(PpsConstant.DefaultError);
                });
            };
            var validate = function () {
                if (!$scope.accountHead.SubHeadId || $scope.accountHead.HeadCode || !$scope.accountHead.HeadName
                    || $scope.accountHead.DrAmount === null || $scope.accountHead.CrAmount === null
                    || $scope.accountHead.DrAmount < 0 || $scope.accountHead.CrAmount < 0) {
                    return false;
                }
                return true;
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
                    $location.path('/login');
                }
                getHeadList();
                getAccountTypeList();
                getPrimaryHeadList();
            };
            pageLoad();

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            $scope.showSuccessNotificatoin = function (msg) {
                toaster.success({ body: msg });
            };
            $scope.showErrorNotificatoin = function (msg) {
                toaster.error({ body: msg });
            };
            var startTimer = function () {
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            };

            $scope.dtOptions = DTOptionsBuilder.newOptions()
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    { extend: 'pdf', title: 'Ledger List' },
                    {
                        extend: 'print',
                        customize: function (win) {
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    }
                ]);
        }]);
