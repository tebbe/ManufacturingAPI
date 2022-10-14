CREATE TABLE [dbo].[PolicyRouteResource] (
    [PolicyId]        INT NOT NULL,
    [RouteResourceId] INT NOT NULL,
    CONSTRAINT [PolicyRouteResource_Policy_Foreign_Key] FOREIGN KEY ([PolicyId]) REFERENCES [dbo].[Policy] ([Id]),
    CONSTRAINT [PolicyRouteResource_RouteResource_Foreign_Key] FOREIGN KEY ([RouteResourceId]) REFERENCES [dbo].[RouteResource] ([Id])
);

