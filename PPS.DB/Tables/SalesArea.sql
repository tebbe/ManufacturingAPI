CREATE TABLE [dbo].[SalesArea]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SalesAreaName] VARCHAR(30) NOT NULL, 
    [SalesDivisionId] INT NOT NULL, 
    CONSTRAINT [FK_SalesArea_SalesDivision] FOREIGN KEY ([SalesDivisionId]) REFERENCES [SalesDivision]([Id])
)
