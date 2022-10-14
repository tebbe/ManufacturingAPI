CREATE TABLE [dbo].[RejectedReasonType] (
    [Id]                     INT           NOT NULL,
    [RejectedReasonTypeName] VARCHAR (300) NOT NULL,
    CONSTRAINT [RejectedReasonType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

