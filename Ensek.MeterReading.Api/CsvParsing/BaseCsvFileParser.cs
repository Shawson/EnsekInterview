using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{
    public class BaseCsvFileParser<TTargetType, TMapping> 
        where TTargetType : new()
        where TMapping : ClassMap
    {
        public async Task<ParseCsvFileResult<TTargetType>> Parse(TextReader reader, CancellationToken cancellationToken) 
            
        {
            var result = new ParseCsvFileResult<TTargetType>();

            using (var csv = new CsvReader(reader, 
                new CsvConfiguration(CultureInfo.InvariantCulture) { 
                    TrimOptions = TrimOptions.Trim,
                    HasHeaderRecord = true
                }))
            {
                csv.Context.RegisterClassMap<TMapping>();

                var records = new List<TTargetType>();
                await csv.ReadAsync();
                csv.ReadHeader();
                while (await csv.ReadAsync())
                {
                    try
                    {
                        var record = csv.GetRecord<TTargetType>();
                        result.ValidRows.Add(record);
                    }
                    catch (CsvValidationException cve)
                    {
                        throw cve;
                    }
                    catch (CsvHelperException ex) when (ex is ReaderException)
                    {
                        throw ex;
                    }
                }

                return result;
            }
        }
    }
}
