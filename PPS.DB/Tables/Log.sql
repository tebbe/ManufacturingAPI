CREATE TABLE [dbo].[Log] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]       VARCHAR (50)  NULL,
    [CreatedOn]    DATETIME      NOT NULL,
    [ErrorMessage] VARCHAR (200) NOT NULL,
    [InnerMessage] VARCHAR (500) NULL,
    [StackTrace]   VARCHAR (MAX) NULL,
    [AbsolutePath] VARCHAR (100) NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
);

