CREATE TABLE [dbo].[Area] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [AreaName] VARCHAR (50) NOT NULL,
    CONSTRAINT [Area_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

