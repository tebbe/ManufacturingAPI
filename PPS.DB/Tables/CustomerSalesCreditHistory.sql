CREATE TABLE [dbo].[CustomerSalesCreditHistory]
(
	[Id] INT NOT NULL  IDENTITY (1, 1), 
    [CustomerId] INT NOT NULL, 
    [MonthlyCredit] DECIMAL NULL, 
    [YearlyCredit] DECIMAL NULL, 
    [EffectiveDate] DATE NULL, 
    [SalesCapacityYearly] DECIMAL NULL, 
    CONSTRAINT [PK_CustomerSalesCreditHistory] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_CustomerSalesCreditHistory_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id])
)
