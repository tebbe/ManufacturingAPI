CREATE TABLE [dbo].[Product] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (300)  NOT NULL,
    [Code]          VARCHAR (20)   NULL,
	[Color]          VARCHAR (20)   NULL,
	[ProductStandardTypeId] INT     NULL,
	[Thickness]          VARCHAR (20)    NULL,
	[Length]     DECIMAL (5, 2) NULL,
	[UnitTypeId] INT            NULL,
    [UnitPrice]     DECIMAL (7, 2) NOT NULL,
    [ProductTypeId] INT            NOT NULL,
    [AccountHeadId] INT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    CONSTRAINT [Product_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Product_ProductType_Foreign_Key] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id]), 
    CONSTRAINT [FK_Product_AccountHead] FOREIGN KEY ([AccountHeadId]) REFERENCES [AccountHead]([Id]), 
    CONSTRAINT [FK_Product_ProductStandardType] FOREIGN KEY ([ProductStandardTypeId]) REFERENCES [ProductStandardType]([Id]), 
    CONSTRAINT [FK_Product_UnitType] FOREIGN KEY ([UnitTypeId]) REFERENCES [UnitType]([Id])
);

