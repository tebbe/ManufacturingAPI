CREATE TABLE [dbo].[EmployeeType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [TypeName] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(50) NULL,
)
