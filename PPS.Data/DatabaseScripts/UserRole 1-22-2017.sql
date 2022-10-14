SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT * FROM sysobjects WHERE name = 'UserRole') 
BEGIN
	CREATE TABLE [dbo].[UserRole](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[User_Id] [int] NOT NULL,
		[Role_Id] [int] NOT NULL,
		[StartDate] [datetime] NOT NULL,
		[EndDate] [datetime] NOT NULL,
		[AssignedBy_Id] [int] NOT NULL)

	ALTER TABLE [dbo].[UserRole] ADD  DEFAULT ((0)) FOR [AssignedBy_Id]

	ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRole_dbo.Role_Role_Id] FOREIGN KEY([Role_Id])
	REFERENCES [dbo].[Role] ([Id])
	
	ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_dbo.UserRole_dbo.Role_Role_Id]

	ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRole_dbo.User_User_Id] FOREIGN KEY([User_Id])
	REFERENCES [dbo].[User] ([Id])
	
	ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_dbo.UserRole_dbo.User_User_Id]
END
