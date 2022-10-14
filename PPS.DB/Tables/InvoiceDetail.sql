CREATE TABLE [dbo].[InvoiceDetail] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
	[InvoiceId]		INT			NOT NULL,
    [ProductId]     INT			NOT NULL,
    [Quantity]      INT			NOT NULL,
    [TotalAmount] DECIMAL(10, 2) NOT NULL, 
    CONSTRAINT [InvoiceDetail_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [InvoiceDetail_Invoice_Foreign_Key] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]),
	CONSTRAINT [InvoiceDetail_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

