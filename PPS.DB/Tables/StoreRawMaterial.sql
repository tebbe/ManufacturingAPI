CREATE TABLE [dbo].[StoreRawMaterial]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [PurchaseOrderId] INT NOT NULL, 
	[RawMaterialTypeId] INT NOT NULL,
	[Quantity] FLOAT NOT NULL,
    [ReceivedBy] INT NOT NULL, 
    [ReceivedOn] DATETIME NOT NULL, 
    CONSTRAINT [PK_StoreRawMaterial] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_StoreRawMaterial_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrder]([Id]),
	CONSTRAINT [FK_StoreRawMaterial_User] FOREIGN KEY ([ReceivedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_StoreRawMaterial_RawMaterialType] FOREIGN KEY ([RawMaterialTypeId]) REFERENCES [RawMaterialType]([Id])
)
