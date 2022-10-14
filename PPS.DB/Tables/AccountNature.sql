CREATE TABLE [dbo].[AccountNature] (
    [Id]                INT           NOT NULL,
    [AccountNatureName] VARCHAR (100) NOT NULL,
    [CompanyId]         INT           NOT NULL,
    [CreatedById]       INT           NULL,
    [CreatedDate]       DATETIME      NULL,
    [UpdatedById]       INT           NULL,
    [UpdatedDate]       DATETIME      NULL,
    CONSTRAINT [PK_dbo.AccountNature] PRIMARY KEY CLUSTERED ([Id] ASC)
);

