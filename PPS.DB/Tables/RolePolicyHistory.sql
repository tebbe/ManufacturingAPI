CREATE TABLE [dbo].[RolePolicyHistory] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [RolePolicyId] INT NOT NULL, 
    [RoleId]   INT NOT NULL,
    [PolicyId] INT NOT NULL,
	[AssignedBy] INT NULL, 
    [AssignedOn] DATETIME NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePolicyHistory_Policy] FOREIGN KEY ([PolicyId]) REFERENCES [dbo].[Policy] ([Id]),
    CONSTRAINT [FK_RolePolicyHistory_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);

