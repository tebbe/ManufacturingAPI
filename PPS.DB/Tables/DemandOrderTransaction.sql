CREATE TABLE [dbo].[DemandOrderTransaction] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [DemandOrderId] INT        NOT NULL,
	[TransactionAmount] FLOAT NOT NULL, 
    [TransactionDate] DATETIME NOT NULL,
    [CreatedBy]      INT NOT NULL,
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    CONSTRAINT [DemandOrderTransaction_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [DemandOrderTransaction_DemandOrder_Foreign_Key] FOREIGN KEY ([DemandOrderId]) REFERENCES [dbo].[DemandOrder] ([Id])
);