CREATE TABLE [dbo].[AccountSubHead] (
    [Id]                   INT           NOT NULL,
    [AccountSubHeadName]   VARCHAR (100) NOT NULL,
    [AccountPrimaryHeadId] INT           NOT NULL,
    [CompanyId]            INT           NOT NULL,
    [CreatedById]          INT           NULL,
    [CreatedDate]          DATETIME      NULL,
    [UpdatedById]          INT           NULL,
    [UpdatedDate]          DATETIME      NULL,
    CONSTRAINT [PK_dbo.AccountSubHead] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountSubHead_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_dbo.AccountSubHead_dbo.AccountPrimaryHead_AccountPrimaryHeadId] FOREIGN KEY ([AccountPrimaryHeadId]) REFERENCES [dbo].[AccountPrimaryHead] ([Id]) ON DELETE CASCADE
);

