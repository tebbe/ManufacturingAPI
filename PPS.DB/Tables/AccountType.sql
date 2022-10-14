CREATE TABLE [dbo].[AccountType] (
    [Id]              INT           NOT NULL,
    [AccountTypeName] VARCHAR (100) NOT NULL,
    [AccountNatureId] INT           NOT NULL,
    [CompanyId]       INT           NOT NULL,
    CONSTRAINT [PK_dbo.AccountType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountType_dbo.AccountNature_AccountNatureId] FOREIGN KEY ([AccountNatureId]) REFERENCES [dbo].[AccountNature] ([Id]) ON DELETE CASCADE
);

