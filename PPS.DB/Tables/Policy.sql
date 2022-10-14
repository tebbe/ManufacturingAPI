CREATE TABLE [dbo].[Policy] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [PolicyName]  VARCHAR (150) NOT NULL,
    [Description] VARCHAR (200) NULL,
    [PolicyCode]  INT           NULL,
    [AppTypeId] INT NULL, 
    CONSTRAINT [PK_Policy] PRIMARY KEY CLUSTERED ([Id] ASC)
);

