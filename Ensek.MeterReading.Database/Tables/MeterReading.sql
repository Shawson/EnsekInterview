CREATE TABLE [dbo].[MeterReading]
(
	[MeterReadingId] INT NOT NULL IDENTITY(1,1),
	[AccountId]	INT NOT NULL,
	[MeterReadingDateTime] DateTime2 NOT NULL,
	[MeterReadValue] INT NOT NULL,
	CONSTRAINT [PK_MeterReading] PRIMARY KEY CLUSTERED ([MeterReadingId] ASC),
	CONSTRAINT [FK_MeterReading_CustomerAccount] FOREIGN KEY ([AccountId]) REFERENCES [CustomerAccount] ([AccountId])
)
