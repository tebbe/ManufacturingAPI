CREATE TABLE [dbo].[AccountHead] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [AccountHeadCode]  VARCHAR (10)  NULL,
    [AccountHeadName]  VARCHAR (100) NULL,
    [AccountSubHeadId] INT           NOT NULL,
    [Active]           BIT           NOT NULL,
    [LedgerType]       VARCHAR (2)   NULL,
    [CompanyId]        INT           NOT NULL,
    [CreatedById]      INT           NOT NULL,
    [CreatedDate]      DATETIME      NOT NULL,
    [UpdatedById]      INT           NULL,
    [UpdatedDate]      DATETIME      NULL,
    CONSTRAINT [PK_dbo.AccountHead] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountHead_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_dbo.AccountHead_dbo.AccountSubHead_AccountSubHeadId] FOREIGN KEY ([AccountSubHeadId]) REFERENCES [dbo].[AccountSubHead] ([Id]) ON DELETE CASCADE
);

