CREATE TABLE [dbo].[Department] (
    [Id]             INT           NOT NULL,
    [DepartmentName] VARCHAR (100) NOT NULL,
	[Description] VARCHAR(50) NULL, 
    [CompanyId] INT NULL, 
    CONSTRAINT [Department_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [Department_Company_Foreign_Key] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);

