CREATE TABLE [dbo].[MonthlyProcessing]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ProcessingTypeId] INT NOT NULL, 
    [Month] INT NOT NULL, 
    [Year] INT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [ReprocessedBy] INT NULL, 
    [ReprocessedOn] DATETIME NULL, 
    CONSTRAINT [FK_MonthlyProcessing_ProcessingType] FOREIGN KEY ([ProcessingTypeId]) REFERENCES [ProcessingType]([Id]), 
    CONSTRAINT [FK_MonthlyProcessing_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_MonthlyProcessing_User1] FOREIGN KEY ([ReprocessedBy]) REFERENCES [User]([Id])
)
