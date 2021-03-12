CREATE TABLE [dbo].[CustomerAccount]
(
	[AccountId] INT NOT NULL IDENTITY(1,1),
	[FirstName]	NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL
	CONSTRAINT [PK_CustomerAccount] PRIMARY KEY CLUSTERED ([AccountId] ASC)
)
