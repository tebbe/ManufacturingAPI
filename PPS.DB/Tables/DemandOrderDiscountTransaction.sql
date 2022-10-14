CREATE TABLE [dbo].[DemandOrderDiscountTransaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [DemandOrderId] INT NOT NULL, 
    [DemandOrderDiscountTypeId] INT NOT NULL,
	[TransactionAmount] FLOAT NOT NULL, 
    [TransactionDate] DATETIME NOT NULL,
    [CreatedBy]      INT NOT NULL,
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL,
	[VerifiedBy] INT NULL, 
    [VerifiedOn] DATETIME NULL, 
	[IsVerified] BIT NULL DEFAULT 0,
	[ApprovedBy] INT NULL, 
    [ApprovedOn] DATETIME NULL, 
    [IsApproved] BIT NOT NULL DEFAULT 0, 
	[TransactionEntryId] INT NULL,
	[DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [IsDeleted] BIT NULL DEFAULT 0, 
    CONSTRAINT [DemandOrderDiscountTransaction_DemandOrder_Foreign_Key] FOREIGN KEY ([DemandOrderId]) REFERENCES [dbo].[DemandOrder] ([Id]),
	CONSTRAINT [DemandOrderDiscountTransaction_DemandOrderDiscountType_Foreign_Key] FOREIGN KEY ([DemandOrderDiscountTypeId]) REFERENCES [dbo].[DemandOrderDiscountType] ([Id])
)
