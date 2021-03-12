/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--https://documentation.red-gate.com/sca/developing-databases/working-with-the-visual-studio-extension/advanced-scenarios-for-visual-studio/using-seed-data-in-visual-studio

--BULK INSERT [CustomerAccount]  
--FROM '$(DeployPath)TransactionHistoryData.csv'  
--WITH (FIELDTERMINATOR = ',', FIRSTROW = 2);