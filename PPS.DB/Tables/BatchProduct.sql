CREATE TABLE [dbo].[BatchProduct] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [BatchRequisitionId] INT NOT NULL,
    [ProductId]          INT NOT NULL,
    [EstimatedQuantity]  INT NOT NULL,
    CONSTRAINT [BatchProduct_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [BatchProduct_BatchRequisition_Foreign_Key] FOREIGN KEY ([BatchRequisitionId]) REFERENCES [dbo].[BatchRequisition] ([Id]),
    CONSTRAINT [BatchProduct_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

