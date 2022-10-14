CREATE TABLE [dbo].[InvoiceTransaction]
(
	[Id] INT  IDENTITY (1, 1) NOT NULL,
    [InvoiceId] INT        NOT NULL,
	[TransactionAmount] FLOAT NOT NULL, 
    [TransactionDate] DATETIME NOT NULL,
    [CreatedBy]      INT NOT NULL,
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    CONSTRAINT [InvoiceTransaction_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [InvoiceTransaction_Invoice_Foreign_Key] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id])
)
