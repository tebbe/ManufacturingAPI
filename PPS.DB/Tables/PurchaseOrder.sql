CREATE TABLE [dbo].[PurchaseOrder] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [PurchaseOrderNo]       INT           NOT NULL,
    [PurchaseOrderDate]     DATETIME      NOT NULL,
    [IsCashPurchase] BIT NULL, 
	[IsCreditPurchase] BIT NULL,
    [IsLcPurchase] BIT NULL, 
    [Note]                  VARCHAR (500) NULL,
    [PaymentType]           VARCHAR (20)  NULL,
    [EstimatedDeliveryDate] DATETIME      NULL,
    [PriceValidity]         INT           NULL,
	[TotalAmount]			FLOAT		  NOT NULL,
	[VerifiedBy]            INT           NULL,
    [VerifiedOn]            DATETIME      NULL,
    [ApprovedBy]            INT           NULL,
    [ApprovedOn]            DATETIME      NULL,
    [CreatedBy]             INT           NOT NULL,
    [CreatedOn]             DATETIME      NOT NULL,
	[UpatedBy]				INT           NULL,
    [UpatedOn]				DATETIME      NULL,
    [IsCurrentRecord]       BIT           NOT NULL,
    [PreviousId]            INT           NULL,
    [Locked]                BIT           NOT NULL,
    [RejectedReasonTypeId]  INT           NULL,
    [RejectedBy]            INT           NULL,
    [RejectedOn]            DATETIME      NULL,
    [PurchaseOrderStatusId] INT NULL, 
    [CashAccountHeadId] INT NULL, 
	[CashAmount] FLOAT NULL, 
	[BankAccountHeadId] INT NULL,
    [BankAmount] FLOAT NULL, 
	[SupplierId]            INT           NULL,
	[SupplierAccountHeadId]            INT           NULL,
    [SupplierAmount] FLOAT NULL, 
	[LCNo] VARCHAR(20) NULL, 
	[LCAccountHeadId]            INT           NULL,
    [LCAmount] FLOAT NULL, 
	[TransactionEntryId] INT NULL, 
    [IsVerified] BIT NULL, 
    [IsApproved] BIT NULL, 
    [IsPaymentComplete] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PurchaseOrder_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchaseOrder_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [PurchaseOrder_RejectedReasonType_Foreign_Key] FOREIGN KEY ([RejectedReasonTypeId]) REFERENCES [dbo].[RejectedReasonType] ([Id]),
    CONSTRAINT [PurchaseOrder_Supplier_Foreign_Key] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([Id]), 
    CONSTRAINT [FK_PurchaseOrder_PurchaseOrderStatus] FOREIGN KEY ([PurchaseOrderStatusId]) REFERENCES [PurchaseOrderStatus]([Id]),
	CONSTRAINT [FK_dbo.PurchaseOrder_AccountHead_Cash] FOREIGN KEY ([CashAccountHeadId]) REFERENCES [dbo].[AccountHead] ([Id]),
	CONSTRAINT [FK_dbo.PurchaseOrder_AccountHead_Bank] FOREIGN KEY ([BankAccountHeadId]) REFERENCES [dbo].[AccountHead] ([Id])
);

