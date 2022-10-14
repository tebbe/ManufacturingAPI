CREATE TABLE [dbo].[PurchaseOrderDetail] (
    [Id]                INT        IDENTITY (1, 1) NOT NULL,
    [PurchaseOrderId]   INT        NOT NULL,
    [RawMaterialTypeId] INT        NOT NULL,
    [Quantity]          FLOAT        NULL,
    [Price]             FLOAT (53) NULL,
    [AccountHeadId] INT NULL, 
    CONSTRAINT [PurchaseOrderDetail_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [PurchaseOrderDetail_PurchaseOrder_Foreign_Key] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrder] ([Id]),
    CONSTRAINT [PurchaseOrderDetail_RawMaterialType_Foreign_Key] FOREIGN KEY ([RawMaterialTypeId]) REFERENCES [dbo].[RawMaterialType] ([Id])
);

