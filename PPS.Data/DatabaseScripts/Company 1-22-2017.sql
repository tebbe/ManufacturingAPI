SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT * FROM sysobjects WHERE name = 'Company') 
BEGIN
	CREATE TABLE [dbo].[Company](
		[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
		[Name] [nvarchar](200) NOT NULL,
		[ContactPerson] [nvarchar](50) NULL,
		[ContactNumber] [nvarchar](50) NULL,
		[Address] [nvarchar](200) NOT NULL,
		[Phone] [nvarchar](50) NULL,
		[Fax] [nvarchar](50) NULL,
		[LogoPath] [nvarchar](500) NULL,
		[Group_Id] [int] NOT NULL)

	ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Company_dbo.Group_Group_Id] FOREIGN KEY([Group_Id])
	REFERENCES [dbo].[Group] ([Id])
	
	ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_dbo.Company_dbo.Group_Group_Id]
END

