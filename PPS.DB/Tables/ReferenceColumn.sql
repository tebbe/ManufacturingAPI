CREATE TABLE [dbo].[ReferenceColumn]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [CustomerAccountSubHeadId] INT NULL, 
    [BankAccountSubHeadId] INT NULL, 
    [CashAccountSubHeadId] INT NULL, 
    [SalesAccountHeadId] INT NULL,
	[SupplierAccountSubHeadId] INT NULL, 
    CONSTRAINT [PK_ReferenceColumn] PRIMARY KEY ([Id]),
)
