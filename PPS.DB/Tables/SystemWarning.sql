CREATE TABLE [dbo].[SystemWarning]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SystemWarningTypeId] INT NOT NULL, 
	[EntityId] INT NOT NULL,
    [Message] NVARCHAR(200) NOT NULL, 
    [WarningDays] INT NOT NULL,
	[Active] BIT NOT NULL, 
	[CreatedOn] DATE NOT NULL, 
    [CreatedBy] INT NULL, 
    [UpdatedOn] DATE NULL, 
    [UpdatedBy] INT NULL,     
    CONSTRAINT [FK_SystemWarning_SystemWarningType_SystemWarningTypeId] FOREIGN KEY ([SystemWarningTypeId]) REFERENCES [dbo].[SystemWarningType] ([Id]) ON DELETE CASCADE
)
