CREATE TABLE [dbo].[BankInfo] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [AccountNumber] VARCHAR (15)  NOT NULL,
    [BankName]      VARCHAR (100) NOT NULL,
    [BranchName]    VARCHAR (50)  NOT NULL,
    [Address]       VARCHAR (200) NOT NULL,
    [Active]        BIT           NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [UpdatedBy]     INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [CompanyId]    INT            NOT NULL,
    CONSTRAINT [PK_dbo.BankInfo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.BankInfo_dbo.Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);

