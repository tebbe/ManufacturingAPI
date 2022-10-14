CREATE TABLE [dbo].[FinishedGood] (
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
	[ProductionGroupId] INT NOT NULL, 
    [ProductId]          INT NOT NULL,
    [Quantity]           INT NOT NULL,
	[ProductionDate]	DATETIME NOT NULL,
	[CreatedBy]			INT NOT NULL,
	[CreatedOn]			DATETIME NOT NULL,
	[ApprovedBy]		INT,
	[ApprovedOn]		DATETIME,
	[IsApproved]	BIT	NOT NULL DEFAULT 0,
    CONSTRAINT [FinishedGood_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FinishedGood_Product_Foreign_Key] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]), 
    CONSTRAINT [FK_FinishedGood_User_Created] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_FinishedGood_User_Approved] FOREIGN KEY ([ApprovedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_FinishedGood_ToProductionGroup] FOREIGN KEY ([ProductionGroupId]) REFERENCES [ProductionGroup]([Id])
);

