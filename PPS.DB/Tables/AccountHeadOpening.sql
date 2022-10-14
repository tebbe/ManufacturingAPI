CREATE TABLE [dbo].[AccountHeadOpening] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [AccountHeadId] INT        NOT NULL,
    [DrAmount]      FLOAT (53) NOT NULL,
    [CrAmount]      FLOAT (53) NOT NULL,
    [FiscalYear]    INT        NOT NULL,
    [CompanyId]     INT        NOT NULL,
    [CreatedById]   INT        NOT NULL,
    [CreatedDate]   DATETIME   NOT NULL,
    [UpdatedById]   INT        NULL,
    [UpdatedDate]   DATETIME   NULL,
    CONSTRAINT [PK_dbo.AccountHeadOpening] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountHeadOpening_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_dbo.AccountHeadOpening_dbo.AccountHead_AccountHeadId] FOREIGN KEY ([AccountHeadId]) REFERENCES [dbo].[AccountHead] ([Id]) ON DELETE CASCADE
);

