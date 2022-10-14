-- Variable Declaration 
DECLARE @roleId INT
DECLARE @policyId INT
DECLARE @routeResourceId INT
-- END - Variable Declaration 
-------------------------------------------------------------------------------
-- Role
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'General')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('General')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Director')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Director')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Managing Director')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Managing Director')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Director General Manager')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Director General Manager')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Accountant')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Accountant')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Accounts Manager')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Accounts Manager')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'IT Officer')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('IT Officer')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'IT Manager')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('IT Manager')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Sales Manager')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Sales Manager')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Sales Officer')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Sales Officer')
END
IF NOT EXISTS (SELECT * FROM [Role] WHERE RoleName = N'Sales Audit')
BEGIN
	INSERT INTO [dbo].[Role] 
	VALUES('Sales Audit')
END
-- END - Role
-------------------------------------------------------------------------------
-- Policy
IF NOT EXISTS (SELECT * FROM [Policy] WHERE PolicyName = N'Access Login')
BEGIN
	INSERT INTO [dbo].[Policy]([PolicyName], [Description])
	VALUES('Access Login', 'Access to the applciation');
END
IF NOT EXISTS (SELECT * FROM [Policy] WHERE PolicyName = N'Access Profie Basic')
BEGIN
	INSERT INTO [dbo].[Policy]([PolicyName], [Description])
	VALUES('Access Profie Basic', 'Access to the profile(Basic)');
END
IF NOT EXISTS (SELECT * FROM [Policy] WHERE PolicyName = N'Access Profie Advanced')
BEGIN
	INSERT INTO [dbo].[Policy]([PolicyName], [Description])
	VALUES('Access Profie Advanced', 'Access to the profile(Advanced)');
END
IF NOT EXISTS (SELECT * FROM [Policy] WHERE PolicyName = N'Access Deshboard')
BEGIN
	INSERT INTO [dbo].[Policy]([PolicyName], [Description])
	VALUES('Access Deshboard', 'Access to the dashboard');
END
-- END - Policy
-------------------------------------------------------------------------------
-- Role Policy
SET @roleId = (SELECT TOP(1) Id FROM [Role] WHERE RoleName = N'General');

SET @policyId = (SELECT TOP(1) Id FROM [Role] WHERE PolicyName = N'Access Login');
IF NOT EXISTS (SELECT * FROM [RolePolicy] WHERE RoleId = @roleId AND PolicyId = @policyId)
BEGIN
	INSERT INTO [dbo].[RolePolicy](RoleId, PolicyId) VALUES(@roleId, @policyId);
END

SET @policyId = (SELECT TOP(1) Id FROM [Role] WHERE PolicyName = N'Access Profie Basic');
IF NOT EXISTS (SELECT * FROM [RolePolicy] WHERE RoleId = @roleId AND PolicyId = @policyId)
BEGIN
	INSERT INTO [dbo].[RolePolicy](RoleId, PolicyId) VALUES(@roleId, @policyId);
END

SET @policyId = (SELECT TOP(1) Id FROM [Role] WHERE PolicyName = N'Access Deshboard');
IF NOT EXISTS (SELECT * FROM [RolePolicy] WHERE RoleId = @roleId AND PolicyId = @policyId)
BEGIN
	INSERT INTO [dbo].[RolePolicy](RoleId, PolicyId) VALUES(@roleId, @policyId);
END
-- END - Role Policy
-------------------------------------------------------------------------------
-- RouteResource

-- END - RouteResource
-------------------------------------------------------------------------------
-- RouteResourcePolicy

-- END - RouteResourcePolicy
-------------------------------------------------------------------------------

-- TransactionRejectReasonType
DECLARE @reasonText varchar(200);

SET @reasonText = 'Amount mismatch';
IF NOT EXISTS (SELECT * FROM [TransactionRejectReasonType] WHERE ReasonText = @reasonText)
BEGIN
	INSERT INTO [dbo].[TransactionRejectReasonType] VALUES(@reasonText);
END
SET @reasonText = 'Without check sign';
IF NOT EXISTS (SELECT * FROM [TransactionRejectReasonType] WHERE ReasonText = @reasonText)
BEGIN
	INSERT INTO [dbo].[TransactionRejectReasonType] VALUES(@reasonText);
END
-- END - TransactionRejectReasonType