using Ensek.MeterReading.Api.DataClient.Enums;
using Ensek.MeterReading.Data.Api.Database.Entities;
using Ensek.MeterReading.Data.Api.Repository;
using Ensek.MeterReading.Data.Client.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Data.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : ControllerBase
    {

        private readonly ILogger<MeterReadingController> _logger;
		private readonly IRepository<CustomerAccount> _customerAccountRepository;
		private readonly IRepository<Database.Entities.MeterReading> _meterReadingRepository;

		public MeterReadingController(
			ILogger<MeterReadingController> logger,
			IRepository<CustomerAccount> customerAccountRepository,
			IRepository<Database.Entities.MeterReading> meterReadingRepository)
        {
            _logger = logger;
			_customerAccountRepository = customerAccountRepository;
			_meterReadingRepository = meterReadingRepository;
		}

        [HttpPost]
        public async Task<ActionResult<SubmitMeterReadingResponseEnum>> Post(MeterReadingDto reading)
        {
            // is reading null
            if (reading == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reading cannot be null");
            }

            if (reading.AccountId < 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid account number");
            }

            if (reading.MeterReadValue < 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid reading value");
            }

            
			try
			{
				// does account exist
				if (!await _customerAccountRepository.Any(x => x.AccountId == reading.AccountId))
				{
					return SubmitMeterReadingResponseEnum.AccountNotFound;
				}

				// has reading already been submitted?
				if (await _meterReadingRepository.Any(x => x.AccountId == reading.AccountId &&
															x.MeterReadValue == reading.MeterReadValue &&
															x.MeterReadingDateTime == reading.MeterReadingDateTime))
				{
					return SubmitMeterReadingResponseEnum.DuplicateReading;
				}

				// store the reading

				_meterReadingRepository.Add(reading);
				await _meterReadingRepository.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failure while trying to save meter reading for Account {accountId} for date {readingDate}", reading.AccountId, reading.MeterReadingDateTime);
				return SubmitMeterReadingResponseEnum.Failure;
			}

            return SubmitMeterReadingResponseEnum.Success;
        }
    }
}
