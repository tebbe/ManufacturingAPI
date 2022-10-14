CREATE TABLE [dbo].[EmployeeSalesLocation]
(
	[Id] INT NOT NULL  IDENTITY (1, 1),
	[EmployeeId] INT NOT NULL,
	[DivisionId] INT NOT NULL,
	[AreaId] INT NULL,
	[BaseId] INT NULL,
	[CreatedBy] INT  NOT NULL,
	[CreatedOn] DATETIME  NOT NULL,
	[UpdatedBy] INT NULL,
	[UpdatedOn] DATETIME NULL,
	CONSTRAINT [EmployeeSalesLocation_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [EmployeeSalesLocation_Employee_Foreign_Key] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
	CONSTRAINT [EmployeeSalesLocation_SalesDivision_Foreign_Key] FOREIGN KEY ([DivisionId]) REFERENCES [dbo].[SalesDivision] ([Id]),
	CONSTRAINT [EmployeeSalesLocation_SalesArea_Foreign_Key] FOREIGN KEY ([AreaId]) REFERENCES [dbo].[SalesArea] ([Id]),
	CONSTRAINT [EmployeeSalesLocation_SalesBase_Foreign_Key] FOREIGN KEY ([BaseId]) REFERENCES [dbo].[SalesBase] ([Id]),
)


