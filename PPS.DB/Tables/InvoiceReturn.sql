CREATE TABLE [dbo].[InvoiceReturn]
(
	[Id] INT NOT NULL  IDENTITY(1,1),
	[InvoiceId]     INT           NOT NULL,
    [ReturnDate]   DATETIME      NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [CreatedOn]     DATETIME      NOT NULL,
    [Note]          VARCHAR (500) NULL,
    [ApprovedBy] INT NULL, 
    [ApprovedOn] DATETIME NULL, 
	[TotalAmount] FLOAT NULL, 
    [TotalGrandAmount] FLOAT NULL,    
	[TransactionEntryId] INT NULL, 
    [UpdateBy] INT NULL, 
    [UpdateOn] DATETIME NULL, 
    CONSTRAINT [InvoiceReturn_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvoiceReturn_TransactionEntry] FOREIGN KEY ([TransactionEntryId]) REFERENCES [dbo].[TransactionEntry] ([Id]),
	CONSTRAINT [FK_InvoiceReturn_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id])
)
