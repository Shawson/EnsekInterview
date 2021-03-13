using Ensek.MeterReading.Api.DataClient.Enums;
using Ensek.MeterReading.Data.Client.Dtos;
using RestEase;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.DataClient
{
    public interface IMeterReadingDataService
    {
        [Header("X-MeterReadingData-ApiKey")]
        string ApiKey { get; set; }

        [Post("api/MeterReading")]
        Task<SubmitMeterReadingResponseEnum> SubmitReading([Body] MeterReadingDto model);
    }


}
