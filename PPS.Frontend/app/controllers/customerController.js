//'use strict';

angular.module('AtlasPPS').controller('customerController',
    ['$scope', '$rootScope', '$q', 'localStorageService', '$location', 'notificationService', 'authService', 'customerService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window', 'employeeService', 'fileService',
        function ($scope, $rootScope, $q, localStorageService, $location, notificationService, authService, customerService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window, employeeService, fileService) {
            $scope.processComplated = true;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            $scope.isNew = false;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.minDate = PpsConstant.MinDate;
            $scope.maxDate = PpsConstant.MaxDate;

            $scope.modelHeading = "Add Customer";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;

            $scope.area = [];
            $scope.customerType = [];
            $scope.postOffice = [];
            $scope.salesOfficer = [];
            $scope.selectedSalesAccount = {};
            $scope.attachmentType = [];
            $scope.isCustomerValidated = true;

            $scope.customer = {
                CustomerName: null,
                CustomerCode: null,
                CustomerAddress: null,
                CustomerMobile: null,
                CustomerPhone: null,
                OwnerName: null,
                OwnerMobile: null,
                OwnerPhone: null,
                OwnerBirthDate: null,
                FiscalYear: null,
                ContactPersonName: null,
                ContactPersonMobile: null,
                PrimaryContactNo: null,
                Villege: null,
                PostOfficeId: null,
                AreaId: null,
                CustomerTypeId: null,
                Email: null,
                EmployeeId: null,
                MonthlyCredit: null,
                YearlyCredit: null,
                SalesCapacityYearly: null,
                EffectiveDate: null
            };

            var getPostOfficeList = function () {
                var promise = customerService.getPostOfficeList();
                promise.then(function (response) {
                    $scope.postOffice = response;
                }, function (err) {
                    $scope.postOffice = [];
                });
            };

            var getAreaList = function () {
                var promise = customerService.getAreaList();
                promise.then(function (response) {
                    $scope.area = response;
                }, function (err) {
                    $scope.area = [];
                });
            };
            //var getCustomerType = function () {
            //authService.loadingOn();
            //    var promise = customerService.getCustomerType();
            //    promise.then(function (response) {
            //authService.loadingOff();
            //        $scope.customerType = response;
            //    }, function (err) {
            //        $scope.customerType = [];
            //    });
            //};
            var getSalesOfficer = function () {
                var promise = employeeService.getSalesOfficer();
                promise.then(function (response) {
                    $scope.salesOfficer = response;
                }, function (err) {
                    $scope.salesOfficer = [];
                });
            };

            var getAttachmentType = function () {
                var promise = customerService.getAttachmentType();
                promise.then(function (response) {
                    $scope.attachmentType = response;
                }, function (err) {
                    $scope.attachmentType = [];
                });
            };

            $scope.uploadFile = function () {
                var f = document.getElementById('file').files[0],
                    r = new FileReader();

                r.onloadend = function (e) {
                    var data = e.target.result;
                    //send your binary data via $http or $resource or do anything else with it
                }
                r.readAsBinaryString(f);
            }

            var clearField = function () {
                $scope.customer = {
                    CustomerName: null,
                    CustomerCode: null,
                    CustomerAddress: null,
                    CustomerMobile: null,
                    CustomerPhone: null,
                    OwnerName: null,
                    OwnerMobile: null,
                    OwnerPhone: null,
                    OwnerBirthDate: null,
                    ContactPersonName: null,
                    ContactPersonMobile: null,
                    PrimaryContactNo: null,
                    Villege: null,
                    PostOfficeId: null,
                    AreaId: null,
                    CustomerTypeId: null,
                    FiscalYear:null,
                    Email: null,
                    EmployeeId: null,
                    MonthlyCredit: null,
                    YearlyCredit: null,
                    SalesCapacityYearly: null,
                    EffectiveDate: null
                };
            };

            $scope.saveCustomer = function () {
                $scope.customer.PostOfficeId = $scope.selectedPostOffice.Id;
                $scope.customer.AreaId = $scope.selectedArea.Id;
                $scope.customer.CompanyId = companyId;
                $scope.customer.FiscalYear = fiscalYear;
                $scope.customer.EmployeeId = $scope.selectedSalesAccount.selected.Id;
                authService.loadingOn();
                var promise = customerService.SaveCustomer($scope.customer);

                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    clearField();
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
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



                var promiseGetPostOfficeList = customerService.getPostOfficeList();
                promiseGetPostOfficeList.then(function (response) {
                    $scope.postOffice = response;
                }, function (err) {
                    $scope.postOffice = [];
                });

                var promiseGetAreaList = customerService.getAreaList();
                promiseGetAreaList.then(function (response) {
                    $scope.area = response;
                }, function (err) {
                    $scope.area = [];
                });

                var promiseGetSalesOfficer = employeeService.getSalesOfficer();
                promiseGetSalesOfficer.then(function (response) {
                    $scope.salesOfficer = response;
                }, function (err) {
                    $scope.salesOfficer = [];
                });

                var promiseGetAttachmentType = customerService.getAttachmentType();
                promiseGetAttachmentType.then(function (response) {
                    $scope.attachmentType = response;
                }, function (err) {
                    $scope.attachmentType = [];
                });

                authService.loadingOn();

                $q.all([
                    promiseGetPostOfficeList,
                    promiseGetAreaList,
                    promiseGetSalesOfficer,
                    promiseGetAttachmentType
                ]).then(function () {
                    authService.loadingOff();
                });
                $scope.isNew = true;
            };

            pageLoad();

            
            $scope.addCustomerAttachment = function (fileToBeUploaded) {
                //if (!$scope.selectedAttachementType) {
                //    attachementModel.attachementTypeId = selectedAttachementType.Id;
                //}
            
                var formData = new FormData();
                //formData.append("attachmentType", $scope.selectedAttachementType.Id);
                formData.append("file", fileToBeUploaded);
                formData.append("description", $scope.attachmentDescription);

                fileService.fileUpload(formData);
            };
            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.datetimeClicked = function(e) {
                e.stopPropagation = true;
            }

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
        }]);