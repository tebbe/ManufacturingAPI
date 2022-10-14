CREATE TABLE [dbo].[CurrentProductStock]
(
	[Id] INT NOT NULL IDENTITY, 
    [ProductId] INT NOT NULL, 
    [TotalQuantity] INT NOT NULL, 
    [DeliveredQuantity] INT NOT NULL, 
    [AllocatedQuantity] INT NOT NULL, 
    [AvailableQuantity] INT NOT NULL, 
    CONSTRAINT [PK_CurrentProductStock] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_CurrentProductStock_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
    
)
