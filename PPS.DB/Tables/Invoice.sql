CREATE TABLE [dbo].[Invoice] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [InvoiceNo]     INT           NOT NULL,
    [DemandOrderId] INT           NOT NULL,
    [InvoiceDate]   DATETIME      NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [CreatedOn]     DATETIME      NOT NULL,
    [Note]          VARCHAR (500) NULL,
    [ApprovedBy] INT NULL, 
    [ApprovedOn] DATETIME NULL, 
	[DeliveredBy] INT NULL, 
    [DeliveredOn] DATETIME NULL,
	[TotalAmount] FLOAT NULL, 
	[RegularDiscountInPercentage] FLOAT NULL, 
    [RegularDiscountAmount] FLOAT NULL, 
	[SpecialDiscountInPercentage] FLOAT NULL, 
    [SpecialDiscountAmount] FLOAT NULL, 
	[AdditionalDiscountInPercentage] FLOAT NULL, 
    [AdditionalDiscountAmount] FLOAT NULL, 
	[ExtraDiscountInPercentage] FLOAT NULL, 
    [ExtraDiscountAmount] FLOAT NULL, 
    [CashBackAmount] FLOAT NULL, 
	[TotalDiscountAmount] FLOAT NULL, 
    [TotalGrandAmount] FLOAT NULL, 
    [TotalDueAmount] FLOAT NULL,  
	[TransactionEntryId] INT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    CONSTRAINT [Invoice_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Invoice_DemandOrder_Foreign_Key] FOREIGN KEY ([DemandOrderId]) REFERENCES [dbo].[DemandOrder] ([Id]),
    CONSTRAINT [FK_Invoice_ToTransactionEntry] FOREIGN KEY ([TransactionEntryId]) REFERENCES [dbo].[TransactionEntry] ([Id])
);

