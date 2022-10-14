CREATE TABLE [dbo].[DemandOrderDetail] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [DemandOrderId] INT        NOT NULL,
    [ProductId]     INT        NOT NULL,
    [Quantity]      INT        NOT NULL,
    [Discount]      FLOAT  NULL,
    [UnitPrice] FLOAT NOT NULL, 
	[TotalPrice] FLOAT NOT NULL,     
    CONSTRAINT [DemandOrderDetail_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [DemandOrderDetail_DemandOrder_Foreign_Key] FOREIGN KEY ([DemandOrderId]) REFERENCES [dbo].[DemandOrder] ([Id]),
    CONSTRAINT [DemandOrderDetail_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

