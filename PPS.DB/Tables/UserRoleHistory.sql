CREATE TABLE [dbo].[UserRoleHistory] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [UserRoleId] INT NOT NULL, 
    [UserId]       INT      NOT NULL,
    [RoleId]       INT      NOT NULL,
	[AssignedBy] INT      NULL,
    [AssignedOn] DATETIME NULL, 
    CONSTRAINT [PK_dbo.UserRoleHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserRoleHistory_dbo.Role_Role_Id] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserRoleHistory_dbo.User_User_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

