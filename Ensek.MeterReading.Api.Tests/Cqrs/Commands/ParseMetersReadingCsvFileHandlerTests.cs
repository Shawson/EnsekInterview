using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using Ensek.MeterReading.Api.Cqrs.Commands;
using NUnit.Framework;

namespace Ensek.MeterReading.Api.Tests.Cqrs.Commands
{
	public class ParseMetersReadingCsvFileHandlerTests
	{
		private TextReader _textReader;
		private ParseMeterReadingCsvFileHandler _subject;

		[SetUp]
		public void Setup()
		{
			_subject = new ParseMeterReadingCsvFileHandler();
		}

		[TearDown]
		public void TearDown()
		{
			if (_textReader != null)
			{
				_textReader.Close();
				_textReader.Dispose();
			}
		}

		[TestCase(TestCsvFiles.FiveValidRows, 5, TestName = "Correct Valid Result Count : CSV File with 5 valid rows only")]
		[TestCase(TestCsvFiles.FiveInvalidRows, 0, TestName = "Correct Valid Result Count : CSV File with 5 invalid rows only")]
		[TestCase(TestCsvFiles.CompleteFile, 29, TestName = "Correct Valid Result Count : Ensek sample CSV File with 29 valid rows")]
		[TestCase(TestCsvFiles.TenMixedRows, 5, TestName = "Correct Valid Result Count : CSV File with 5 valid and 5 invalid rows")]
		public async Task Csv_parser_extracts_correct_number_of_valid_rows(string csvContent, int rowCount)
		{
			_textReader = new StringReader(csvContent);

			var response = await _subject.Handle(new ParseMeterReadingCsvFileRequest(_textReader), CancellationToken.None);

			Assert.AreEqual(rowCount, response.ValidRows.Count);
		}

		[TestCase(TestCsvFiles.FiveValidRows, 0, TestName = "Correct Invalid Result Count : CSV File with 5 valid rows only")]
		[TestCase(TestCsvFiles.FiveInvalidRows, 5, TestName = "Correct Invalid Result Count : CSV File with 5 invalid rows only")]
		[TestCase(TestCsvFiles.CompleteFile, 6, TestName = "Correct Invalid Result Count : Ensek sample CSV File with 6 invalid rows")]
		[TestCase(TestCsvFiles.TenMixedRows, 5, TestName = "Correct Invalid Result Count : CSV File with 5 valid and 5 invalid rows")]
		[TestCase(TestCsvFiles.BadMeterReadingFormats, 4, TestName = "Correct Invalid Result Count : CSV File with 4 invalid meter reading formats")]
		[TestCase(TestCsvFiles.MissingOrInvalidAccountNumbers, 2, TestName = "Correct Invalid Result Count : CSV File with 2 missing or invalid account numbers")]
		[TestCase(TestCsvFiles.BadDateTimeFormats, 4, TestName = "Correct Invalid Result Count : CSV File with 4 bad date time formats")]
		public async Task Csv_parser_identifies_correct_number_of_errors(string csvContent, int rowCount)
		{
			_textReader = new StringReader(csvContent);

			var response = await _subject.Handle(new ParseMeterReadingCsvFileRequest(_textReader), CancellationToken.None);

			Assert.AreEqual(rowCount, response.Errors.Count);
		}

		[Test]
		public void Empty_file_throws_argument_exception()
		{
			_textReader = new StringReader(TestCsvFiles.EmptyFile);

			Assert.ThrowsAsync<MalformedFileException>(async () =>
				await _subject.Handle(new ParseMeterReadingCsvFileRequest(_textReader), CancellationToken.None)
			);
		}

		[Test]
		public void Arg_exception_if_reader_is_null()
		{
			Assert.ThrowsAsync<ArgumentException>(async () =>
				await _subject.Handle(new ParseMeterReadingCsvFileRequest(null), CancellationToken.None)
			);
		}
	}

	public static class TestCsvFiles
	{
		public const string EmptyFile = @"";

		public const string FiveValidRows = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,01002
2233,22/04/2019 12:25,00323
8766,22/04/2019 12:25,03440
2344,22/04/2019 12:25,01002
2345,22/04/2019 12:25,45522";

		public const string BadMeterReadingFormats = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,ABCDE
2233,22/04/2019 12:25,999999
8766,22/04/2019 12:25,123,
8766,22/04/2019 12:25,";

		public const string MissingOrInvalidAccountNumbers = @"AccountId,MeterReadingDateTime,MeterReadValue
,22/04/2019 09:24,ABCDE
ABC,22/04/2019 12:25,999999";

		public const string BadDateTimeFormats = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,2019-04-22 09:24,01002
2233,2019/04/22 12:25,00323
8766,04/22/2019 12:25,03440
2344,22/04/2019 9:25,01002";

		public const string FiveInvalidRows = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,0A002
2233,22/04/2019 12:25,
8766,22/04/2019 12:25,abc
2344,22/04/2019 12:25,VOID
2345,22/04/2019 12:25,999999";

		public const string TenMixedRows = @"AccountId,MeterReadingDateTime,MeterReadValue
2344,22/04/2019 09:24,0A002
2344,22/04/2019 09:24,01002
2233,22/04/2019 12:25,
2233,22/04/2019 12:25,00323
8766,22/04/2019 12:25,abc
8766,22/04/2019 12:25,03440
2344,22/04/2019 12:25,VOID
2344,22/04/2019 12:25,01002
2345,22/04/2019 12:25,999999
2345,22/04/2019 12:25,45522";

		public const string CompleteFile = @"AccountId,MeterReadingDateTime,MeterReadValue
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
