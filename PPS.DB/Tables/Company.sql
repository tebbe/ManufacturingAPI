CREATE TABLE [dbo].[Company] (
    [Id]             INT            NOT NULL,
    [Name]           NVARCHAR (200) NOT NULL,
	[FullName]       NVARCHAR (200) NULL,
    [ContactPerson]  VARCHAR (50)   NULL,
    [ContactNumber]  VARCHAR (50)   NULL,
    [Address]        VARCHAR (200)  NOT NULL,
    [Phone]          VARCHAR (50)   NULL,
    [Fax]            VARCHAR (50)   NULL,
    [LogoPath]       VARCHAR (500)  NULL,
	[Email]       VARCHAR (200)  NULL,
    [GroupId]        INT            NOT NULL,
    [AllowedInvalid] INT            CONSTRAINT [DF_Company_InvalidTryCount] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Company] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Company_dbo.Group_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]) ON DELETE CASCADE
);

