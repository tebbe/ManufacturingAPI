CREATE TABLE [dbo].[District] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [DistrictName] VARCHAR (50) NOT NULL,
    CONSTRAINT [District_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC)
);

