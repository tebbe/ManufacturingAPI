IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Area')
BEGIN
    CREATE TABLE Area
	(
		Id INT NOT NULL IDENTITY(1,1),
		AreaName VARCHAR(50) NOT NULL,
		CONSTRAINT Area_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'District')
BEGIN
	CREATE TABLE District
	(
		Id INT NOT NULL IDENTITY(1,1),
		DistrictName VARCHAR(50) NOT NULL,
		CONSTRAINT District_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PoliceStation')
BEGIN
	CREATE TABLE PoliceStation
	(
		Id INT NOT NULL IDENTITY(1,1),
		PoliceStationName VARCHAR(50) NOT NULL,
		DistrictId INT NOT NULL,
		CONSTRAINT PoliceStation_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT PoliceStation_District_Foreign_Key FOREIGN KEY(DistrictId) REFERENCES District(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PostOffice')
BEGIN
	CREATE TABLE PostOffice
	(
		Id INT NOT NULL IDENTITY(1,1),
		PostOfficeName VARCHAR(50) NOT NULL,
		PostCode VARCHAR(10) NOT NULL,
		PoliceStationId INT NOT NULL,
		CONSTRAINT PostOffice_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT PostOffice_PoliceStation_Foreign_Key FOREIGN KEY(PoliceStationId) REFERENCES PoliceStation(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Department')
BEGIN
	CREATE TABLE Department
	(
		Id INT NOT NULL IDENTITY(1,1),
		DepartmentName VARCHAR(100) NOT NULL,
		CONSTRAINT Department_Primary_Key PRIMARY KEY (Id),
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Employee')
BEGIN
	CREATE TABLE Employee
	(
		Id INT NOT NULL IDENTITY(1,1),
		EmployeeId VARCHAR(50) NOT NULL,
		FirstName VARCHAR(50) NOT NULL,
		LastName VARCHAR(50) NOT NULL,
		PostOfficeId INT NOT NULL,
		Email VARCHAR(50) NOT NULL,
		Mobile VARCHAR(16) NOT NULL,
		Phone VARCHAR(16) NOT NULL,
		DepartmentId INT NOT NULL,
		BloodGroup VARCHAR(10) NOT NULL,
		ImageId VARCHAR(300) NOT NULL,
		CONSTRAINT Employee_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT Employee_PostOffice_Foreign_Key FOREIGN KEY(PostOfficeId) REFERENCES PostOffice(Id),
		CONSTRAINT Employee_Department_Foreign_Key FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RejectedReasonType')
BEGIN
	CREATE TABLE RejectedReasonType
	(
		Id INT NOT NULL IDENTITY(1,1),
		RejectedReasonTypeName VARCHAR(300) NOT NULL,
		CONSTRAINT RejectedReasonType_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Supplier')
BEGIN
	CREATE TABLE Supplier
	(
		Id INT NOT NULL IDENTITY(1,1),
		SupplierName VARCHAR(50) NOT NULL,
		Address VARCHAR(300) NOT NULL,
		ContactPerson VARCHAR(150) NOT NULL,
		Phone VARCHAR(16) NOT NULL,
		ContactPersonPhone VARCHAR(16) NOT NULL,
		Email VARCHAR(50) NOT NULL,
		ContactPersonEmail VARCHAR(50) NOT NULL,
		CONSTRAINT Supplier_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PurchaseOrder')
BEGIN
	CREATE TABLE PurchaseOrder
	(
		Id INT NOT NULL IDENTITY(1,1),
		PurchaseOrderDate DATETIME NOT NULL,
		SupplierId INT NOT NULL,
		Note VARCHAR(500) NOT NULL,
		VerifiedBy INT NOT NULL,
		VerifiedOn DATETIME NOT NULL,
		ApprovedBy INT NOT NULL,
		ApprovedOn DATETIME NOT NULL,
		CreatedBy INT NOT NULL,
		CreatedOn DATETIME NOT NULL,
		IsCurrentRecord BIT NOT NULL,
		PreviousId INT NOT NULL,
		Locked BIT NOT NULL,
		RejectedReasonTypeId INT NOT NULL,
		RejectedBy INT NOT NULL,
		RejectedOn DATETIME NOT NULL,
		PaymentType VARCHAR(20) NOT NULL,
		EstimatedDeliveryDate DATETIME NOT NULL,
		PriceValidity INT NOT NULL,
		CONSTRAINT PurchaseOrder_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT PurchaseOrder_Supplier_Foreign_Key FOREIGN KEY(SupplierId) REFERENCES Supplier(Id),
		CONSTRAINT PurchaseOrder_RejectedReasonType_Foreign_Key FOREIGN KEY(RejectedReasonTypeId) REFERENCES RejectedReasonType(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'UnitType')
BEGIN
	CREATE TABLE UnitType
	(
		Id INT NOT NULL IDENTITY(1,1),
		UnitTypeName VARCHAR(50) NOT NULL,
		CONSTRAINT UnitType_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RawMaterialType')
BEGIN
	CREATE TABLE RawMaterialType
	(
		Id INT NOT NULL IDENTITY(1,1),
		RawMaterialTypeName VARCHAR(50) NOT NULL,
		UnitTypeId INT NOT NULL,
		CONSTRAINT RawMaterialType_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT RawMaterialType_UnitType_Foreign_Key FOREIGN KEY(UnitTypeId) REFERENCES UnitType(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PurchaseOrderDetail')
BEGIN
	CREATE TABLE PurchaseOrderDetail
	(
		Id INT NOT NULL IDENTITY(1,1),
		PurchaseOrderId INT NOT NULL,
		RawMaterialTypeId INT NOT NULL,
		Quantity INT NOT NULL,
		Price FLOAT NOT NULL,
		CONSTRAINT PurchaseOrderDetail_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT PurchaseOrderDetail_PurchaseOrder_Foreign_Key FOREIGN KEY(PurchaseOrderId) REFERENCES PurchaseOrder(Id),
		CONSTRAINT PurchaseOrderDetail_RawMaterialType_Foreign_Key FOREIGN KEY(RawMaterialTypeId) REFERENCES RawMaterialType(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CustomerType')
BEGIN
	CREATE TABLE CustomerType
	(
		Id INT NOT NULL IDENTITY(1,1),
		CustomerTypeName VARCHAR(50) NOT NULL,
		CONSTRAINT CustomerType_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Customer')
BEGIN
	CREATE TABLE Customer
	(
		Id INT NOT NULL IDENTITY(1,1),
		CustomerName VARCHAR(50) NOT NULL,
		Code VARCHAR(20) NOT NULL,
		CustomerAddress VARCHAR(300) NOT NULL,
		Mobile VARCHAR(16) NOT NULL,
		Phone VARCHAR(16) NOT NULL,
		Villege VARCHAR(100) NOT NULL,
		PostOfficeId INT NOT NULL,
		CONSTRAINT Customer_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT Customer_PostOffice_Foreign_Key FOREIGN KEY(PostOfficeId) REFERENCES PostOffice(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DemandOrderType')
BEGIN
	CREATE TABLE DemandOrderType
	(
		Id INT NOT NULL IDENTITY(1,1),
		DemandOrderTypeName VARCHAR(50) NOT NULL,
		CONSTRAINT DemandOrderType_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DiscountType')
BEGIN
	CREATE TABLE DiscountType
	(
		Id INT NOT NULL IDENTITY(1,1),
		DiscountTypeName VARCHAR(300) NOT NULL,
		CONSTRAINT DiscountType_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DemandOrder')
BEGIN
	CREATE TABLE DemandOrder
	(
		Id INT NOT NULL IDENTITY(1,1),
		DONo INT NOT NULL,
		DemandOrderTypeId INT NOT NULL,
		DiscountTypeId INT NOT NULL,
		DODate DATETIME NOT NULL,
		VerifiedBy INT NOT NULL,
		VerifiedOn DATETIME NOT NULL,
		ApprovedBy INT NOT NULL,
		ApprovedOn DATETIME NOT NULL,
		ReferenceDONo VARCHAR(25) NOT NULL,
		SubmittedBy INT NOT NULL,
		SubmittedOn INT NOT NULL,
		RejectedBy INT NOT NULL,
		RejectedOn DATETIME NOT NULL,
		ReturnedBy INT NOT NULL,
		ReturnedOn DATETIME NOT NULL,
		CreatedBy INT NOT NULL,
		CreatedOn DATETIME NOT NULL,
		IsCurrentRecord INT NOT NULL,
		PreviousId INT NOT NULL,
		Locked BIT NOT NULL,
		RejectedReasonTypeId INT NOT NULL,
		CONSTRAINT DemandOrder_Primary_Key PRIMARY KEY(Id),
		CONSTRAINT DemandOrder_DemandOrderType_Foreign_Key FOREIGN KEY(DemandOrderTypeId) REFERENCES DemandOrderType(Id),
		CONSTRAINT DemandOrder_DiscountType_Foreign_Key FOREIGN KEY(DiscountTypeId) REFERENCES DiscountType(Id),
		CONSTRAINT DemandOrder_RejectedReasonType_Foreign_Key FOREIGN KEY(RejectedReasonTypeId) REFERENCES RejectedReasonType(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ProductType')
BEGIN
	CREATE TABLE ProductType
	(
		Id INT NOT NULL IDENTITY(1,1),
		ProductTypeName VARCHAR(300) NOT NULL,
		CONSTRAINT ProductType_Primary_Key PRIMARY KEY (Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BatchRequisition')
BEGIN
	CREATE TABLE BatchRequisition
	(
		Id INT NOT NULL IDENTITY(1,1),
		RawMaterialTypeId INT NOT NULL,
		Quantity INT NOT NULL,
		CreatedBy INT NOT NULL,
		CreatedOn DATETIME NOT NULL,
		DeliveredBy INT NOT NULL,
		DeliveredOn DATETIME NOT NULL,
		CONSTRAINT BatchRequisition_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT BatchRequisition_RawMaterialType_Foreign_Key FOREIGN KEY(RawMaterialTypeId) REFERENCES RawMaterialType(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'FinishedGood')
BEGIN
	CREATE TABLE FinishedGood
	(
		Id INT NOT NULL IDENTITY(1,1),
		ProductTypeId INT NOT NULL,
		BatchRequisitionId INT NOT NULL,
		Quantity INT NOT NULL,
		CONSTRAINT FinishedGood_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT FinishedGood_ProductType_Foreign_Key FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
		CONSTRAINT FinishedGood_BatchRequisition_Foreign_Key FOREIGN KEY(BatchRequisitionId) REFERENCES BatchRequisition(Id),
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BatchProduct')
BEGIN
	CREATE TABLE BatchProduct
	(
		Id INT NOT NULL IDENTITY(1,1),
		BatchRequisitionId INT NOT NULL,
		ProductTypeId INT NOT NULL,
		EstimatedQuantity INT NOT NULL,
		CONSTRAINT BatchProduct_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT BatchProduct_BatchRequisition_Foreign_Key FOREIGN KEY(BatchRequisitionId) REFERENCES BatchRequisition(Id),
		CONSTRAINT BatchProduct_ProductType_Foreign_Key FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Invoice')
BEGIN
	CREATE TABLE Invoice
	(
		Id INT NOT NULL IDENTITY(1,1),
		InvoiceNo INT NOT NULL,
		DemandOrderId INT NOT NULL,
		InvoiceDate DATETIME NOT NULL,
		CreatedBy INT NOT NULL,
		CreatedOn DATETIME NOT NULL,
		Note VARCHAR(500) NOT NULL,
		CONSTRAINT Invoice_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT Invoice_DemandOrder_Foreign_Key FOREIGN KEY(DemandOrderId) REFERENCES DemandOrder(Id)
	);
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DemandOrderDetail')
BEGIN
	CREATE TABLE DemandOrderDetail
	(
		Id INT NOT NULL IDENTITY(1,1),
		DemandOrderId INT NOT NULL,
		ProductTypeId INT NOT NULL,
		Quantity INT NOT NULL,
		Discount FLOAT NOT NULL,
		CONSTRAINT DemandOrderDetail_Primary_Key PRIMARY KEY (Id),
		CONSTRAINT DemandOrderDetail_DemandOrder_Foreign_Key FOREIGN KEY(DemandOrderId) REFERENCES DemandOrder(Id),
		CONSTRAINT DemandOrderDetail_ProductType_Foreign_Key FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id)
	);
END
--IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Area')
--BEGIN
--	CREATE TABLE User
--	(
--		Id INT NOT NULL IDENTITY(1,1),
--		UserId VARCHAR(50) NOT NULL,
--		EmployeeId INT NOT NULL,
--		CONSTRAINT User_Primary_Key PRIMARY KEY (Id),
--		CONSTRAINT User_Employee_Foreign_Key FOREIGN KEY(EmployeeId) REFERENCES Employee(Id)
--	);
--END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Policy')
BEGIN
	CREATE TABLE [dbo].[Policy](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[PolicyName] [varchar](150) NOT NULL,
		[Description] [varchar](200) NULL,
		CONSTRAINT Policy_Primary_Key PRIMARY KEY (Id)
	);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RolePolicy')
BEGIN
	CREATE TABLE [dbo].[RolePolicy](
		[RoleId] [int] NOT NULL,
		[PolicyId] [int] NOT NULL
		CONSTRAINT RolePolicy_Role_Foreign_Key FOREIGN KEY(RoleId) REFERENCES [Role](Id),
		CONSTRAINT RolePolicy_Policy_Foreign_Key FOREIGN KEY(PolicyId) REFERENCES [Policy](Id)
	);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RouteResource')
BEGIN
	CREATE TABLE [dbo].[RouteResource](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ControllerName] [varchar](50) NOT NULL,
		[ActionName] [varchar](50) NOT NULL,
		CONSTRAINT RouteResource_Primary_Key PRIMARY KEY (Id)
	);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PolicyRouteResource')
BEGIN
	CREATE TABLE [dbo].[PolicyRouteResource](
		[PolicyId] [int] NOT NULL,
		[RouteResourceId] [int] NOT NULL
		CONSTRAINT PolicyRouteResource_Policy_Foreign_Key FOREIGN KEY(PolicyId) REFERENCES [Policy](Id),
		CONSTRAINT PolicyRouteResource_RouteResource_Foreign_Key FOREIGN KEY(RouteResourceId) REFERENCES RouteResource(Id)		
	);
END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TransactionEntry')
BEGIN
	IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'UpdatedById' AND Object_ID = Object_ID(N'TransactionEntry'))
	BEGIN
		ALTER TABLE TransactionEntry ALTER COLUMN UpdatedById INT NULL;
	END	
	IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'UpdatedDate' AND Object_ID = Object_ID(N'TransactionEntry'))
	BEGIN
		ALTER TABLE TransactionEntry ALTER COLUMN UpdatedDate DATETIME NULL;
	END	
	IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'RejectedById' AND Object_ID = Object_ID(N'TransactionEntry'))
	BEGIN
		ALTER TABLE TransactionEntry ADD RejectedById INT NULL;
	END	
	IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'RejectedReasonTypeId' AND Object_ID = Object_ID(N'TransactionEntry'))
	BEGIN
		ALTER TABLE TransactionEntry ADD RejectedReasonTypeId INT NULL;
	END	
	IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'RejectedDate' AND Object_ID = Object_ID(N'TransactionEntry'))
	BEGIN
		ALTER TABLE TransactionEntry ADD RejectedDate DATETIME NULL;
	END	
	IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DeleteReason' AND Object_ID = Object_ID(N'TransactionEntry'))
	BEGIN
		ALTER TABLE TransactionEntry DROP COLUMN DeleteReason;
	END		
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TransactionRejectReasonType')
BEGIN
	CREATE TABLE [dbo].[TransactionRejectReasonType](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ReasonText] [varchar](150) NOT NULL,
		CONSTRAINT TransactionRejectReasonType_Primary_Key PRIMARY KEY (Id)
	);
END