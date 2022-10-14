CREATE TABLE [dbo].[UnitType] (
    [Id]           INT          NOT NULL,
    [UnitTypeName] VARCHAR (50) NOT NULL,
    [TypeFlag] INT NULL, 
    CONSTRAINT [UnitType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

