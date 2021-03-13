using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ensek.MeterReading.Api.Controllers;
using Ensek.MeterReading.Api.Cqrs.Commands;
using Ensek.MeterReading.Data.Client.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Ensek.MeterReading.Api.Tests
{
    public class MeterReadingControllerTests
    {
		private ILogger<MeterReadingController> _logger;
		private IMediator _mediator;
		private MeterReadingController _subject;

        [SetUp]
        public void Setup()
        {
			_logger = Substitute.For<ILogger<MeterReadingController>>();
			_mediator = Substitute.For<IMediator>();
			_subject = new MeterReadingController(_logger, _mediator);
        }

		[Test]
		public async Task Null_file_returns_400()
		{
			var response = await _subject.Post(null);
			Assert.AreEqual(400, ((ObjectResult)response.Result).StatusCode);
		}

		[Test]
		public async Task Null_file_returns_message()
		{
			var response = await _subject.Post(null);
			Assert.AreEqual("No file submitted", ((ObjectResult)response.Result).Value.ToString());
		}

		[Test]
		public async Task Too_large_file_returns_400()
		{
			var response = await _subject.Post(FormFileFactory.Get("test", 2000000, "big.csv", "text/csv"));
			Assert.AreEqual(400, ((ObjectResult)response.Result).StatusCode);
		}

		[Test]
		public async Task Too_large_file_returns_message()
		{
			var response = await _subject.Post(FormFileFactory.Get("test", 2000000, "big.csv", "text/csv"));
			Assert.AreEqual("File larger than max size of 1048576 bytes", ((ObjectResult)response.Result).Value.ToString());
		}

		[Test]
		public async Task Wrong_format_file_returns_400()
		{
			var response = await _subject.Post(FormFileFactory.Get("test", 300, "big.csv", "text/plain"));
			Assert.AreEqual(400, ((ObjectResult)response.Result).StatusCode);
		}

		[Test]
		public async Task Wrong_format_file_returns_message()
		{
			var response = await _subject.Post(FormFileFactory.Get("test", 300, "big.csv", "text/plain"));
			Assert.AreEqual("File must be in CSV format", ((ObjectResult)response.Result).Value.ToString());
		}

		[Test]
		public async Task Malformed_file_error_returns_400()
		{
			_mediator
				.Send(Arg.Any<ParseMeterReadingCsvFileRequest>(), Arg.Any<CancellationToken>())
				.Returns<Task<ParseCsvFileResult<MeterReadingDto>>>(x => { throw new MalformedFileException("error message"); });

			var response = await _subject.Post(FormFileFactory.Get("test", 300, "big.csv", "text/csv"));

			Assert.AreEqual(400, ((ObjectResult)response.Result).StatusCode);
		}

		[Test]
		public async Task Malformed_file_error_returns_message()
		{
			_mediator
				.Send(Arg.Any<ParseMeterReadingCsvFileRequest>(), Arg.Any<CancellationToken>())
				.Returns<Task<ParseCsvFileResult<MeterReadingDto>>>(x => { throw new MalformedFileException("error message"); });

			var response = await _subject.Post(FormFileFactory.Get("test", 300, "big.csv", "text/csv"));

			Assert.AreEqual("error message", ((ObjectResult)response.Result).Value.ToString());
		}

		[Test]
		public async Task Correct_success_returned()
		{
			_mediator
				.Send(Arg.Any<ParseMeterReadingCsvFileRequest>(), Arg.Any<CancellationToken>())
				.Returns(new ParseCsvFileResult<MeterReadingDto> { 
					Errors = new System.Collections.Generic.List<CsvRowError> { 
						new CsvRowError(),
						new CsvRowError(),
						new CsvRowError(),
						new CsvRowError(),
						new CsvRowError(),
					},
					ValidRows = new System.Collections.Generic.List<MeterReadingDto>
					{
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto()
					}
				});

			_mediator
				.Send(Arg.Any<StoreMeterReadingRecordsRequest>(), Arg.Any<CancellationToken>())
				.Returns(1);

			var response = await _subject.Post(FormFileFactory.Get("test", 300, "big.csv", "text/csv"));
			Assert.AreEqual(9, response.Value.SuccessfulRows);
		}

		[Test]
		public async Task Correct_failures_returned()
		{
			_mediator
				.Send(Arg.Any<ParseMeterReadingCsvFileRequest>(), Arg.Any<CancellationToken>())
				.Returns(new ParseCsvFileResult<MeterReadingDto>
				{
					Errors = new System.Collections.Generic.List<CsvRowError> {
						new CsvRowError(),
						new CsvRowError(),
						new CsvRowError(),
						new CsvRowError(),
						new CsvRowError(),
					},
					ValidRows = new System.Collections.Generic.List<MeterReadingDto>
					{
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto(),
						new MeterReadingDto()
					}
				});

			_mediator
				.Send(Arg.Any<StoreMeterReadingRecordsRequest>(), Arg.Any<CancellationToken>())
				.Returns(1);

			var response = await _subject.Post(FormFileFactory.Get("test", 300, "big.csv", "text/csv"));
			Assert.AreEqual(6, response.Value.ErrorRows);
		}
	}

	public static class FormFileFactory
	{
		public static IFormFile Get(string content, int length, string fileName, string mimeType)
		{
			return new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, length, "Data", fileName) 
			{
				Headers = new HeaderDictionary(),
				ContentType = mimeType
			};
		}
	}
}