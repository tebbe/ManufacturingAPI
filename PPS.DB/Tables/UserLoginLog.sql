CREATE TABLE [dbo].[UserLoginLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserEmail] VARCHAR(50) NOT NULL, 
    [LoggedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [IsSucceeded] BIT NOT NULL, 
    [ErrorMessage] VARCHAR(50) NULL
)
