CREATE TABLE [dbo].[ProductionGroup]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [ProductionGroupId] VARCHAR(20) NOT NULL, 
	[IsClosed] BIT NOT NULL DEFAULT 0, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    CONSTRAINT [FK_ProductionGroup_ToUser] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id])
)
