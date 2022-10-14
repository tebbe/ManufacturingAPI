
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/14/2022 22:46:55
-- Generated from EDMX file: D:\FZProject\ManufacturingAPI\PPS.Data\Edmx\PPS.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [APIDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BatchProduct_BatchRequisition_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchProduct] DROP CONSTRAINT [FK_BatchProduct_BatchRequisition_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchProduct_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchProduct] DROP CONSTRAINT [FK_BatchProduct_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchRequisitionDetail_BatchRequisition_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchRequisitionDetail] DROP CONSTRAINT [FK_BatchRequisitionDetail_BatchRequisition_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchRequisitionDetail_RawMaterialType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchRequisitionDetail] DROP CONSTRAINT [FK_BatchRequisitionDetail_RawMaterialType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyHolidays_Company_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyHolidays] DROP CONSTRAINT [FK_CompanyHolidays_Company_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_AccountHead_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_AccountHead_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_Area_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_Area_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_CustomerStatus_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_CustomerStatus_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_CustomerType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_CustomerType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_PostOffice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_PostOffice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryQuantity_Invoice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryQuantity] DROP CONSTRAINT [FK_DeliveryQuantity_Invoice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryQuantityDetail_DeliveryQuantity_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryQuantityDetail] DROP CONSTRAINT [FK_DeliveryQuantityDetail_DeliveryQuantity_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DeliveryQuantityDetail_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeliveryQuantityDetail] DROP CONSTRAINT [FK_DeliveryQuantityDetail_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_DemandOrderType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_DemandOrderType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_DiscountType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_DiscountType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_RejectedReasonType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_RejectedReasonType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_SaleType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_SaleType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrderDetail_DemandOrder_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrderDetail] DROP CONSTRAINT [FK_DemandOrderDetail_DemandOrder_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrderDetail_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrderDetail] DROP CONSTRAINT [FK_DemandOrderDetail_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrderDiscountTransaction_DemandOrder_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrderDiscountTransaction] DROP CONSTRAINT [FK_DemandOrderDiscountTransaction_DemandOrder_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrderDiscountTransaction_DemandOrderDiscountType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrderDiscountTransaction] DROP CONSTRAINT [FK_DemandOrderDiscountTransaction_DemandOrderDiscountType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrderTransaction_DemandOrder_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrderTransaction] DROP CONSTRAINT [FK_DemandOrderTransaction_DemandOrder_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Department_Company_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Department] DROP CONSTRAINT [FK_Department_Company_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_Department_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_Department_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_Designation_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_Designation_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_Manager_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_Manager_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_PostOffice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_PostOffice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_Department_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_Department_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_Designation_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_Designation_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_Manager_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_Manager_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_PostOffice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_PostOffice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocation_Employee_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocation] DROP CONSTRAINT [FK_EmployeeSalesLocation_Employee_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocation_SalesArea_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocation] DROP CONSTRAINT [FK_EmployeeSalesLocation_SalesArea_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocation_SalesBase_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocation] DROP CONSTRAINT [FK_EmployeeSalesLocation_SalesBase_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocation_SalesDivision_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocation] DROP CONSTRAINT [FK_EmployeeSalesLocation_SalesDivision_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocationHistory_Employee_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocationHistory] DROP CONSTRAINT [FK_EmployeeSalesLocationHistory_Employee_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocationHistory_EmployeeHistory_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocationHistory] DROP CONSTRAINT [FK_EmployeeSalesLocationHistory_EmployeeHistory_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocationHistory_SalesArea_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocationHistory] DROP CONSTRAINT [FK_EmployeeSalesLocationHistory_SalesArea_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocationHistory_SalesBase_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocationHistory] DROP CONSTRAINT [FK_EmployeeSalesLocationHistory_SalesBase_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesLocationHistory_SalesDivision_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesLocationHistory] DROP CONSTRAINT [FK_EmployeeSalesLocationHistory_SalesDivision_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_FinishedGood_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FinishedGood] DROP CONSTRAINT [FK_FinishedGood_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_FinishedGoodOpening_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FinishedGoodOpening] DROP CONSTRAINT [FK_FinishedGoodOpening_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountHead_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountHead] DROP CONSTRAINT [FK_AccountHead_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountHeadOpening_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountHeadOpening] DROP CONSTRAINT [FK_AccountHeadOpening_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountPrimaryHead_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountPrimaryHead] DROP CONSTRAINT [FK_AccountPrimaryHead_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountSubHead_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountSubHead] DROP CONSTRAINT [FK_AccountSubHead_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchRequisition_ToProductionGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchRequisition] DROP CONSTRAINT [FK_BatchRequisition_ToProductionGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchRequisition_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchRequisition] DROP CONSTRAINT [FK_BatchRequisition_User];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchRequisitionProductionEstimation_BatchRequisition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchRequisitionProductionEstimation] DROP CONSTRAINT [FK_BatchRequisitionProductionEstimation_BatchRequisition];
GO
IF OBJECT_ID(N'[dbo].[FK_BatchRequisitionProductionEstimation_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BatchRequisitionProductionEstimation] DROP CONSTRAINT [FK_BatchRequisitionProductionEstimation_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyLeave_LeaveCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyLeave] DROP CONSTRAINT [FK_CompanyLeave_LeaveCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CrashedGood_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrashedGood] DROP CONSTRAINT [FK_CrashedGood_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_CrashedGood_Group]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrashedGood] DROP CONSTRAINT [FK_CrashedGood_Group];
GO
IF OBJECT_ID(N'[dbo].[FK_CrashedGood_Invoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrashedGood] DROP CONSTRAINT [FK_CrashedGood_Invoice];
GO
IF OBJECT_ID(N'[dbo].[FK_CrashedGood_ProductDamageStatusType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CrashedGood] DROP CONSTRAINT [FK_CrashedGood_ProductDamageStatusType];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrentProductStock_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CurrentProductStock] DROP CONSTRAINT [FK_CurrentProductStock_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAttachment_AttachmentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAttachment] DROP CONSTRAINT [FK_CustomerAttachment_AttachmentType];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAttachment_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAttachment] DROP CONSTRAINT [FK_CustomerAttachment_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAttachment_FileType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAttachment] DROP CONSTRAINT [FK_CustomerAttachment_FileType];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerSalesCredit_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSalesCredit] DROP CONSTRAINT [FK_CustomerSalesCredit_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerSalesCreditHistory_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSalesCreditHistory] DROP CONSTRAINT [FK_CustomerSalesCreditHistory_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountHead_dbo_AccountSubHead_AccountSubHeadId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountHead] DROP CONSTRAINT [FK_dbo_AccountHead_dbo_AccountSubHead_AccountSubHeadId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountHeadOpening_dbo_AccountHead_AccountHeadId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountHeadOpening] DROP CONSTRAINT [FK_dbo_AccountHeadOpening_dbo_AccountHead_AccountHeadId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountPrimaryHead_dbo_AccountType_AccountTypeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountPrimaryHead] DROP CONSTRAINT [FK_dbo_AccountPrimaryHead_dbo_AccountType_AccountTypeId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountSubHead_dbo_AccountPrimaryHead_AccountPrimaryHeadId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountSubHead] DROP CONSTRAINT [FK_dbo_AccountSubHead_dbo_AccountPrimaryHead_AccountPrimaryHeadId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountType_dbo_AccountNature_AccountNatureId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountType] DROP CONSTRAINT [FK_dbo_AccountType_dbo_AccountNature_AccountNatureId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_BankInfo_dbo_Company_CompanyId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankInfo] DROP CONSTRAINT [FK_dbo_BankInfo_dbo_Company_CompanyId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ClientInfo_dbo_AccountHead_AccountHeadId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientInfo] DROP CONSTRAINT [FK_dbo_ClientInfo_dbo_AccountHead_AccountHeadId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ClientInfo_dbo_Company_Company_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientInfo] DROP CONSTRAINT [FK_dbo_ClientInfo_dbo_Company_Company_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Company_dbo_Group_GroupId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Company] DROP CONSTRAINT [FK_dbo_Company_dbo_Group_GroupId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransaction_BankInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransaction] DROP CONSTRAINT [FK_dbo_CustomerTransaction_BankInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransaction_TransactionDetail_BankCharge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransaction] DROP CONSTRAINT [FK_dbo_CustomerTransaction_TransactionDetail_BankCharge];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransaction_TransactionDetail_CashBank]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransaction] DROP CONSTRAINT [FK_dbo_CustomerTransaction_TransactionDetail_CashBank];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransaction_TransactionEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransaction] DROP CONSTRAINT [FK_dbo_CustomerTransaction_TransactionEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransactionDetail_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransactionDetail] DROP CONSTRAINT [FK_dbo_CustomerTransactionDetail_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransactionDetail_CustomerTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransactionDetail] DROP CONSTRAINT [FK_dbo_CustomerTransactionDetail_CustomerTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerTransactionDetail_TransactionDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerTransactionDetail] DROP CONSTRAINT [FK_dbo_CustomerTransactionDetail_TransactionDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_PurchaseOrder_AccountHead_Bank]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrder] DROP CONSTRAINT [FK_dbo_PurchaseOrder_AccountHead_Bank];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_PurchaseOrder_AccountHead_Cash]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrder] DROP CONSTRAINT [FK_dbo_PurchaseOrder_AccountHead_Cash];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_PurchaseOrderTransaction_AccountHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrderTransaction] DROP CONSTRAINT [FK_dbo_PurchaseOrderTransaction_AccountHead];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_PurchaseOrderTransaction_PurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrderTransaction] DROP CONSTRAINT [FK_dbo_PurchaseOrderTransaction_PurchaseOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_PurchaseOrderTransaction_Supplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrderTransaction] DROP CONSTRAINT [FK_dbo_PurchaseOrderTransaction_Supplier];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionDetail_dbo_AccountHead_AccountHeadId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDetail] DROP CONSTRAINT [FK_dbo_TransactionDetail_dbo_AccountHead_AccountHeadId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionDetail_dbo_TransactionEntry_TransactionEntryId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDetail] DROP CONSTRAINT [FK_dbo_TransactionDetail_dbo_TransactionEntry_TransactionEntryId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionDetailHistory_dbo_AccountHead_AccountHeadId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDetailHistory] DROP CONSTRAINT [FK_dbo_TransactionDetailHistory_dbo_AccountHead_AccountHeadId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionDetailHistory_dbo_TransactionEntryHistory_TransactionEntryHistoryId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionDetailHistory] DROP CONSTRAINT [FK_dbo_TransactionDetailHistory_dbo_TransactionEntryHistory_TransactionEntryHistoryId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionEntry_dbo_Company_CompanyId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntry] DROP CONSTRAINT [FK_dbo_TransactionEntry_dbo_Company_CompanyId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionEntry_dbo_TransactionType_TransactionTypeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntry] DROP CONSTRAINT [FK_dbo_TransactionEntry_dbo_TransactionType_TransactionTypeId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionEntryHistory_dbo_Company_CompanyId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryHistory] DROP CONSTRAINT [FK_dbo_TransactionEntryHistory_dbo_Company_CompanyId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TransactionEntryHistory_dbo_TransactionType_TransactionTypeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntryHistory] DROP CONSTRAINT [FK_dbo_TransactionEntryHistory_dbo_TransactionType_TransactionTypeId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_User_dbo_Company_CompanyId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_dbo_User_dbo_Company_CompanyId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_UserRole_dbo_Role_Role_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_dbo_UserRole_dbo_Role_Role_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_UserRole_dbo_User_User_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_dbo_UserRole_dbo_User_User_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_UserRoleHistory_dbo_Role_Role_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleHistory] DROP CONSTRAINT [FK_dbo_UserRoleHistory_dbo_Role_Role_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_UserRoleHistory_dbo_User_User_Id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleHistory] DROP CONSTRAINT [FK_dbo_UserRoleHistory_dbo_User_User_Id];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_DemandOrderStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_DemandOrderStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_DemandOrderTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_DemandOrderTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_PaymentStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_PaymentStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_DemandOrder_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DemandOrder] DROP CONSTRAINT [FK_DemandOrder_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_EmployeeType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_EmployeeType];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_SalesArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_SalesArea];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_SalesBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_SalesBase];
GO
IF OBJECT_ID(N'[dbo].[FK_Employee_SalesDivision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_SalesDivision];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_SalesArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_SalesArea];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_SalesBase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_SalesBase];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeHistory_SalesDivision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeHistory] DROP CONSTRAINT [FK_EmployeeHistory_SalesDivision];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeLeave_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeLeave] DROP CONSTRAINT [FK_EmployeeLeave_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeLeave_LeaveCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeLeave] DROP CONSTRAINT [FK_EmployeeLeave_LeaveCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalesTargetMonthly_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesTargetMonthly] DROP CONSTRAINT [FK_EmployeeSalesTargetMonthly_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_FinishedGood_ToProductionGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FinishedGood] DROP CONSTRAINT [FK_FinishedGood_ToProductionGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_FinishedGood_User_Approved]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FinishedGood] DROP CONSTRAINT [FK_FinishedGood_User_Approved];
GO
IF OBJECT_ID(N'[dbo].[FK_FinishedGood_User_Created]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FinishedGood] DROP CONSTRAINT [FK_FinishedGood_User_Created];
GO
IF OBJECT_ID(N'[dbo].[FK_FloorStoreRawMaterial_BatchRequisition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FloorStoreRawMaterial] DROP CONSTRAINT [FK_FloorStoreRawMaterial_BatchRequisition];
GO
IF OBJECT_ID(N'[dbo].[FK_FloorStoreRawMaterial_RawMaterialType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FloorStoreRawMaterial] DROP CONSTRAINT [FK_FloorStoreRawMaterial_RawMaterialType];
GO
IF OBJECT_ID(N'[dbo].[FK_FloorStoreRawMaterial_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FloorStoreRawMaterial] DROP CONSTRAINT [FK_FloorStoreRawMaterial_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoice_ToTransactionEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_Invoice_ToTransactionEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceReturn_Invoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceReturn] DROP CONSTRAINT [FK_InvoiceReturn_Invoice];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceReturn_TransactionEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceReturn] DROP CONSTRAINT [FK_InvoiceReturn_TransactionEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocument_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocument] DROP CONSTRAINT [FK_LegalDocument_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocument_DocumentRenewalCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocument] DROP CONSTRAINT [FK_LegalDocument_DocumentRenewalCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocument_DocumentStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocument] DROP CONSTRAINT [FK_LegalDocument_DocumentStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocument_DocumentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocument] DROP CONSTRAINT [FK_LegalDocument_DocumentType];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocument_User_CreatedBy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocument] DROP CONSTRAINT [FK_LegalDocument_User_CreatedBy];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocument_User_UpdatedBy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocument] DROP CONSTRAINT [FK_LegalDocument_User_UpdatedBy];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocumentHistory_Company]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocumentHistory] DROP CONSTRAINT [FK_LegalDocumentHistory_Company];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocumentHistory_DocumentRenewalCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocumentHistory] DROP CONSTRAINT [FK_LegalDocumentHistory_DocumentRenewalCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocumentHistory_DocumentStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocumentHistory] DROP CONSTRAINT [FK_LegalDocumentHistory_DocumentStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocumentHistory_DocumentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocumentHistory] DROP CONSTRAINT [FK_LegalDocumentHistory_DocumentType];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocumentHistory_User_CreatedBy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocumentHistory] DROP CONSTRAINT [FK_LegalDocumentHistory_User_CreatedBy];
GO
IF OBJECT_ID(N'[dbo].[FK_LegalDocumentHistory_User_UpdatedBy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LegalDocumentHistory] DROP CONSTRAINT [FK_LegalDocumentHistory_User_UpdatedBy];
GO
IF OBJECT_ID(N'[dbo].[FK_MonthlyProcessing_ProcessingType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MonthlyProcessing] DROP CONSTRAINT [FK_MonthlyProcessing_ProcessingType];
GO
IF OBJECT_ID(N'[dbo].[FK_MonthlyProcessing_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MonthlyProcessing] DROP CONSTRAINT [FK_MonthlyProcessing_User];
GO
IF OBJECT_ID(N'[dbo].[FK_MonthlyProcessing_User1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MonthlyProcessing] DROP CONSTRAINT [FK_MonthlyProcessing_User1];
GO
IF OBJECT_ID(N'[dbo].[FK_NotificationSetting_DocumentRenewalCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NotificationSetting] DROP CONSTRAINT [FK_NotificationSetting_DocumentRenewalCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_AccountHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_Product_AccountHead];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_ProductStandardType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_Product_ProductStandardType];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_UnitType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_Product_UnitType];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductHistory_AccountHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductHistory] DROP CONSTRAINT [FK_ProductHistory_AccountHead];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductHistory_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductHistory] DROP CONSTRAINT [FK_ProductHistory_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductHistory_ProductStandardType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductHistory] DROP CONSTRAINT [FK_ProductHistory_ProductStandardType];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductHistory_ProductType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductHistory] DROP CONSTRAINT [FK_ProductHistory_ProductType];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductHistory_UnitType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductHistory] DROP CONSTRAINT [FK_ProductHistory_UnitType];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionForecastMonthly_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionForecastMonthly] DROP CONSTRAINT [FK_ProductionForecastMonthly_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionGroup_ToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionGroup] DROP CONSTRAINT [FK_ProductionGroup_ToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductType_ProductTypeGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductType] DROP CONSTRAINT [FK_ProductType_ProductTypeGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrder_PurchaseOrderStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrder] DROP CONSTRAINT [FK_PurchaseOrder_PurchaseOrderStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrder_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrder] DROP CONSTRAINT [FK_PurchaseOrder_User];
GO
IF OBJECT_ID(N'[dbo].[FK_RawMaterialType_AccountHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RawMaterialType] DROP CONSTRAINT [FK_RawMaterialType_AccountHead];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePolicy_Policy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePolicy] DROP CONSTRAINT [FK_RolePolicy_Policy];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePolicy_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePolicy] DROP CONSTRAINT [FK_RolePolicy_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePolicyHistory_Policy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePolicyHistory] DROP CONSTRAINT [FK_RolePolicyHistory_Policy];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePolicyHistory_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePolicyHistory] DROP CONSTRAINT [FK_RolePolicyHistory_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesArea_SalesDivision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesArea] DROP CONSTRAINT [FK_SalesArea_SalesDivision];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesBase_SalesArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesBase] DROP CONSTRAINT [FK_SalesBase_SalesArea];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreRawMaterial_PurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StoreRawMaterial] DROP CONSTRAINT [FK_StoreRawMaterial_PurchaseOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreRawMaterial_RawMaterialType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StoreRawMaterial] DROP CONSTRAINT [FK_StoreRawMaterial_RawMaterialType];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreRawMaterial_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StoreRawMaterial] DROP CONSTRAINT [FK_StoreRawMaterial_User];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreRawMaterialOpening_RawMaterialType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StoreRawMaterialOpening] DROP CONSTRAINT [FK_StoreRawMaterialOpening_RawMaterialType];
GO
IF OBJECT_ID(N'[dbo].[FK_Supplier_AccountHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Supplier] DROP CONSTRAINT [FK_Supplier_AccountHead];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemWarning_SystemWarningType_SystemWarningTypeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SystemWarning] DROP CONSTRAINT [FK_SystemWarning_SystemWarningType_SystemWarningTypeId];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemWarningHistory_SystemWarning_SystemWarningId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SystemWarningHistory] DROP CONSTRAINT [FK_SystemWarningHistory_SystemWarning_SystemWarningId];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemWarningHistory_SystemWarningType_SystemWarningTypeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SystemWarningHistory] DROP CONSTRAINT [FK_SystemWarningHistory_SystemWarningType_SystemWarningTypeId];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemWarningType_EntityType_EntityTypeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SystemWarningType] DROP CONSTRAINT [FK_SystemWarningType_EntityType_EntityTypeId];
GO
IF OBJECT_ID(N'[dbo].[FK_TransactionEntry_RejectedReasonType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionEntry] DROP CONSTRAINT [FK_TransactionEntry_RejectedReasonType];
GO
IF OBJECT_ID(N'[dbo].[FK_User_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_User_UserStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_UserStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_UserActivityLog_ToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserActivityLog] DROP CONSTRAINT [FK_UserActivityLog_ToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryCustomer_AccountHead_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerHistory] DROP CONSTRAINT [FK_HistoryCustomer_AccountHead_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryCustomer_Area_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerHistory] DROP CONSTRAINT [FK_HistoryCustomer_Area_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryCustomer_CustomerStatus_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerHistory] DROP CONSTRAINT [FK_HistoryCustomer_CustomerStatus_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryCustomer_PostOffice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerHistory] DROP CONSTRAINT [FK_HistoryCustomer_PostOffice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoice_DemandOrder_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_Invoice_DemandOrder_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceDetail_Invoice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [FK_InvoiceDetail_Invoice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceDetail_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [FK_InvoiceDetail_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceReturnDetail_InvoiceReturn_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceReturnDetail] DROP CONSTRAINT [FK_InvoiceReturnDetail_InvoiceReturn_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceReturnDetail_Product_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceReturnDetail] DROP CONSTRAINT [FK_InvoiceReturnDetail_Product_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceTransaction_Invoice_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceTransaction] DROP CONSTRAINT [FK_InvoiceTransaction_Invoice_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_PoliceStation_District_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PoliceStation] DROP CONSTRAINT [FK_PoliceStation_District_Foreign_Key];
GO
IF OBJECT_ID(N'[PPSStoreContainer].[FK_PolicyRouteResource_Policy_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [PPSStoreContainer].[PolicyRouteResource] DROP CONSTRAINT [FK_PolicyRouteResource_Policy_Foreign_Key];
GO
IF OBJECT_ID(N'[PPSStoreContainer].[FK_PolicyRouteResource_RouteResource_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [PPSStoreContainer].[PolicyRouteResource] DROP CONSTRAINT [FK_PolicyRouteResource_RouteResource_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Position_Department_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Designation] DROP CONSTRAINT [FK_Position_Department_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_PostOffice_PoliceStation_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostOffice] DROP CONSTRAINT [FK_PostOffice_PoliceStation_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_ProductType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_Product_ProductType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrder_RejectedReasonType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrder] DROP CONSTRAINT [FK_PurchaseOrder_RejectedReasonType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrder_Supplier_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrder] DROP CONSTRAINT [FK_PurchaseOrder_Supplier_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrderDetail_PurchaseOrder_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrderDetail] DROP CONSTRAINT [FK_PurchaseOrderDetail_PurchaseOrder_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrderDetail_RawMaterialType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrderDetail] DROP CONSTRAINT [FK_PurchaseOrderDetail_RawMaterialType_Foreign_Key];
GO
IF OBJECT_ID(N'[dbo].[FK_RawMaterialType_UnitType_Foreign_Key]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RawMaterialType] DROP CONSTRAINT [FK_RawMaterialType_UnitType_Foreign_Key];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccountHead]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountHead];
GO
IF OBJECT_ID(N'[dbo].[AccountHeadOpening]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountHeadOpening];
GO
IF OBJECT_ID(N'[dbo].[AccountNature]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountNature];
GO
IF OBJECT_ID(N'[dbo].[AccountPrimaryHead]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountPrimaryHead];
GO
IF OBJECT_ID(N'[dbo].[AccountSubHead]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountSubHead];
GO
IF OBJECT_ID(N'[dbo].[AccountType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountType];
GO
IF OBJECT_ID(N'[dbo].[Area]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Area];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[AttachmentType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AttachmentType];
GO
IF OBJECT_ID(N'[dbo].[BankInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BankInfo];
GO
IF OBJECT_ID(N'[dbo].[BatchProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BatchProduct];
GO
IF OBJECT_ID(N'[dbo].[BatchRequisition]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BatchRequisition];
GO
IF OBJECT_ID(N'[dbo].[BatchRequisitionDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BatchRequisitionDetail];
GO
IF OBJECT_ID(N'[dbo].[BatchRequisitionProductionEstimation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BatchRequisitionProductionEstimation];
GO
IF OBJECT_ID(N'[dbo].[ClientInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientInfo];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[CompanyHolidays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyHolidays];
GO
IF OBJECT_ID(N'[dbo].[CompanyLeave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyLeave];
GO
IF OBJECT_ID(N'[dbo].[CompanySalesTarget]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanySalesTarget];
GO
IF OBJECT_ID(N'[dbo].[ControlType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ControlType];
GO
IF OBJECT_ID(N'[dbo].[CrashedGood]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CrashedGood];
GO
IF OBJECT_ID(N'[dbo].[CurrentProductStock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrentProductStock];
GO
IF OBJECT_ID(N'[dbo].[Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer];
GO
IF OBJECT_ID(N'[dbo].[CustomerAttachment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerAttachment];
GO
IF OBJECT_ID(N'[dbo].[CustomerHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerHistory];
GO
IF OBJECT_ID(N'[dbo].[CustomerSalesCredit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSalesCredit];
GO
IF OBJECT_ID(N'[dbo].[CustomerSalesCreditHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSalesCreditHistory];
GO
IF OBJECT_ID(N'[dbo].[CustomerStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerStatus];
GO
IF OBJECT_ID(N'[dbo].[CustomerTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerTransaction];
GO
IF OBJECT_ID(N'[dbo].[CustomerTransactionDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerTransactionDetail];
GO
IF OBJECT_ID(N'[dbo].[CustomerTransactionMonthly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerTransactionMonthly];
GO
IF OBJECT_ID(N'[dbo].[CustomerType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerType];
GO
IF OBJECT_ID(N'[dbo].[DeliveryQuantity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeliveryQuantity];
GO
IF OBJECT_ID(N'[dbo].[DeliveryQuantityDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeliveryQuantityDetail];
GO
IF OBJECT_ID(N'[dbo].[DemandOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrder];
GO
IF OBJECT_ID(N'[dbo].[DemandOrderDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[DemandOrderDiscountTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrderDiscountTransaction];
GO
IF OBJECT_ID(N'[dbo].[DemandOrderDiscountType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrderDiscountType];
GO
IF OBJECT_ID(N'[dbo].[DemandOrderStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrderStatus];
GO
IF OBJECT_ID(N'[dbo].[DemandOrderTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrderTransaction];
GO
IF OBJECT_ID(N'[dbo].[DemandOrderType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DemandOrderType];
GO
IF OBJECT_ID(N'[dbo].[Department]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Department];
GO
IF OBJECT_ID(N'[dbo].[Designation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Designation];
GO
IF OBJECT_ID(N'[dbo].[DiscountType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiscountType];
GO
IF OBJECT_ID(N'[dbo].[District]', 'U') IS NOT NULL
    DROP TABLE [dbo].[District];
GO
IF OBJECT_ID(N'[dbo].[DocumentRenewalCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentRenewalCategory];
GO
IF OBJECT_ID(N'[dbo].[DocumentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentStatus];
GO
IF OBJECT_ID(N'[dbo].[DocumentType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentType];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[EmployeeHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeHistory];
GO
IF OBJECT_ID(N'[dbo].[EmployeeLeave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeLeave];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSalesLocation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSalesLocation];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSalesLocationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSalesLocationHistory];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSalesTargetMonthly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSalesTargetMonthly];
GO
IF OBJECT_ID(N'[dbo].[EmployeeType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeType];
GO
IF OBJECT_ID(N'[dbo].[EntityType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntityType];
GO
IF OBJECT_ID(N'[dbo].[FileType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FileType];
GO
IF OBJECT_ID(N'[dbo].[FinishedGood]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FinishedGood];
GO
IF OBJECT_ID(N'[dbo].[FinishedGoodOpening]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FinishedGoodOpening];
GO
IF OBJECT_ID(N'[dbo].[FiscalYear]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FiscalYear];
GO
IF OBJECT_ID(N'[dbo].[FloorStoreRawMaterial]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FloorStoreRawMaterial];
GO
IF OBJECT_ID(N'[dbo].[Group]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Group];
GO
IF OBJECT_ID(N'[dbo].[Invoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoice];
GO
IF OBJECT_ID(N'[dbo].[InvoiceDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceDetail];
GO
IF OBJECT_ID(N'[dbo].[InvoiceReturn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceReturn];
GO
IF OBJECT_ID(N'[dbo].[InvoiceReturnDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceReturnDetail];
GO
IF OBJECT_ID(N'[dbo].[InvoiceTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceTransaction];
GO
IF OBJECT_ID(N'[dbo].[LeaveCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LeaveCategory];
GO
IF OBJECT_ID(N'[dbo].[LegalDocument]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LegalDocument];
GO
IF OBJECT_ID(N'[dbo].[LegalDocumentHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LegalDocumentHistory];
GO
IF OBJECT_ID(N'[dbo].[Log]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Log];
GO
IF OBJECT_ID(N'[dbo].[MasterSetting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MasterSetting];
GO
IF OBJECT_ID(N'[dbo].[MonthlyProcessing]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MonthlyProcessing];
GO
IF OBJECT_ID(N'[dbo].[NotificationSetting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotificationSetting];
GO
IF OBJECT_ID(N'[dbo].[NotificationType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotificationType];
GO
IF OBJECT_ID(N'[dbo].[PaymentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentStatus];
GO
IF OBJECT_ID(N'[dbo].[PoliceStation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PoliceStation];
GO
IF OBJECT_ID(N'[dbo].[Policy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy];
GO
IF OBJECT_ID(N'[dbo].[PostOffice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostOffice];
GO
IF OBJECT_ID(N'[dbo].[ProcessingType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProcessingType];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[ProductDamageStatusType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductDamageStatusType];
GO
IF OBJECT_ID(N'[dbo].[ProductHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductHistory];
GO
IF OBJECT_ID(N'[dbo].[ProductionForecastMonthly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionForecastMonthly];
GO
IF OBJECT_ID(N'[dbo].[ProductionGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionGroup];
GO
IF OBJECT_ID(N'[dbo].[ProductStandardType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductStandardType];
GO
IF OBJECT_ID(N'[dbo].[ProductType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductType];
GO
IF OBJECT_ID(N'[dbo].[ProductTypeGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductTypeGroup];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrder];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderStatus];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderTransaction];
GO
IF OBJECT_ID(N'[dbo].[RawMaterialType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RawMaterialType];
GO
IF OBJECT_ID(N'[dbo].[ReferenceColumn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReferenceColumn];
GO
IF OBJECT_ID(N'[dbo].[ReferenceTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReferenceTable];
GO
IF OBJECT_ID(N'[dbo].[RejectedReasonType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RejectedReasonType];
GO
IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[RolePolicy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolePolicy];
GO
IF OBJECT_ID(N'[dbo].[RolePolicyHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolePolicyHistory];
GO
IF OBJECT_ID(N'[dbo].[RouteResource]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RouteResource];
GO
IF OBJECT_ID(N'[dbo].[SalesArea]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesArea];
GO
IF OBJECT_ID(N'[dbo].[SalesBase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesBase];
GO
IF OBJECT_ID(N'[dbo].[SalesDivision]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesDivision];
GO
IF OBJECT_ID(N'[dbo].[SaleType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaleType];
GO
IF OBJECT_ID(N'[dbo].[StoreRawMaterial]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StoreRawMaterial];
GO
IF OBJECT_ID(N'[dbo].[StoreRawMaterialOpening]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StoreRawMaterialOpening];
GO
IF OBJECT_ID(N'[dbo].[Supplier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supplier];
GO
IF OBJECT_ID(N'[dbo].[SystemWarning]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemWarning];
GO
IF OBJECT_ID(N'[dbo].[SystemWarningHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemWarningHistory];
GO
IF OBJECT_ID(N'[dbo].[SystemWarningType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemWarningType];
GO
IF OBJECT_ID(N'[dbo].[TransactionDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionDetail];
GO
IF OBJECT_ID(N'[dbo].[TransactionDetailHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionDetailHistory];
GO
IF OBJECT_ID(N'[dbo].[TransactionEntry]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionEntry];
GO
IF OBJECT_ID(N'[dbo].[TransactionEntryHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionEntryHistory];
GO
IF OBJECT_ID(N'[dbo].[TransactionRejectReasonType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionRejectReasonType];
GO
IF OBJECT_ID(N'[dbo].[TransactionStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionStatus];
GO
IF OBJECT_ID(N'[dbo].[TransactionType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionType];
GO
IF OBJECT_ID(N'[dbo].[UnitType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitType];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[UserActivityLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserActivityLog];
GO
IF OBJECT_ID(N'[dbo].[UserLoginLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserLoginLog];
GO
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[UserRoleHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoleHistory];
GO
IF OBJECT_ID(N'[dbo].[UserStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserStatus];
GO
IF OBJECT_ID(N'[PPSStoreContainer].[PolicyRouteResource]', 'U') IS NOT NULL
    DROP TABLE [PPSStoreContainer].[PolicyRouteResource];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AccountHead'
CREATE TABLE [dbo].[AccountHead] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountHeadCode] varchar(10)  NULL,
    [AccountHeadName] varchar(100)  NULL,
    [AccountSubHeadId] int  NOT NULL,
    [Active] bit  NOT NULL,
    [LedgerType] varchar(2)  NULL,
    [CompanyId] int  NOT NULL,
    [CreatedById] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'AccountHeadOpening'
CREATE TABLE [dbo].[AccountHeadOpening] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountHeadId] int  NOT NULL,
    [DrAmount] float  NOT NULL,
    [CrAmount] float  NOT NULL,
    [FiscalYear] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [CreatedById] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'AccountNature'
CREATE TABLE [dbo].[AccountNature] (
    [Id] int  NOT NULL,
    [AccountNatureName] varchar(100)  NOT NULL,
    [CompanyId] int  NOT NULL,
    [CreatedById] int  NULL,
    [CreatedDate] datetime  NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'AccountPrimaryHead'
CREATE TABLE [dbo].[AccountPrimaryHead] (
    [Id] int  NOT NULL,
    [AccountPrimaryHeadName] varchar(100)  NOT NULL,
    [AccountTypeId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [CreatedById] int  NULL,
    [CreatedDate] datetime  NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'AccountSubHead'
CREATE TABLE [dbo].[AccountSubHead] (
    [Id] int  NOT NULL,
    [AccountSubHeadName] varchar(100)  NOT NULL,
    [AccountPrimaryHeadId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [CreatedById] int  NULL,
    [CreatedDate] datetime  NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL
);
GO

-- Creating table 'AccountType'
CREATE TABLE [dbo].[AccountType] (
    [Id] int  NOT NULL,
    [AccountTypeName] varchar(100)  NOT NULL,
    [AccountNatureId] int  NOT NULL,
    [CompanyId] int  NOT NULL
);
GO

-- Creating table 'Area'
CREATE TABLE [dbo].[Area] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AreaName] varchar(50)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AttachmentType'
CREATE TABLE [dbo].[AttachmentType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AttachmentTypeName] varchar(50)  NULL,
    [Strength] int  NULL
);
GO

-- Creating table 'BankInfo'
CREATE TABLE [dbo].[BankInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountNumber] varchar(15)  NOT NULL,
    [BankName] varchar(100)  NOT NULL,
    [BranchName] varchar(50)  NOT NULL,
    [Address] varchar(200)  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedDate] datetime  NULL,
    [CompanyId] int  NOT NULL
);
GO

-- Creating table 'BatchProduct'
CREATE TABLE [dbo].[BatchProduct] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BatchRequisitionId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [EstimatedQuantity] int  NOT NULL
);
GO

-- Creating table 'BatchRequisition'
CREATE TABLE [dbo].[BatchRequisition] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductionGroupId] int  NOT NULL,
    [BatchRequisitionNo] int  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [DeliveredBy] int  NULL,
    [DeliveredOn] datetime  NULL,
    [ReceivedBy] int  NULL,
    [ReceivedOn] datetime  NULL,
    [SendToProductionBy] int  NULL,
    [SendToProductionOn] datetime  NULL,
    [EstimatedProductionDate] datetime  NULL
);
GO

-- Creating table 'BatchRequisitionDetail'
CREATE TABLE [dbo].[BatchRequisitionDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BatchRequisitionId] int  NOT NULL,
    [RawMaterialTypeId] int  NOT NULL,
    [Quantity] float  NOT NULL
);
GO

-- Creating table 'BatchRequisitionProductionEstimation'
CREATE TABLE [dbo].[BatchRequisitionProductionEstimation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BatchRequisitionId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- Creating table 'ClientInfo'
CREATE TABLE [dbo].[ClientInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientName] varchar(100)  NOT NULL,
    [Address] varchar(200)  NOT NULL,
    [Location] varchar(50)  NOT NULL,
    [PhoneNumber] varchar(50)  NOT NULL,
    [Fax] varchar(50)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [MajorActivity] int  NULL,
    [CustomerCategory] int  NULL,
    [AccountHeadId] int  NOT NULL,
    [CreditLimit] float  NOT NULL,
    [Active] bit  NOT NULL,
    [AlternateName] varchar(50)  NULL,
    [ContactPerson] varchar(50)  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedBy] int  NOT NULL,
    [UpdatedDate] datetime  NOT NULL,
    [Company_Id] int  NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [Id] int  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [FullName] nvarchar(200)  NULL,
    [ContactPerson] varchar(50)  NULL,
    [ContactNumber] varchar(50)  NULL,
    [Address] varchar(200)  NOT NULL,
    [Phone] varchar(50)  NULL,
    [Fax] varchar(50)  NULL,
    [LogoPath] varchar(500)  NULL,
    [Email] varchar(200)  NULL,
    [GroupId] int  NOT NULL,
    [AllowedInvalid] int  NOT NULL
);
GO

-- Creating table 'CompanyHolidays'
CREATE TABLE [dbo].[CompanyHolidays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [HolidayName] nvarchar(200)  NOT NULL,
    [HolidayDate] datetime  NOT NULL
);
GO

-- Creating table 'CompanyLeave'
CREATE TABLE [dbo].[CompanyLeave] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [LeaveCategoryId] int  NOT NULL,
    [TotalLeave] int  NOT NULL,
    [Year] int  NOT NULL
);
GO

-- Creating table 'CompanySalesTarget'
CREATE TABLE [dbo].[CompanySalesTarget] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesTarget] decimal(18,0)  NOT NULL,
    [SalesYear] int  NOT NULL,
    [SalesMonth] int  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [IsApproved] bit  NOT NULL
);
GO

-- Creating table 'ControlType'
CREATE TABLE [dbo].[ControlType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] varchar(50)  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedBy] int  NOT NULL,
    [UpdatedDate] datetime  NOT NULL
);
GO

-- Creating table 'CrashedGood'
CREATE TABLE [dbo].[CrashedGood] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Product_Id] int  NOT NULL,
    [ProductQuantity] int  NOT NULL,
    [InvoiceId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [GroupId] int  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [DamageStatusId] int  NULL
);
GO

-- Creating table 'CurrentProductStock'
CREATE TABLE [dbo].[CurrentProductStock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [TotalQuantity] int  NOT NULL,
    [DeliveredQuantity] int  NOT NULL,
    [AllocatedQuantity] int  NOT NULL,
    [AvailableQuantity] int  NOT NULL
);
GO

-- Creating table 'Customer'
CREATE TABLE [dbo].[Customer] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerName] varchar(50)  NOT NULL,
    [CustomerCode] int  NOT NULL,
    [CustomerAddress] varchar(300)  NOT NULL,
    [CustomerMobile] varchar(16)  NULL,
    [CustomerPhone] varchar(16)  NULL,
    [OwnerName] varchar(100)  NOT NULL,
    [OwnerMobile] varchar(16)  NULL,
    [OwnerPhone] varchar(16)  NULL,
    [OwnerBirthDate] datetime  NULL,
    [ContactPersonName] varchar(100)  NULL,
    [ContactPersonMobile] varchar(16)  NULL,
    [PrimaryContactNo] varchar(16)  NULL,
    [Village] varchar(100)  NULL,
    [PostOfficeId] int  NULL,
    [Email] varchar(50)  NULL,
    [AreaId] int  NULL,
    [EmployeeId] int  NULL,
    [CustomerTypeId] int  NULL,
    [AccountHeadId] int  NULL,
    [CustomerStatusId] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedOn] datetime  NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'CustomerAttachment'
CREATE TABLE [dbo].[CustomerAttachment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileGuid] uniqueidentifier  NOT NULL,
    [CustomerId] int  NOT NULL,
    [AttachmentTypeId] int  NOT NULL,
    [Description] varchar(300)  NULL,
    [FileTypeId] int  NULL,
    [FileName] varchar(50)  NULL,
    [FileSize] int  NULL
);
GO

-- Creating table 'CustomerHistory'
CREATE TABLE [dbo].[CustomerHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [CustomerName] varchar(50)  NOT NULL,
    [CustomerCode] int  NOT NULL,
    [CustomerAddress] varchar(300)  NOT NULL,
    [CustomerMobile] varchar(16)  NULL,
    [CustomerPhone] varchar(16)  NULL,
    [OwnerName] varchar(100)  NOT NULL,
    [OwnerMobile] varchar(16)  NULL,
    [OwnerPhone] varchar(16)  NULL,
    [OwnerBirthDate] datetime  NULL,
    [ContactPersonName] varchar(100)  NULL,
    [ContactPersonMobile] varchar(16)  NULL,
    [PrimaryContactNo] varchar(16)  NULL,
    [Village] varchar(100)  NULL,
    [PostOfficeId] int  NULL,
    [Email] varchar(50)  NULL,
    [AreaId] int  NULL,
    [EmployeeId] int  NULL,
    [CustomerTypeId] int  NULL,
    [AccountHeadId] int  NULL,
    [CustomerStatusId] int  NULL,
    [CreatedBy] int  NULL,
    [CreatedOn] datetime  NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'CustomerSalesCredit'
CREATE TABLE [dbo].[CustomerSalesCredit] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [MonthlyCredit] decimal(18,0)  NULL,
    [YearlyCredit] decimal(18,0)  NULL,
    [EffectiveDate] datetime  NULL,
    [SalesCapacityYearly] decimal(18,0)  NULL
);
GO

-- Creating table 'CustomerSalesCreditHistory'
CREATE TABLE [dbo].[CustomerSalesCreditHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [MonthlyCredit] decimal(18,0)  NULL,
    [YearlyCredit] decimal(18,0)  NULL,
    [EffectiveDate] datetime  NULL,
    [SalesCapacityYearly] decimal(18,0)  NULL
);
GO

-- Creating table 'CustomerStatus'
CREATE TABLE [dbo].[CustomerStatus] (
    [Id] int  NOT NULL,
    [Status] varchar(50)  NULL
);
GO

-- Creating table 'CustomerTransaction'
CREATE TABLE [dbo].[CustomerTransaction] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CashBankAccountHeadId] int  NOT NULL,
    [TransactionReference] varchar(20)  NULL,
    [TransactionAmount] float  NOT NULL,
    [CashBankTransactionDetailId] int  NULL,
    [BankChargeAccountHeadId] int  NULL,
    [BankChargeAmount] float  NULL,
    [BankChargeTransactionDetailId] int  NULL,
    [TransactionDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [IsApproved] bit  NOT NULL,
    [TransactionEntryId] int  NULL
);
GO

-- Creating table 'CustomerTransactionDetail'
CREATE TABLE [dbo].[CustomerTransactionDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerTransactionId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [BookNo] int  NULL,
    [BookSerialNo] int  NULL,
    [TransactionAmount] float  NOT NULL,
    [TransactionDetailId] int  NULL
);
GO

-- Creating table 'CustomerTransactionMonthly'
CREATE TABLE [dbo].[CustomerTransactionMonthly] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [TransactionMonth] datetime  NOT NULL,
    [TotalDoAmount] decimal(10,0)  NOT NULL,
    [TotalPaidAmount] decimal(10,0)  NOT NULL,
    [CarryForwardedBalanceAmount] decimal(10,0)  NOT NULL,
    [BalanceAmount] decimal(10,0)  NOT NULL
);
GO

-- Creating table 'CustomerType'
CREATE TABLE [dbo].[CustomerType] (
    [Id] int  NOT NULL,
    [CustomerTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'DeliveryQuantity'
CREATE TABLE [dbo].[DeliveryQuantity] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceId] int  NOT NULL,
    [ChallanDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdateBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [Note] varchar(250)  NULL
);
GO

-- Creating table 'DeliveryQuantityDetail'
CREATE TABLE [dbo].[DeliveryQuantityDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DeliveryQuantityId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- Creating table 'DemandOrder'
CREATE TABLE [dbo].[DemandOrder] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DemandOrderNo] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [DemandOrderTypeId] int  NOT NULL,
    [DiscountTypeId] int  NULL,
    [DODate] datetime  NOT NULL,
    [VerifiedBy] int  NULL,
    [VerifiedOn] datetime  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [ReferenceDONo] varchar(25)  NULL,
    [SubmittedBy] int  NULL,
    [SubmittedOn] datetime  NULL,
    [RejectedBy] int  NULL,
    [RejectedOn] datetime  NULL,
    [ReturnedBy] int  NULL,
    [ReturnedOn] datetime  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [DeliveryConfirmedBy] int  NULL,
    [DeliveryConfirmedOn] datetime  NULL,
    [IsCurrentRecord] bit  NOT NULL,
    [PreviousId] int  NULL,
    [Locked] bit  NOT NULL,
    [RejectedReasonTypeId] int  NULL,
    [SaleTypeId] int  NOT NULL,
    [DemandOrderStatusId] int  NOT NULL,
    [DemandOrderTransactionId] int  NULL,
    [TotalAmount] float  NOT NULL,
    [RegularDiscountInPercentage] float  NULL,
    [RegularDiscountAmount] float  NULL,
    [SpecialDiscountInPercentage] float  NULL,
    [SpecialDiscountAmount] float  NULL,
    [AdditionalDiscountInPercentage] float  NULL,
    [AdditionalDiscountAmount] float  NULL,
    [ExtraDiscountInPercentage] float  NULL,
    [ExtraDiscountAmount] float  NULL,
    [CashBackAmount] float  NULL,
    [TotalDiscountAmount] float  NULL,
    [TotalGrandAmount] float  NOT NULL,
    [TotalDueAmount] float  NULL,
    [EmployeeId] int  NOT NULL,
    [Note] varchar(250)  NULL,
    [PaymentStatusId] int  NOT NULL,
    [PaymentCompleteDate] datetime  NULL,
    [IsInvoiceCompleted] bit  NULL
);
GO

-- Creating table 'DemandOrderDetail'
CREATE TABLE [dbo].[DemandOrderDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DemandOrderId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Discount] float  NULL,
    [UnitPrice] float  NOT NULL,
    [TotalPrice] float  NOT NULL
);
GO

-- Creating table 'DemandOrderDiscountTransaction'
CREATE TABLE [dbo].[DemandOrderDiscountTransaction] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DemandOrderId] int  NOT NULL,
    [DemandOrderDiscountTypeId] int  NOT NULL,
    [TransactionAmount] float  NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [VerifiedBy] int  NULL,
    [VerifiedOn] datetime  NULL,
    [IsVerified] bit  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [IsApproved] bit  NOT NULL,
    [TransactionEntryId] int  NULL,
    [DeletedBy] int  NULL,
    [DeletedOn] datetime  NULL,
    [IsDeleted] bit  NULL
);
GO

-- Creating table 'DemandOrderDiscountType'
CREATE TABLE [dbo].[DemandOrderDiscountType] (
    [Id] int  NOT NULL,
    [DemandOrderDiscountTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'DemandOrderStatus'
CREATE TABLE [dbo].[DemandOrderStatus] (
    [Id] int  NOT NULL,
    [Status] varchar(15)  NOT NULL
);
GO

-- Creating table 'DemandOrderTransaction'
CREATE TABLE [dbo].[DemandOrderTransaction] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DemandOrderId] int  NOT NULL,
    [TransactionAmount] float  NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'DemandOrderType'
CREATE TABLE [dbo].[DemandOrderType] (
    [Id] int  NOT NULL,
    [DemandOrderTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'Department'
CREATE TABLE [dbo].[Department] (
    [Id] int  NOT NULL,
    [DepartmentName] varchar(100)  NOT NULL,
    [Description] varchar(50)  NULL,
    [CompanyId] int  NULL
);
GO

-- Creating table 'Designation'
CREATE TABLE [dbo].[Designation] (
    [Id] int  NOT NULL,
    [DesignationName] varchar(100)  NOT NULL,
    [Description] varchar(50)  NULL,
    [DepartmentId] int  NOT NULL,
    [DesignationSerialNo] int  NOT NULL
);
GO

-- Creating table 'DiscountType'
CREATE TABLE [dbo].[DiscountType] (
    [Id] int  NOT NULL,
    [DiscountTypeName] varchar(300)  NOT NULL
);
GO

-- Creating table 'District'
CREATE TABLE [dbo].[District] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DistrictName] varchar(50)  NOT NULL
);
GO

-- Creating table 'DocumentRenewalCategory'
CREATE TABLE [dbo].[DocumentRenewalCategory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'DocumentStatus'
CREATE TABLE [dbo].[DocumentStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'DocumentType'
CREATE TABLE [dbo].[DocumentType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DocumentTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'Employee'
CREATE TABLE [dbo].[Employee] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeCode] int  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Address] varchar(300)  NULL,
    [PostOfficeId] int  NULL,
    [Email] varchar(50)  NOT NULL,
    [Mobile] varchar(16)  NOT NULL,
    [Phone] varchar(16)  NULL,
    [DepartmentId] int  NULL,
    [EmployeeTypeId] int  NULL,
    [BloodGroup] varchar(10)  NULL,
    [ImageId] varchar(300)  NULL,
    [DesignationId] int  NULL,
    [ManagerId] int  NULL,
    [IsActive] bit  NOT NULL,
    [SalesDivisionId] int  NULL,
    [SalesAreaId] int  NULL,
    [SalesBaseId] int  NULL,
    [CompanyId] int  NULL,
    [DateOfJoin] datetime  NULL,
    [JobConfirmationDate] datetime  NULL
);
GO

-- Creating table 'EmployeeHistory'
CREATE TABLE [dbo].[EmployeeHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [EmployeeCode] int  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Address] varchar(300)  NULL,
    [PostOfficeId] int  NULL,
    [Email] varchar(50)  NOT NULL,
    [Mobile] varchar(16)  NOT NULL,
    [Phone] varchar(16)  NULL,
    [DepartmentId] int  NULL,
    [BloodGroup] varchar(10)  NULL,
    [ImageId] varchar(300)  NULL,
    [DesignationId] int  NULL,
    [ManagerId] int  NULL,
    [IsActive] bit  NOT NULL,
    [SalesDivisionId] int  NULL,
    [SalesAreaId] int  NULL,
    [SalesBaseId] int  NULL,
    [CompanyId] int  NULL,
    [DateOfJoin] datetime  NULL
);
GO

-- Creating table 'EmployeeLeave'
CREATE TABLE [dbo].[EmployeeLeave] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [LeaveCategoryId] int  NOT NULL,
    [LeaveDays] int  NULL,
    [UnpaidLeaveDays] int  NULL,
    [FromDate] datetime  NOT NULL,
    [ToDate] datetime  NOT NULL,
    [ReasonOfLeave] nvarchar(300)  NOT NULL,
    [Address] nvarchar(100)  NULL,
    [MobileNo] varchar(16)  NOT NULL,
    [DateOfApplication] datetime  NOT NULL,
    [ApprovedByDepartmentHead] bit  NULL,
    [ApprovedByMD] bit  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [LeaveYear] int  NOT NULL,
    [IsApproved] int  NULL,
    [ApprovedByAdminOn] datetime  NULL,
    [ApprovedByHeadOn] datetime  NULL
);
GO

-- Creating table 'EmployeeSalesLocation'
CREATE TABLE [dbo].[EmployeeSalesLocation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [DivisionId] int  NOT NULL,
    [AreaId] int  NULL,
    [BaseId] int  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'EmployeeSalesLocationHistory'
CREATE TABLE [dbo].[EmployeeSalesLocationHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeHistoryId] int  NOT NULL,
    [EmployeeId] int  NOT NULL,
    [SalesDivisionId] int  NOT NULL,
    [SalesAreaId] int  NULL,
    [SalesBaseId] int  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'EmployeeSalesTargetMonthly'
CREATE TABLE [dbo].[EmployeeSalesTargetMonthly] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [SalesTarget] decimal(8,0)  NOT NULL,
    [TeamTarget] decimal(8,0)  NOT NULL,
    [Achievement] decimal(8,0)  NULL,
    [Percentage] decimal(4,2)  NULL,
    [SalesYear] int  NOT NULL,
    [SalesMonth] int  NOT NULL
);
GO

-- Creating table 'EmployeeType'
CREATE TABLE [dbo].[EmployeeType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeName] varchar(50)  NOT NULL,
    [Description] varchar(50)  NULL
);
GO

-- Creating table 'EntityType'
CREATE TABLE [dbo].[EntityType] (
    [Id] int  NOT NULL,
    [EntityTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'FileType'
CREATE TABLE [dbo].[FileType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileTypeName] varchar(10)  NULL
);
GO

-- Creating table 'FinishedGood'
CREATE TABLE [dbo].[FinishedGood] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductionGroupId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [ProductionDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [IsApproved] bit  NOT NULL
);
GO

-- Creating table 'FinishedGoodOpening'
CREATE TABLE [dbo].[FinishedGoodOpening] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [FiscalYear] int  NOT NULL
);
GO

-- Creating table 'FiscalYear'
CREATE TABLE [dbo].[FiscalYear] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Year] int  NOT NULL,
    [OpenDate] datetime  NOT NULL,
    [CloseDate] datetime  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedById] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedById] int  NOT NULL,
    [UpdatedDate] datetime  NOT NULL
);
GO

-- Creating table 'FloorStoreRawMaterial'
CREATE TABLE [dbo].[FloorStoreRawMaterial] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BatchRequisitionId] int  NOT NULL,
    [RawMaterialTypeId] int  NOT NULL,
    [Quantity] float  NOT NULL,
    [ReceivedBy] int  NULL,
    [ReceivedOn] datetime  NULL
);
GO

-- Creating table 'Group'
CREATE TABLE [dbo].[Group] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Invoice'
CREATE TABLE [dbo].[Invoice] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceNo] int  NOT NULL,
    [DemandOrderId] int  NOT NULL,
    [InvoiceDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [Note] varchar(500)  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [DeliveredBy] int  NULL,
    [DeliveredOn] datetime  NULL,
    [TotalAmount] float  NULL,
    [RegularDiscountInPercentage] float  NULL,
    [RegularDiscountAmount] float  NULL,
    [SpecialDiscountInPercentage] float  NULL,
    [SpecialDiscountAmount] float  NULL,
    [AdditionalDiscountInPercentage] float  NULL,
    [AdditionalDiscountAmount] float  NULL,
    [ExtraDiscountInPercentage] float  NULL,
    [ExtraDiscountAmount] float  NULL,
    [CashBackAmount] float  NULL,
    [TotalDiscountAmount] float  NULL,
    [TotalGrandAmount] float  NULL,
    [TotalDueAmount] float  NULL,
    [TransactionEntryId] int  NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'InvoiceDetail'
CREATE TABLE [dbo].[InvoiceDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [TotalAmount] decimal(10,2)  NOT NULL
);
GO

-- Creating table 'InvoiceReturn'
CREATE TABLE [dbo].[InvoiceReturn] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceId] int  NOT NULL,
    [ReturnDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [Note] varchar(500)  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [TotalAmount] float  NULL,
    [TotalGrandAmount] float  NULL,
    [TransactionEntryId] int  NULL,
    [UpdateBy] int  NULL,
    [UpdateOn] datetime  NULL
);
GO

-- Creating table 'InvoiceReturnDetail'
CREATE TABLE [dbo].[InvoiceReturnDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceReturnId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [TotalAmount] decimal(10,2)  NOT NULL
);
GO

-- Creating table 'InvoiceTransaction'
CREATE TABLE [dbo].[InvoiceTransaction] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvoiceId] int  NOT NULL,
    [TransactionAmount] float  NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'LeaveCategory'
CREATE TABLE [dbo].[LeaveCategory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'LegalDocument'
CREATE TABLE [dbo].[LegalDocument] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [DocumentTypeId] int  NOT NULL,
    [IssueDate] datetime  NOT NULL,
    [DocumentNumber] varchar(20)  NULL,
    [ExpireDate] datetime  NULL,
    [DocumentRenewCategoryId] int  NOT NULL,
    [DocumentStatusId] int  NOT NULL,
    [IsActive] bit  NOT NULL,
    [OrganizationName] nvarchar(150)  NOT NULL,
    [OrganizationAddress] nvarchar(300)  NOT NULL,
    [OrganizationContactEmail] nvarchar(50)  NULL,
    [OrganizationContactName] nvarchar(30)  NULL,
    [OrganizationPhoneNumber] nvarchar(20)  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'LegalDocumentHistory'
CREATE TABLE [dbo].[LegalDocumentHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LegalDocumentId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [DocumentTypeId] int  NOT NULL,
    [IssueDate] datetime  NOT NULL,
    [DocumentNumber] varchar(20)  NULL,
    [ExpireDate] datetime  NULL,
    [DocumentRenewCategoryId] int  NOT NULL,
    [DocumentStatusId] int  NOT NULL,
    [IsActive] bit  NOT NULL,
    [OrganizationName] nvarchar(150)  NOT NULL,
    [OrganizationAddress] nvarchar(500)  NOT NULL,
    [OrganizationContactEmail] nvarchar(50)  NULL,
    [OrganizationContactName] nvarchar(30)  NULL,
    [OrganizationPhoneNumber] nvarchar(20)  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'Log'
CREATE TABLE [dbo].[Log] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserId] varchar(50)  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ErrorMessage] varchar(200)  NOT NULL,
    [InnerMessage] varchar(500)  NULL,
    [StackTrace] varchar(max)  NULL,
    [AbsolutePath] varchar(100)  NULL
);
GO

-- Creating table 'MasterSetting'
CREATE TABLE [dbo].[MasterSetting] (
    [Id] int  NOT NULL,
    [EarlyPaymentDiscountInPercentage] float  NULL,
    [EarlyPaymentInDays] int  NULL
);
GO

-- Creating table 'MonthlyProcessing'
CREATE TABLE [dbo].[MonthlyProcessing] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProcessingTypeId] int  NOT NULL,
    [Month] int  NOT NULL,
    [Year] int  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ReprocessedBy] int  NULL,
    [ReprocessedOn] datetime  NULL
);
GO

-- Creating table 'NotificationSetting'
CREATE TABLE [dbo].[NotificationSetting] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DocumentRenewalCategoryId] int  NOT NULL,
    [FirstNotificationDays] int  NOT NULL,
    [SecondNotificationDays] int  NOT NULL,
    [FinalNotificationDays] int  NOT NULL,
    [FirstNotificationContinuity] bit  NOT NULL,
    [SecondNotificationContinuity] bit  NOT NULL
);
GO

-- Creating table 'NotificationType'
CREATE TABLE [dbo].[NotificationType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(20)  NOT NULL
);
GO

-- Creating table 'PaymentStatus'
CREATE TABLE [dbo].[PaymentStatus] (
    [Id] int  NOT NULL,
    [PaymentStatusName] varchar(20)  NOT NULL
);
GO

-- Creating table 'PoliceStation'
CREATE TABLE [dbo].[PoliceStation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PoliceStationName] varchar(50)  NOT NULL,
    [DistrictId] int  NOT NULL
);
GO

-- Creating table 'Policy'
CREATE TABLE [dbo].[Policy] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PolicyName] varchar(150)  NOT NULL,
    [Description] varchar(200)  NULL,
    [PolicyCode] int  NULL,
    [AppTypeId] int  NULL
);
GO

-- Creating table 'PostOffice'
CREATE TABLE [dbo].[PostOffice] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PostOfficeName] varchar(50)  NOT NULL,
    [PostCode] varchar(10)  NOT NULL,
    [PoliceStationId] int  NOT NULL
);
GO

-- Creating table 'ProcessingType'
CREATE TABLE [dbo].[ProcessingType] (
    [Id] int  NOT NULL,
    [ProcessingTypeName] varchar(30)  NOT NULL
);
GO

-- Creating table 'Product'
CREATE TABLE [dbo].[Product] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(300)  NOT NULL,
    [Code] varchar(20)  NULL,
    [Color] varchar(20)  NULL,
    [ProductStandardTypeId] int  NULL,
    [Thickness] varchar(20)  NULL,
    [Length] decimal(5,2)  NULL,
    [UnitTypeId] int  NULL,
    [UnitPrice] decimal(7,2)  NOT NULL,
    [ProductTypeId] int  NOT NULL,
    [AccountHeadId] int  NOT NULL,
    [CreatedBy] int  NULL,
    [CreatedOn] datetime  NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL
);
GO

-- Creating table 'ProductDamageStatusType'
CREATE TABLE [dbo].[ProductDamageStatusType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'ProductHistory'
CREATE TABLE [dbo].[ProductHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Name] varchar(300)  NOT NULL,
    [Code] varchar(20)  NULL,
    [Color] varchar(20)  NULL,
    [ProductStandardTypeId] int  NULL,
    [Thickness] varchar(20)  NULL,
    [Length] decimal(5,2)  NULL,
    [UnitTypeId] int  NULL,
    [UnitPrice] decimal(7,2)  NOT NULL,
    [ProductTypeId] int  NOT NULL,
    [AccountHeadId] int  NOT NULL,
    [HistoryById] int  NOT NULL,
    [HistoryDate] datetime  NOT NULL,
    [BatchId] int  NULL
);
GO

-- Creating table 'ProductionForecastMonthly'
CREATE TABLE [dbo].[ProductionForecastMonthly] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [SalesYear] int  NOT NULL,
    [SalesMonth] int  NOT NULL,
    [CompanySalesTargetId] int  NOT NULL
);
GO

-- Creating table 'ProductionGroup'
CREATE TABLE [dbo].[ProductionGroup] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductionGroupId] varchar(20)  NOT NULL,
    [IsClosed] bit  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'ProductStandardType'
CREATE TABLE [dbo].[ProductStandardType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductStandardTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'ProductType'
CREATE TABLE [dbo].[ProductType] (
    [Id] int  NOT NULL,
    [ProductTypeName] varchar(300)  NOT NULL,
    [Description] varchar(500)  NULL,
    [ProductTypeGroupId] int  NOT NULL
);
GO

-- Creating table 'ProductTypeGroup'
CREATE TABLE [dbo].[ProductTypeGroup] (
    [Id] int  NOT NULL,
    [ProductTypeGroupName] varchar(30)  NOT NULL,
    [Description] varchar(150)  NULL
);
GO

-- Creating table 'PurchaseOrder'
CREATE TABLE [dbo].[PurchaseOrder] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PurchaseOrderNo] int  NOT NULL,
    [PurchaseOrderDate] datetime  NOT NULL,
    [IsCashPurchase] bit  NULL,
    [IsCreditPurchase] bit  NULL,
    [IsLcPurchase] bit  NULL,
    [Note] varchar(500)  NULL,
    [PaymentType] varchar(20)  NULL,
    [EstimatedDeliveryDate] datetime  NULL,
    [PriceValidity] int  NULL,
    [TotalAmount] float  NOT NULL,
    [VerifiedBy] int  NULL,
    [VerifiedOn] datetime  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpatedBy] int  NULL,
    [UpatedOn] datetime  NULL,
    [IsCurrentRecord] bit  NOT NULL,
    [PreviousId] int  NULL,
    [Locked] bit  NOT NULL,
    [RejectedReasonTypeId] int  NULL,
    [RejectedBy] int  NULL,
    [RejectedOn] datetime  NULL,
    [PurchaseOrderStatusId] int  NULL,
    [CashAccountHeadId] int  NULL,
    [CashAmount] float  NULL,
    [BankAccountHeadId] int  NULL,
    [BankAmount] float  NULL,
    [SupplierId] int  NULL,
    [SupplierAccountHeadId] int  NULL,
    [SupplierAmount] float  NULL,
    [LCNo] varchar(20)  NULL,
    [LCAccountHeadId] int  NULL,
    [LCAmount] float  NULL,
    [TransactionEntryId] int  NULL,
    [IsVerified] bit  NULL,
    [IsApproved] bit  NULL,
    [IsPaymentComplete] bit  NOT NULL
);
GO

-- Creating table 'PurchaseOrderDetail'
CREATE TABLE [dbo].[PurchaseOrderDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PurchaseOrderId] int  NOT NULL,
    [RawMaterialTypeId] int  NOT NULL,
    [Quantity] float  NULL,
    [Price] float  NULL,
    [AccountHeadId] int  NULL
);
GO

-- Creating table 'PurchaseOrderStatus'
CREATE TABLE [dbo].[PurchaseOrderStatus] (
    [Id] int  NOT NULL,
    [Status] varchar(20)  NOT NULL
);
GO

-- Creating table 'PurchaseOrderTransaction'
CREATE TABLE [dbo].[PurchaseOrderTransaction] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PurchaseOrderId] int  NOT NULL,
    [SupplierId] int  NOT NULL,
    [CashBankAccountHeadId] int  NOT NULL,
    [TransactionAmount] float  NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [UpdatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [ApprovedBy] int  NULL,
    [ApprovedOn] datetime  NULL,
    [IsApproved] bit  NOT NULL
);
GO

-- Creating table 'RawMaterialType'
CREATE TABLE [dbo].[RawMaterialType] (
    [Id] int  NOT NULL,
    [RawMaterialTypeName] varchar(50)  NOT NULL,
    [UnitTypeId] int  NOT NULL,
    [AccountHeadId] int  NOT NULL
);
GO

-- Creating table 'ReferenceColumn'
CREATE TABLE [dbo].[ReferenceColumn] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerAccountSubHeadId] int  NULL,
    [BankAccountSubHeadId] int  NULL,
    [CashAccountSubHeadId] int  NULL,
    [SalesAccountHeadId] int  NULL,
    [SupplierAccountSubHeadId] int  NULL
);
GO

-- Creating table 'ReferenceTable'
CREATE TABLE [dbo].[ReferenceTable] (
    [Id] int  NOT NULL,
    [ReferenceName] varchar(50)  NOT NULL,
    [ReferenceValue] varchar(50)  NOT NULL
);
GO

-- Creating table 'RejectedReasonType'
CREATE TABLE [dbo].[RejectedReasonType] (
    [Id] int  NOT NULL,
    [RejectedReasonTypeName] varchar(300)  NOT NULL
);
GO

-- Creating table 'Role'
CREATE TABLE [dbo].[Role] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleName] varchar(50)  NOT NULL,
    [Description] varchar(200)  NULL
);
GO

-- Creating table 'RolePolicy'
CREATE TABLE [dbo].[RolePolicy] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] int  NOT NULL,
    [PolicyId] int  NOT NULL,
    [AssignedBy] int  NULL,
    [AssignedOn] datetime  NULL
);
GO

-- Creating table 'RolePolicyHistory'
CREATE TABLE [dbo].[RolePolicyHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RolePolicyId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [PolicyId] int  NOT NULL,
    [AssignedBy] int  NULL,
    [AssignedOn] datetime  NULL
);
GO

-- Creating table 'RouteResource'
CREATE TABLE [dbo].[RouteResource] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ControllerName] varchar(50)  NOT NULL,
    [ActionName] varchar(50)  NOT NULL
);
GO

-- Creating table 'SalesArea'
CREATE TABLE [dbo].[SalesArea] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesAreaName] varchar(30)  NOT NULL,
    [SalesDivisionId] int  NOT NULL
);
GO

-- Creating table 'SalesBase'
CREATE TABLE [dbo].[SalesBase] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesBaseName] varchar(30)  NOT NULL,
    [SalesAreaId] int  NOT NULL
);
GO

-- Creating table 'SalesDivision'
CREATE TABLE [dbo].[SalesDivision] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesDivisionName] varchar(30)  NOT NULL
);
GO

-- Creating table 'SaleType'
CREATE TABLE [dbo].[SaleType] (
    [Id] int  NOT NULL,
    [SaleTypeName] varchar(300)  NOT NULL,
    [DurationInDays] int  NOT NULL,
    [WarningInDays] int  NULL,
    [EarlyPaymentInDays] int  NULL,
    [EarlyPaymentDiscountInPercentage] float  NULL
);
GO

-- Creating table 'StoreRawMaterial'
CREATE TABLE [dbo].[StoreRawMaterial] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PurchaseOrderId] int  NOT NULL,
    [RawMaterialTypeId] int  NOT NULL,
    [Quantity] float  NOT NULL,
    [ReceivedBy] int  NOT NULL,
    [ReceivedOn] datetime  NOT NULL
);
GO

-- Creating table 'StoreRawMaterialOpening'
CREATE TABLE [dbo].[StoreRawMaterialOpening] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RawMaterialTypeId] int  NOT NULL,
    [Quantity] float  NOT NULL,
    [FiscalYear] int  NOT NULL
);
GO

-- Creating table 'Supplier'
CREATE TABLE [dbo].[Supplier] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierName] varchar(50)  NOT NULL,
    [SupplierCode] int  NOT NULL,
    [Address] varchar(300)  NOT NULL,
    [ContactPerson] varchar(150)  NOT NULL,
    [Phone] varchar(16)  NOT NULL,
    [ContactPersonPhone] varchar(16)  NOT NULL,
    [Email] varchar(50)  NULL,
    [ContactPersonEmail] varchar(50)  NULL,
    [AccountHeadId] int  NOT NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'SystemWarning'
CREATE TABLE [dbo].[SystemWarning] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemWarningTypeId] int  NOT NULL,
    [EntityId] int  NOT NULL,
    [Message] nvarchar(200)  NOT NULL,
    [WarningDays] int  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [UpdatedBy] int  NULL
);
GO

-- Creating table 'SystemWarningHistory'
CREATE TABLE [dbo].[SystemWarningHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemWarningId] int  NOT NULL,
    [SystemWarningTypeId] int  NOT NULL,
    [EntityId] int  NOT NULL,
    [Message] nvarchar(200)  NOT NULL,
    [WarningDays] int  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [UpdatedOn] datetime  NULL,
    [UpdatedBy] int  NULL
);
GO

-- Creating table 'SystemWarningType'
CREATE TABLE [dbo].[SystemWarningType] (
    [Id] int  NOT NULL,
    [SystemWarningTypeName] varchar(30)  NOT NULL,
    [WarningPeriod] int  NOT NULL,
    [EntityId] int  NOT NULL,
    [EntityTypeId] int  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'TransactionDetail'
CREATE TABLE [dbo].[TransactionDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountHeadId] int  NOT NULL,
    [DrAmount] float  NOT NULL,
    [CrAmount] float  NOT NULL,
    [TransactionEntryId] int  NOT NULL,
    [Note] varchar(1000)  NULL
);
GO

-- Creating table 'TransactionDetailHistory'
CREATE TABLE [dbo].[TransactionDetailHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TransactionDetailId] int  NOT NULL,
    [AccountHeadId] int  NOT NULL,
    [DrAmount] float  NOT NULL,
    [CrAmount] float  NOT NULL,
    [TransactionEntryId] int  NOT NULL,
    [Note] varchar(1000)  NULL
);
GO

-- Creating table 'TransactionEntry'
CREATE TABLE [dbo].[TransactionEntry] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TransactionNumber] varchar(15)  NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [FiscalYear] int  NOT NULL,
    [TransactionTypeId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [PostingDate] datetime  NOT NULL,
    [Active] bit  NOT NULL,
    [Deleted] bit  NULL,
    [Accepted] bit  NOT NULL,
    [AcceptedById] int  NULL,
    [AcceptedDate] datetime  NULL,
    [PreviousId] int  NULL,
    [UpdateReason] varchar(250)  NULL,
    [CreatedById] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL,
    [Particulars] varchar(250)  NULL,
    [VerifiedById] int  NULL,
    [VerifiedDate] datetime  NULL,
    [RejectedById] int  NULL,
    [RejectedReasonTypeId] int  NULL,
    [RejectedDate] datetime  NULL,
    [IsSystemGenerated] bit  NOT NULL
);
GO

-- Creating table 'TransactionEntryHistory'
CREATE TABLE [dbo].[TransactionEntryHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TransactionEntryId] int  NOT NULL,
    [TransactionNumber] varchar(15)  NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [FiscalYear] int  NOT NULL,
    [TransactionTypeId] int  NOT NULL,
    [CompanyId] int  NOT NULL,
    [PostingDate] datetime  NOT NULL,
    [Active] bit  NOT NULL,
    [Deleted] bit  NULL,
    [Accepted] bit  NOT NULL,
    [AcceptedById] int  NULL,
    [AcceptedDate] datetime  NULL,
    [PreviousId] int  NULL,
    [UpdateReason] varchar(250)  NULL,
    [CreatedById] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedById] int  NULL,
    [UpdatedDate] datetime  NULL,
    [Particulars] varchar(250)  NULL,
    [RejectedById] int  NULL,
    [RejectedReasonTypeId] int  NULL,
    [RejectedDate] datetime  NULL,
    [VerifiedById] int  NULL,
    [VerifiedDate] datetime  NULL,
    [IsSystemGenerated] bit  NOT NULL
);
GO

-- Creating table 'TransactionRejectReasonType'
CREATE TABLE [dbo].[TransactionRejectReasonType] (
    [Id] int  NOT NULL,
    [ReasonText] varchar(150)  NOT NULL
);
GO

-- Creating table 'TransactionStatus'
CREATE TABLE [dbo].[TransactionStatus] (
    [Id] int  NOT NULL,
    [Status] varchar(50)  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedBy] int  NOT NULL,
    [UpdatedDate] datetime  NOT NULL
);
GO

-- Creating table 'TransactionType'
CREATE TABLE [dbo].[TransactionType] (
    [Id] int  NOT NULL,
    [Type] varchar(50)  NOT NULL
);
GO

-- Creating table 'UnitType'
CREATE TABLE [dbo].[UnitType] (
    [Id] int  NOT NULL,
    [UnitTypeName] varchar(50)  NOT NULL,
    [TypeFlag] int  NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Password] varchar(max)  NULL,
    [FirstName] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Phone] varchar(50)  NOT NULL,
    [CompanyId] int  NOT NULL,
    [StatusId] int  NOT NULL,
    [Locked] bit  NOT NULL,
    [InvalidCount] int  NOT NULL,
    [PasswordKey] varchar(max)  NULL,
    [EmployeeId] int  NULL,
    [AspNetUserId] nvarchar(128)  NULL
);
GO

-- Creating table 'UserActivityLog'
CREATE TABLE [dbo].[UserActivityLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NULL,
    [UserEmail] varchar(30)  NOT NULL,
    [ControllerName] varchar(50)  NOT NULL,
    [ActionName] varchar(50)  NOT NULL,
    [Elapsed] decimal(18,4)  NOT NULL,
    [RequestUri] varchar(500)  NULL,
    [ReferrerUri] varchar(50)  NULL,
    [CreatedOn] datetime  NOT NULL
);
GO

-- Creating table 'UserLoginLog'
CREATE TABLE [dbo].[UserLoginLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserEmail] varchar(50)  NOT NULL,
    [LoggedOn] datetime  NOT NULL,
    [IsSucceeded] bit  NOT NULL,
    [ErrorMessage] varchar(50)  NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [AssignedBy] int  NULL,
    [AssignedOn] datetime  NULL
);
GO

-- Creating table 'UserRoleHistory'
CREATE TABLE [dbo].[UserRoleHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserRoleId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [AssignedBy] int  NULL,
    [AssignedOn] datetime  NULL
);
GO

-- Creating table 'UserStatus'
CREATE TABLE [dbo].[UserStatus] (
    [Id] int  NOT NULL,
    [Status] varchar(50)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'PolicyRouteResource'
CREATE TABLE [dbo].[PolicyRouteResource] (
    [Policy_Id] int  NOT NULL,
    [RouteResource_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AccountHead'
ALTER TABLE [dbo].[AccountHead]
ADD CONSTRAINT [PK_AccountHead]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountHeadOpening'
ALTER TABLE [dbo].[AccountHeadOpening]
ADD CONSTRAINT [PK_AccountHeadOpening]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountNature'
ALTER TABLE [dbo].[AccountNature]
ADD CONSTRAINT [PK_AccountNature]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountPrimaryHead'
ALTER TABLE [dbo].[AccountPrimaryHead]
ADD CONSTRAINT [PK_AccountPrimaryHead]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountSubHead'
ALTER TABLE [dbo].[AccountSubHead]
ADD CONSTRAINT [PK_AccountSubHead]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountType'
ALTER TABLE [dbo].[AccountType]
ADD CONSTRAINT [PK_AccountType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Area'
ALTER TABLE [dbo].[Area]
ADD CONSTRAINT [PK_Area]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AttachmentType'
ALTER TABLE [dbo].[AttachmentType]
ADD CONSTRAINT [PK_AttachmentType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BankInfo'
ALTER TABLE [dbo].[BankInfo]
ADD CONSTRAINT [PK_BankInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BatchProduct'
ALTER TABLE [dbo].[BatchProduct]
ADD CONSTRAINT [PK_BatchProduct]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BatchRequisition'
ALTER TABLE [dbo].[BatchRequisition]
ADD CONSTRAINT [PK_BatchRequisition]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BatchRequisitionDetail'
ALTER TABLE [dbo].[BatchRequisitionDetail]
ADD CONSTRAINT [PK_BatchRequisitionDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BatchRequisitionProductionEstimation'
ALTER TABLE [dbo].[BatchRequisitionProductionEstimation]
ADD CONSTRAINT [PK_BatchRequisitionProductionEstimation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientInfo'
ALTER TABLE [dbo].[ClientInfo]
ADD CONSTRAINT [PK_ClientInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [PK_Company]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyHolidays'
ALTER TABLE [dbo].[CompanyHolidays]
ADD CONSTRAINT [PK_CompanyHolidays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyLeave'
ALTER TABLE [dbo].[CompanyLeave]
ADD CONSTRAINT [PK_CompanyLeave]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanySalesTarget'
ALTER TABLE [dbo].[CompanySalesTarget]
ADD CONSTRAINT [PK_CompanySalesTarget]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ControlType'
ALTER TABLE [dbo].[ControlType]
ADD CONSTRAINT [PK_ControlType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CrashedGood'
ALTER TABLE [dbo].[CrashedGood]
ADD CONSTRAINT [PK_CrashedGood]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CurrentProductStock'
ALTER TABLE [dbo].[CurrentProductStock]
ADD CONSTRAINT [PK_CurrentProductStock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [PK_Customer]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerAttachment'
ALTER TABLE [dbo].[CustomerAttachment]
ADD CONSTRAINT [PK_CustomerAttachment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerHistory'
ALTER TABLE [dbo].[CustomerHistory]
ADD CONSTRAINT [PK_CustomerHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerSalesCredit'
ALTER TABLE [dbo].[CustomerSalesCredit]
ADD CONSTRAINT [PK_CustomerSalesCredit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerSalesCreditHistory'
ALTER TABLE [dbo].[CustomerSalesCreditHistory]
ADD CONSTRAINT [PK_CustomerSalesCreditHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerStatus'
ALTER TABLE [dbo].[CustomerStatus]
ADD CONSTRAINT [PK_CustomerStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerTransaction'
ALTER TABLE [dbo].[CustomerTransaction]
ADD CONSTRAINT [PK_CustomerTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerTransactionDetail'
ALTER TABLE [dbo].[CustomerTransactionDetail]
ADD CONSTRAINT [PK_CustomerTransactionDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerTransactionMonthly'
ALTER TABLE [dbo].[CustomerTransactionMonthly]
ADD CONSTRAINT [PK_CustomerTransactionMonthly]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerType'
ALTER TABLE [dbo].[CustomerType]
ADD CONSTRAINT [PK_CustomerType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DeliveryQuantity'
ALTER TABLE [dbo].[DeliveryQuantity]
ADD CONSTRAINT [PK_DeliveryQuantity]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DeliveryQuantityDetail'
ALTER TABLE [dbo].[DeliveryQuantityDetail]
ADD CONSTRAINT [PK_DeliveryQuantityDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [PK_DemandOrder]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrderDetail'
ALTER TABLE [dbo].[DemandOrderDetail]
ADD CONSTRAINT [PK_DemandOrderDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrderDiscountTransaction'
ALTER TABLE [dbo].[DemandOrderDiscountTransaction]
ADD CONSTRAINT [PK_DemandOrderDiscountTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrderDiscountType'
ALTER TABLE [dbo].[DemandOrderDiscountType]
ADD CONSTRAINT [PK_DemandOrderDiscountType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrderStatus'
ALTER TABLE [dbo].[DemandOrderStatus]
ADD CONSTRAINT [PK_DemandOrderStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrderTransaction'
ALTER TABLE [dbo].[DemandOrderTransaction]
ADD CONSTRAINT [PK_DemandOrderTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DemandOrderType'
ALTER TABLE [dbo].[DemandOrderType]
ADD CONSTRAINT [PK_DemandOrderType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Department'
ALTER TABLE [dbo].[Department]
ADD CONSTRAINT [PK_Department]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Designation'
ALTER TABLE [dbo].[Designation]
ADD CONSTRAINT [PK_Designation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiscountType'
ALTER TABLE [dbo].[DiscountType]
ADD CONSTRAINT [PK_DiscountType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'District'
ALTER TABLE [dbo].[District]
ADD CONSTRAINT [PK_District]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentRenewalCategory'
ALTER TABLE [dbo].[DocumentRenewalCategory]
ADD CONSTRAINT [PK_DocumentRenewalCategory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentStatus'
ALTER TABLE [dbo].[DocumentStatus]
ADD CONSTRAINT [PK_DocumentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentType'
ALTER TABLE [dbo].[DocumentType]
ADD CONSTRAINT [PK_DocumentType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [PK_Employee]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [PK_EmployeeHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeLeave'
ALTER TABLE [dbo].[EmployeeLeave]
ADD CONSTRAINT [PK_EmployeeLeave]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSalesLocation'
ALTER TABLE [dbo].[EmployeeSalesLocation]
ADD CONSTRAINT [PK_EmployeeSalesLocation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSalesLocationHistory'
ALTER TABLE [dbo].[EmployeeSalesLocationHistory]
ADD CONSTRAINT [PK_EmployeeSalesLocationHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSalesTargetMonthly'
ALTER TABLE [dbo].[EmployeeSalesTargetMonthly]
ADD CONSTRAINT [PK_EmployeeSalesTargetMonthly]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeType'
ALTER TABLE [dbo].[EmployeeType]
ADD CONSTRAINT [PK_EmployeeType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntityType'
ALTER TABLE [dbo].[EntityType]
ADD CONSTRAINT [PK_EntityType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FileType'
ALTER TABLE [dbo].[FileType]
ADD CONSTRAINT [PK_FileType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FinishedGood'
ALTER TABLE [dbo].[FinishedGood]
ADD CONSTRAINT [PK_FinishedGood]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FinishedGoodOpening'
ALTER TABLE [dbo].[FinishedGoodOpening]
ADD CONSTRAINT [PK_FinishedGoodOpening]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FiscalYear'
ALTER TABLE [dbo].[FiscalYear]
ADD CONSTRAINT [PK_FiscalYear]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FloorStoreRawMaterial'
ALTER TABLE [dbo].[FloorStoreRawMaterial]
ADD CONSTRAINT [PK_FloorStoreRawMaterial]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Group'
ALTER TABLE [dbo].[Group]
ADD CONSTRAINT [PK_Group]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [PK_Invoice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceDetail'
ALTER TABLE [dbo].[InvoiceDetail]
ADD CONSTRAINT [PK_InvoiceDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceReturn'
ALTER TABLE [dbo].[InvoiceReturn]
ADD CONSTRAINT [PK_InvoiceReturn]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceReturnDetail'
ALTER TABLE [dbo].[InvoiceReturnDetail]
ADD CONSTRAINT [PK_InvoiceReturnDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceTransaction'
ALTER TABLE [dbo].[InvoiceTransaction]
ADD CONSTRAINT [PK_InvoiceTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LeaveCategory'
ALTER TABLE [dbo].[LeaveCategory]
ADD CONSTRAINT [PK_LeaveCategory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [PK_LegalDocument]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [PK_LegalDocumentHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Log'
ALTER TABLE [dbo].[Log]
ADD CONSTRAINT [PK_Log]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MasterSetting'
ALTER TABLE [dbo].[MasterSetting]
ADD CONSTRAINT [PK_MasterSetting]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MonthlyProcessing'
ALTER TABLE [dbo].[MonthlyProcessing]
ADD CONSTRAINT [PK_MonthlyProcessing]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotificationSetting'
ALTER TABLE [dbo].[NotificationSetting]
ADD CONSTRAINT [PK_NotificationSetting]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotificationType'
ALTER TABLE [dbo].[NotificationType]
ADD CONSTRAINT [PK_NotificationType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentStatus'
ALTER TABLE [dbo].[PaymentStatus]
ADD CONSTRAINT [PK_PaymentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PoliceStation'
ALTER TABLE [dbo].[PoliceStation]
ADD CONSTRAINT [PK_PoliceStation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Policy'
ALTER TABLE [dbo].[Policy]
ADD CONSTRAINT [PK_Policy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PostOffice'
ALTER TABLE [dbo].[PostOffice]
ADD CONSTRAINT [PK_PostOffice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProcessingType'
ALTER TABLE [dbo].[ProcessingType]
ADD CONSTRAINT [PK_ProcessingType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [PK_Product]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductDamageStatusType'
ALTER TABLE [dbo].[ProductDamageStatusType]
ADD CONSTRAINT [PK_ProductDamageStatusType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductHistory'
ALTER TABLE [dbo].[ProductHistory]
ADD CONSTRAINT [PK_ProductHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductionForecastMonthly'
ALTER TABLE [dbo].[ProductionForecastMonthly]
ADD CONSTRAINT [PK_ProductionForecastMonthly]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductionGroup'
ALTER TABLE [dbo].[ProductionGroup]
ADD CONSTRAINT [PK_ProductionGroup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductStandardType'
ALTER TABLE [dbo].[ProductStandardType]
ADD CONSTRAINT [PK_ProductStandardType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductType'
ALTER TABLE [dbo].[ProductType]
ADD CONSTRAINT [PK_ProductType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductTypeGroup'
ALTER TABLE [dbo].[ProductTypeGroup]
ADD CONSTRAINT [PK_ProductTypeGroup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [PK_PurchaseOrder]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseOrderDetail'
ALTER TABLE [dbo].[PurchaseOrderDetail]
ADD CONSTRAINT [PK_PurchaseOrderDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseOrderStatus'
ALTER TABLE [dbo].[PurchaseOrderStatus]
ADD CONSTRAINT [PK_PurchaseOrderStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseOrderTransaction'
ALTER TABLE [dbo].[PurchaseOrderTransaction]
ADD CONSTRAINT [PK_PurchaseOrderTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RawMaterialType'
ALTER TABLE [dbo].[RawMaterialType]
ADD CONSTRAINT [PK_RawMaterialType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReferenceColumn'
ALTER TABLE [dbo].[ReferenceColumn]
ADD CONSTRAINT [PK_ReferenceColumn]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReferenceTable'
ALTER TABLE [dbo].[ReferenceTable]
ADD CONSTRAINT [PK_ReferenceTable]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RejectedReasonType'
ALTER TABLE [dbo].[RejectedReasonType]
ADD CONSTRAINT [PK_RejectedReasonType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Role'
ALTER TABLE [dbo].[Role]
ADD CONSTRAINT [PK_Role]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolePolicy'
ALTER TABLE [dbo].[RolePolicy]
ADD CONSTRAINT [PK_RolePolicy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolePolicyHistory'
ALTER TABLE [dbo].[RolePolicyHistory]
ADD CONSTRAINT [PK_RolePolicyHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RouteResource'
ALTER TABLE [dbo].[RouteResource]
ADD CONSTRAINT [PK_RouteResource]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesArea'
ALTER TABLE [dbo].[SalesArea]
ADD CONSTRAINT [PK_SalesArea]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesBase'
ALTER TABLE [dbo].[SalesBase]
ADD CONSTRAINT [PK_SalesBase]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesDivision'
ALTER TABLE [dbo].[SalesDivision]
ADD CONSTRAINT [PK_SalesDivision]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaleType'
ALTER TABLE [dbo].[SaleType]
ADD CONSTRAINT [PK_SaleType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StoreRawMaterial'
ALTER TABLE [dbo].[StoreRawMaterial]
ADD CONSTRAINT [PK_StoreRawMaterial]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StoreRawMaterialOpening'
ALTER TABLE [dbo].[StoreRawMaterialOpening]
ADD CONSTRAINT [PK_StoreRawMaterialOpening]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Supplier'
ALTER TABLE [dbo].[Supplier]
ADD CONSTRAINT [PK_Supplier]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemWarning'
ALTER TABLE [dbo].[SystemWarning]
ADD CONSTRAINT [PK_SystemWarning]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemWarningHistory'
ALTER TABLE [dbo].[SystemWarningHistory]
ADD CONSTRAINT [PK_SystemWarningHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemWarningType'
ALTER TABLE [dbo].[SystemWarningType]
ADD CONSTRAINT [PK_SystemWarningType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionDetail'
ALTER TABLE [dbo].[TransactionDetail]
ADD CONSTRAINT [PK_TransactionDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionDetailHistory'
ALTER TABLE [dbo].[TransactionDetailHistory]
ADD CONSTRAINT [PK_TransactionDetailHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionEntry'
ALTER TABLE [dbo].[TransactionEntry]
ADD CONSTRAINT [PK_TransactionEntry]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionEntryHistory'
ALTER TABLE [dbo].[TransactionEntryHistory]
ADD CONSTRAINT [PK_TransactionEntryHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionRejectReasonType'
ALTER TABLE [dbo].[TransactionRejectReasonType]
ADD CONSTRAINT [PK_TransactionRejectReasonType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionStatus'
ALTER TABLE [dbo].[TransactionStatus]
ADD CONSTRAINT [PK_TransactionStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionType'
ALTER TABLE [dbo].[TransactionType]
ADD CONSTRAINT [PK_TransactionType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitType'
ALTER TABLE [dbo].[UnitType]
ADD CONSTRAINT [PK_UnitType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserActivityLog'
ALTER TABLE [dbo].[UserActivityLog]
ADD CONSTRAINT [PK_UserActivityLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserLoginLog'
ALTER TABLE [dbo].[UserLoginLog]
ADD CONSTRAINT [PK_UserLoginLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [PK_UserRole]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoleHistory'
ALTER TABLE [dbo].[UserRoleHistory]
ADD CONSTRAINT [PK_UserRoleHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserStatus'
ALTER TABLE [dbo].[UserStatus]
ADD CONSTRAINT [PK_UserStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- Creating primary key on [Policy_Id], [RouteResource_Id] in table 'PolicyRouteResource'
ALTER TABLE [dbo].[PolicyRouteResource]
ADD CONSTRAINT [PK_PolicyRouteResource]
    PRIMARY KEY CLUSTERED ([Policy_Id], [RouteResource_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AccountHeadId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [FK_Customer_AccountHead_Foreign_Key]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_AccountHead_Foreign_Key'
CREATE INDEX [IX_FK_Customer_AccountHead_Foreign_Key]
ON [dbo].[Customer]
    ([AccountHeadId]);
GO

-- Creating foreign key on [CompanyId] in table 'AccountHead'
ALTER TABLE [dbo].[AccountHead]
ADD CONSTRAINT [FK_AccountHead_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountHead_Company'
CREATE INDEX [IX_FK_AccountHead_Company]
ON [dbo].[AccountHead]
    ([CompanyId]);
GO

-- Creating foreign key on [AccountSubHeadId] in table 'AccountHead'
ALTER TABLE [dbo].[AccountHead]
ADD CONSTRAINT [FK_dbo_AccountHead_dbo_AccountSubHead_AccountSubHeadId]
    FOREIGN KEY ([AccountSubHeadId])
    REFERENCES [dbo].[AccountSubHead]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountHead_dbo_AccountSubHead_AccountSubHeadId'
CREATE INDEX [IX_FK_dbo_AccountHead_dbo_AccountSubHead_AccountSubHeadId]
ON [dbo].[AccountHead]
    ([AccountSubHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'AccountHeadOpening'
ALTER TABLE [dbo].[AccountHeadOpening]
ADD CONSTRAINT [FK_dbo_AccountHeadOpening_dbo_AccountHead_AccountHeadId]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountHeadOpening_dbo_AccountHead_AccountHeadId'
CREATE INDEX [IX_FK_dbo_AccountHeadOpening_dbo_AccountHead_AccountHeadId]
ON [dbo].[AccountHeadOpening]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'ClientInfo'
ALTER TABLE [dbo].[ClientInfo]
ADD CONSTRAINT [FK_dbo_ClientInfo_dbo_AccountHead_AccountHeadId]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ClientInfo_dbo_AccountHead_AccountHeadId'
CREATE INDEX [IX_FK_dbo_ClientInfo_dbo_AccountHead_AccountHeadId]
ON [dbo].[ClientInfo]
    ([AccountHeadId]);
GO

-- Creating foreign key on [CashBankAccountHeadId] in table 'CustomerTransaction'
ALTER TABLE [dbo].[CustomerTransaction]
ADD CONSTRAINT [FK_dbo_CustomerTransaction_BankInfo]
    FOREIGN KEY ([CashBankAccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransaction_BankInfo'
CREATE INDEX [IX_FK_dbo_CustomerTransaction_BankInfo]
ON [dbo].[CustomerTransaction]
    ([CashBankAccountHeadId]);
GO

-- Creating foreign key on [BankAccountHeadId] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [FK_dbo_PurchaseOrder_AccountHead_Bank]
    FOREIGN KEY ([BankAccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_PurchaseOrder_AccountHead_Bank'
CREATE INDEX [IX_FK_dbo_PurchaseOrder_AccountHead_Bank]
ON [dbo].[PurchaseOrder]
    ([BankAccountHeadId]);
GO

-- Creating foreign key on [CashAccountHeadId] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [FK_dbo_PurchaseOrder_AccountHead_Cash]
    FOREIGN KEY ([CashAccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_PurchaseOrder_AccountHead_Cash'
CREATE INDEX [IX_FK_dbo_PurchaseOrder_AccountHead_Cash]
ON [dbo].[PurchaseOrder]
    ([CashAccountHeadId]);
GO

-- Creating foreign key on [CashBankAccountHeadId] in table 'PurchaseOrderTransaction'
ALTER TABLE [dbo].[PurchaseOrderTransaction]
ADD CONSTRAINT [FK_dbo_PurchaseOrderTransaction_AccountHead]
    FOREIGN KEY ([CashBankAccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_PurchaseOrderTransaction_AccountHead'
CREATE INDEX [IX_FK_dbo_PurchaseOrderTransaction_AccountHead]
ON [dbo].[PurchaseOrderTransaction]
    ([CashBankAccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'TransactionDetail'
ALTER TABLE [dbo].[TransactionDetail]
ADD CONSTRAINT [FK_dbo_TransactionDetail_dbo_AccountHead_AccountHeadId]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionDetail_dbo_AccountHead_AccountHeadId'
CREATE INDEX [IX_FK_dbo_TransactionDetail_dbo_AccountHead_AccountHeadId]
ON [dbo].[TransactionDetail]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'TransactionDetailHistory'
ALTER TABLE [dbo].[TransactionDetailHistory]
ADD CONSTRAINT [FK_dbo_TransactionDetailHistory_dbo_AccountHead_AccountHeadId]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionDetailHistory_dbo_AccountHead_AccountHeadId'
CREATE INDEX [IX_FK_dbo_TransactionDetailHistory_dbo_AccountHead_AccountHeadId]
ON [dbo].[TransactionDetailHistory]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_Product_AccountHead]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_AccountHead'
CREATE INDEX [IX_FK_Product_AccountHead]
ON [dbo].[Product]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'ProductHistory'
ALTER TABLE [dbo].[ProductHistory]
ADD CONSTRAINT [FK_ProductHistory_AccountHead]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductHistory_AccountHead'
CREATE INDEX [IX_FK_ProductHistory_AccountHead]
ON [dbo].[ProductHistory]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'RawMaterialType'
ALTER TABLE [dbo].[RawMaterialType]
ADD CONSTRAINT [FK_RawMaterialType_AccountHead]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RawMaterialType_AccountHead'
CREATE INDEX [IX_FK_RawMaterialType_AccountHead]
ON [dbo].[RawMaterialType]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'Supplier'
ALTER TABLE [dbo].[Supplier]
ADD CONSTRAINT [FK_Supplier_AccountHead]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Supplier_AccountHead'
CREATE INDEX [IX_FK_Supplier_AccountHead]
ON [dbo].[Supplier]
    ([AccountHeadId]);
GO

-- Creating foreign key on [AccountHeadId] in table 'CustomerHistory'
ALTER TABLE [dbo].[CustomerHistory]
ADD CONSTRAINT [FK_HistoryCustomer_AccountHead_Foreign_Key]
    FOREIGN KEY ([AccountHeadId])
    REFERENCES [dbo].[AccountHead]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryCustomer_AccountHead_Foreign_Key'
CREATE INDEX [IX_FK_HistoryCustomer_AccountHead_Foreign_Key]
ON [dbo].[CustomerHistory]
    ([AccountHeadId]);
GO

-- Creating foreign key on [CompanyId] in table 'AccountHeadOpening'
ALTER TABLE [dbo].[AccountHeadOpening]
ADD CONSTRAINT [FK_AccountHeadOpening_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountHeadOpening_Company'
CREATE INDEX [IX_FK_AccountHeadOpening_Company]
ON [dbo].[AccountHeadOpening]
    ([CompanyId]);
GO

-- Creating foreign key on [AccountNatureId] in table 'AccountType'
ALTER TABLE [dbo].[AccountType]
ADD CONSTRAINT [FK_dbo_AccountType_dbo_AccountNature_AccountNatureId]
    FOREIGN KEY ([AccountNatureId])
    REFERENCES [dbo].[AccountNature]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountType_dbo_AccountNature_AccountNatureId'
CREATE INDEX [IX_FK_dbo_AccountType_dbo_AccountNature_AccountNatureId]
ON [dbo].[AccountType]
    ([AccountNatureId]);
GO

-- Creating foreign key on [CompanyId] in table 'AccountPrimaryHead'
ALTER TABLE [dbo].[AccountPrimaryHead]
ADD CONSTRAINT [FK_AccountPrimaryHead_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountPrimaryHead_Company'
CREATE INDEX [IX_FK_AccountPrimaryHead_Company]
ON [dbo].[AccountPrimaryHead]
    ([CompanyId]);
GO

-- Creating foreign key on [AccountTypeId] in table 'AccountPrimaryHead'
ALTER TABLE [dbo].[AccountPrimaryHead]
ADD CONSTRAINT [FK_dbo_AccountPrimaryHead_dbo_AccountType_AccountTypeId]
    FOREIGN KEY ([AccountTypeId])
    REFERENCES [dbo].[AccountType]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountPrimaryHead_dbo_AccountType_AccountTypeId'
CREATE INDEX [IX_FK_dbo_AccountPrimaryHead_dbo_AccountType_AccountTypeId]
ON [dbo].[AccountPrimaryHead]
    ([AccountTypeId]);
GO

-- Creating foreign key on [AccountPrimaryHeadId] in table 'AccountSubHead'
ALTER TABLE [dbo].[AccountSubHead]
ADD CONSTRAINT [FK_dbo_AccountSubHead_dbo_AccountPrimaryHead_AccountPrimaryHeadId]
    FOREIGN KEY ([AccountPrimaryHeadId])
    REFERENCES [dbo].[AccountPrimaryHead]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountSubHead_dbo_AccountPrimaryHead_AccountPrimaryHeadId'
CREATE INDEX [IX_FK_dbo_AccountSubHead_dbo_AccountPrimaryHead_AccountPrimaryHeadId]
ON [dbo].[AccountSubHead]
    ([AccountPrimaryHeadId]);
GO

-- Creating foreign key on [CompanyId] in table 'AccountSubHead'
ALTER TABLE [dbo].[AccountSubHead]
ADD CONSTRAINT [FK_AccountSubHead_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountSubHead_Company'
CREATE INDEX [IX_FK_AccountSubHead_Company]
ON [dbo].[AccountSubHead]
    ([CompanyId]);
GO

-- Creating foreign key on [AreaId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [FK_Customer_Area_Foreign_Key]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Area]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_Area_Foreign_Key'
CREATE INDEX [IX_FK_Customer_Area_Foreign_Key]
ON [dbo].[Customer]
    ([AreaId]);
GO

-- Creating foreign key on [AreaId] in table 'CustomerHistory'
ALTER TABLE [dbo].[CustomerHistory]
ADD CONSTRAINT [FK_HistoryCustomer_Area_Foreign_Key]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Area]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryCustomer_Area_Foreign_Key'
CREATE INDEX [IX_FK_HistoryCustomer_Area_Foreign_Key]
ON [dbo].[CustomerHistory]
    ([AreaId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [AspNetUserId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [FK_User_AspNetUsers]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_AspNetUsers'
CREATE INDEX [IX_FK_User_AspNetUsers]
ON [dbo].[User]
    ([AspNetUserId]);
GO

-- Creating foreign key on [AttachmentTypeId] in table 'CustomerAttachment'
ALTER TABLE [dbo].[CustomerAttachment]
ADD CONSTRAINT [FK_CustomerAttachment_AttachmentType]
    FOREIGN KEY ([AttachmentTypeId])
    REFERENCES [dbo].[AttachmentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAttachment_AttachmentType'
CREATE INDEX [IX_FK_CustomerAttachment_AttachmentType]
ON [dbo].[CustomerAttachment]
    ([AttachmentTypeId]);
GO

-- Creating foreign key on [CompanyId] in table 'BankInfo'
ALTER TABLE [dbo].[BankInfo]
ADD CONSTRAINT [FK_dbo_BankInfo_dbo_Company_CompanyId]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_BankInfo_dbo_Company_CompanyId'
CREATE INDEX [IX_FK_dbo_BankInfo_dbo_Company_CompanyId]
ON [dbo].[BankInfo]
    ([CompanyId]);
GO

-- Creating foreign key on [BatchRequisitionId] in table 'BatchProduct'
ALTER TABLE [dbo].[BatchProduct]
ADD CONSTRAINT [FK_BatchProduct_BatchRequisition_Foreign_Key]
    FOREIGN KEY ([BatchRequisitionId])
    REFERENCES [dbo].[BatchRequisition]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchProduct_BatchRequisition_Foreign_Key'
CREATE INDEX [IX_FK_BatchProduct_BatchRequisition_Foreign_Key]
ON [dbo].[BatchProduct]
    ([BatchRequisitionId]);
GO

-- Creating foreign key on [ProductId] in table 'BatchProduct'
ALTER TABLE [dbo].[BatchProduct]
ADD CONSTRAINT [FK_BatchProduct_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchProduct_Product_Foreign_Key'
CREATE INDEX [IX_FK_BatchProduct_Product_Foreign_Key]
ON [dbo].[BatchProduct]
    ([ProductId]);
GO

-- Creating foreign key on [BatchRequisitionId] in table 'BatchRequisitionDetail'
ALTER TABLE [dbo].[BatchRequisitionDetail]
ADD CONSTRAINT [FK_BatchRequisitionDetail_BatchRequisition_Foreign_Key]
    FOREIGN KEY ([BatchRequisitionId])
    REFERENCES [dbo].[BatchRequisition]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchRequisitionDetail_BatchRequisition_Foreign_Key'
CREATE INDEX [IX_FK_BatchRequisitionDetail_BatchRequisition_Foreign_Key]
ON [dbo].[BatchRequisitionDetail]
    ([BatchRequisitionId]);
GO

-- Creating foreign key on [ProductionGroupId] in table 'BatchRequisition'
ALTER TABLE [dbo].[BatchRequisition]
ADD CONSTRAINT [FK_BatchRequisition_ToProductionGroup]
    FOREIGN KEY ([ProductionGroupId])
    REFERENCES [dbo].[ProductionGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchRequisition_ToProductionGroup'
CREATE INDEX [IX_FK_BatchRequisition_ToProductionGroup]
ON [dbo].[BatchRequisition]
    ([ProductionGroupId]);
GO

-- Creating foreign key on [CreatedBy] in table 'BatchRequisition'
ALTER TABLE [dbo].[BatchRequisition]
ADD CONSTRAINT [FK_BatchRequisition_User]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchRequisition_User'
CREATE INDEX [IX_FK_BatchRequisition_User]
ON [dbo].[BatchRequisition]
    ([CreatedBy]);
GO

-- Creating foreign key on [BatchRequisitionId] in table 'BatchRequisitionProductionEstimation'
ALTER TABLE [dbo].[BatchRequisitionProductionEstimation]
ADD CONSTRAINT [FK_BatchRequisitionProductionEstimation_BatchRequisition]
    FOREIGN KEY ([BatchRequisitionId])
    REFERENCES [dbo].[BatchRequisition]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchRequisitionProductionEstimation_BatchRequisition'
CREATE INDEX [IX_FK_BatchRequisitionProductionEstimation_BatchRequisition]
ON [dbo].[BatchRequisitionProductionEstimation]
    ([BatchRequisitionId]);
GO

-- Creating foreign key on [BatchRequisitionId] in table 'FloorStoreRawMaterial'
ALTER TABLE [dbo].[FloorStoreRawMaterial]
ADD CONSTRAINT [FK_FloorStoreRawMaterial_BatchRequisition]
    FOREIGN KEY ([BatchRequisitionId])
    REFERENCES [dbo].[BatchRequisition]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FloorStoreRawMaterial_BatchRequisition'
CREATE INDEX [IX_FK_FloorStoreRawMaterial_BatchRequisition]
ON [dbo].[FloorStoreRawMaterial]
    ([BatchRequisitionId]);
GO

-- Creating foreign key on [RawMaterialTypeId] in table 'BatchRequisitionDetail'
ALTER TABLE [dbo].[BatchRequisitionDetail]
ADD CONSTRAINT [FK_BatchRequisitionDetail_RawMaterialType_Foreign_Key]
    FOREIGN KEY ([RawMaterialTypeId])
    REFERENCES [dbo].[RawMaterialType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchRequisitionDetail_RawMaterialType_Foreign_Key'
CREATE INDEX [IX_FK_BatchRequisitionDetail_RawMaterialType_Foreign_Key]
ON [dbo].[BatchRequisitionDetail]
    ([RawMaterialTypeId]);
GO

-- Creating foreign key on [ProductId] in table 'BatchRequisitionProductionEstimation'
ALTER TABLE [dbo].[BatchRequisitionProductionEstimation]
ADD CONSTRAINT [FK_BatchRequisitionProductionEstimation_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BatchRequisitionProductionEstimation_Product'
CREATE INDEX [IX_FK_BatchRequisitionProductionEstimation_Product]
ON [dbo].[BatchRequisitionProductionEstimation]
    ([ProductId]);
GO

-- Creating foreign key on [Company_Id] in table 'ClientInfo'
ALTER TABLE [dbo].[ClientInfo]
ADD CONSTRAINT [FK_dbo_ClientInfo_dbo_Company_Company_Id]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ClientInfo_dbo_Company_Company_Id'
CREATE INDEX [IX_FK_dbo_ClientInfo_dbo_Company_Company_Id]
ON [dbo].[ClientInfo]
    ([Company_Id]);
GO

-- Creating foreign key on [CompanyId] in table 'CompanyHolidays'
ALTER TABLE [dbo].[CompanyHolidays]
ADD CONSTRAINT [FK_CompanyHolidays_Company_Foreign_Key]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyHolidays_Company_Foreign_Key'
CREATE INDEX [IX_FK_CompanyHolidays_Company_Foreign_Key]
ON [dbo].[CompanyHolidays]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'Department'
ALTER TABLE [dbo].[Department]
ADD CONSTRAINT [FK_Department_Company_Foreign_Key]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Department_Company_Foreign_Key'
CREATE INDEX [IX_FK_Department_Company_Foreign_Key]
ON [dbo].[Department]
    ([CompanyId]);
GO

-- Creating foreign key on [GroupId] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [FK_dbo_Company_dbo_Group_GroupId]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Group]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Company_dbo_Group_GroupId'
CREATE INDEX [IX_FK_dbo_Company_dbo_Group_GroupId]
ON [dbo].[Company]
    ([GroupId]);
GO

-- Creating foreign key on [CompanyId] in table 'TransactionEntry'
ALTER TABLE [dbo].[TransactionEntry]
ADD CONSTRAINT [FK_dbo_TransactionEntry_dbo_Company_CompanyId]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionEntry_dbo_Company_CompanyId'
CREATE INDEX [IX_FK_dbo_TransactionEntry_dbo_Company_CompanyId]
ON [dbo].[TransactionEntry]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'TransactionEntryHistory'
ALTER TABLE [dbo].[TransactionEntryHistory]
ADD CONSTRAINT [FK_dbo_TransactionEntryHistory_dbo_Company_CompanyId]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionEntryHistory_dbo_Company_CompanyId'
CREATE INDEX [IX_FK_dbo_TransactionEntryHistory_dbo_Company_CompanyId]
ON [dbo].[TransactionEntryHistory]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [FK_dbo_User_dbo_Company_CompanyId]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_User_dbo_Company_CompanyId'
CREATE INDEX [IX_FK_dbo_User_dbo_Company_CompanyId]
ON [dbo].[User]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_Company'
CREATE INDEX [IX_FK_Employee_Company]
ON [dbo].[Employee]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_Company'
CREATE INDEX [IX_FK_EmployeeHistory_Company]
ON [dbo].[EmployeeHistory]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [FK_LegalDocument_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocument_Company'
CREATE INDEX [IX_FK_LegalDocument_Company]
ON [dbo].[LegalDocument]
    ([CompanyId]);
GO

-- Creating foreign key on [CompanyId] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [FK_LegalDocumentHistory_Company]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Company]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocumentHistory_Company'
CREATE INDEX [IX_FK_LegalDocumentHistory_Company]
ON [dbo].[LegalDocumentHistory]
    ([CompanyId]);
GO

-- Creating foreign key on [LeaveCategoryId] in table 'CompanyLeave'
ALTER TABLE [dbo].[CompanyLeave]
ADD CONSTRAINT [FK_CompanyLeave_LeaveCategory]
    FOREIGN KEY ([LeaveCategoryId])
    REFERENCES [dbo].[LeaveCategory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyLeave_LeaveCategory'
CREATE INDEX [IX_FK_CompanyLeave_LeaveCategory]
ON [dbo].[CompanyLeave]
    ([LeaveCategoryId]);
GO

-- Creating foreign key on [CustomerId] in table 'CrashedGood'
ALTER TABLE [dbo].[CrashedGood]
ADD CONSTRAINT [FK_CrashedGood_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CrashedGood_Customer'
CREATE INDEX [IX_FK_CrashedGood_Customer]
ON [dbo].[CrashedGood]
    ([CustomerId]);
GO

-- Creating foreign key on [GroupId] in table 'CrashedGood'
ALTER TABLE [dbo].[CrashedGood]
ADD CONSTRAINT [FK_CrashedGood_Group]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Group]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CrashedGood_Group'
CREATE INDEX [IX_FK_CrashedGood_Group]
ON [dbo].[CrashedGood]
    ([GroupId]);
GO

-- Creating foreign key on [InvoiceId] in table 'CrashedGood'
ALTER TABLE [dbo].[CrashedGood]
ADD CONSTRAINT [FK_CrashedGood_Invoice]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CrashedGood_Invoice'
CREATE INDEX [IX_FK_CrashedGood_Invoice]
ON [dbo].[CrashedGood]
    ([InvoiceId]);
GO

-- Creating foreign key on [DamageStatusId] in table 'CrashedGood'
ALTER TABLE [dbo].[CrashedGood]
ADD CONSTRAINT [FK_CrashedGood_ProductDamageStatusType]
    FOREIGN KEY ([DamageStatusId])
    REFERENCES [dbo].[ProductDamageStatusType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CrashedGood_ProductDamageStatusType'
CREATE INDEX [IX_FK_CrashedGood_ProductDamageStatusType]
ON [dbo].[CrashedGood]
    ([DamageStatusId]);
GO

-- Creating foreign key on [ProductId] in table 'CurrentProductStock'
ALTER TABLE [dbo].[CurrentProductStock]
ADD CONSTRAINT [FK_CurrentProductStock_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrentProductStock_Product'
CREATE INDEX [IX_FK_CurrentProductStock_Product]
ON [dbo].[CurrentProductStock]
    ([ProductId]);
GO

-- Creating foreign key on [CustomerStatusId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [FK_Customer_CustomerStatus_Foreign_Key]
    FOREIGN KEY ([CustomerStatusId])
    REFERENCES [dbo].[CustomerStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_CustomerStatus_Foreign_Key'
CREATE INDEX [IX_FK_Customer_CustomerStatus_Foreign_Key]
ON [dbo].[Customer]
    ([CustomerStatusId]);
GO

-- Creating foreign key on [CustomerTypeId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [FK_Customer_CustomerType_Foreign_Key]
    FOREIGN KEY ([CustomerTypeId])
    REFERENCES [dbo].[CustomerType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_CustomerType_Foreign_Key'
CREATE INDEX [IX_FK_Customer_CustomerType_Foreign_Key]
ON [dbo].[Customer]
    ([CustomerTypeId]);
GO

-- Creating foreign key on [PostOfficeId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [FK_Customer_PostOffice_Foreign_Key]
    FOREIGN KEY ([PostOfficeId])
    REFERENCES [dbo].[PostOffice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_PostOffice_Foreign_Key'
CREATE INDEX [IX_FK_Customer_PostOffice_Foreign_Key]
ON [dbo].[Customer]
    ([PostOfficeId]);
GO

-- Creating foreign key on [EmployeeId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [FK_Customer_Employee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_Employee'
CREATE INDEX [IX_FK_Customer_Employee]
ON [dbo].[Customer]
    ([EmployeeId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerAttachment'
ALTER TABLE [dbo].[CustomerAttachment]
ADD CONSTRAINT [FK_CustomerAttachment_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAttachment_Customer'
CREATE INDEX [IX_FK_CustomerAttachment_Customer]
ON [dbo].[CustomerAttachment]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerSalesCredit'
ALTER TABLE [dbo].[CustomerSalesCredit]
ADD CONSTRAINT [FK_CustomerSalesCredit_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerSalesCredit_Customer'
CREATE INDEX [IX_FK_CustomerSalesCredit_Customer]
ON [dbo].[CustomerSalesCredit]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerSalesCreditHistory'
ALTER TABLE [dbo].[CustomerSalesCreditHistory]
ADD CONSTRAINT [FK_CustomerSalesCreditHistory_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerSalesCreditHistory_Customer'
CREATE INDEX [IX_FK_CustomerSalesCreditHistory_Customer]
ON [dbo].[CustomerSalesCreditHistory]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerTransactionDetail'
ALTER TABLE [dbo].[CustomerTransactionDetail]
ADD CONSTRAINT [FK_dbo_CustomerTransactionDetail_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransactionDetail_Customer'
CREATE INDEX [IX_FK_dbo_CustomerTransactionDetail_Customer]
ON [dbo].[CustomerTransactionDetail]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_Customer'
CREATE INDEX [IX_FK_DemandOrder_Customer]
ON [dbo].[DemandOrder]
    ([CustomerId]);
GO

-- Creating foreign key on [FileTypeId] in table 'CustomerAttachment'
ALTER TABLE [dbo].[CustomerAttachment]
ADD CONSTRAINT [FK_CustomerAttachment_FileType]
    FOREIGN KEY ([FileTypeId])
    REFERENCES [dbo].[FileType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAttachment_FileType'
CREATE INDEX [IX_FK_CustomerAttachment_FileType]
ON [dbo].[CustomerAttachment]
    ([FileTypeId]);
GO

-- Creating foreign key on [CustomerStatusId] in table 'CustomerHistory'
ALTER TABLE [dbo].[CustomerHistory]
ADD CONSTRAINT [FK_HistoryCustomer_CustomerStatus_Foreign_Key]
    FOREIGN KEY ([CustomerStatusId])
    REFERENCES [dbo].[CustomerStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryCustomer_CustomerStatus_Foreign_Key'
CREATE INDEX [IX_FK_HistoryCustomer_CustomerStatus_Foreign_Key]
ON [dbo].[CustomerHistory]
    ([CustomerStatusId]);
GO

-- Creating foreign key on [PostOfficeId] in table 'CustomerHistory'
ALTER TABLE [dbo].[CustomerHistory]
ADD CONSTRAINT [FK_HistoryCustomer_PostOffice_Foreign_Key]
    FOREIGN KEY ([PostOfficeId])
    REFERENCES [dbo].[PostOffice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryCustomer_PostOffice_Foreign_Key'
CREATE INDEX [IX_FK_HistoryCustomer_PostOffice_Foreign_Key]
ON [dbo].[CustomerHistory]
    ([PostOfficeId]);
GO

-- Creating foreign key on [BankChargeTransactionDetailId] in table 'CustomerTransaction'
ALTER TABLE [dbo].[CustomerTransaction]
ADD CONSTRAINT [FK_dbo_CustomerTransaction_TransactionDetail_BankCharge]
    FOREIGN KEY ([BankChargeTransactionDetailId])
    REFERENCES [dbo].[TransactionDetail]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransaction_TransactionDetail_BankCharge'
CREATE INDEX [IX_FK_dbo_CustomerTransaction_TransactionDetail_BankCharge]
ON [dbo].[CustomerTransaction]
    ([BankChargeTransactionDetailId]);
GO

-- Creating foreign key on [CashBankTransactionDetailId] in table 'CustomerTransaction'
ALTER TABLE [dbo].[CustomerTransaction]
ADD CONSTRAINT [FK_dbo_CustomerTransaction_TransactionDetail_CashBank]
    FOREIGN KEY ([CashBankTransactionDetailId])
    REFERENCES [dbo].[TransactionDetail]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransaction_TransactionDetail_CashBank'
CREATE INDEX [IX_FK_dbo_CustomerTransaction_TransactionDetail_CashBank]
ON [dbo].[CustomerTransaction]
    ([CashBankTransactionDetailId]);
GO

-- Creating foreign key on [TransactionEntryId] in table 'CustomerTransaction'
ALTER TABLE [dbo].[CustomerTransaction]
ADD CONSTRAINT [FK_dbo_CustomerTransaction_TransactionEntry]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntry]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransaction_TransactionEntry'
CREATE INDEX [IX_FK_dbo_CustomerTransaction_TransactionEntry]
ON [dbo].[CustomerTransaction]
    ([TransactionEntryId]);
GO

-- Creating foreign key on [CustomerTransactionId] in table 'CustomerTransactionDetail'
ALTER TABLE [dbo].[CustomerTransactionDetail]
ADD CONSTRAINT [FK_dbo_CustomerTransactionDetail_CustomerTransaction]
    FOREIGN KEY ([CustomerTransactionId])
    REFERENCES [dbo].[CustomerTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransactionDetail_CustomerTransaction'
CREATE INDEX [IX_FK_dbo_CustomerTransactionDetail_CustomerTransaction]
ON [dbo].[CustomerTransactionDetail]
    ([CustomerTransactionId]);
GO

-- Creating foreign key on [TransactionDetailId] in table 'CustomerTransactionDetail'
ALTER TABLE [dbo].[CustomerTransactionDetail]
ADD CONSTRAINT [FK_dbo_CustomerTransactionDetail_TransactionDetail]
    FOREIGN KEY ([TransactionDetailId])
    REFERENCES [dbo].[TransactionDetail]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerTransactionDetail_TransactionDetail'
CREATE INDEX [IX_FK_dbo_CustomerTransactionDetail_TransactionDetail]
ON [dbo].[CustomerTransactionDetail]
    ([TransactionDetailId]);
GO

-- Creating foreign key on [InvoiceId] in table 'DeliveryQuantity'
ALTER TABLE [dbo].[DeliveryQuantity]
ADD CONSTRAINT [FK_DeliveryQuantity_Invoice_Foreign_Key]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryQuantity_Invoice_Foreign_Key'
CREATE INDEX [IX_FK_DeliveryQuantity_Invoice_Foreign_Key]
ON [dbo].[DeliveryQuantity]
    ([InvoiceId]);
GO

-- Creating foreign key on [DeliveryQuantityId] in table 'DeliveryQuantityDetail'
ALTER TABLE [dbo].[DeliveryQuantityDetail]
ADD CONSTRAINT [FK_DeliveryQuantityDetail_DeliveryQuantity_Foreign_Key]
    FOREIGN KEY ([DeliveryQuantityId])
    REFERENCES [dbo].[DeliveryQuantity]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryQuantityDetail_DeliveryQuantity_Foreign_Key'
CREATE INDEX [IX_FK_DeliveryQuantityDetail_DeliveryQuantity_Foreign_Key]
ON [dbo].[DeliveryQuantityDetail]
    ([DeliveryQuantityId]);
GO

-- Creating foreign key on [ProductId] in table 'DeliveryQuantityDetail'
ALTER TABLE [dbo].[DeliveryQuantityDetail]
ADD CONSTRAINT [FK_DeliveryQuantityDetail_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeliveryQuantityDetail_Product_Foreign_Key'
CREATE INDEX [IX_FK_DeliveryQuantityDetail_Product_Foreign_Key]
ON [dbo].[DeliveryQuantityDetail]
    ([ProductId]);
GO

-- Creating foreign key on [DemandOrderTypeId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_DemandOrderType_Foreign_Key]
    FOREIGN KEY ([DemandOrderTypeId])
    REFERENCES [dbo].[DemandOrderType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_DemandOrderType_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrder_DemandOrderType_Foreign_Key]
ON [dbo].[DemandOrder]
    ([DemandOrderTypeId]);
GO

-- Creating foreign key on [DiscountTypeId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_DiscountType_Foreign_Key]
    FOREIGN KEY ([DiscountTypeId])
    REFERENCES [dbo].[DiscountType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_DiscountType_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrder_DiscountType_Foreign_Key]
ON [dbo].[DemandOrder]
    ([DiscountTypeId]);
GO

-- Creating foreign key on [RejectedReasonTypeId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_RejectedReasonType_Foreign_Key]
    FOREIGN KEY ([RejectedReasonTypeId])
    REFERENCES [dbo].[RejectedReasonType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_RejectedReasonType_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrder_RejectedReasonType_Foreign_Key]
ON [dbo].[DemandOrder]
    ([RejectedReasonTypeId]);
GO

-- Creating foreign key on [SaleTypeId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_SaleType_Foreign_Key]
    FOREIGN KEY ([SaleTypeId])
    REFERENCES [dbo].[SaleType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_SaleType_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrder_SaleType_Foreign_Key]
ON [dbo].[DemandOrder]
    ([SaleTypeId]);
GO

-- Creating foreign key on [DemandOrderId] in table 'DemandOrderDetail'
ALTER TABLE [dbo].[DemandOrderDetail]
ADD CONSTRAINT [FK_DemandOrderDetail_DemandOrder_Foreign_Key]
    FOREIGN KEY ([DemandOrderId])
    REFERENCES [dbo].[DemandOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrderDetail_DemandOrder_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrderDetail_DemandOrder_Foreign_Key]
ON [dbo].[DemandOrderDetail]
    ([DemandOrderId]);
GO

-- Creating foreign key on [DemandOrderId] in table 'DemandOrderDiscountTransaction'
ALTER TABLE [dbo].[DemandOrderDiscountTransaction]
ADD CONSTRAINT [FK_DemandOrderDiscountTransaction_DemandOrder_Foreign_Key]
    FOREIGN KEY ([DemandOrderId])
    REFERENCES [dbo].[DemandOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrderDiscountTransaction_DemandOrder_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrderDiscountTransaction_DemandOrder_Foreign_Key]
ON [dbo].[DemandOrderDiscountTransaction]
    ([DemandOrderId]);
GO

-- Creating foreign key on [DemandOrderId] in table 'DemandOrderTransaction'
ALTER TABLE [dbo].[DemandOrderTransaction]
ADD CONSTRAINT [FK_DemandOrderTransaction_DemandOrder_Foreign_Key]
    FOREIGN KEY ([DemandOrderId])
    REFERENCES [dbo].[DemandOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrderTransaction_DemandOrder_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrderTransaction_DemandOrder_Foreign_Key]
ON [dbo].[DemandOrderTransaction]
    ([DemandOrderId]);
GO

-- Creating foreign key on [DemandOrderStatusId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_DemandOrderStatus]
    FOREIGN KEY ([DemandOrderStatusId])
    REFERENCES [dbo].[DemandOrderStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_DemandOrderStatus'
CREATE INDEX [IX_FK_DemandOrder_DemandOrderStatus]
ON [dbo].[DemandOrder]
    ([DemandOrderStatusId]);
GO

-- Creating foreign key on [DemandOrderTransactionId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_DemandOrderTransaction]
    FOREIGN KEY ([DemandOrderTransactionId])
    REFERENCES [dbo].[DemandOrderTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_DemandOrderTransaction'
CREATE INDEX [IX_FK_DemandOrder_DemandOrderTransaction]
ON [dbo].[DemandOrder]
    ([DemandOrderTransactionId]);
GO

-- Creating foreign key on [EmployeeId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_Employee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_Employee'
CREATE INDEX [IX_FK_DemandOrder_Employee]
ON [dbo].[DemandOrder]
    ([EmployeeId]);
GO

-- Creating foreign key on [PaymentStatusId] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_PaymentStatus]
    FOREIGN KEY ([PaymentStatusId])
    REFERENCES [dbo].[PaymentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_PaymentStatus'
CREATE INDEX [IX_FK_DemandOrder_PaymentStatus]
ON [dbo].[DemandOrder]
    ([PaymentStatusId]);
GO

-- Creating foreign key on [CreatedBy] in table 'DemandOrder'
ALTER TABLE [dbo].[DemandOrder]
ADD CONSTRAINT [FK_DemandOrder_User]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrder_User'
CREATE INDEX [IX_FK_DemandOrder_User]
ON [dbo].[DemandOrder]
    ([CreatedBy]);
GO

-- Creating foreign key on [DemandOrderId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_Invoice_DemandOrder_Foreign_Key]
    FOREIGN KEY ([DemandOrderId])
    REFERENCES [dbo].[DemandOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_DemandOrder_Foreign_Key'
CREATE INDEX [IX_FK_Invoice_DemandOrder_Foreign_Key]
ON [dbo].[Invoice]
    ([DemandOrderId]);
GO

-- Creating foreign key on [ProductId] in table 'DemandOrderDetail'
ALTER TABLE [dbo].[DemandOrderDetail]
ADD CONSTRAINT [FK_DemandOrderDetail_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrderDetail_Product_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrderDetail_Product_Foreign_Key]
ON [dbo].[DemandOrderDetail]
    ([ProductId]);
GO

-- Creating foreign key on [DemandOrderDiscountTypeId] in table 'DemandOrderDiscountTransaction'
ALTER TABLE [dbo].[DemandOrderDiscountTransaction]
ADD CONSTRAINT [FK_DemandOrderDiscountTransaction_DemandOrderDiscountType_Foreign_Key]
    FOREIGN KEY ([DemandOrderDiscountTypeId])
    REFERENCES [dbo].[DemandOrderDiscountType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemandOrderDiscountTransaction_DemandOrderDiscountType_Foreign_Key'
CREATE INDEX [IX_FK_DemandOrderDiscountTransaction_DemandOrderDiscountType_Foreign_Key]
ON [dbo].[DemandOrderDiscountTransaction]
    ([DemandOrderDiscountTypeId]);
GO

-- Creating foreign key on [DepartmentId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_Department_Foreign_Key]
    FOREIGN KEY ([DepartmentId])
    REFERENCES [dbo].[Department]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_Department_Foreign_Key'
CREATE INDEX [IX_FK_Employee_Department_Foreign_Key]
ON [dbo].[Employee]
    ([DepartmentId]);
GO

-- Creating foreign key on [DepartmentId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_Department_Foreign_Key]
    FOREIGN KEY ([DepartmentId])
    REFERENCES [dbo].[Department]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_Department_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeHistory_Department_Foreign_Key]
ON [dbo].[EmployeeHistory]
    ([DepartmentId]);
GO

-- Creating foreign key on [DepartmentId] in table 'Designation'
ALTER TABLE [dbo].[Designation]
ADD CONSTRAINT [FK_Position_Department_Foreign_Key]
    FOREIGN KEY ([DepartmentId])
    REFERENCES [dbo].[Department]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Position_Department_Foreign_Key'
CREATE INDEX [IX_FK_Position_Department_Foreign_Key]
ON [dbo].[Designation]
    ([DepartmentId]);
GO

-- Creating foreign key on [DesignationId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_Designation_Foreign_Key]
    FOREIGN KEY ([DesignationId])
    REFERENCES [dbo].[Designation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_Designation_Foreign_Key'
CREATE INDEX [IX_FK_Employee_Designation_Foreign_Key]
ON [dbo].[Employee]
    ([DesignationId]);
GO

-- Creating foreign key on [DesignationId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_Designation_Foreign_Key]
    FOREIGN KEY ([DesignationId])
    REFERENCES [dbo].[Designation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_Designation_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeHistory_Designation_Foreign_Key]
ON [dbo].[EmployeeHistory]
    ([DesignationId]);
GO

-- Creating foreign key on [DistrictId] in table 'PoliceStation'
ALTER TABLE [dbo].[PoliceStation]
ADD CONSTRAINT [FK_PoliceStation_District_Foreign_Key]
    FOREIGN KEY ([DistrictId])
    REFERENCES [dbo].[District]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PoliceStation_District_Foreign_Key'
CREATE INDEX [IX_FK_PoliceStation_District_Foreign_Key]
ON [dbo].[PoliceStation]
    ([DistrictId]);
GO

-- Creating foreign key on [DocumentRenewCategoryId] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [FK_LegalDocument_DocumentRenewalCategory]
    FOREIGN KEY ([DocumentRenewCategoryId])
    REFERENCES [dbo].[DocumentRenewalCategory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocument_DocumentRenewalCategory'
CREATE INDEX [IX_FK_LegalDocument_DocumentRenewalCategory]
ON [dbo].[LegalDocument]
    ([DocumentRenewCategoryId]);
GO

-- Creating foreign key on [DocumentRenewCategoryId] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [FK_LegalDocumentHistory_DocumentRenewalCategory]
    FOREIGN KEY ([DocumentRenewCategoryId])
    REFERENCES [dbo].[DocumentRenewalCategory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocumentHistory_DocumentRenewalCategory'
CREATE INDEX [IX_FK_LegalDocumentHistory_DocumentRenewalCategory]
ON [dbo].[LegalDocumentHistory]
    ([DocumentRenewCategoryId]);
GO

-- Creating foreign key on [DocumentRenewalCategoryId] in table 'NotificationSetting'
ALTER TABLE [dbo].[NotificationSetting]
ADD CONSTRAINT [FK_NotificationSetting_DocumentRenewalCategory]
    FOREIGN KEY ([DocumentRenewalCategoryId])
    REFERENCES [dbo].[DocumentRenewalCategory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NotificationSetting_DocumentRenewalCategory'
CREATE INDEX [IX_FK_NotificationSetting_DocumentRenewalCategory]
ON [dbo].[NotificationSetting]
    ([DocumentRenewalCategoryId]);
GO

-- Creating foreign key on [DocumentStatusId] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [FK_LegalDocument_DocumentStatus]
    FOREIGN KEY ([DocumentStatusId])
    REFERENCES [dbo].[DocumentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocument_DocumentStatus'
CREATE INDEX [IX_FK_LegalDocument_DocumentStatus]
ON [dbo].[LegalDocument]
    ([DocumentStatusId]);
GO

-- Creating foreign key on [DocumentStatusId] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [FK_LegalDocumentHistory_DocumentStatus]
    FOREIGN KEY ([DocumentStatusId])
    REFERENCES [dbo].[DocumentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocumentHistory_DocumentStatus'
CREATE INDEX [IX_FK_LegalDocumentHistory_DocumentStatus]
ON [dbo].[LegalDocumentHistory]
    ([DocumentStatusId]);
GO

-- Creating foreign key on [DocumentTypeId] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [FK_LegalDocument_DocumentType]
    FOREIGN KEY ([DocumentTypeId])
    REFERENCES [dbo].[DocumentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocument_DocumentType'
CREATE INDEX [IX_FK_LegalDocument_DocumentType]
ON [dbo].[LegalDocument]
    ([DocumentTypeId]);
GO

-- Creating foreign key on [DocumentTypeId] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [FK_LegalDocumentHistory_DocumentType]
    FOREIGN KEY ([DocumentTypeId])
    REFERENCES [dbo].[DocumentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocumentHistory_DocumentType'
CREATE INDEX [IX_FK_LegalDocumentHistory_DocumentType]
ON [dbo].[LegalDocumentHistory]
    ([DocumentTypeId]);
GO

-- Creating foreign key on [ManagerId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_Manager_Foreign_Key]
    FOREIGN KEY ([ManagerId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_Manager_Foreign_Key'
CREATE INDEX [IX_FK_Employee_Manager_Foreign_Key]
ON [dbo].[Employee]
    ([ManagerId]);
GO

-- Creating foreign key on [PostOfficeId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_PostOffice_Foreign_Key]
    FOREIGN KEY ([PostOfficeId])
    REFERENCES [dbo].[PostOffice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_PostOffice_Foreign_Key'
CREATE INDEX [IX_FK_Employee_PostOffice_Foreign_Key]
ON [dbo].[Employee]
    ([PostOfficeId]);
GO

-- Creating foreign key on [ManagerId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_Manager_Foreign_Key]
    FOREIGN KEY ([ManagerId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_Manager_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeHistory_Manager_Foreign_Key]
ON [dbo].[EmployeeHistory]
    ([ManagerId]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeeSalesLocation'
ALTER TABLE [dbo].[EmployeeSalesLocation]
ADD CONSTRAINT [FK_EmployeeSalesLocation_Employee_Foreign_Key]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocation_Employee_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocation_Employee_Foreign_Key]
ON [dbo].[EmployeeSalesLocation]
    ([EmployeeId]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeeSalesLocationHistory'
ALTER TABLE [dbo].[EmployeeSalesLocationHistory]
ADD CONSTRAINT [FK_EmployeeSalesLocationHistory_Employee_Foreign_Key]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocationHistory_Employee_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocationHistory_Employee_Foreign_Key]
ON [dbo].[EmployeeSalesLocationHistory]
    ([EmployeeId]);
GO

-- Creating foreign key on [EmployeeTypeId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_EmployeeType]
    FOREIGN KEY ([EmployeeTypeId])
    REFERENCES [dbo].[EmployeeType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_EmployeeType'
CREATE INDEX [IX_FK_Employee_EmployeeType]
ON [dbo].[Employee]
    ([EmployeeTypeId]);
GO

-- Creating foreign key on [SalesAreaId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_SalesArea]
    FOREIGN KEY ([SalesAreaId])
    REFERENCES [dbo].[SalesArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_SalesArea'
CREATE INDEX [IX_FK_Employee_SalesArea]
ON [dbo].[Employee]
    ([SalesAreaId]);
GO

-- Creating foreign key on [SalesBaseId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_SalesBase]
    FOREIGN KEY ([SalesBaseId])
    REFERENCES [dbo].[SalesBase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_SalesBase'
CREATE INDEX [IX_FK_Employee_SalesBase]
ON [dbo].[Employee]
    ([SalesBaseId]);
GO

-- Creating foreign key on [SalesDivisionId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [FK_Employee_SalesDivision]
    FOREIGN KEY ([SalesDivisionId])
    REFERENCES [dbo].[SalesDivision]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employee_SalesDivision'
CREATE INDEX [IX_FK_Employee_SalesDivision]
ON [dbo].[Employee]
    ([SalesDivisionId]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeeLeave'
ALTER TABLE [dbo].[EmployeeLeave]
ADD CONSTRAINT [FK_EmployeeLeave_Employee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeLeave_Employee'
CREATE INDEX [IX_FK_EmployeeLeave_Employee]
ON [dbo].[EmployeeLeave]
    ([EmployeeId]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeeSalesTargetMonthly'
ALTER TABLE [dbo].[EmployeeSalesTargetMonthly]
ADD CONSTRAINT [FK_EmployeeSalesTargetMonthly_Employee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesTargetMonthly_Employee'
CREATE INDEX [IX_FK_EmployeeSalesTargetMonthly_Employee]
ON [dbo].[EmployeeSalesTargetMonthly]
    ([EmployeeId]);
GO

-- Creating foreign key on [PostOfficeId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_PostOffice_Foreign_Key]
    FOREIGN KEY ([PostOfficeId])
    REFERENCES [dbo].[PostOffice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_PostOffice_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeHistory_PostOffice_Foreign_Key]
ON [dbo].[EmployeeHistory]
    ([PostOfficeId]);
GO

-- Creating foreign key on [EmployeeHistoryId] in table 'EmployeeSalesLocationHistory'
ALTER TABLE [dbo].[EmployeeSalesLocationHistory]
ADD CONSTRAINT [FK_EmployeeSalesLocationHistory_EmployeeHistory_Foreign_Key]
    FOREIGN KEY ([EmployeeHistoryId])
    REFERENCES [dbo].[EmployeeHistory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocationHistory_EmployeeHistory_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocationHistory_EmployeeHistory_Foreign_Key]
ON [dbo].[EmployeeSalesLocationHistory]
    ([EmployeeHistoryId]);
GO

-- Creating foreign key on [SalesAreaId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_SalesArea]
    FOREIGN KEY ([SalesAreaId])
    REFERENCES [dbo].[SalesArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_SalesArea'
CREATE INDEX [IX_FK_EmployeeHistory_SalesArea]
ON [dbo].[EmployeeHistory]
    ([SalesAreaId]);
GO

-- Creating foreign key on [SalesBaseId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_SalesBase]
    FOREIGN KEY ([SalesBaseId])
    REFERENCES [dbo].[SalesBase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_SalesBase'
CREATE INDEX [IX_FK_EmployeeHistory_SalesBase]
ON [dbo].[EmployeeHistory]
    ([SalesBaseId]);
GO

-- Creating foreign key on [SalesDivisionId] in table 'EmployeeHistory'
ALTER TABLE [dbo].[EmployeeHistory]
ADD CONSTRAINT [FK_EmployeeHistory_SalesDivision]
    FOREIGN KEY ([SalesDivisionId])
    REFERENCES [dbo].[SalesDivision]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHistory_SalesDivision'
CREATE INDEX [IX_FK_EmployeeHistory_SalesDivision]
ON [dbo].[EmployeeHistory]
    ([SalesDivisionId]);
GO

-- Creating foreign key on [LeaveCategoryId] in table 'EmployeeLeave'
ALTER TABLE [dbo].[EmployeeLeave]
ADD CONSTRAINT [FK_EmployeeLeave_LeaveCategory]
    FOREIGN KEY ([LeaveCategoryId])
    REFERENCES [dbo].[LeaveCategory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeLeave_LeaveCategory'
CREATE INDEX [IX_FK_EmployeeLeave_LeaveCategory]
ON [dbo].[EmployeeLeave]
    ([LeaveCategoryId]);
GO

-- Creating foreign key on [AreaId] in table 'EmployeeSalesLocation'
ALTER TABLE [dbo].[EmployeeSalesLocation]
ADD CONSTRAINT [FK_EmployeeSalesLocation_SalesArea_Foreign_Key]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[SalesArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocation_SalesArea_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocation_SalesArea_Foreign_Key]
ON [dbo].[EmployeeSalesLocation]
    ([AreaId]);
GO

-- Creating foreign key on [BaseId] in table 'EmployeeSalesLocation'
ALTER TABLE [dbo].[EmployeeSalesLocation]
ADD CONSTRAINT [FK_EmployeeSalesLocation_SalesBase_Foreign_Key]
    FOREIGN KEY ([BaseId])
    REFERENCES [dbo].[SalesBase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocation_SalesBase_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocation_SalesBase_Foreign_Key]
ON [dbo].[EmployeeSalesLocation]
    ([BaseId]);
GO

-- Creating foreign key on [DivisionId] in table 'EmployeeSalesLocation'
ALTER TABLE [dbo].[EmployeeSalesLocation]
ADD CONSTRAINT [FK_EmployeeSalesLocation_SalesDivision_Foreign_Key]
    FOREIGN KEY ([DivisionId])
    REFERENCES [dbo].[SalesDivision]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocation_SalesDivision_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocation_SalesDivision_Foreign_Key]
ON [dbo].[EmployeeSalesLocation]
    ([DivisionId]);
GO

-- Creating foreign key on [SalesAreaId] in table 'EmployeeSalesLocationHistory'
ALTER TABLE [dbo].[EmployeeSalesLocationHistory]
ADD CONSTRAINT [FK_EmployeeSalesLocationHistory_SalesArea_Foreign_Key]
    FOREIGN KEY ([SalesAreaId])
    REFERENCES [dbo].[SalesArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocationHistory_SalesArea_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocationHistory_SalesArea_Foreign_Key]
ON [dbo].[EmployeeSalesLocationHistory]
    ([SalesAreaId]);
GO

-- Creating foreign key on [SalesBaseId] in table 'EmployeeSalesLocationHistory'
ALTER TABLE [dbo].[EmployeeSalesLocationHistory]
ADD CONSTRAINT [FK_EmployeeSalesLocationHistory_SalesBase_Foreign_Key]
    FOREIGN KEY ([SalesBaseId])
    REFERENCES [dbo].[SalesBase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocationHistory_SalesBase_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocationHistory_SalesBase_Foreign_Key]
ON [dbo].[EmployeeSalesLocationHistory]
    ([SalesBaseId]);
GO

-- Creating foreign key on [SalesDivisionId] in table 'EmployeeSalesLocationHistory'
ALTER TABLE [dbo].[EmployeeSalesLocationHistory]
ADD CONSTRAINT [FK_EmployeeSalesLocationHistory_SalesDivision_Foreign_Key]
    FOREIGN KEY ([SalesDivisionId])
    REFERENCES [dbo].[SalesDivision]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalesLocationHistory_SalesDivision_Foreign_Key'
CREATE INDEX [IX_FK_EmployeeSalesLocationHistory_SalesDivision_Foreign_Key]
ON [dbo].[EmployeeSalesLocationHistory]
    ([SalesDivisionId]);
GO

-- Creating foreign key on [EntityTypeId] in table 'SystemWarningType'
ALTER TABLE [dbo].[SystemWarningType]
ADD CONSTRAINT [FK_SystemWarningType_EntityType_EntityTypeId]
    FOREIGN KEY ([EntityTypeId])
    REFERENCES [dbo].[EntityType]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemWarningType_EntityType_EntityTypeId'
CREATE INDEX [IX_FK_SystemWarningType_EntityType_EntityTypeId]
ON [dbo].[SystemWarningType]
    ([EntityTypeId]);
GO

-- Creating foreign key on [ProductId] in table 'FinishedGood'
ALTER TABLE [dbo].[FinishedGood]
ADD CONSTRAINT [FK_FinishedGood_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FinishedGood_Product_Foreign_Key'
CREATE INDEX [IX_FK_FinishedGood_Product_Foreign_Key]
ON [dbo].[FinishedGood]
    ([ProductId]);
GO

-- Creating foreign key on [ProductionGroupId] in table 'FinishedGood'
ALTER TABLE [dbo].[FinishedGood]
ADD CONSTRAINT [FK_FinishedGood_ToProductionGroup]
    FOREIGN KEY ([ProductionGroupId])
    REFERENCES [dbo].[ProductionGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FinishedGood_ToProductionGroup'
CREATE INDEX [IX_FK_FinishedGood_ToProductionGroup]
ON [dbo].[FinishedGood]
    ([ProductionGroupId]);
GO

-- Creating foreign key on [ApprovedBy] in table 'FinishedGood'
ALTER TABLE [dbo].[FinishedGood]
ADD CONSTRAINT [FK_FinishedGood_User_Approved]
    FOREIGN KEY ([ApprovedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FinishedGood_User_Approved'
CREATE INDEX [IX_FK_FinishedGood_User_Approved]
ON [dbo].[FinishedGood]
    ([ApprovedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'FinishedGood'
ALTER TABLE [dbo].[FinishedGood]
ADD CONSTRAINT [FK_FinishedGood_User_Created]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FinishedGood_User_Created'
CREATE INDEX [IX_FK_FinishedGood_User_Created]
ON [dbo].[FinishedGood]
    ([CreatedBy]);
GO

-- Creating foreign key on [ProductId] in table 'FinishedGoodOpening'
ALTER TABLE [dbo].[FinishedGoodOpening]
ADD CONSTRAINT [FK_FinishedGoodOpening_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FinishedGoodOpening_Product_Foreign_Key'
CREATE INDEX [IX_FK_FinishedGoodOpening_Product_Foreign_Key]
ON [dbo].[FinishedGoodOpening]
    ([ProductId]);
GO

-- Creating foreign key on [RawMaterialTypeId] in table 'FloorStoreRawMaterial'
ALTER TABLE [dbo].[FloorStoreRawMaterial]
ADD CONSTRAINT [FK_FloorStoreRawMaterial_RawMaterialType]
    FOREIGN KEY ([RawMaterialTypeId])
    REFERENCES [dbo].[RawMaterialType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FloorStoreRawMaterial_RawMaterialType'
CREATE INDEX [IX_FK_FloorStoreRawMaterial_RawMaterialType]
ON [dbo].[FloorStoreRawMaterial]
    ([RawMaterialTypeId]);
GO

-- Creating foreign key on [ReceivedBy] in table 'FloorStoreRawMaterial'
ALTER TABLE [dbo].[FloorStoreRawMaterial]
ADD CONSTRAINT [FK_FloorStoreRawMaterial_User]
    FOREIGN KEY ([ReceivedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FloorStoreRawMaterial_User'
CREATE INDEX [IX_FK_FloorStoreRawMaterial_User]
ON [dbo].[FloorStoreRawMaterial]
    ([ReceivedBy]);
GO

-- Creating foreign key on [TransactionEntryId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_Invoice_ToTransactionEntry]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntry]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_ToTransactionEntry'
CREATE INDEX [IX_FK_Invoice_ToTransactionEntry]
ON [dbo].[Invoice]
    ([TransactionEntryId]);
GO

-- Creating foreign key on [InvoiceId] in table 'InvoiceReturn'
ALTER TABLE [dbo].[InvoiceReturn]
ADD CONSTRAINT [FK_InvoiceReturn_Invoice]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceReturn_Invoice'
CREATE INDEX [IX_FK_InvoiceReturn_Invoice]
ON [dbo].[InvoiceReturn]
    ([InvoiceId]);
GO

-- Creating foreign key on [InvoiceId] in table 'InvoiceDetail'
ALTER TABLE [dbo].[InvoiceDetail]
ADD CONSTRAINT [FK_InvoiceDetail_Invoice_Foreign_Key]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceDetail_Invoice_Foreign_Key'
CREATE INDEX [IX_FK_InvoiceDetail_Invoice_Foreign_Key]
ON [dbo].[InvoiceDetail]
    ([InvoiceId]);
GO

-- Creating foreign key on [InvoiceId] in table 'InvoiceTransaction'
ALTER TABLE [dbo].[InvoiceTransaction]
ADD CONSTRAINT [FK_InvoiceTransaction_Invoice_Foreign_Key]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceTransaction_Invoice_Foreign_Key'
CREATE INDEX [IX_FK_InvoiceTransaction_Invoice_Foreign_Key]
ON [dbo].[InvoiceTransaction]
    ([InvoiceId]);
GO

-- Creating foreign key on [ProductId] in table 'InvoiceDetail'
ALTER TABLE [dbo].[InvoiceDetail]
ADD CONSTRAINT [FK_InvoiceDetail_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceDetail_Product_Foreign_Key'
CREATE INDEX [IX_FK_InvoiceDetail_Product_Foreign_Key]
ON [dbo].[InvoiceDetail]
    ([ProductId]);
GO

-- Creating foreign key on [TransactionEntryId] in table 'InvoiceReturn'
ALTER TABLE [dbo].[InvoiceReturn]
ADD CONSTRAINT [FK_InvoiceReturn_TransactionEntry]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntry]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceReturn_TransactionEntry'
CREATE INDEX [IX_FK_InvoiceReturn_TransactionEntry]
ON [dbo].[InvoiceReturn]
    ([TransactionEntryId]);
GO

-- Creating foreign key on [InvoiceReturnId] in table 'InvoiceReturnDetail'
ALTER TABLE [dbo].[InvoiceReturnDetail]
ADD CONSTRAINT [FK_InvoiceReturnDetail_InvoiceReturn_Foreign_Key]
    FOREIGN KEY ([InvoiceReturnId])
    REFERENCES [dbo].[InvoiceReturn]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceReturnDetail_InvoiceReturn_Foreign_Key'
CREATE INDEX [IX_FK_InvoiceReturnDetail_InvoiceReturn_Foreign_Key]
ON [dbo].[InvoiceReturnDetail]
    ([InvoiceReturnId]);
GO

-- Creating foreign key on [ProductId] in table 'InvoiceReturnDetail'
ALTER TABLE [dbo].[InvoiceReturnDetail]
ADD CONSTRAINT [FK_InvoiceReturnDetail_Product_Foreign_Key]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceReturnDetail_Product_Foreign_Key'
CREATE INDEX [IX_FK_InvoiceReturnDetail_Product_Foreign_Key]
ON [dbo].[InvoiceReturnDetail]
    ([ProductId]);
GO

-- Creating foreign key on [CreatedBy] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [FK_LegalDocument_User_CreatedBy]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocument_User_CreatedBy'
CREATE INDEX [IX_FK_LegalDocument_User_CreatedBy]
ON [dbo].[LegalDocument]
    ([CreatedBy]);
GO

-- Creating foreign key on [UpdatedBy] in table 'LegalDocument'
ALTER TABLE [dbo].[LegalDocument]
ADD CONSTRAINT [FK_LegalDocument_User_UpdatedBy]
    FOREIGN KEY ([UpdatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocument_User_UpdatedBy'
CREATE INDEX [IX_FK_LegalDocument_User_UpdatedBy]
ON [dbo].[LegalDocument]
    ([UpdatedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [FK_LegalDocumentHistory_User_CreatedBy]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocumentHistory_User_CreatedBy'
CREATE INDEX [IX_FK_LegalDocumentHistory_User_CreatedBy]
ON [dbo].[LegalDocumentHistory]
    ([CreatedBy]);
GO

-- Creating foreign key on [UpdatedBy] in table 'LegalDocumentHistory'
ALTER TABLE [dbo].[LegalDocumentHistory]
ADD CONSTRAINT [FK_LegalDocumentHistory_User_UpdatedBy]
    FOREIGN KEY ([UpdatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LegalDocumentHistory_User_UpdatedBy'
CREATE INDEX [IX_FK_LegalDocumentHistory_User_UpdatedBy]
ON [dbo].[LegalDocumentHistory]
    ([UpdatedBy]);
GO

-- Creating foreign key on [ProcessingTypeId] in table 'MonthlyProcessing'
ALTER TABLE [dbo].[MonthlyProcessing]
ADD CONSTRAINT [FK_MonthlyProcessing_ProcessingType]
    FOREIGN KEY ([ProcessingTypeId])
    REFERENCES [dbo].[ProcessingType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MonthlyProcessing_ProcessingType'
CREATE INDEX [IX_FK_MonthlyProcessing_ProcessingType]
ON [dbo].[MonthlyProcessing]
    ([ProcessingTypeId]);
GO

-- Creating foreign key on [CreatedBy] in table 'MonthlyProcessing'
ALTER TABLE [dbo].[MonthlyProcessing]
ADD CONSTRAINT [FK_MonthlyProcessing_User]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MonthlyProcessing_User'
CREATE INDEX [IX_FK_MonthlyProcessing_User]
ON [dbo].[MonthlyProcessing]
    ([CreatedBy]);
GO

-- Creating foreign key on [ReprocessedBy] in table 'MonthlyProcessing'
ALTER TABLE [dbo].[MonthlyProcessing]
ADD CONSTRAINT [FK_MonthlyProcessing_User1]
    FOREIGN KEY ([ReprocessedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MonthlyProcessing_User1'
CREATE INDEX [IX_FK_MonthlyProcessing_User1]
ON [dbo].[MonthlyProcessing]
    ([ReprocessedBy]);
GO

-- Creating foreign key on [PoliceStationId] in table 'PostOffice'
ALTER TABLE [dbo].[PostOffice]
ADD CONSTRAINT [FK_PostOffice_PoliceStation_Foreign_Key]
    FOREIGN KEY ([PoliceStationId])
    REFERENCES [dbo].[PoliceStation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostOffice_PoliceStation_Foreign_Key'
CREATE INDEX [IX_FK_PostOffice_PoliceStation_Foreign_Key]
ON [dbo].[PostOffice]
    ([PoliceStationId]);
GO

-- Creating foreign key on [PolicyId] in table 'RolePolicy'
ALTER TABLE [dbo].[RolePolicy]
ADD CONSTRAINT [FK_RolePolicy_Policy]
    FOREIGN KEY ([PolicyId])
    REFERENCES [dbo].[Policy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePolicy_Policy'
CREATE INDEX [IX_FK_RolePolicy_Policy]
ON [dbo].[RolePolicy]
    ([PolicyId]);
GO

-- Creating foreign key on [PolicyId] in table 'RolePolicyHistory'
ALTER TABLE [dbo].[RolePolicyHistory]
ADD CONSTRAINT [FK_RolePolicyHistory_Policy]
    FOREIGN KEY ([PolicyId])
    REFERENCES [dbo].[Policy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePolicyHistory_Policy'
CREATE INDEX [IX_FK_RolePolicyHistory_Policy]
ON [dbo].[RolePolicyHistory]
    ([PolicyId]);
GO

-- Creating foreign key on [ProductStandardTypeId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_Product_ProductStandardType]
    FOREIGN KEY ([ProductStandardTypeId])
    REFERENCES [dbo].[ProductStandardType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_ProductStandardType'
CREATE INDEX [IX_FK_Product_ProductStandardType]
ON [dbo].[Product]
    ([ProductStandardTypeId]);
GO

-- Creating foreign key on [UnitTypeId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_Product_UnitType]
    FOREIGN KEY ([UnitTypeId])
    REFERENCES [dbo].[UnitType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_UnitType'
CREATE INDEX [IX_FK_Product_UnitType]
ON [dbo].[Product]
    ([UnitTypeId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductHistory'
ALTER TABLE [dbo].[ProductHistory]
ADD CONSTRAINT [FK_ProductHistory_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductHistory_Product'
CREATE INDEX [IX_FK_ProductHistory_Product]
ON [dbo].[ProductHistory]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductionForecastMonthly'
ALTER TABLE [dbo].[ProductionForecastMonthly]
ADD CONSTRAINT [FK_ProductionForecastMonthly_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionForecastMonthly_Product'
CREATE INDEX [IX_FK_ProductionForecastMonthly_Product]
ON [dbo].[ProductionForecastMonthly]
    ([ProductId]);
GO

-- Creating foreign key on [ProductTypeId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_Product_ProductType_Foreign_Key]
    FOREIGN KEY ([ProductTypeId])
    REFERENCES [dbo].[ProductType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_ProductType_Foreign_Key'
CREATE INDEX [IX_FK_Product_ProductType_Foreign_Key]
ON [dbo].[Product]
    ([ProductTypeId]);
GO

-- Creating foreign key on [ProductStandardTypeId] in table 'ProductHistory'
ALTER TABLE [dbo].[ProductHistory]
ADD CONSTRAINT [FK_ProductHistory_ProductStandardType]
    FOREIGN KEY ([ProductStandardTypeId])
    REFERENCES [dbo].[ProductStandardType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductHistory_ProductStandardType'
CREATE INDEX [IX_FK_ProductHistory_ProductStandardType]
ON [dbo].[ProductHistory]
    ([ProductStandardTypeId]);
GO

-- Creating foreign key on [ProductTypeId] in table 'ProductHistory'
ALTER TABLE [dbo].[ProductHistory]
ADD CONSTRAINT [FK_ProductHistory_ProductType]
    FOREIGN KEY ([ProductTypeId])
    REFERENCES [dbo].[ProductType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductHistory_ProductType'
CREATE INDEX [IX_FK_ProductHistory_ProductType]
ON [dbo].[ProductHistory]
    ([ProductTypeId]);
GO

-- Creating foreign key on [UnitTypeId] in table 'ProductHistory'
ALTER TABLE [dbo].[ProductHistory]
ADD CONSTRAINT [FK_ProductHistory_UnitType]
    FOREIGN KEY ([UnitTypeId])
    REFERENCES [dbo].[UnitType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductHistory_UnitType'
CREATE INDEX [IX_FK_ProductHistory_UnitType]
ON [dbo].[ProductHistory]
    ([UnitTypeId]);
GO

-- Creating foreign key on [CreatedBy] in table 'ProductionGroup'
ALTER TABLE [dbo].[ProductionGroup]
ADD CONSTRAINT [FK_ProductionGroup_ToUser]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionGroup_ToUser'
CREATE INDEX [IX_FK_ProductionGroup_ToUser]
ON [dbo].[ProductionGroup]
    ([CreatedBy]);
GO

-- Creating foreign key on [ProductTypeGroupId] in table 'ProductType'
ALTER TABLE [dbo].[ProductType]
ADD CONSTRAINT [FK_ProductType_ProductTypeGroup]
    FOREIGN KEY ([ProductTypeGroupId])
    REFERENCES [dbo].[ProductTypeGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductType_ProductTypeGroup'
CREATE INDEX [IX_FK_ProductType_ProductTypeGroup]
ON [dbo].[ProductType]
    ([ProductTypeGroupId]);
GO

-- Creating foreign key on [PurchaseOrderId] in table 'PurchaseOrderTransaction'
ALTER TABLE [dbo].[PurchaseOrderTransaction]
ADD CONSTRAINT [FK_dbo_PurchaseOrderTransaction_PurchaseOrder]
    FOREIGN KEY ([PurchaseOrderId])
    REFERENCES [dbo].[PurchaseOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_PurchaseOrderTransaction_PurchaseOrder'
CREATE INDEX [IX_FK_dbo_PurchaseOrderTransaction_PurchaseOrder]
ON [dbo].[PurchaseOrderTransaction]
    ([PurchaseOrderId]);
GO

-- Creating foreign key on [PurchaseOrderStatusId] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [FK_PurchaseOrder_PurchaseOrderStatus]
    FOREIGN KEY ([PurchaseOrderStatusId])
    REFERENCES [dbo].[PurchaseOrderStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrder_PurchaseOrderStatus'
CREATE INDEX [IX_FK_PurchaseOrder_PurchaseOrderStatus]
ON [dbo].[PurchaseOrder]
    ([PurchaseOrderStatusId]);
GO

-- Creating foreign key on [CreatedBy] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [FK_PurchaseOrder_User]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrder_User'
CREATE INDEX [IX_FK_PurchaseOrder_User]
ON [dbo].[PurchaseOrder]
    ([CreatedBy]);
GO

-- Creating foreign key on [PurchaseOrderId] in table 'StoreRawMaterial'
ALTER TABLE [dbo].[StoreRawMaterial]
ADD CONSTRAINT [FK_StoreRawMaterial_PurchaseOrder]
    FOREIGN KEY ([PurchaseOrderId])
    REFERENCES [dbo].[PurchaseOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreRawMaterial_PurchaseOrder'
CREATE INDEX [IX_FK_StoreRawMaterial_PurchaseOrder]
ON [dbo].[StoreRawMaterial]
    ([PurchaseOrderId]);
GO

-- Creating foreign key on [RejectedReasonTypeId] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [FK_PurchaseOrder_RejectedReasonType_Foreign_Key]
    FOREIGN KEY ([RejectedReasonTypeId])
    REFERENCES [dbo].[RejectedReasonType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrder_RejectedReasonType_Foreign_Key'
CREATE INDEX [IX_FK_PurchaseOrder_RejectedReasonType_Foreign_Key]
ON [dbo].[PurchaseOrder]
    ([RejectedReasonTypeId]);
GO

-- Creating foreign key on [SupplierId] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [FK_PurchaseOrder_Supplier_Foreign_Key]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Supplier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrder_Supplier_Foreign_Key'
CREATE INDEX [IX_FK_PurchaseOrder_Supplier_Foreign_Key]
ON [dbo].[PurchaseOrder]
    ([SupplierId]);
GO

-- Creating foreign key on [PurchaseOrderId] in table 'PurchaseOrderDetail'
ALTER TABLE [dbo].[PurchaseOrderDetail]
ADD CONSTRAINT [FK_PurchaseOrderDetail_PurchaseOrder_Foreign_Key]
    FOREIGN KEY ([PurchaseOrderId])
    REFERENCES [dbo].[PurchaseOrder]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrderDetail_PurchaseOrder_Foreign_Key'
CREATE INDEX [IX_FK_PurchaseOrderDetail_PurchaseOrder_Foreign_Key]
ON [dbo].[PurchaseOrderDetail]
    ([PurchaseOrderId]);
GO

-- Creating foreign key on [RawMaterialTypeId] in table 'PurchaseOrderDetail'
ALTER TABLE [dbo].[PurchaseOrderDetail]
ADD CONSTRAINT [FK_PurchaseOrderDetail_RawMaterialType_Foreign_Key]
    FOREIGN KEY ([RawMaterialTypeId])
    REFERENCES [dbo].[RawMaterialType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrderDetail_RawMaterialType_Foreign_Key'
CREATE INDEX [IX_FK_PurchaseOrderDetail_RawMaterialType_Foreign_Key]
ON [dbo].[PurchaseOrderDetail]
    ([RawMaterialTypeId]);
GO

-- Creating foreign key on [SupplierId] in table 'PurchaseOrderTransaction'
ALTER TABLE [dbo].[PurchaseOrderTransaction]
ADD CONSTRAINT [FK_dbo_PurchaseOrderTransaction_Supplier]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Supplier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_PurchaseOrderTransaction_Supplier'
CREATE INDEX [IX_FK_dbo_PurchaseOrderTransaction_Supplier]
ON [dbo].[PurchaseOrderTransaction]
    ([SupplierId]);
GO

-- Creating foreign key on [RawMaterialTypeId] in table 'StoreRawMaterial'
ALTER TABLE [dbo].[StoreRawMaterial]
ADD CONSTRAINT [FK_StoreRawMaterial_RawMaterialType]
    FOREIGN KEY ([RawMaterialTypeId])
    REFERENCES [dbo].[RawMaterialType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreRawMaterial_RawMaterialType'
CREATE INDEX [IX_FK_StoreRawMaterial_RawMaterialType]
ON [dbo].[StoreRawMaterial]
    ([RawMaterialTypeId]);
GO

-- Creating foreign key on [RawMaterialTypeId] in table 'StoreRawMaterialOpening'
ALTER TABLE [dbo].[StoreRawMaterialOpening]
ADD CONSTRAINT [FK_StoreRawMaterialOpening_RawMaterialType]
    FOREIGN KEY ([RawMaterialTypeId])
    REFERENCES [dbo].[RawMaterialType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreRawMaterialOpening_RawMaterialType'
CREATE INDEX [IX_FK_StoreRawMaterialOpening_RawMaterialType]
ON [dbo].[StoreRawMaterialOpening]
    ([RawMaterialTypeId]);
GO

-- Creating foreign key on [UnitTypeId] in table 'RawMaterialType'
ALTER TABLE [dbo].[RawMaterialType]
ADD CONSTRAINT [FK_RawMaterialType_UnitType_Foreign_Key]
    FOREIGN KEY ([UnitTypeId])
    REFERENCES [dbo].[UnitType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RawMaterialType_UnitType_Foreign_Key'
CREATE INDEX [IX_FK_RawMaterialType_UnitType_Foreign_Key]
ON [dbo].[RawMaterialType]
    ([UnitTypeId]);
GO

-- Creating foreign key on [RejectedReasonTypeId] in table 'TransactionEntry'
ALTER TABLE [dbo].[TransactionEntry]
ADD CONSTRAINT [FK_TransactionEntry_RejectedReasonType]
    FOREIGN KEY ([RejectedReasonTypeId])
    REFERENCES [dbo].[RejectedReasonType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransactionEntry_RejectedReasonType'
CREATE INDEX [IX_FK_TransactionEntry_RejectedReasonType]
ON [dbo].[TransactionEntry]
    ([RejectedReasonTypeId]);
GO

-- Creating foreign key on [RoleId] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_dbo_UserRole_dbo_Role_Role_Id]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Role]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_UserRole_dbo_Role_Role_Id'
CREATE INDEX [IX_FK_dbo_UserRole_dbo_Role_Role_Id]
ON [dbo].[UserRole]
    ([RoleId]);
GO

-- Creating foreign key on [RoleId] in table 'UserRoleHistory'
ALTER TABLE [dbo].[UserRoleHistory]
ADD CONSTRAINT [FK_dbo_UserRoleHistory_dbo_Role_Role_Id]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Role]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_UserRoleHistory_dbo_Role_Role_Id'
CREATE INDEX [IX_FK_dbo_UserRoleHistory_dbo_Role_Role_Id]
ON [dbo].[UserRoleHistory]
    ([RoleId]);
GO

-- Creating foreign key on [RoleId] in table 'RolePolicy'
ALTER TABLE [dbo].[RolePolicy]
ADD CONSTRAINT [FK_RolePolicy_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Role]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePolicy_Role'
CREATE INDEX [IX_FK_RolePolicy_Role]
ON [dbo].[RolePolicy]
    ([RoleId]);
GO

-- Creating foreign key on [RoleId] in table 'RolePolicyHistory'
ALTER TABLE [dbo].[RolePolicyHistory]
ADD CONSTRAINT [FK_RolePolicyHistory_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Role]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePolicyHistory_Role'
CREATE INDEX [IX_FK_RolePolicyHistory_Role]
ON [dbo].[RolePolicyHistory]
    ([RoleId]);
GO

-- Creating foreign key on [SalesDivisionId] in table 'SalesArea'
ALTER TABLE [dbo].[SalesArea]
ADD CONSTRAINT [FK_SalesArea_SalesDivision]
    FOREIGN KEY ([SalesDivisionId])
    REFERENCES [dbo].[SalesDivision]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesArea_SalesDivision'
CREATE INDEX [IX_FK_SalesArea_SalesDivision]
ON [dbo].[SalesArea]
    ([SalesDivisionId]);
GO

-- Creating foreign key on [SalesAreaId] in table 'SalesBase'
ALTER TABLE [dbo].[SalesBase]
ADD CONSTRAINT [FK_SalesBase_SalesArea]
    FOREIGN KEY ([SalesAreaId])
    REFERENCES [dbo].[SalesArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesBase_SalesArea'
CREATE INDEX [IX_FK_SalesBase_SalesArea]
ON [dbo].[SalesBase]
    ([SalesAreaId]);
GO

-- Creating foreign key on [ReceivedBy] in table 'StoreRawMaterial'
ALTER TABLE [dbo].[StoreRawMaterial]
ADD CONSTRAINT [FK_StoreRawMaterial_User]
    FOREIGN KEY ([ReceivedBy])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreRawMaterial_User'
CREATE INDEX [IX_FK_StoreRawMaterial_User]
ON [dbo].[StoreRawMaterial]
    ([ReceivedBy]);
GO

-- Creating foreign key on [SystemWarningTypeId] in table 'SystemWarning'
ALTER TABLE [dbo].[SystemWarning]
ADD CONSTRAINT [FK_SystemWarning_SystemWarningType_SystemWarningTypeId]
    FOREIGN KEY ([SystemWarningTypeId])
    REFERENCES [dbo].[SystemWarningType]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemWarning_SystemWarningType_SystemWarningTypeId'
CREATE INDEX [IX_FK_SystemWarning_SystemWarningType_SystemWarningTypeId]
ON [dbo].[SystemWarning]
    ([SystemWarningTypeId]);
GO

-- Creating foreign key on [SystemWarningId] in table 'SystemWarningHistory'
ALTER TABLE [dbo].[SystemWarningHistory]
ADD CONSTRAINT [FK_SystemWarningHistory_SystemWarning_SystemWarningId]
    FOREIGN KEY ([SystemWarningId])
    REFERENCES [dbo].[SystemWarning]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemWarningHistory_SystemWarning_SystemWarningId'
CREATE INDEX [IX_FK_SystemWarningHistory_SystemWarning_SystemWarningId]
ON [dbo].[SystemWarningHistory]
    ([SystemWarningId]);
GO

-- Creating foreign key on [SystemWarningTypeId] in table 'SystemWarningHistory'
ALTER TABLE [dbo].[SystemWarningHistory]
ADD CONSTRAINT [FK_SystemWarningHistory_SystemWarningType_SystemWarningTypeId]
    FOREIGN KEY ([SystemWarningTypeId])
    REFERENCES [dbo].[SystemWarningType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemWarningHistory_SystemWarningType_SystemWarningTypeId'
CREATE INDEX [IX_FK_SystemWarningHistory_SystemWarningType_SystemWarningTypeId]
ON [dbo].[SystemWarningHistory]
    ([SystemWarningTypeId]);
GO

-- Creating foreign key on [TransactionEntryId] in table 'TransactionDetail'
ALTER TABLE [dbo].[TransactionDetail]
ADD CONSTRAINT [FK_dbo_TransactionDetail_dbo_TransactionEntry_TransactionEntryId]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntry]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionDetail_dbo_TransactionEntry_TransactionEntryId'
CREATE INDEX [IX_FK_dbo_TransactionDetail_dbo_TransactionEntry_TransactionEntryId]
ON [dbo].[TransactionDetail]
    ([TransactionEntryId]);
GO

-- Creating foreign key on [TransactionEntryId] in table 'TransactionDetailHistory'
ALTER TABLE [dbo].[TransactionDetailHistory]
ADD CONSTRAINT [FK_dbo_TransactionDetailHistory_dbo_TransactionEntryHistory_TransactionEntryHistoryId]
    FOREIGN KEY ([TransactionEntryId])
    REFERENCES [dbo].[TransactionEntryHistory]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionDetailHistory_dbo_TransactionEntryHistory_TransactionEntryHistoryId'
CREATE INDEX [IX_FK_dbo_TransactionDetailHistory_dbo_TransactionEntryHistory_TransactionEntryHistoryId]
ON [dbo].[TransactionDetailHistory]
    ([TransactionEntryId]);
GO

-- Creating foreign key on [TransactionTypeId] in table 'TransactionEntry'
ALTER TABLE [dbo].[TransactionEntry]
ADD CONSTRAINT [FK_dbo_TransactionEntry_dbo_TransactionType_TransactionTypeId]
    FOREIGN KEY ([TransactionTypeId])
    REFERENCES [dbo].[TransactionType]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionEntry_dbo_TransactionType_TransactionTypeId'
CREATE INDEX [IX_FK_dbo_TransactionEntry_dbo_TransactionType_TransactionTypeId]
ON [dbo].[TransactionEntry]
    ([TransactionTypeId]);
GO

-- Creating foreign key on [TransactionTypeId] in table 'TransactionEntryHistory'
ALTER TABLE [dbo].[TransactionEntryHistory]
ADD CONSTRAINT [FK_dbo_TransactionEntryHistory_dbo_TransactionType_TransactionTypeId]
    FOREIGN KEY ([TransactionTypeId])
    REFERENCES [dbo].[TransactionType]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TransactionEntryHistory_dbo_TransactionType_TransactionTypeId'
CREATE INDEX [IX_FK_dbo_TransactionEntryHistory_dbo_TransactionType_TransactionTypeId]
ON [dbo].[TransactionEntryHistory]
    ([TransactionTypeId]);
GO

-- Creating foreign key on [UserId] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_dbo_UserRole_dbo_User_User_Id]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_UserRole_dbo_User_User_Id'
CREATE INDEX [IX_FK_dbo_UserRole_dbo_User_User_Id]
ON [dbo].[UserRole]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserRoleHistory'
ALTER TABLE [dbo].[UserRoleHistory]
ADD CONSTRAINT [FK_dbo_UserRoleHistory_dbo_User_User_Id]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_UserRoleHistory_dbo_User_User_Id'
CREATE INDEX [IX_FK_dbo_UserRoleHistory_dbo_User_User_Id]
ON [dbo].[UserRoleHistory]
    ([UserId]);
GO

-- Creating foreign key on [StatusId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [FK_User_UserStatus]
    FOREIGN KEY ([StatusId])
    REFERENCES [dbo].[UserStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_UserStatus'
CREATE INDEX [IX_FK_User_UserStatus]
ON [dbo].[User]
    ([StatusId]);
GO

-- Creating foreign key on [UserId] in table 'UserActivityLog'
ALTER TABLE [dbo].[UserActivityLog]
ADD CONSTRAINT [FK_UserActivityLog_ToUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserActivityLog_ToUser'
CREATE INDEX [IX_FK_UserActivityLog_ToUser]
ON [dbo].[UserActivityLog]
    ([UserId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [Policy_Id] in table 'PolicyRouteResource'
ALTER TABLE [dbo].[PolicyRouteResource]
ADD CONSTRAINT [FK_PolicyRouteResource_Policy]
    FOREIGN KEY ([Policy_Id])
    REFERENCES [dbo].[Policy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RouteResource_Id] in table 'PolicyRouteResource'
ALTER TABLE [dbo].[PolicyRouteResource]
ADD CONSTRAINT [FK_PolicyRouteResource_RouteResource]
    FOREIGN KEY ([RouteResource_Id])
    REFERENCES [dbo].[RouteResource]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyRouteResource_RouteResource'
CREATE INDEX [IX_FK_PolicyRouteResource_RouteResource]
ON [dbo].[PolicyRouteResource]
    ([RouteResource_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------