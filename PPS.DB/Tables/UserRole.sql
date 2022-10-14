CREATE TABLE [dbo].[UserRole] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [UserId]       INT      NOT NULL,
    [RoleId]       INT      NOT NULL,
    [AssignedBy] INT      NULL,
    [AssignedOn] DATETIME NULL, 
    CONSTRAINT [PK_dbo.UserRole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserRole_dbo.Role_Role_Id] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserRole_dbo.User_User_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

