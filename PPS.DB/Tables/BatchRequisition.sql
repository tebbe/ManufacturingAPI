CREATE TABLE [dbo].[BatchRequisition] (
    [Id]                INT      IDENTITY (1, 1) NOT NULL,
    [ProductionGroupId] INT NOT NULL,
	[BatchRequisitionNo] INT      NOT NULL,
    [CreatedBy]         INT      NOT NULL,
    [CreatedOn]         DATETIME NOT NULL,
    [DeliveredBy]       INT      NULL,
    [DeliveredOn]       DATETIME NULL,
	[ReceivedBy]       INT      NULL,
    [ReceivedOn]       DATETIME NULL,
	[SendToProductionBy]       INT      NULL,
    [SendToProductionOn]       DATETIME NULL,
    [EstimatedProductionDate] DATETIME NULL, 
    CONSTRAINT [BatchRequisition_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_BatchRequisition_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_BatchRequisition_ToProductionGroup] FOREIGN KEY ([ProductionGroupId]) REFERENCES [ProductionGroup]([Id])
);

