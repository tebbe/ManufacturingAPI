CREATE TABLE [dbo].[SaleType] (
    [Id]           INT           NOT NULL,
    [SaleTypeName] VARCHAR (300) NOT NULL,
    [DurationInDays]     INT           NOT NULL,
    [WarningInDays] INT NULL, 
    [EarlyPaymentInDays] INT NULL, 
    [EarlyPaymentDiscountInPercentage] FLOAT NULL, 
    CONSTRAINT [SaleType_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

