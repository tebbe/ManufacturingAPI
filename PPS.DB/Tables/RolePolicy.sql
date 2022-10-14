CREATE TABLE [dbo].[RolePolicy] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [RoleId]   INT NOT NULL,
    [PolicyId] INT NOT NULL,
    [AssignedBy] INT NULL, 
    [AssignedOn] DATETIME NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePolicy_Policy] FOREIGN KEY ([PolicyId]) REFERENCES [dbo].[Policy] ([Id]),
    CONSTRAINT [FK_RolePolicy_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);

