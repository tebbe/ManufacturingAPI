CREATE TABLE [dbo].[PostOffice] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [PostOfficeName]  VARCHAR (50) NOT NULL,
    [PostCode]        VARCHAR (10) NOT NULL,
    [PoliceStationId] INT          NOT NULL,
    CONSTRAINT [PostOffice_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [PostOffice_PoliceStation_Foreign_Key] FOREIGN KEY ([PoliceStationId]) REFERENCES [dbo].[PoliceStation] ([Id])
);

