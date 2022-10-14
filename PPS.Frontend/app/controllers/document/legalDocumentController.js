'use strict';

angular.module('AtlasPPS').controller('legalDocumentController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'legalDocumentService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, legalDocumentService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.tranMode = 1;
            $scope.company = [];
            $scope.legalDocumentType = [];
            $scope.legalDocumentRenewalCategory = [];
            $scope.legalDocumentStatus = [];

            $scope.modelHeading = "Add Legal Document";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.CompanyId = null;
            $scope.DocumentTypeId = null;
            $scope.IssueDate = null;
            $scope.DocumentNumber = null;
            $scope.ExpireDate = null;
            $scope.DocumentRenewCategoryId = null;
            $scope.DocumentStatusId = null;
            $scope.IsActive = null;
            $scope.OrganizationName = null;
            $scope.OrganizationAddress = null;
            $scope.OrganizationContactEmail = null;
            $scope.OrganizationContactName = null;
            $scope.OrganizationPhoneNumber = null;
            $scope.CreatedBy = null;
            $scope.CreatedOn = null;


            $scope.selectedCompanyName = {
                selected: null
            }
            $scope.selectedDate= {
                selected: null
            }
            $scope.selectedLegalDocumentType = {
                selected: null
            }
            $scope.selectedLegalDocumentRenewalCategory = {
                selected: null
            }
            $scope.selectedLegalDocumentStatus = {
                selected: null
            }
            $scope.legalDocument = {
                CompanyId: null,
                DocumentTypeId: null,
                IssueDate: new Date(),
                DocumentNumber: null,
                ExpireDate: null,
                DocumentRenewCategoryId: null,
                DocumentStatusId: null,
                IsActive: null,
                OrganizationName: null,
                OrganizationAddress: null,
                OrganizationContactEmail: null,
                OrganizationContactName: null,
                OrganizationPhoneNumber: null,
                CreatedBy: null,
                CreatedOn: new Date()
            };

            $scope.onChangeCompany = function (selectedCompanyName) {
                if (selectedCompanyName && selectedCompanyName.Id) {
                    var result = $scope.company.filter(function (v) {
                        return v.Id === selectedCompanyName.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedCompanyName.selected = result[0];
                    }
                    else {
                        $scope.selectedCompanyName.selected = null;
                    }
                } else {
                    $scope.selectedCompanyName.selected = null;
                }
            }
            $scope.onChangeDocumentType = function (selectedLegalDocumentType) {
                if (selectedLegalDocumentType && selectedLegalDocumentType.Id) {
                    var result = $scope.legalDocumentType.filter(function (v) {
                        return v.Id === selectedLegalDocumentType.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedLegalDocumentType.selected = result[0];
                    }
                    else {
                        $scope.selectedLegalDocumentType.selected = null;
                    }
                } else {
                    $scope.selectedLegalDocumentType.selected = null;
                }
            }
            $scope.onChangeDocRenewalctegory = function (selectedLegalDocumentRenewalCategory) {
                if (selectedLegalDocumentRenewalCategory && selectedLegalDocumentRenewalCategory.Id) {
                    var result = $scope.legalDocumentRenewalCategory.filter(function (v) {
                        return v.Id === selectedLegalDocumentRenewalCategory.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedLegalDocumentRenewalCategory.selected = result[0];
                    }
                    else {
                        $scope.selectedLegalDocumentRenewalCategory.selected = null;
                    }
                } else {
                    $scope.selectedLegalDocumentRenewalCategory.selected = null;
                }
            }
            $scope.onChangeDocumentStatus = function (selectedLegalDocumentStatus) {
                if (selectedLegalDocumentStatus && selectedLegalDocumentStatus.Id) {
                    var result = $scope.legalDocumentStatus.filter(function (v) {
                        return v.Id == selectedLegalDocumentStatus.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedLegalDocumentStatus.selected = result[0];
                    } else {
                        $scope.selectedLegalDocumentStatus.selected == null;
                    }
                } else {
                    $scope.selectedLegalDocumentStatus.selected = null;
                }
            }
            var validateDoEntry;

            var validate = function () {
                if ($scope.selectedCompanyName.selected.Id != null && $scope.selectedLegalDocumentType.selected.Id != null && $scope.selectedLegalDocumentRenewalCategory.selected.Id != null && $scope.selectedLegalDocumentStatus.Id != null) {
                    return false;
                }
                return true;
            };

            var clearField;


            $scope.saveLegalDocument = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.legalDocument.CompanyId =$scope.selectedCompanyName.selected.Id;
                $scope.legalDocument.DocumentTypeId = $scope.selectedLegalDocumentType.selected.Id;
                $scope.legalDocument.IssueDate = moment($scope.issueDate).format("MM-DD-YYYY");
                $scope.legalDocument.DocumentNumber = $scope.documentNumber;
                if ($scope.expireDateCheckbox) {
                    $scope.legalDocument.ExpireDate = "";
                } else {
                    $scope.legalDocument.ExpireDate = moment($scope.expireDate).format("MM-DD-YYYY");
                }
                
                $scope.legalDocument.DocumentRenewCategoryId =$scope.selectedLegalDocumentRenewalCategory.selected.Id;
                $scope.legalDocument.DocumentStatusId =$scope.selectedLegalDocumentStatus.selected.Id;
                $scope.legalDocument.OrganizationName = $scope.organizationName;
                $scope.legalDocument.OrganizationAddress = $scope.organizationAddress;
                $scope.legalDocument.OrganizationContactEmail = $scope.organizationContactEmail;
                $scope.legalDocument.OrganizationContactName = $scope.organizationContactName;
                $scope.legalDocument.OrganizationPhoneNumber = $scope.organizationPhoneNumber;
                authService.loadingOn();
                var promise = legalDocumentService.saveLegalDocument($scope.legalDocument);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    $location.path("/Document/LegalDocument/View/"+response.Id);
                    hasTransaction = true;
                    authService.loadingOff();
                }, function (err) {
                    notificationService.showErrorNotificatoin(PpsConstant.DefaultError);
                    $scope.processComplated = true;
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
              //get data for dropdown list
                var getCompanyList = legalDocumentService.getCompanyList();
                getCompanyList.then(function (response) {
                    $scope.company = response;

                }, function (err) {
                    $scope.company = [];
                });

                var getlegalDocTypeList = legalDocumentService.getLegalDocumentType();
                getlegalDocTypeList.then(function (response) {
                    $scope.legalDocumentType = response;
                   
                }, function (err) {
                    $scope.legalDocumentType = [];
                });

                var getLegalDocRenewalCategory = legalDocumentService.getLegalDocumentRenewalCategory();
                getLegalDocRenewalCategory.then(function (response) {
                    $scope.legalDocumentRenewalCategory = response;

                }, function (err) {
                    $scope.legalDocumentRenewalCategory = [];
                });

                var getLegalDocStatus = legalDocumentService.getLeglDocumentStatus();
                getLegalDocStatus.then(function (response) {
                    $scope.legalDocumentStatus = response;

                }, function (err) {
                    $scope.legalDocumentStatus = [];
                });

                $scope.selectedDate = new Date();
            };
            pageLoad();

            $scope.gotoLegalDocList = function () {
                $location.path("/Document/LegalDocumentList")
                $window.location.reload();
            }

            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };

            $scope.addPrdToGrid = function (event) {
                if (event.which === 13) {
                    $scope.addProductItem();
                }
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