CREATE TABLE [dbo].[ProductStandardType] (
    [Id]           INT          NOT NULL IDENTITY (1,1),
    [ProductStandardTypeName] VARCHAR (50) NOT NULL
    CONSTRAINT [ProductStandardType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

