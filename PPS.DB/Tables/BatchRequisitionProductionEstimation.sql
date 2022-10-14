CREATE TABLE [dbo].[BatchRequisitionProductionEstimation] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [BatchRequisitionId]     INT        NOT NULL,
	[ProductId] INT        NOT NULL,
    [Quantity]      INT        NOT NULL,
    CONSTRAINT [BatchRequisitionProductionEstimation_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_BatchRequisitionProductionEstimation_BatchRequisition] FOREIGN KEY ([BatchRequisitionId]) REFERENCES [BatchRequisition]([Id]), 
    CONSTRAINT [FK_BatchRequisitionProductionEstimation_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
);

