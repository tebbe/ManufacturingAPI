CREATE TABLE [dbo].[AccountPrimaryHead] (
    [Id]                     INT           NOT NULL,
    [AccountPrimaryHeadName] VARCHAR (100) NOT NULL,
    [AccountTypeId]          INT           NOT NULL,
    [CompanyId]              INT           NOT NULL,
    [CreatedById]            INT           NULL,
    [CreatedDate]            DATETIME      NULL,
    [UpdatedById]            INT           NULL,
    [UpdatedDate]            DATETIME      NULL,
    CONSTRAINT [PK_dbo.AccountPrimaryHead] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountPrimaryHead_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_dbo.AccountPrimaryHead_dbo.AccountType_AccountTypeId] FOREIGN KEY ([AccountTypeId]) REFERENCES [dbo].[AccountType] ([Id]) ON DELETE CASCADE
);

