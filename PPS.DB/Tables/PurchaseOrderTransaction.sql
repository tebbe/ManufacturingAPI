CREATE TABLE [dbo].[PurchaseOrderTransaction] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
	[PurchaseOrderId]	INT NOT NULL,
    [SupplierId]	INT			  NOT NULL,
    [CashBankAccountHeadId]	INT			  NOT NULL,
	[TransactionAmount] FLOAT     NOT NULL, 
    [TransactionDate] DATETIME    NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [CreatedOn]   DATETIME      NOT NULL,
    [UpdatedBy]     INT           NULL,
    [UpdatedOn]   DATETIME      NULL,
    [ApprovedBy] INT NULL, 
    [ApprovedOn] DATETIME NULL, 
    [IsApproved] BIT NOT NULL, 
    CONSTRAINT [PK_dbo.PurchaseOrderTransaction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PurchaseOrderTransaction_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrder] ([Id]),
	CONSTRAINT [FK_dbo.PurchaseOrderTransaction_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([Id]),
    CONSTRAINT [FK_dbo.PurchaseOrderTransaction_AccountHead] FOREIGN KEY ([CashBankAccountHeadId]) REFERENCES [dbo].[AccountHead] ([Id])
);

