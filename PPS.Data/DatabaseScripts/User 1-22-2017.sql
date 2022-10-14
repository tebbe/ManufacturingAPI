SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT * FROM sysobjects WHERE name = 'User') 
BEGIN
	CREATE TABLE [dbo].[User](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[Email] [nvarchar](50) NOT NULL,
		[Password] [nvarchar](200) NOT NULL,
		[FirstName] [nvarchar](50) NOT NULL,
		[LastName] [nvarchar](50) NOT NULL,
		[Phone] [nvarchar](50) NOT NULL,
		[Company_Id] [int] NOT NULL,
		[Status_Id] [int] NOT NULL)

	ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_dbo.Company_Company_Id] FOREIGN KEY([Company_Id])
	REFERENCES [dbo].[Company] ([Id])
	
	ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_dbo.User_dbo.Company_Company_Id]
	
	ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_dbo.UserStatus_Status_Id] FOREIGN KEY([Status_Id])
	REFERENCES [dbo].[UserStatus] ([Id])
	
	ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_dbo.User_dbo.UserStatus_Status_Id]
END
