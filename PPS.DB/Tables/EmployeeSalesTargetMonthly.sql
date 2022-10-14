CREATE TABLE [dbo].[EmployeeSalesTargetMonthly]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [EmployeeId] INT NOT NULL, 
    [SalesTarget] DECIMAL(8) NOT NULL DEFAULT 0, 
	[TeamTarget] DECIMAL(8) NOT NULL DEFAULT 0, 
    [Achievement] DECIMAL(8) NULL, 
    [Percentage] DECIMAL(4, 2) NULL,
    [SalesYear] INT NOT NULL,
	[SalesMonth] INT NOT NULL, 
    CONSTRAINT [FK_EmployeeSalesTargetMonthly_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee]([Id]) 
)
