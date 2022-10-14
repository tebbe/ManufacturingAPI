IF NOT EXISTS (SELECT * FROM [District] WHERE DistrictName = N'Dhaka')
BEGIN
	INSERT INTO [dbo].[District](DistrictName) VALUES('Dhaka')
END

DECLARE @districtId INT;
SET @districtId = (SELECT Id FROM [District] WHERE DistrictName = N'Dhaka');
IF NOT EXISTS (SELECT * FROM [dbo].[PoliceStation] WHERE PoliceStationName = N'Demra')
BEGIN
	INSERT INTO [dbo].[PoliceStation](PoliceStationName, DistrictId) VALUES('Demra')
END

DECLARE @policeStationId INT;
SET @policeStationId = (SELECT Id FROM [PostOffice] WHERE PostOfficeName = N'Demra');
IF NOT EXISTS (SELECT * FROM [dbo].[PostOffice] WHERE PostOfficeName = N'Demra')
BEGIN
	INSERT INTO PostOffice (PostOfficeName, PostCode, PoliceStationId) VALUES('Demra','1360',@policeStationId);
END

