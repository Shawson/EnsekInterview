using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Ensek.MeterReading.Data.Api.Controllers;
using Ensek.MeterReading.Data.Client.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Ensek.MeterReading.Data.Api.Tests.Controllers
{

	public class MeterReadingControllerTests
	{
		private ILogger<MeterReadingController> _logger;
		private IMapper _mapper;
		private IMediator _mediator;
		private MeterReadingController _subject;

		[SetUp]
		public void Setup()
		{
			_logger = Substitute.For<ILogger<MeterReadingController>>();
			_mapper = Substitute.For<IMapper>();
			_mediator = Substitute.For<IMediator>();

			_subject = new MeterReadingController(_logger, _mapper, _mediator);
		}

		public static List<MeterReadingTestCase> ReturnCodeTestCases => new List<MeterReadingTestCase> {
			new MeterReadingTestCase
			{
				TestName = "Null Reading",
				Dto = null,
				ExpectedStatusCode = 400,
				ExpectedMessage = "Reading cannot be null"
			},
			new MeterReadingTestCase
			{
				TestName = "Invalid Account Id",
				Dto = new MeterReadingDto {
					AccountId = 0
				},
				ExpectedStatusCode = 400,
				ExpectedMessage = "Invalid account number"
			},
			new MeterReadingTestCase
			{
				TestName = "Negative Account Id",
				Dto = new MeterReadingDto {
					AccountId = -1
				},
				ExpectedStatusCode = 400,
				ExpectedMessage = "Invalid account number"
			},
			new MeterReadingTestCase
			{
				TestName = "Invalid meter reading",
				Dto = new MeterReadingDto {
					AccountId = 1234,
					MeterReadValue = -1
				},
				ExpectedStatusCode = 400,
				ExpectedMessage = "Invalid reading value"
			}
		};
	
		[TestCaseSource(nameof(ReturnCodeTestCases))]
		public async Task Return_code_correct(MeterReadingTestCase testCase)
		{
			var response = await _subject.Post(testCase.Dto);
			Assert.AreEqual(testCase.ExpectedStatusCode, ((ObjectResult)response.Result).StatusCode);
		}

		[TestCaseSource(nameof(ReturnCodeTestCases))]
		public async Task Null_file_returns_message(MeterReadingTestCase testCase)
		{
			var response = await _subject.Post(testCase.Dto);
			Assert.AreEqual(testCase.ExpectedMessage, ((ObjectResult)response.Result).Value.ToString());
		}
	}
}
