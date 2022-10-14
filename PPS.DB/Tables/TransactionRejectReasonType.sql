CREATE TABLE [dbo].[TransactionRejectReasonType] (
    [Id]         INT           NOT NULL,
    [ReasonText] VARCHAR (150) NOT NULL,
    CONSTRAINT [TransactionRejectReasonType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

