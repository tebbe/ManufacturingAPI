CREATE TABLE [dbo].[AttachmentType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [AttachmentTypeName] VARCHAR(50) NULL, 
    [Strength] INT NULL
)
