CREATE TABLE [dbo].[DeliveryQuantity]
(
	[Id] INT NOT NULL IDENTITY (1, 1), 
	[InvoiceId] INT NOT NULL, 
	[ChallanDate] DATETIME NOT NULL, 
	[CreatedBy] INT NOT NULL, 
	[CreatedOn] DATETIME NOT NULL, 
	[UpdateBy] INT NULL, 
	[UpdatedOn] DATETIME NULL, 
	[ApprovedBy] INT NULL, 
	[ApprovedOn] DATETIME NULL,
	[Note] VARCHAR(250) NULL, 
    CONSTRAINT [DeliveryQuantity_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [DeliveryQuantity_Invoice_Foreign_Key] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]),
)
