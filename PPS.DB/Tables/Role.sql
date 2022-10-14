CREATE TABLE [dbo].[Role] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [RoleName]    VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NULL,
    CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);

