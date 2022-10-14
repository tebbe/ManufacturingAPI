'use strict';

angular.module('AtlasPPS').controller('legalDocumentEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'legalDocumentService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$state', '$q', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, legalDocumentService, ngAuthSettings, DTOptionsBuilder, PpsConstant,$state,$q, $window) {
            $scope.processComplated = true;
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

            $scope.modelHeading = "Edit Legal Document";
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Edit";
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
            var leDocId;
            //Page load

          
        
            $scope.selectedCompanyName = {
                selected: null
            }
            $scope.selectedDate = {
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
            $scope.legalDocument = [];
            $scope.selectedLegalDocument = {
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
                UpdatedBy: null,
                UpdatedOn: new Date()
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
            $scope.setExpireDateNotApplicable = function () {
                if ($scope.expireDateCheckbox) {
                    $scope.legalDocument.ExpireDate = null;
                }
            }
            var fillDataFunction = function () {
                $scope.legalDocument.IssueDate = new Date($scope.legalDocument.IssueDate);
                $scope.legalDocument.ExpireDate = new Date($scope.legalDocument.ExpireDate);
                $scope.selectedCompanyName.selected = _.filter($scope.company,
                    function (item) {
                        return item.Id === $scope.legalDocument.CompanyId;
                    })[0];

                $scope.selectedLegalDocumentType.selected = _.filter($scope.legalDocumentType,
                    function (item) {
                        return item.Id === $scope.legalDocument.DocumentTypeId;
                    })[0];

                $scope.selectedLegalDocumentRenewalCategory.selected = _.filter($scope.legalDocumentRenewalCategory,
                    function (item) {
                        return item.Id === $scope.legalDocument.DocumentRenewCategoryId;
                    })[0];

                $scope.selectedLegalDocumentStatus.selected = _.filter($scope.legalDocumentStatus,
                    function (item) {
                        return item.Id === $scope.legalDocument.DocumentStatusId;
                    })[0];
            }

            var validate = function () {
                if ($scope.selectedCompanyName.selected.Id != null && $scope.selectedLegalDocumentType.selected.Id != null && $scope.selectedLegalDocumentRenewalCategory.selected.Id != null && $scope.selectedLegalDocumentStatus.Id != null) {
                    return false;
                }
                return true;
            };

            var clearField;


            $scope.updateLegalDocument = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.selectedLegalDocument.Id = $scope.legalDocument.Id;
                $scope.selectedLegalDocument.CompanyId = $scope.selectedCompanyName.selected.Id;
                $scope.selectedLegalDocument.DocumentTypeId = $scope.selectedLegalDocumentType.selected.Id;
                $scope.selectedLegalDocument.IssueDate = moment($scope.legalDocument.IssueDate).format("MM-DD-YYYY");
                $scope.selectedLegalDocument.DocumentNumber = $scope.legalDocument.DocumentNumber;
                if ($scope.expireDateCheckbox) {
                    $scope.selectedLegalDocument.ExpireDate = null;
                } else {
                    $scope.selectedLegalDocument.ExpireDate = moment($scope.legalDocument.ExpireDate).format("MM-DD-YYYY");
                }
                $scope.selectedLegalDocument.DocumentRenewCategoryId = $scope.selectedLegalDocumentRenewalCategory.selected.Id;
                $scope.selectedLegalDocument.DocumentStatusId = $scope.selectedLegalDocumentStatus.selected.Id;
                $scope.selectedLegalDocument.OrganizationName = $scope.legalDocument.OrganizationName;
                $scope.selectedLegalDocument.OrganizationAddress = $scope.legalDocument.OrganizationAddress;
                $scope.selectedLegalDocument.OrganizationContactEmail = $scope.legalDocument.OrganizationContactEmail;
                $scope.selectedLegalDocument.OrganizationContactName = $scope.legalDocument.OrganizationContactName;
                $scope.selectedLegalDocument.OrganizationPhoneNumber = $scope.legalDocument.OrganizationPhoneNumber;
                $scope.selectedLegalDocument.IsActive = $scope.activeOrInactiveCheckBox;
                authService.loadingOn();
                var promise = legalDocumentService.updateLegalDocument($scope.selectedLegalDocument);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true; 
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
                    $rootScope.userId = authData.userId;
                } else {
                    $rootScope.userName = null;
                    $location.path('/login');
                }

                if ($state.params && $state.params.Id) {
                    leDocId = _.parseInt($state.params.Id);
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

                var getLegalDocById = legalDocumentService.getLegalDocumentById(leDocId);
                getLegalDocById.then(function (response) {
                    $scope.legalDocument = response;
                    if ($scope.legalDocument.ExpireDate === null) {
                        $scope.legalDocument.ExpireDate = null;
                        $scope.activeOrInactiveCheckBox = true;
                    }
                    if ($scope.legalDocument.IsActive) {
                        $scope.activeOrInactiveCheckBox = true;
                    }
                }, function (err) {
                    $scope.legalDocument = [];
                });

               

                authService.loadingOn();
                $q.all([
                    getCompanyList,
                    getlegalDocTypeList,
                    getLegalDocRenewalCategory,
                    getLegalDocStatus,                   
                    getLegalDocById]).then(function () {
                        fillDataFunction();
                        authService.loadingOff();
                    });


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