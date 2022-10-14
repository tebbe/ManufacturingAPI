CREATE TABLE [dbo].[ProductionForecastMonthly]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL,
	[SalesYear] INT NOT NULL, 
    [SalesMonth] INT NOT NULL, 
    [CompanySalesTargetId] INT NOT NULL, 
    CONSTRAINT [FK_ProductionForecastMonthly_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
)
