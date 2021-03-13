using Ensek.MeterReading.Data.Client.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Data.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerAcccountController : ControllerBase
    {

        private readonly ILogger<CustomerAcccountController> _logger;

        public CustomerAcccountController(ILogger<CustomerAcccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{accountNumber:int}")]
        public async Task<CustomAccountDto> Get(int accountNumber)
        {
            return new CustomAccountDto();
        }
    }
}
