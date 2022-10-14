CREATE TABLE [dbo].[RouteResource] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [ControllerName] VARCHAR (50) NOT NULL,
    [ActionName]     VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_RouteResource] PRIMARY KEY CLUSTERED ([Id] ASC)
);

