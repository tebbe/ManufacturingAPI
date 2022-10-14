CREATE TABLE [dbo].[PoliceStation] (
    [Id]                INT          IDENTITY (1, 1) NOT NULL,
    [PoliceStationName] VARCHAR (50) NOT NULL,
    [DistrictId]        INT          NOT NULL,
    CONSTRAINT [PoliceStation_Primary_Key] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [PoliceStation_District_Foreign_Key] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[District] ([Id])
);

