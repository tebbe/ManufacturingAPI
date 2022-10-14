CREATE TABLE [dbo].[FinishedGoodOpening] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [ProductId]          INT NOT NULL,
    [Quantity]           INT NOT NULL,
    [FiscalYear] INT NOT NULL , 
    CONSTRAINT [FinishedGoodOpening_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FinishedGoodOpening_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

