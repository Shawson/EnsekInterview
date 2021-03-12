using Ensek.MeterReading.Api.Dtos;
using Ensek.MeterReading.Api.Mappings;
using Ensek.MeterReading.Data.Client.Dtos;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{

    public class ParseMeterReadingCsvFileRequest : IRequest<ParseCsvFileResult<MeterReadingDto>>
    {
        public ParseMeterReadingCsvFileRequest(TextReader reader)
        {
            Reader = reader;
        }

        public TextReader Reader { get; }
    }

    public class ParseMeterReadingCsvFileHandler :
        BaseCsvFileParser<MeterReadingDto, MeterReadingDtoMap>,
        IRequestHandler<ParseMeterReadingCsvFileRequest, ParseCsvFileResult<MeterReadingDto>>
    {
        public Task<ParseCsvFileResult<MeterReadingDto>> Handle(ParseMeterReadingCsvFileRequest request, CancellationToken cancellationToken) =>
            base.Parse(request.Reader, cancellationToken);
    }

    
}
