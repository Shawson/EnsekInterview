using System.Collections.Generic;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{
    public class ParseCsvFileResult<T> where T : new()
    {
        public ParseCsvFileResult()
        {
            ValidRows = new List<T>();
            Errors = new List<CsvRowError>();
        }
        public List<T> ValidRows { get; set; }
        public List<CsvRowError> Errors { get; set; }
    }
}
