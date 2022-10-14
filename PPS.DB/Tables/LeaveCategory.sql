CREATE TABLE [dbo].[LeaveCategory]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [Name] VARCHAR(50) NULL, 
    [IsActive] BIT NULL,
	CONSTRAINT [LeaveCategory_Primary_key] PRIMARY KEY CLUSTERED ([Id] ASC)	
)
