CREATE TABLE [dbo].[Supplier] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [SupplierName]       VARCHAR (50)  NOT NULL,
    [SupplierCode] INT NOT NULL, 
	[Address]            VARCHAR (300) NOT NULL,
    [ContactPerson]      VARCHAR (150) NOT NULL,
    [Phone]              VARCHAR (16)  NOT NULL,
    [ContactPersonPhone] VARCHAR (16)  NOT NULL,
    [Email]              VARCHAR (50)  NULL,
    [ContactPersonEmail] VARCHAR (50)  NULL,
    [AccountHeadId] INT NOT NULL, 
	[IsActive] BIT NOT NULL,
    CONSTRAINT [Supplier_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Supplier_AccountHead] FOREIGN KEY ([AccountHeadId]) REFERENCES [AccountHead]([Id])
);

