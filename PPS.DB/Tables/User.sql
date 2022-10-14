CREATE TABLE [dbo].[User] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Email]        VARCHAR (50)   NOT NULL,
    [Password]     VARCHAR (MAX)  NULL,
    [FirstName]    VARCHAR (50)   NOT NULL,
    [LastName]     VARCHAR (50)   NOT NULL,
    [Phone]        VARCHAR (50)   NOT NULL,
    [CompanyId]    INT            NOT NULL,
    [StatusId]     INT            NOT NULL,
    [Locked]       BIT            CONSTRAINT [DF_User_Locked] DEFAULT ((0)) NOT NULL,
    [InvalidCount] INT            CONSTRAINT [DF_User_TriedCount] DEFAULT ((0)) NOT NULL,
    [PasswordKey]  VARCHAR (MAX)  NULL,
    [EmployeeId]   INT            NULL,
    [AspNetUserId] NVARCHAR (128) NULL,
    CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.User_dbo.Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_User_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_User_UserStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[UserStatus] ([Id])
);

