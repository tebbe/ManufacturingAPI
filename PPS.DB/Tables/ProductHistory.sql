CREATE TABLE [dbo].[ProductHistory]
(
	 [Id] INT IDENTITY (1, 1) NOT NULL,
 [ProductId] INT NOT NULL,
    [Name]          VARCHAR (300)  NOT NULL,
    [Code]          VARCHAR (20)   NULL,
 [Color]          VARCHAR (20)   NULL,
 [ProductStandardTypeId] INT     NULL,
 [Thickness]          VARCHAR (20)    NULL,
 [Length]     DECIMAL (5, 2) NULL,
 [UnitTypeId] INT            NULL,
    [UnitPrice]     DECIMAL (7, 2) NOT NULL,
    [ProductTypeId] INT  NOT NULL,
    [AccountHeadId] INT NOT NULL,
 [HistoryById] INT NOT NULL,
 [HistoryDate] DATETIME NOT NULL,
    [BatchId] INT NULL, 
    CONSTRAINT [ProductHistory_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductHistory_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id]), 
    CONSTRAINT [FK_ProductHistory_AccountHead] FOREIGN KEY ([AccountHeadId]) REFERENCES [AccountHead]([Id]), 
    CONSTRAINT [FK_ProductHistory_ProductStandardType] FOREIGN KEY ([ProductStandardTypeId]) REFERENCES [ProductStandardType]([Id]), 
    CONSTRAINT [FK_ProductHistory_UnitType] FOREIGN KEY ([UnitTypeId]) REFERENCES [UnitType]([Id]), 
    CONSTRAINT [FK_ProductHistory_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
)
