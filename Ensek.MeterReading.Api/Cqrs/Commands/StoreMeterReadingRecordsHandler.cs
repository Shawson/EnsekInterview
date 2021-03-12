using Ensek.MeterReading.Api.Dtos;
using Ensek.MeterReading.Data.Client.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{

    public class StoreMeterReadingRecordsRequest : IRequest<MeterReadingUploadResult>
    {
        public StoreMeterReadingRecordsRequest(List<MeterReadingDto> meterReadings)
        {

        }
    }

    public class StoreMeterReadingRecordsHandler : IRequestHandler<StoreMeterReadingRecordsRequest, MeterReadingUploadResult>
    {
        public StoreMeterReadingRecordsHandler()
        {

        }

        public async Task<MeterReadingUploadResult> Handle(StoreMeterReadingRecordsRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
