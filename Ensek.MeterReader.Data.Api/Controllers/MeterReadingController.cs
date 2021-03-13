using AutoMapper;
using Ensek.MeterReading.Api.DataClient.Enums;
using Ensek.MeterReading.Data.Api.Cqrs.Commands;
using Ensek.MeterReading.Data.Api.Database.Entities;
using Ensek.MeterReading.Data.Api.Repository;
using Ensek.MeterReading.Data.Client.Dtos;
using MediatR;
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
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;
	

		public MeterReadingController(
			ILogger<MeterReadingController> logger,
			IMapper mapper,
			IMediator mediator
			)
        {
            _logger = logger;
			_mapper = mapper;
			_mediator = mediator;
			
		}

        [HttpPost]
        public async Task<ActionResult<SubmitMeterReadingResponseEnum>> Post(MeterReadingDto readingDto)
        {
            // is reading null
            if (readingDto == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reading cannot be null");
            }

            if (readingDto.AccountId < 1)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid account number");
            }

            if (readingDto.MeterReadValue < 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid reading value");
            }

			var reading = _mapper.Map<Database.Entities.MeterReading>(readingDto);

			return await _mediator.Send(new SaveMeterReadingRequest(reading));
        }
    }
}
