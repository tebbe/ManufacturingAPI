CREATE TABLE [dbo].[CustomerSalesCredit]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [CustomerId] INT NOT NULL, 
    [MonthlyCredit] DECIMAL NULL, 
    [YearlyCredit] DECIMAL NULL, 
    [EffectiveDate] DATE NULL, 
    [SalesCapacityYearly] DECIMAL NULL, 
    CONSTRAINT [FK_CustomerSalesCredit_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id])
)
