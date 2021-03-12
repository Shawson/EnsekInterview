using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{
    public class BaseCsvFileParser<TTargetType, TMapping> 
        where TMapping : ClassMap
    {
        public async Task<ParseCsvFileResult<TTargetType>> Parse(TextReader reader, CancellationToken cancellationToken) 
            
        {
            var result = new ParseCsvFileResult<TTargetType>();

            using (var csv = new CsvReader(reader, 
                new CsvConfiguration(CultureInfo.InvariantCulture) { 
                    //TrimOptions = TrimOptions.Trim,
                    //HasHeaderRecord = true
					Delimiter=",",
					Encoding= new UTF8Encoding(false)
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
					catch(FieldValidationException fve)
					{
						result.Errors.Add(new CsvRowError
						{
							LineNumber = fve.Context.Parser.Row,
							FieldName = fve.Context.Reader.HeaderRecord[fve.Context.Reader.CurrentIndex],
							FieldValue = fve.Field,
							ErrorMessage = fve.Message
						});
					}
				}

                return result;
            }
        }
    }
}
