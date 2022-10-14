CREATE TABLE [dbo].[TransactionStatus] (
    [Id]          INT          NOT NULL,
    [Status]      VARCHAR (50) NOT NULL,
    [CreatedBy]   INT          NOT NULL,
    [CreatedDate] DATETIME     NOT NULL,
    [UpdatedBy]   INT          NOT NULL,
    [UpdatedDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_dbo.TransactionStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

