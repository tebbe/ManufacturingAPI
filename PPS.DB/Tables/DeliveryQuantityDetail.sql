CREATE TABLE [dbo].[DeliveryQuantityDetail]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
	[DeliveryQuantityId] INT NOT NULL, 
	[ProductId] INT NOT NULL, 
	[Quantity] INT NOT NULL,
	CONSTRAINT [DeliveryQuantityDetail_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [DeliveryQuantityDetail_DeliveryQuantity_Foreign_Key] FOREIGN KEY ([DeliveryQuantityId]) REFERENCES [dbo].[DeliveryQuantity] ([Id]),
	CONSTRAINT [DeliveryQuantityDetail_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
)
