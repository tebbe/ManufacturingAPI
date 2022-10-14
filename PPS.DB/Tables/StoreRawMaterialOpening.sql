CREATE TABLE [dbo].[StoreRawMaterialOpening]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
	[RawMaterialTypeId] INT NOT NULL,
	[Quantity] FLOAT NOT NULL,
    [FiscalYear] INT NOT NULL, 
    CONSTRAINT [PK_StoreRawMaterialOpening] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StoreRawMaterialOpening_RawMaterialType] FOREIGN KEY ([RawMaterialTypeId]) REFERENCES [RawMaterialType]([Id])
)
