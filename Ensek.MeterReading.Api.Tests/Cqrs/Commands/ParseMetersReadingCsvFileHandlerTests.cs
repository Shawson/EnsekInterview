using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Ensek.MeterReading.Api.Tests.Cqrs.Commands
{
	public class ParseMetersReadingCsvFileHandlerTests
	{
		private TextReader _reader;
		private ParseMeterReadingCsvFileHandler _subject;

		[SetUp]
		public void Setup()
		{
			_subject = new ParseMeterReadingCsvFileHandler();
		}

		[TearDown]
		public void TearDown()
		{
			_reader.Close();
			_reader.Dispose();
		}

		[TestCase(TestCsvFiles.FiveValidRows, 5)]
		[TestCase(TestCsvFiles.CompleteFile, 25)]
		public async Task Csv_parser_extracts_correct_number_of_valid_rows(string csvContent, int validRowCount)
		{
			_textReader = new StringReader(csvContent);

			var response = await _subject.Handle(new ParseMeterReadingCsvFileRequest(_reader));

			Assert.AreEqual(validRowCount, response.ValidRows.Count);
		}

		[Test]
		public async Task Csv_parser_identifies_correct_number_of_errors()
		{

		}
	}

	public static class TestCsvFiles
	{
		public static string FiveValidRows = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,01002
2233,22/04/2019 12:25,00323
8766,22/04/2019 12:25,03440
2344,22/04/2019 12:25,01002
2345,22/04/2019 12:25,45522";

		public static string FiveInvalidRows = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,0A002
2233,22/04/2019 12:25,
8766,22/04/2019 12:25,abc
2344,22/04/2019 12:25,VOID
2345,22/04/2019 12:25,999999";

		public static string CompleteFile = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,01002
2233,22/04/2019 12:25,00323
8766,22/04/2019 12:25,03440
2344,22/04/2019 12:25,01002
2345,22/04/2019 12:25,45522
2346,22/04/2019 12:25,999999
2347,22/04/2019 12:25,00054
2348,22/04/2019 12:25,00123
2349,22/04/2019 12:25,VOID
2350,22/04/2019 12:25,05684
2351,22/04/2019 12:25,57579
2352,22/04/2019 12:25,00455
2353,22/04/2019 12:25,01212
2354,22/04/2019 12:25,00889
2355,06/05/2019 09:24,00001
2356,07/05/2019 09:24,00000
2344,08/05/2019 09:24,0X765
6776,09/05/2019 09:24,-06575
6776,10/05/2019 09:24,23566
4534,11/05/2019 09:24,
1234,12/05/2019 09:24,09787
1235,13/05/2019 09:24,
1236,10/04/2019 19:34,08898
1237,15/05/2019 09:24,03455
1238,16/05/2019 09:24,00000
1239,17/05/2019 09:24,45345
1240,18/05/2019 09:24,00978
1241,11/04/2019 09:24,00436,X
1242,20/05/2019 09:24,00124
1243,21/05/2019 09:24,00077
1244,25/05/2019 09:24,03478
1245,25/05/2019 14:26,00676
1246,25/05/2019 09:24,03455
1247,25/05/2019 09:24,00003
1248,26/05/2019 09:24,03467";
	}
}
