CREATE TABLE [dbo].[UserActivityLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[UserId] INT NULL,
    [UserEmail] VARCHAR(30) NOT NULL, 
    [ControllerName] VARCHAR(50) NOT NULL, 
    [ActionName] VARCHAR(50) NOT NULL, 
    [Elapsed] DECIMAL(18, 4) NOT NULL, 
    [RequestUri] VARCHAR(500) NULL,
    [ReferrerUri] VARCHAR(50) NULL, 
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_UserActivityLog_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
)
