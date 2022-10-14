CREATE TABLE [dbo].[CustomerTransactionDetail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [CustomerTransactionId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [BookNo] INT NULL, 
    [BookSerialNo] INT NULL, 
    [TransactionAmount] FLOAT NOT NULL,
	[TransactionDetailId] INT NULL, 
    CONSTRAINT [FK_dbo.CustomerTransactionDetail_CustomerTransaction] FOREIGN KEY ([CustomerTransactionId]) REFERENCES [dbo].[CustomerTransaction] ([Id]),
	CONSTRAINT [FK_dbo.CustomerTransactionDetail_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]),
	CONSTRAINT [FK_dbo.CustomerTransactionDetail_TransactionDetail] FOREIGN KEY ([TransactionDetailId]) REFERENCES [dbo].[TransactionDetail] ([Id])
)
