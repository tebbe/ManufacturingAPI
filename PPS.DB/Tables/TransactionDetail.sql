CREATE TABLE [dbo].[TransactionDetail] (
    [Id]                 INT        IDENTITY (1, 1) NOT NULL,
    [AccountHeadId]      INT        NOT NULL,
    [DrAmount]           FLOAT (53) NOT NULL,
    [CrAmount]           FLOAT (53) NOT NULL,
    [TransactionEntryId] INT        NOT NULL,
    [Note] VARCHAR(1000) NULL, 
    CONSTRAINT [PK_dbo.TransactionDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TransactionDetail_dbo.AccountHead_AccountHeadId] FOREIGN KEY ([AccountHeadId]) REFERENCES [dbo].[AccountHead] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TransactionDetail_dbo.TransactionEntry_TransactionEntryId] FOREIGN KEY ([TransactionEntryId]) REFERENCES [dbo].[TransactionEntry] ([Id]) ON DELETE CASCADE
);

