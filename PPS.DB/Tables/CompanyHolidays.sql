CREATE TABLE [dbo].[CompanyHolidays] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,    
    [CompanyId]      INT            NOT NULL,
	[HolidayName]    NVARCHAR (200) NOT NULL,
    [HolidayDate]    DATETIME		NOT NULL, 
    CONSTRAINT [CompanyHolidays_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [CompanyHolidays_Company_Foreign_Key] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);

