using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ensek.MeterReading.Api.DataClient.Enums;
using Ensek.MeterReading.Data.Api.Cqrs.Commands;
using Ensek.MeterReading.Data.Api.Database.Entities;
using Ensek.MeterReading.Data.Api.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Ensek.MeterReading.Data.Api.Tests
{
    public class SaveMeterReadingHandlerTests
    {
		private ILogger<SaveMeterReadingHandler> _logger;
		private IRepository<CustomerAccount> _customerAccountRepository;
		private IRepository<Database.Entities.MeterReading> _meterReadingRepository;
		private SaveMeterReadingHandler _subject;

		[SetUp]
        public void Setup()
        {
			_logger = Substitute.For<ILogger<SaveMeterReadingHandler>>();
			_customerAccountRepository = Substitute.For<IRepository<CustomerAccount>>();
			_meterReadingRepository = Substitute.For<IRepository<Database.Entities.MeterReading>>();

			_subject = new SaveMeterReadingHandler(_logger, _customerAccountRepository, _meterReadingRepository);
		}

		[Test]
		public void Arg_Exception_if_request_is_null()
		{
			Assert.ThrowsAsync<ArgumentException>(async () =>
				await _subject.Handle(new SaveMeterReadingRequest(null), CancellationToken.None)
			);
		}

		[Test]
        public async Task Reading_rejected_if_account_doesnt_exist()
        {
			_customerAccountRepository
				.Any(Arg.Any<Expression<Func<CustomerAccount, bool>>>())
				.Returns(false);

			var response = await _subject.Handle(
				new SaveMeterReadingRequest(new Database.Entities.MeterReading
				{
					AccountId = 1234,
					MeterReadValue = 5678
				}),
				CancellationToken.None);

			Assert.AreEqual(SubmitMeterReadingResponseEnum.AccountNotFound, response);
        }

		[Test]
		public async Task Reading_rejected_if_meter_reading_is_a_dupe()
		{
			_customerAccountRepository
				.Any(Arg.Any<Expression<Func<CustomerAccount, bool>>>())
				.Returns(true);

			_meterReadingRepository
				.Any(Arg.Any<Expression<Func<Database.Entities.MeterReading, bool>>>())
				.Returns(true);

			var response = await _subject.Handle(
				new SaveMeterReadingRequest(new Database.Entities.MeterReading
				{
					AccountId = 1234,
					MeterReadValue = 5678
				}),
				CancellationToken.None);

			Assert.AreEqual(SubmitMeterReadingResponseEnum.DuplicateReading, response);
		}

		[Test]
		public async Task Reading_added_if_validation_passed()
		{
			_customerAccountRepository
				.Any(Arg.Any<Expression<Func<CustomerAccount, bool>>>())
				.Returns(true);

			_meterReadingRepository
				.Any(Arg.Any<Expression<Func<Database.Entities.MeterReading, bool>>>())
				.Returns(false);

			_ = await _subject.Handle(
				new SaveMeterReadingRequest(new Database.Entities.MeterReading
				{
					AccountId = 1234,
					MeterReadValue = 5678
				}),
				CancellationToken.None);

			await _meterReadingRepository
				.Received()
				.Add(Arg.Any<Database.Entities.MeterReading>());
		}

		[Test]
		public async Task Save_changes_called_on_meter_reading_repo()
		{
			_customerAccountRepository
				.Any(Arg.Any<Expression<Func<CustomerAccount, bool>>>())
				.Returns(true);

			_meterReadingRepository
				.Any(Arg.Any<Expression<Func<Database.Entities.MeterReading, bool>>>())
				.Returns(false);

			_ = await _subject.Handle(
				new SaveMeterReadingRequest(new Database.Entities.MeterReading
				{
					AccountId = 1234,
					MeterReadValue = 5678
				}),
				CancellationToken.None);

			await _meterReadingRepository
				.Received()
				.SaveChangesAsync();
		}

		[Test]
		public async Task Successful_add_returns_success_code()
		{
			_customerAccountRepository
				.Any(Arg.Any<Expression<Func<CustomerAccount, bool>>>())
				.Returns(true);

			_meterReadingRepository
				.Any(Arg.Any<Expression<Func<Database.Entities.MeterReading, bool>>>())
				.Returns(false);

			var response = await _subject.Handle(
				new SaveMeterReadingRequest(new Database.Entities.MeterReading
				{
					AccountId = 1234,
					MeterReadValue = 5678
				}),
				CancellationToken.None);

			Assert.AreEqual(SubmitMeterReadingResponseEnum.Success, response);
		}
	}
}