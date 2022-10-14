'use strict';

angular.module('AtlasPPS').controller('employeeEditController',
    ['$scope', '$rootScope', 'localStorageService', '$location', 'notificationService', 'authService', 'employeeService', 'lookUpService', 'ngAuthSettings', 'DTOptionsBuilder', 'PpsConstant', '$q', '$state', '$window',
        function ($scope, $rootScope, localStorageService, $location, notificationService, authService, employeeService, lookUpService, ngAuthSettings, DTOptionsBuilder, PpsConstant, $q, $state, $window) {
            $scope.processComplated = true;
            var hasTransaction = false;
            var authData = localStorageService.get('authorizationData');
            var fiscalYear = null;
            var companyId = null;

            $rootScope.userId = null;
            $rootScope.userName = null;
            $rootScope.userFullName = null;
            $scope.employeeData = [];
            $scope.company = [];
            $scope.department = [];
            $scope.manager = [];
            $scope.designation = [];
            $scope.salesDivision = [];
            $scope.salesBase = [];
            $scope.salesArea = [];
            $scope.salesBaseList = [];
            $scope.salesAreaList = [];
            $scope.EmployeeTypeList = [];
            
            $scope.modelHeading = "Edit Employee";
            $scope.modelActionText = "Update";
            $scope.modelDetailText = "Edit";
            $scope.modelDetailMode = PpsConstant.TranMode.AddNew;
            var selectedModelDetailIndex = -1;
            var employeeId = null;
            $scope.editIndex = null;
            $scope.Id = null;
            $scope.EmployeeCode = null;
            $scope.FirstName = null;
            $scope.LastName = null;
            $scope.Address = null;
            $scope.PostOfficeId = null;
            $scope.Email = null;
            $scope.Mobile = null;
            $scope.Phone = null;
            $scope.JobConfirmationDate = null;
            $scope.EmployeeTypeId=null;
            $scope.IsActive = null;
            $scope.DepartmentId = null;
            $scope.BloodGroup = null;
            $scope.DateOfJoin = new Date();
            $scope.DesignationId = null;
            $scope.ManagerId = null;
            $scope.SalesDivisionId = null;
            $scope.SalesAreaId = null;
            $scope.SalesBaseId = null;
            $scope.CompanyId = null;

            $scope.btnCheckId = null;
            $scope.employeeLocationModelList = [];
            $scope.employeeLocationFinalList = [];
            $scope.employeeViewModel = {
                Id: null,
                DivisionId: null,
                AreaId: null,
                BaseId: null,
                DivisionName: null,
                AreaName: null,
                BaseName: null
            };
            $scope.employeeLocationModel = {
                Id: null,
                ActionTypeId: null,
                DivisionId: null,
                AreaId: null,
                BaseId: null
               
            };
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

            $scope.employeeModel = {
                Id: null,
                EmployeeCode: null,
                FirstName: null,
                LastName: null,
                Address: null,
                PostOfficeId: null,
                Email: null,
                IsActive: null,
                Mobile: null,
                Phone: null,
                JobConfirmationDate: new Date(),
                DepartmentId: null,
                EmployeeTypeId: null,
                BloodGroup: null,
                DesignationId: null,
                ManagerId: null,
                SalesDivisionId: null,
                SalesAreaId: null,
                SalesBaseId: null,
                CompanyId: null,
                DateOfJoin: new Date(),
                SalesLocation:[]
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


            var fillDataFunction = function () {
                $scope.employeeData.DateOfJoin = new Date($scope.employeeData.DateOfJoin);
                $scope.employeeData.JobConfirmationDate = new Date($scope.employeeData.JobConfirmationDate);
                $scope.selectedCompanyName.selected = _.filter($scope.company,
                    function (item) {
                        return item.Id === $scope.employeeData.CompanyId;
                    })[0];
                $scope.selectedEmployeeType.selected = _.filter($scope.EmployeeTypeList,
                    function (item) {
                        return item.Id === $scope.employeeData.EmployeeTypeId;
                    })[0];
                $scope.selectedDepartmentName.selected = _.filter($scope.department,
                    function (item) {
                        return item.Id === $scope.employeeData.DepartmentId;
                    })[0];

                $scope.selectedManager.selected = _.filter($scope.manager,
                    function (item) {
                        return item.Id === $scope.employeeData.ManagerId;
                    })[0];

                $scope.selectedDesignation.selected = _.filter($scope.designation,
                    function (item) {
                        return item.Id === $scope.employeeData.DesignationId;
                    })[0];

                $scope.selectedSalesDivision.selected = _.filter($scope.salesDivision,
                    function (item) {
                        return item.Id === $scope.employeeData.SalesDivisionId;
                    })[0];
                $scope.selectedSalesBase.selected = _.filter($scope.salesBase,
                    function (item) {
                        return item.Id === $scope.employeeData.SalesBaseId;
                    })[0];

                $scope.selectedSalesArea.selected = _.filter($scope.salesArea,
                    function (item) {
                        return item.Id === $scope.employeeData.SalesAreaId;
                    })[0];

            }
            var fillEditLocationMatch = function (location) {
                $scope.employeeData.DateOfJoin = new Date($scope.employeeData.DateOfJoin);
                    $scope.selectedSalesLocDivision.selected = _.filter($scope.salesDivision,
                        function (item) {
                            return item.Id === location.DivisionId;
                        })[0];
                    $scope.selectedSalesLocArea.selected = _.filter($scope.salesAreaList,
                        function (item) {
                            return item.Id === location.AreaId;
                        })[0];
                    $scope.selectedSalesLocBase.selected = _.filter($scope.salesBaseList,
                        function (item) {
                            return item.Id === location.BaseId;
                        })[0];
            }
           
            //Employee Sales Location  start
            $scope.addSalesLocation = function () {
                $scope.btnCheckId = null;
                $('#editSalesLocation').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.navigateToSalesLocationEdit = function (location,index) {
                $scope.btnCheckId = location.Id;
                $scope.editIndex = index;
                fillEditLocationMatch(location);
                $('#editSalesLocation').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            };
            $scope.navigateToSalesLocationDelete = function (location, index,type) {
                $scope.employeeLocationModel = {
                    Id: location.Id,
                    ActionTypeId: type,
                    DivisionId: location.DivisionId,
                    AreaId: location.AreaId,
                    BaseId: location.BaseId,
                };
                if (index != null) {
                    $scope.employeeLocationModelList.splice(index, 1);   
                }
                pushFinalObjectToArray($scope.employeeLocationModel,index);
            };

            $scope.SalesEditLocationClick = function (actionType) {
                $scope.employeeViewModel = {
                    DivisionName: $scope.selectedSalesLocDivision.selected === null ? "" : $scope.selectedSalesLocDivision.selected.SalesDivisionName,
                    AreaName: $scope.selectedSalesLocArea.selected === null ? "" : $scope.selectedSalesLocArea.selected.SalesAreaName,
                    BaseName: $scope.selectedSalesLocBase.selected === null ? "" : $scope.selectedSalesLocBase.selected.SalesBaseName,
                }
                $scope.employeeLocationModel = {
                    ActionTypeId: actionType,
                    DivisionId: $scope.selectedSalesLocDivision.selected === null ? "" : $scope.selectedSalesLocDivision.selected.Id,
                    AreaId: $scope.selectedSalesLocArea.selected === null ? "" : $scope.selectedSalesLocArea.selected.Id,
                    BaseId: $scope.selectedSalesLocBase.selected === null ? "" : $scope.selectedSalesLocBase.selected.Id,
                };
               
                if ($scope.editIndex != null) {
                    $scope.employeeViewModel.Id = $scope.btnCheckId;
                    $scope.employeeLocationModel.Id= $scope.btnCheckId;
                    $scope.employeeLocationModelList.splice($scope.editIndex,1);
                }
                $scope.employeeLocationModelList.push($scope.employeeViewModel);
                pushFinalObjectToArray($scope.employeeLocationModel,$scope.editIndex);

                $scope.editIndex = null;
                $scope.btnCheckId = null;
                $scope.selectedSalesLocDivision.selected = null;
                $scope.selectedSalesLocBase.selected = null;
                $scope.selectedSalesLocArea.selected = null;

                $('#editSalesLocation').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            };
            $scope.closeEditLocationModal = function () {
                $scope.btnCheckId = null;
                $scope.selectedSalesLocDivision.selected = null;
                $scope.selectedSalesLocBase.selected = null;
                $scope.selectedSalesLocArea.selected = null;
                $('#editSalesLocation').modal('toggle');
                $(".modal-backdrop").hide();
                $("body").removeClass("modal-open");
            }
          
            var pushFinalObjectToArray = function (data,indexId) {
                if (indexId != null) {
                    $scope.employeeLocationFinalList.splice(indexId, 1);
                    $scope.employeeLocationFinalList.push(data);
                } else {
                    $scope.employeeLocationFinalList.push(data);
                }
            };
            var dataForListView = function (dataList) {
                for (var i = 0; i < dataList.length; i++) {
                    $scope.employeeViewModel = {
                        Id: dataList[i].Id,
                        DivisionId: dataList[i].DivisionId,
                        AreaId: dataList[i].AreaId,
                        BaseId: dataList[i].BaseId,
                        DivisionName: dataList[i].DivisionName,
                        AreaName: dataList[i].AreaName,
                        BaseName: dataList[i].BaseName
                    };
                    $scope.employeeLocationModelList.push($scope.employeeViewModel);
                }
                $scope.employeeViewModel = null;
            };
            //Employee Sales Location end
          
            var validate = function () {
                if ($scope.selectedCompanyName.selected != null
                    && $scope.selectedDepartmentName.selected != null
                    && $scope.selectedEmployeeType.selected != null
                    && $scope.selectedDesignation.selected != null) {
                    return true;
                }
                return false;
            };


            $scope.updateEmployee = function () {
                if (!validate()) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                if ($scope.selectedSalesDivision.selected != null && $scope.selectedSalesArea.selected === null
                    && $scope.selectedSalesBase.selected === null) {
                    notificationService.showErrorNotificatoin(PpsConstant.IncompleteModelInput);
                    return;
                }
                $scope.employeeModel.Id = $scope.employeeData.Id;
                $scope.employeeModel.CompanyId = $scope.selectedCompanyName.selected.Id;
                $scope.employeeModel.EmployeeTypeId = $scope.selectedEmployeeType.selected.Id;
                $scope.employeeModel.DepartmentId = $scope.selectedDepartmentName.selected.Id;
                $scope.employeeModel.ManagerId = $scope.selectedManager.selected == null ? "" : $scope.selectedManager.selected.Id;
                $scope.employeeModel.DesignationId = $scope.selectedDesignation.selected.Id;
                $scope.employeeModel.SalesDivisionId = $scope.selectedSalesDivision.selected == null ? "" : $scope.selectedSalesDivision.selected.Id;
                $scope.employeeModel.SalesAreaId = $scope.selectedSalesArea.selected == null ? "" : $scope.selectedSalesArea.selected.Id;
                $scope.employeeModel.SalesBaseId = $scope.selectedSalesBase.selected == null ? "" : $scope.selectedSalesBase.selected.Id;
                $scope.employeeModel.IsActive = $scope.employeeData.IsActive;
                $scope.employeeModel.EmployeeCode = $scope.employeeData.EmployeeCode;
                $scope.employeeModel.FirstName = $scope.employeeData.FirstName;
                $scope.employeeModel.LastName = $scope.employeeData.LastName;
                $scope.employeeModel.Address = $scope.employeeData.Address;
                $scope.employeeModel.Email = $scope.employeeData.Email;
                $scope.employeeModel.Mobile = $scope.employeeData.Mobile;
                $scope.employeeModel.Phone = $scope.employeeData.Phone;
                $scope.employeeModel.JobConfirmationDate = moment($scope.employeeData.JobConfirmationDate).format("MM-DD-YYYY");
                $scope.employeeModel.BloodGroup = $scope.employeeData.BloodGroup;
                $scope.employeeModel.DateOfJoin = moment($scope.employeeData.DateOfJoin).format("MM-DD-YYYY");
                $scope.employeeModel.SalesLocation = $scope.employeeLocationFinalList;

                authService.loadingOn();
                var promise = employeeService.updateEmployee($scope.employeeModel);
                promise.then(function (response) {
                    notificationService.showSuccessNotificatoin(PpsConstant.DefaultSuccess);
                    $scope.processComplated = true;
                    authService.loadingOff();
                    $location.path("/Employee/Employee/View/" + employeeId);
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

                if ($state.params && $state.params.Id) {
                    employeeId = _.parseInt($state.params.Id);
                }

                var getEmployeeById = employeeService.getEmployeeById(employeeId);
                getEmployeeById.then(function (response) {
                    $scope.employeeData = response.Employee;
                    $scope.salesArea = response.SalesAreaList;
                    $scope.salesBase = response.SalesBaseList;
                    $scope.salesAreaList = response.AreaList;
                    $scope.salesBaseList = response.BaseList;
                    $scope.employeeLocationFinalList = response.sLocation;
                    dataForListView(response.sLocation);
                }, function (err) {
                    $scope.employeeData = [];
                    $scope.salesArea = [];
                    $scope.salesBase = [];
                    $scope.salesAreaList = [];
                    $scope.salesBaseList = [];
                    $scope.employeeLocationFinalList = [];
                });
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
                    getAllDropDownList,
                    getEmployeeById]).then(function () {
                        fillDataFunction();
                        authService.loadingOff();
                    });
            };
            pageLoad();

            $scope.gotoEmployeeList = function () {
                $location.path("Employee/EmployeeList")
                $window.location.reload();
            }

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