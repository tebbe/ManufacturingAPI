CREATE TABLE [dbo].[Group] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [GroupName] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_dbo.Group] PRIMARY KEY CLUSTERED ([Id] ASC)
);

