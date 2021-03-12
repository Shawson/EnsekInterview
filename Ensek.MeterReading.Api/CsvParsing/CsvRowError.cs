namespace Ensek.MeterReading.Api.Cqrs.Commands
{
    public class CsvRowError
    {
        public int LineNumber { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string ErrorMessage { get; set; }
    }
}
