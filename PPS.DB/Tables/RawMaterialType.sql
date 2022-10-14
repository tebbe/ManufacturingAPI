CREATE TABLE [dbo].[RawMaterialType] (
    [Id]                  INT          NOT NULL,
    [RawMaterialTypeName] VARCHAR (50) NOT NULL,
    [UnitTypeId]          INT          NOT NULL,
    [AccountHeadId] INT NOT NULL, 
    CONSTRAINT [RawMaterialType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [RawMaterialType_UnitType_Foreign_Key] FOREIGN KEY ([UnitTypeId]) REFERENCES [dbo].[UnitType] ([Id]),
    CONSTRAINT [FK_RawMaterialType_AccountHead] FOREIGN KEY ([AccountHeadId]) REFERENCES [AccountHead]([Id])
);

