CREATE TABLE [dbo].[LegalDocumentHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[LegalDocumentId] INT NOT NULL,
	[CompanyId] INT NOT NULL, 
    [DocumentTypeId] INT NOT NULL, 
    [IssueDate] DATETIME NOT NULL,
	[DocumentNumber] varchar(20) NULL, 
    [ExpireDate] DATETIME NULL, 
    [DocumentRenewCategoryId] INT NOT NULL, 
    [DocumentStatusId] INT NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [OrganizationName] NVARCHAR(150) NOT NULL, 
    [OrganizationAddress] NVARCHAR(500)NOT  NULL, 
    [OrganizationContactEmail] NVARCHAR(50)  NULL, 
    [OrganizationContactName] NVARCHAR(30)  NULL, 
    [OrganizationPhoneNumber] NVARCHAR(20)  NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL,
	CONSTRAINT [FK_LegalDocumentHistory_Company] FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id]), 
    CONSTRAINT [FK_LegalDocumentHistory_DocumentType] FOREIGN KEY ([DocumentTypeId]) REFERENCES [DocumentType]([Id]),
	CONSTRAINT [FK_LegalDocumentHistory_DocumentRenewalCategory] FOREIGN KEY ([DocumentRenewCategoryId]) REFERENCES [DocumentRenewalCategory]([Id]),
	CONSTRAINT [FK_LegalDocumentHistory_DocumentStatus] FOREIGN KEY ([DocumentStatusId]) REFERENCES [DocumentStatus]([Id]),
	CONSTRAINT [FK_LegalDocumentHistory_User_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_LegalDocumentHistory_User_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [User]([Id])
)
