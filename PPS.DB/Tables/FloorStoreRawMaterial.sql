CREATE TABLE [dbo].[FloorStoreRawMaterial]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [BatchRequisitionId] INT NOT NULL, 
	[RawMaterialTypeId] INT NOT NULL,
	[Quantity] FLOAT NOT NULL,
    [ReceivedBy] INT NULL, 
    [ReceivedOn] DATETIME NULL, 
    CONSTRAINT [PK_FloorStoreRawMaterial] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_FloorStoreRawMaterial_BatchRequisition] FOREIGN KEY ([BatchRequisitionId]) REFERENCES [BatchRequisition]([Id]),
	CONSTRAINT [FK_FloorStoreRawMaterial_User] FOREIGN KEY ([ReceivedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_FloorStoreRawMaterial_RawMaterialType] FOREIGN KEY ([RawMaterialTypeId]) REFERENCES [RawMaterialType]([Id])
)
