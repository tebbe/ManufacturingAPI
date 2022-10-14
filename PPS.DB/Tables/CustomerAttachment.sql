CREATE TABLE [dbo].[CustomerAttachment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[FileGuid] UNIQUEIDENTIFIER NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [AttachmentTypeId] INT NOT NULL, 
    [Description] VARCHAR(300) NULL, 
    [FileTypeId] INT NULL,
    [FileName] VARCHAR(50) NULL, 
    [FileSize] INT NULL,     
    CONSTRAINT [FK_CustomerAttachment_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT [FK_CustomerAttachment_AttachmentType] FOREIGN KEY ([AttachmentTypeId]) REFERENCES [AttachmentType]([Id]), 
    CONSTRAINT [FK_CustomerAttachment_FileType] FOREIGN KEY ([FileTypeId]) REFERENCES [FileType]([Id]), 
)
