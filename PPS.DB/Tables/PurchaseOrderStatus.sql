CREATE TABLE [dbo].[PurchaseOrderStatus]
(
	[Id] INT NOT NULL , 
    [Status] VARCHAR(20) NOT NULL, 
    CONSTRAINT [PK_PurchaseOrderStatus] PRIMARY KEY ([Id])
)
