﻿CREATE TABLE [dbo].[TransactionEntry] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [TransactionNumber]    VARCHAR (15)  NOT NULL,
    [TransactionDate]      DATETIME      NOT NULL,
    [FiscalYear]           INT           NOT NULL,
    [TransactionTypeId]    INT           NOT NULL,
    [CompanyId]            INT           NOT NULL,
    [PostingDate]          DATETIME      NOT NULL,
    [Active]               BIT            NOT NULL,
    [Deleted]              BIT           NULL,
    [Accepted]             BIT           NOT NULL,
    [AcceptedById]         INT           NULL,
    [AcceptedDate]         DATETIME      NULL,
    [PreviousId]           INT           NULL,
    [UpdateReason]         VARCHAR (250) NULL,
    [CreatedById]          INT            NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [UpdatedById]          INT            NULL,
    [UpdatedDate]          DATETIME       NULL,
    [Particulars]          VARCHAR (250) NULL,
	[VerifiedById]         INT           NULL,
	[VerifiedDate]         DATETIME           NULL,
    [RejectedById]         INT           NULL,
    [RejectedReasonTypeId] INT           NULL,
    [RejectedDate]         DATETIME      NULL,
    [IsSystemGenerated] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.TransactionEntry] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TransactionEntry_dbo.Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TransactionEntry_dbo.TransactionType_TransactionTypeId] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[TransactionType] ([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_TransactionEntry_RejectedReasonType] FOREIGN KEY ([RejectedReasonTypeId]) REFERENCES [RejectedReasonType]([Id])
);

