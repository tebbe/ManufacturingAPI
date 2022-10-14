CREATE TABLE [dbo].[CustomerTransactionMonthly]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [CustomerId] INT NOT NULL, 
    [TransactionMonth] DATE NOT NULL, 
    [TotalDoAmount] DECIMAL(10) NOT NULL, 
    [TotalPaidAmount] DECIMAL(10) NOT NULL, 
	[CarryForwardedBalanceAmount] DECIMAL(10) NOT NULL,
    [BalanceAmount] DECIMAL(10) NOT NULL 
)
