CREATE TABLE [dbo].[BatchRequisitionDetail] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [BatchRequisitionId]     INT        NOT NULL,
	[RawMaterialTypeId] INT        NOT NULL,
    [Quantity]      FLOAT        NOT NULL,
    CONSTRAINT [BatchRequisitionDetail_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [BatchRequisitionDetail_BatchRequisition_Foreign_Key] FOREIGN KEY ([BatchRequisitionId]) REFERENCES [dbo].[BatchRequisition] ([Id]),
    CONSTRAINT [BatchRequisitionDetail_RawMaterialType_Foreign_Key] FOREIGN KEY ([RawMaterialTypeId]) REFERENCES [dbo].[RawMaterialType] ([Id])
);

