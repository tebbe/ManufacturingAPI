CREATE TABLE [dbo].[CompanySalesTarget]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SalesTarget] DECIMAL NOT NULL, 
    [SalesYear] INT NOT NULL, 
	[SalesMonth] INT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [ApprovedBy] INT NULL, 
    [ApprovedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [IsApproved] BIT NOT NULL DEFAULT 0 
)
