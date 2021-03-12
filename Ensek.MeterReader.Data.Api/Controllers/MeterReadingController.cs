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

        public MeterReadingController(ILogger<MeterReadingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<bool>> Post(MeterReadingDto reading)
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

            // does account exist


            // has reading already been submitted?

            // store the reading

            return true;
        }
    }
}
