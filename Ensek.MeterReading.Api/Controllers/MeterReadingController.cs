using Ensek.MeterReading.Api.Cqrs.Commands;
using Ensek.MeterReading.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class MeterReadingController : ControllerBase
    {
        private static readonly int MaxFileSizeBytes = 1048576;

        private readonly ILogger<MeterReadingController> _logger;
        private readonly IMediator _mediator;

        public MeterReadingController(
            ILogger<MeterReadingController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("meter-reading-uploads")]
        public async Task<ActionResult<MeterReadingUploadResult>> Post(IFormFile file)
        {
            if (file == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "No file submitted");
            }

            if (file.Length > MaxFileSizeBytes)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"File larger than max size of {MaxFileSizeBytes} bytes");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();

                    var csvString = Encoding.UTF8.GetString(bytes);

                    using (TextReader reader = new StringReader(csvString))
                    {
                        var meterReaderingRecords = await _mediator.Send(new ParseMeterReadingCsvFileRequest(reader));

                        var failureCount = await _mediator.Send(new StoreMeterReadingRecordsRequest(meterReaderingRecords.ValidRows));

                        return new MeterReadingUploadResult
                        {
                            SuccessfulRows = meterReaderingRecords.ValidRows.Count - failureCount,
                            ErrorRows = meterReaderingRecords.Errors.Count + failureCount
						};
                    }
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception during CSV upload of file {filename}", file.FileName);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
