CREATE TABLE [dbo].[FiscalYear] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Year]        INT      NOT NULL,
    [OpenDate]    DATETIME NOT NULL,
    [CloseDate]   DATETIME NOT NULL,
    [Active]      BIT      NOT NULL,
    [CreatedById] INT      NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [UpdatedById] INT      NOT NULL,
    [UpdatedDate] DATETIME NOT NULL,
    CONSTRAINT [PK_dbo.FiscalYear] PRIMARY KEY CLUSTERED ([Id] ASC)
);

