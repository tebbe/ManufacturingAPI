CREATE TABLE [dbo].[SystemWarningType]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SystemWarningTypeName] VARCHAR(30) NOT NULL, 
    [WarningPeriod] INT NOT NULL, 
	[EntityId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[Active] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_SystemWarningType_EntityType_EntityTypeId] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityType] ([Id]) ON DELETE CASCADE
)
