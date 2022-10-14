CREATE TABLE [dbo].[ProductType] (
    [Id]              INT           NOT NULL,
    [ProductTypeName] VARCHAR (300) NOT NULL,
    [Description]     VARCHAR (500) NULL,
    [ProductTypeGroupId] INT NOT NULL DEFAULT -1, 
    CONSTRAINT [ProductType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_ProductType_ProductTypeGroup] FOREIGN KEY ([ProductTypeGroupId]) REFERENCES [ProductTypeGroup]([Id])
);

