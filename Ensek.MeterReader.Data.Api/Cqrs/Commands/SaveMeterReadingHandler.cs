using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ensek.MeterReading.Api.DataClient.Enums;
using Ensek.MeterReading.Data.Api.Database.Entities;
using Ensek.MeterReading.Data.Api.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ensek.MeterReading.Data.Api.Cqrs.Commands
{

	public class SaveMeterReadingRequest : IRequest<SubmitMeterReadingResponseEnum>
	{
		public SaveMeterReadingRequest(Database.Entities.MeterReading reading)
		{
			Reading = reading;
		}

		public Database.Entities.MeterReading Reading { get; }
	}

	public class SaveMeterReadingHandler : IRequestHandler<SaveMeterReadingRequest, SubmitMeterReadingResponseEnum>
	{
		private readonly ILogger<SaveMeterReadingHandler> _logger;
		private readonly IRepository<CustomerAccount> _customerAccountRepository;
		private readonly IRepository<Database.Entities.MeterReading> _meterReadingRepository;

		public SaveMeterReadingHandler(
			ILogger<SaveMeterReadingHandler> logger,
			IRepository<CustomerAccount> customerAccountRepository,
			IRepository<Database.Entities.MeterReading> meterReadingRepository)
		{
			_logger = logger;
			_customerAccountRepository = customerAccountRepository;
			_meterReadingRepository = meterReadingRepository;
		}

		public async Task<SubmitMeterReadingResponseEnum> Handle(SaveMeterReadingRequest request, CancellationToken cancellationToken)
		{
			if (request?.Reading == null)
			{
				throw new ArgumentException("Reading cannot be null");
			}

			try
			{
				// does account exist
				if (!await _customerAccountRepository.Any(x => x.AccountId == request.Reading.AccountId))
				{
					return SubmitMeterReadingResponseEnum.AccountNotFound;
				}

				// has reading already been submitted?
				if (await _meterReadingRepository.Any(x => x.AccountId == request.Reading.AccountId &&
															x.MeterReadValue == request.Reading.MeterReadValue &&
															x.MeterReadingDateTime == request.Reading.MeterReadingDateTime))
				{
					return SubmitMeterReadingResponseEnum.DuplicateReading;
				}

				// store the reading
				_ = await _meterReadingRepository.Add(request.Reading);
				_ = await _meterReadingRepository.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failure while trying to save meter reading for Account {accountId} for date {readingDate}", request.Reading.AccountId, request.Reading.MeterReadingDateTime);
				return SubmitMeterReadingResponseEnum.Failure;
			}

			return SubmitMeterReadingResponseEnum.Success;
		}
	}
}
