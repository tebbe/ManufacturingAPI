'use strict';

angular.module('AtlasPPS').controller('employeeController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService','$q' ,'authService', 'employeeService', 'lookUpService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, $q, authService, employeeService, lookUpService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;
            var text = null;
            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
           
            $scope.company = [];
            $scope.department = [];
            $scope.manager = [];
            $scope.designation = [];
            $scope.salesDivision = [];
            $scope.salesBase = [];
            $scope.salesArea = [];
            $scope.EmployeeTypeList=[];
            $scope.employeeLocationList= [];
            $scope.employeeLocationModelList = [];
            $scope.btnCheckId = null;
            $scope.modelHeading = "Add New Employee";
            $scope.modelActionText = "Save";
            $scope.modelDetailText = "Add";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;

            $scope.EmployeeCode = null;
            $scope.FirstName = null;
            $scope.LastName = null;
            $scope.Address = null;
            $scope.PostOfficeId = null;
            $scope.Email = null;
            $scope.Mobile = null;
            $scope.Phone = null;
            $scope.JobConfirmationDate = null;
            $scope.EmployeeTypeId = null;
            $scope.DepartmentId = null;
            $scope.BloodGroup = null;
            $scope.DateOfJoin = null;
            $scope.DesignationId = null;
            $scope.ManagerId = null;
            $scope.SalesDivisionId = null;
            $scope.SalesAreaId = null;
            $scope.SalesBaseId = null;
            $scope.CompanyId = null;
            $scope.selectedCompanyName = {
                selected: null
            }
            $scope.selectedEmployeeType = {
                selected: null
            }
            $scope.selectedDepartmentName = {
                selected: null
            }
            $scope.selectedManager = {
                selected: null
            }
            $scope.selectedDesignation = {
                selected: null
            }
            $scope.selectedSalesDivision = {
                selected: null
            }
            $scope.selectedSalesBase = {
                selected: null
            }
            $scope.selectedSalesArea = {
                selected: null
            }

            $scope.selectedSalesLocDivision = {
                selected: null
            }
            $scope.selectedSalesLocBase = {
                selected: null
            }
            $scope.selectedSalesLocArea = {
                selected: null
            }
            $scope.LocationViewModel = { DivisionName: null, AreaName: null, BaseName: null }
      
            $scope.employeeModel = {
                EmployeeCode : null,
                FirstName : null,
                LastName : null,
                Address : null,
                PostOfficeId : null,
                Email : null,
                Mobile : null,
                Phone: null,
                JobConfirmationDate: new Date(),
                EmployeeTypeId: null,
                DepartmentId : null,
                BloodGroup : null,
                DesignationId : null,
                ManagerId : null,
                SalesDivisionId : null,
                SalesAreaId : null,
                SalesBaseId : null,
                CompanyId: null,
                DateOfJoin: new Date(),
                SalesLocation: []

            };
            $scope.employeeLocationModel = { 
                DivisionId: null,
                AreaId: null,
                BaseId: null
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
            $scope.onChangeEmployeeType = function (selectedEmployeeType) {
                if (selectedEmployeeType && selectedEmployeeType.Id) {
                    var result = $scope.EmployeeTypeList.filter(function (v) {
                        return v.Id === selectedEmployeeType.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedEmployeeType.selected = result[0];
                    }
                    else {
                        $scope.selectedEmployeeType.selected = null;
                    }
                } else {
                    $scope.selectedEmployeeType.selected = null;
                }
            }
            $scope.onChangeDepartment = function (selectedDepartmentName) {
                if (selectedDepartmentName && selectedDepartmentName.Id) {
                    var result = $scope.department.filter(function (v) {
                        return v.Id === selectedDepartmentName.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedDepartmentName.selected = result[0];
                    }
                    else {
                        $scope.selectedDepartmentName.selected = null;
                    }
                } else {
                    $scope.selectedDepartmentName.selected = null;
                }
            }
            $scope.onChangeManager = function (selectedManager) {
                if (selectedManager && selectedManager.Id) {
                    var result = $scope.manager.filter(function (v) {
                        return v.Id === selectedManager.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedManager.selected = result[0];
                    }
                    else {
                        $scope.selectedManager.selected = null;
                    }
                } else {
                    $scope.selectedManager.selected = null;
                }
            }
            $scope.onChangeDesignation = function (selectedDesignation) {
                if (selectedDesignation && selectedDesignation.Id) {
                    var result = $scope.designation.filter(function (v) {
                        return v.Id === selectedDesignation.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedDesignation.selected = result[0];
                    }
                    else {
                        $scope.selectedDesignation.selected = null;
                    }
                } else {
                    $scope.selectedDesignation.selected = null;
                }
            }
            $scope.onChangeSalesDivision = function (selectedSalesDivision) {
                if (selectedSalesDivision && selectedSalesDivision.Id) {
                    var result = $scope.salesDivision.filter(function (v) {
                        return v.Id === selectedSalesDivision.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesDivision.selected = result[0];
                        
                        var area = employeeService.getEmployeeAreaList(result[0].Id);
                        area.then(function (response) {
                            $scope.salesArea = response;
                            $scope.selectedSalesArea.selected = null;
                            $scope.selectedSalesBase.selected = null;
                        }, function (err) {
                            $scope.salesArea = [];
                        });
                    
                    } else {
                        $scope.selectedSalesDivision.selected = null;
                    }
                } else {
                    $scope.selectedSalesDivision.selected = null;
                }
            }
            $scope.onChangeArea = function (selectedSalesArea) {
                if (selectedSalesArea && selectedSalesArea.Id) {
                    var result = $scope.salesArea.filter(function (v) {
                        return v.Id === selectedSalesArea.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesArea.selected = result[0];
                        var base = employeeService.getEmployeeBaseList(result[0].Id);
                        base.then(function (response) {
                            $scope.selectedSalesBase.selected = null;
                            $scope.salesBase = response;
                        }, function (err) {
                            $scope.salesBase = [];
                        });
                    }
                    else {
                        $scope.selectedSalesArea.selected = null;
                    }

                } else {
                    $scope.selectedSalesArea.selected = null;
                }
            }
            $scope.onChangeSalesBase = function (selectedSalesBase) {
                if (selectedSalesBase && selectedSalesBase.Id) {
                    var result = $scope.salesBase.filter(function (v) {
                        return v.Id === selectedSalesBase.Id;
                    });
                    if (result && result.length > 0) {
                       $scope.selectedSalesBase.selected = result[0];
                    }
                    else {
                        $scope.selectedSalesBase.selected = null;
                    }
                } else {
                    $scope.selectedSalesBase.selected = null;
                }
            }

            $scope.onChangeSalesLocDivision = function (selectedSalesLocDivision) {
                if (selectedSalesLocDivision && selectedSalesLocDivision.Id) {
                    var result = $scope.salesDivision.filter(function (v) {
                        return v.Id === selectedSalesLocDivision.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesLocDivision.selected = result[0];

                        var area = employeeService.getEmployeeAreaList(result[0].Id);
                        area.then(function (response) {
                            $scope.salesArea = response;
                            $scope.selectedSalesLocArea.selected = null;
                            $scope.selectedSalesLocBase.selected = null;
                        }, function (err) {
                            $scope.salesArea = [];
                        });

                    } else {
                        $scope.selectedSalesLocDivision.selected = null;
                    }
                } else {
                    $scope.selectedSalesLocDivision.selected = null;
                }
            }
            $scope.onChangeLocArea = function (selectedSalesLocArea) {
                if (selectedSalesLocArea && selectedSalesLocArea.Id) {
                    var result = $scope.salesArea.filter(function (v) {
                        return v.Id === selectedSalesLocArea.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesLocArea.selected = result[0];
                        var base = employeeService.getEmployeeBaseList(result[0].Id);
                        base.then(function (response) {
                            $scope.selectedSalesLocBase.selected = null;
                            $scope.salesBase = response;
                        }, function (err) {
                            $scope.salesBase = [];
                        });
                    }
                    else {
                        $scope.selectedSalesLocArea.selected = null;
                    }

                } else {
                    $scope.selectedSalesLocArea.selected = null;
                }
            }
            $scope.onChangeSalesLocBase = function (selectedSalesLocBase) {
                if (selectedSalesLocBase && selectedSalesLocBase.Id) {
                    var result = $scope.salesBase.filter(function (v) {
                        return v.Id === selectedSalesLocBase.Id;
                    });
                    if (result && result.length > 0) {
                        $scope.selectedSalesLocBase.selected = result[0];
                    }
                    else {
                        $scope.selectedSalesLocBase.selected = null;
                    }
                } else {
                    $scope.selectedSalesLocBase.selected = null;
                }
            }

            $scope.SalesLocationClick = function () {
                $scope.LocationViewModel =
                    {
                        DivisionName: $scope.selectedSalesLocDivision.selected === null ? "" : $scope.selectedSalesLocDivision.selected.SalesDivisionName,
                        AreaName: $scope.selectedSalesLocArea.selected === null ? "" : $scope.selectedSalesLocArea.selected.SalesAreaName,
                        BaseName: $scope.selectedSalesLocBase.selected === null ? "" : $scope.selectedSalesLocArea.selected.SalesAreaName
                    };
                $scope.employeeLocationModel = {
                    DivisionId: $scope.selectedSalesLocDivision.selected === null ? "" : $scope.selectedSalesLocDivision.selected.Id,
                    AreaId: $scope.selectedSalesLocArea.selected === null ? "" : $scope.selectedSalesLocArea.selected.Id,
                    BaseId: $scope.selectedSalesLocBase.selected === null ? "" : $scope.selectedSalesLocArea.selected.Id,
                };
                $scope.employeeLocationList.push($scope.LocationViewModel);
                $scope.employeeLocationModelList.push($scope.employeeLocationModel);

                $scope.selectedSalesLocDivision.selected = null;
                $scope.selectedSalesLocBase.selected = null;
                $scope.selectedSalesLocArea.selected = null;

                $('#addSalesLocation').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            };

            var validate = function () {
                if ($scope.selectedCompanyName.selected != null
                    && $scope.selectedEmployeeType.selected != null
                    && $scope.selectedDepartmentName.selected != null) {
                    return true;
                }
                return false;
            };
           
            $scope.saveNewEmployee = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                if ($scope.selectedSalesDivision.selected != null && $scope.selectedSalesArea.selected === null
                    && $scope.selectedSalesBase.selected === null) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.employeeModel.CompanyId = $scope.selectedCompanyName.selected.Id;
                $scope.employeeModel.EmployeeTypeId = $scope.selectedEmployeeType.selected.Id;
                $scope.employeeModel.DepartmentId = $scope.selectedDepartmentName.selected.Id;
                $scope.employeeModel.ManagerId = $scope.selectedManager.selected === null ? "" : $scope.selectedManager.selected.Id;
                $scope.employeeModel.DesignationId = $scope.selectedDesignation.selected.Id;
                $scope.employeeModel.SalesDivisionId = $scope.selectedSalesDivision.selected=== null ?"" : $scope.selectedSalesDivision.selected.Id;
                $scope.employeeModel.SalesAreaId = $scope.selectedSalesArea.selected=== null ?"" : $scope.selectedSalesArea.selected.Id;
                $scope.employeeModel.SalesBaseId = $scope.selectedSalesBase.selected === null ?"": $scope.selectedSalesBase.selected.Id;
                $scope.employeeModel.EmployeeCode = parseInt($scope.EmployeeCode);
                $scope.employeeModel.FirstName = $scope.FirstName;
                $scope.employeeModel.LastName = $scope.LastName;
                $scope.employeeModel.Address = $scope.Address;
                $scope.employeeModel.Email = $scope.Email;
                $scope.employeeModel.Mobile = $scope.Mobile;
                $scope.employeeModel.Phone = $scope.Phone;
                $scope.employeeModel.JobConfirmationDate = moment($scope.JobConfirmationDate).format("MM-DD-YYYY");
                $scope.employeeModel.BloodGroup = $scope.BloodGroup;
                $scope.employeeModel.DateOfJoin = moment($scope.dateOfJoin).format("MM-DD-YYYY");
                $scope.employeeModel.SalesLocation = $scope.employeeLocationModelList;

                authService.loadingOn();
                var promise = employeeService.addNewEmployee($scope.employeeModel);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    authService.loadingOff();
                    //$location.path("Employee/EmployeeList");   
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

                var getCompanyList = lookUpService.getCompanyList();
                getCompanyList.then(function (response) {
                    $scope.company = response;

                }, function (err) {
                    $scope.company = [];
                });

                var getAllDropDownList = lookUpService.getEmployeeDropDownList();
                getAllDropDownList.then(function (response) {
                    $scope.department = response.dept;
                    $scope.manager = response.man;
                    $scope.designation = response.desi;
                    $scope.salesDivision = response.div;
                    $scope.EmployeeTypeList = response.EmployeeType;
                  
                }, function (err) {
                    $scope.department = [];
                    $scope.manager = [];
                    $scope.designation = [];
                    $scope.salesDivision = [];
                    $scope.EmployeeTypeList = [];
                });

               
                  // dropdown list end

                authService.loadingOn();
                $q.all([
                    getCompanyList,
                    getAllDropDownList]).then(function () {
                        authService.loadingOff();
                    });
            };
            pageLoad();

            $scope.gotoEmployeeList = function () {
                $location.path("Employee/EmployeeList")
                $window.location.reload();
            }
            //Employee sales location start
            $scope.addSalesLocation = function () {
                $scope.btnCheckId = null;
                $('#addSalesLocation').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.navigateToSalesLocationEdit = function (location) {
                $scope.btnCheckId = location.Id;               
                $('#addSalesArea').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.navigateToSalesLocationDelete = function (index) {
                $scope.employeeLocationList.splice(index, 1);
                $scope.employeeLocationModelList.splice(index,1);
            }
            $scope.closeAddLocationModal = function () {
                $scope.selectedSalesLocDivision.selected = null;
                $scope.selectedSalesLocArea.selected = null;
                $scope.selectedSalesLocBase.selected = null;
                $scope.btnCheckId = null;
                $('#addSalesLocation').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            };
            //Employee Sales location End
            $rootScope.logout = function () {
                authService.logout();
                $location.path('/login');
            };
            $scope.dtOptionsAll = DTOptionsBuilder.newOptions()
                .withOption('order', [0, 'desc'])
                .withPaginationType('full_numbers').withDisplayLength(25)
                .withDOM('<"html5buttons"B>lTfgitp')
                .withButtons([
                    //{ extend: 'copy' },
                    //{ extend: 'csv' },
                    //{ extend: 'excel', title: 'Ledger List' },
                    //{ extend: 'pdf', title: 'Transactions' }
                ]);

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