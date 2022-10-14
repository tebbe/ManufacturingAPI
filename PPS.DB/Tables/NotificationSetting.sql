CREATE TABLE [dbo].[NotificationSetting]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DocumentRenewalCategoryId] INT NOT NULL, 
    [FirstNotificationDays] INT NOT NULL, 
    [SecondNotificationDays] INT NOT NULL, 
    [FinalNotificationDays] INT NOT NULL, 
    [FirstNotificationContinuity] BIT NOT NULL, 
    [SecondNotificationContinuity] BIT NOT NULL, 
    CONSTRAINT [FK_NotificationSetting_DocumentRenewalCategory] FOREIGN KEY ([DocumentRenewalCategoryId]) REFERENCES [DocumentRenewalCategory]([Id])
)
