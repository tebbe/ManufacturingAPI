﻿--INSERT [dbo].[AccountNature] ([Id], [AccountNatureName], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (1, N'Assets', 1, 1, CAST(N'2017-01-01T00:00:00.000' AS DateTime), 1, CAST(N'2017-01-01T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountNature] ([Id], [AccountNatureName], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (2, N'Liabilities', 1, 1, CAST(N'2017-03-11T00:00:00.000' AS DateTime), 1, CAST(N'2017-03-11T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountNature] ([Id], [AccountNatureName], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (3, N'Revenue', 1, 1, CAST(N'2017-03-11T00:00:00.000' AS DateTime), 1, CAST(N'2017-03-11T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountNature] ([Id], [AccountNatureName], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (4, N'Expenses', 1, 1, CAST(N'2017-03-11T00:00:00.000' AS DateTime), 1, CAST(N'2017-03-11T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountNature] ([Id], [AccountNatureName], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (5, N'OwnersEquity', 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))

--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (1, N'Current Assets', 1, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (2, N'Fixed Assets', 1, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (3, N'Current Liabilities', 2, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (4, N'Long Term Liabilities', 2, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (5, N'Direct Income', 3, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (6, N'Indirect Income', 3, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (7, N'Direct Expense', 4, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (8, N'Indirect Expense', 4, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (9, N'Capital', 5, 1)
--INSERT [dbo].[AccountType] ([Id], [AccountTypeName], [AccountNatureId], [CompanyId]) VALUES (10, N'Drawings', 5, 1)


--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (1, N'Cash', 1, 1, 1, CAST(N'2017-01-01T00:00:00.000' AS DateTime), 1, CAST(N'2017-01-01T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (2, N'ACCOUNT RECEIVABLE
--', 1, 1, 1, CAST(N'2017-01-01T00:00:00.000' AS DateTime), 1, CAST(N'2017-01-01T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (4, N'Inventory
--', 1, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (5, N' Prepaid Insurance
--', 1, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (8, N'EQUIPMENT
--', 2, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (9, N'CURRENT LIABILITIES
--', 3, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (18, N'LONG-TERM LIABILITIES
--', 4, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (19, N'REVENUE
--', 5, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (20, N'INDIRECT INCOME
--', 6, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (21, N'DIRECT EXPENSES
--', 7, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (22, N'INDIRECT EXPENSES
--', 8, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (23, N'CAPITAL
--', 9, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountPrimaryHead] ([Id], [AccountPrimaryHeadName], [AccountTypeId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (24, N'DRAWINGS
--', 10, 1, 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-02T00:00:00.000' AS DateTime))

--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (75, N'CASH AT BANK', 1, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (76, N'CASH IN HAND', 1, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (77, N'ACCOUNT RECEIVABLE', 2, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (78, N'Inventory', 4, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (79, N'Prepaid Insurance', 5, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (80, N'EQUIPMENT', 8, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (81, N'Account Payable', 9, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (82, N'CURRENT LIABILITIES', 9, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (83, N'LONG-TERM LIABILITIES', 18, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (84, N'SALES REVENUE', 19, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (85, N'INDIRECT INCOME', 20, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (86, N'Purchase', 21, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (87, N'INDIRECT EXPENSES', 22, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (88, N'CAPITAL', 23, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))
--INSERT [dbo].[AccountSubHead] ([Id], [AccountSubHeadName], [AccountPrimaryHeadId], [CompanyId], [CreatedById], [CreatedDate], [UpdatedById], [UpdatedDate]) VALUES (89, N'DRAWINGS', 24, 1, 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2017-05-20T00:00:00.000' AS DateTime))

