CREATE TABLE [dbo].[SalesBase]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SalesBaseName] VARCHAR(30) NOT NULL, 
    [SalesAreaId] INT NOT NULL, 
    CONSTRAINT [FK_SalesBase_SalesArea] FOREIGN KEY ([SalesAreaId]) REFERENCES [SalesArea]([Id])
)
