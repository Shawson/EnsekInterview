using Ensek.MeterReading.Api.DataClient.Enums;
using Ensek.MeterReading.Data.Client.Dtos;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.DataClient
{
    public interface IMeterReadingDataService
    {
        [Header("X-MeterReadingData-Token")]
        string ApiKey { get; set; }

        [Post("api/MeterReading")]
        Task<SubmitMeterReadingResponseEnum> SubmitReading([Body] MeterReadingDto model);
    }


}
