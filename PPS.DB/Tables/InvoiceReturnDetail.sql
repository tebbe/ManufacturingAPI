CREATE TABLE [dbo].[InvoiceReturnDetail]
(
	[Id] INT NOT NULL  IDENTITY (1, 1),
	[InvoiceReturnId]		INT			NOT NULL,
    [ProductId]     INT			NOT NULL,
    [Quantity]      INT			NOT NULL,
    [TotalAmount] DECIMAL(10, 2) NOT NULL, 
    CONSTRAINT [InvoiceReturnDetail_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [InvoiceReturnDetail_InvoiceReturn_Foreign_Key] FOREIGN KEY ([InvoiceReturnId]) REFERENCES [dbo].[InvoiceReturn] ([Id]),
	CONSTRAINT [InvoiceReturnDetail_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
)
