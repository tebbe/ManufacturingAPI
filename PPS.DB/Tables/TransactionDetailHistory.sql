CREATE TABLE [dbo].[TransactionDetailHistory] (
    [Id]                 INT        IDENTITY (1, 1) NOT NULL,
    [TransactionDetailId]INT        NOT NULL,
    [AccountHeadId]      INT        NOT NULL,
    [DrAmount]           FLOAT (53) NOT NULL,
    [CrAmount]           FLOAT (53) NOT NULL,
    [TransactionEntryId] INT        NOT NULL,
    [Note] VARCHAR(1000) NULL, 
    CONSTRAINT [PK_dbo.TransactionDetailHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TransactionDetailHistory_dbo.AccountHead_AccountHeadId] FOREIGN KEY ([AccountHeadId]) REFERENCES [dbo].[AccountHead] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TransactionDetailHistory_dbo.TransactionEntryHistory_TransactionEntryHistoryId] FOREIGN KEY ([TransactionEntryId]) REFERENCES [dbo].[TransactionEntryHistory] ([Id]) ON DELETE CASCADE
);

