CREATE TABLE [dbo].[Designation] (
    [Id]             INT           NOT NULL,
    [DesignationName] VARCHAR (100) NOT NULL,    
    [Description] VARCHAR(50) NULL, 
	[DepartmentId] INT NOT NULL,
    [DesignationSerialNo] INT NOT NULL DEFAULT 1, 
    CONSTRAINT [Position_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [Position_Department_Foreign_Key] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department] ([Id])
);

