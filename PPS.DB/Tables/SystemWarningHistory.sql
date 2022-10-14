CREATE TABLE [dbo].[SystemWarningHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SystemWarningId] INT NOT NULL,
    [SystemWarningTypeId] INT NOT NULL, 
	[EntityId] INT NOT NULL,
    [Message] NVARCHAR(200) NOT NULL, 
    [WarningDays] INT NOT NULL,
	[Active] BIT NOT NULL, 
	[CreatedOn] DATE NOT NULL, 
    [CreatedBy] INT NULL, 
    [UpdatedOn] DATE NULL, 
    [UpdatedBy] INT NULL, 
    CONSTRAINT [FK_SystemWarningHistory_SystemWarning_SystemWarningId] FOREIGN KEY ([SystemWarningId]) REFERENCES [dbo].[SystemWarning] ([Id]),
    CONSTRAINT [FK_SystemWarningHistory_SystemWarningType_SystemWarningTypeId] FOREIGN KEY ([SystemWarningTypeId]) REFERENCES [dbo].[SystemWarningType] ([Id])
)
