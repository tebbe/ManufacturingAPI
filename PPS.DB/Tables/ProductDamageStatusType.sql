CREATE TABLE [dbo].[ProductDamageStatusType]
(
	[Id] INT NOT NULL  IDENTITY(1,1), 
    [Name] VARCHAR(50) NOT NULL,
	CONSTRAINT [PK_dbo.ProductDamageStatusType] PRIMARY KEY CLUSTERED ([Id] ASC), 
)
