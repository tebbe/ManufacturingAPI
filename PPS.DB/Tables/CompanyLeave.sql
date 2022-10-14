CREATE TABLE [dbo].[CompanyLeave]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [CompanyId] INT NOT NULL, 
    [LeaveCategoryId] INT NOT NULL, 
    [TotalLeave] INT NOT NULL, 
    [Year] INT NOT NULL, 
    CONSTRAINT [FK_CompanyLeave_LeaveCategory] FOREIGN KEY ([LeaveCategoryId]) REFERENCES [LeaveCategory]([Id]), 

)
