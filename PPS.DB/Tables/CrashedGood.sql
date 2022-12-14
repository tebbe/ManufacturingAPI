CREATE TABLE [dbo].[CrashedGood]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Product Id] INT NOT NULL,
[ProductQuantity]INT NOT NULL,
[InvoiceId] INT NOT NULL,
[CustomerId] INT NOT NULL,
[GroupId] INT NOT NULL,
[CreatedBy]INT NOT NULL,
[CreatedOn]datetime NOT NULL,
[ApprovedBy]INT  NULL,
[ApprovedOn]DATETIME NULL,
[UpdatedBy]INT NULL,
[UpdatedOn]DATETIME NULL,
[DamageStatusId]INT  NULL,
 CONSTRAINT [PK_dbo.CrashedGood] PRIMARY KEY CLUSTERED ([Id] ASC),
 CONSTRAINT [FK_CrashedGood_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([Id]),
 CONSTRAINT [FK_CrashedGood_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
 CONSTRAINT [FK_CrashedGood_Customer] FOREIGN KEY (CustomerId)  REFERENCES [dbo].[Customer] ([Id]),
 CONSTRAINT [FK_CrashedGood_ProductDamageStatusType] FOREIGN KEY ([DamageStatusId]) REFERENCES [dbo].[ProductDamageStatusType] ([Id]),
)
