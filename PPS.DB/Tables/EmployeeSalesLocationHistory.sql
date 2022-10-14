CREATE TABLE [dbo].[EmployeeSalesLocationHistory]
(
	[Id] INT NOT NULL  IDENTITY (1, 1),
	[EmployeeHistoryId] INT NOT NULL,
	[EmployeeId] INT NOT NULL,
	[SalesDivisionId] INT NOT NULL,
	[SalesAreaId] INT NULL,
	[SalesBaseId] INT NULL,
	[CreatedBy] INT  NOT NULL,
	[CreatedOn] DATETIME  NOT NULL,
	CONSTRAINT [EmployeeSalesLocationHistory_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [EmployeeSalesLocationHistory_EmployeeHistory_Foreign_Key] FOREIGN KEY ([EmployeeHistoryId]) REFERENCES [dbo].[EmployeeHistory] ([Id]),
	CONSTRAINT [EmployeeSalesLocationHistory_Employee_Foreign_Key] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
	CONSTRAINT [EmployeeSalesLocationHistory_SalesDivision_Foreign_Key] FOREIGN KEY ([SalesDivisionId]) REFERENCES [dbo].[SalesDivision] ([Id]),
	CONSTRAINT [EmployeeSalesLocationHistory_SalesArea_Foreign_Key] FOREIGN KEY ([SalesAreaId]) REFERENCES [dbo].[SalesArea] ([Id]),
	CONSTRAINT [EmployeeSalesLocationHistory_SalesBase_Foreign_Key] FOREIGN KEY ([SalesBaseId]) REFERENCES [dbo].[SalesBase] ([Id]),
)
