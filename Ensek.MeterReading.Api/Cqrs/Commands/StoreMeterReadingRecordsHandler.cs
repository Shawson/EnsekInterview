using Ensek.MeterReading.Api.DataClient;
using Ensek.MeterReading.Data.Client.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{

    public class StoreMeterReadingRecordsRequest : IRequest<int>
    {
        public StoreMeterReadingRecordsRequest(List<MeterReadingDto> meterReadings)
        {
			MeterReadings = meterReadings;
		}

		public List<MeterReadingDto> MeterReadings { get; }
	}

    public class StoreMeterReadingRecordsHandler : IRequestHandler<StoreMeterReadingRecordsRequest, int>
    {
		private readonly IMeterReadingDataService _meterReadingDataService;

		public StoreMeterReadingRecordsHandler(IMeterReadingDataService meterReadingDataService)
        {
			_meterReadingDataService = meterReadingDataService;
		}

        public async Task<int> Handle(StoreMeterReadingRecordsRequest request, CancellationToken cancellationToken)
        {
			var failureCount = 0;

			foreach(var reading in request.MeterReadings)
			{
				var responseCode = await _meterReadingDataService.SubmitReading(reading);

				if (responseCode != DataClient.Enums.SubmitMeterReadingResponseEnum.Success)
				{
					failureCount++;
				}
			}

			return failureCount;
		}
    }
}
