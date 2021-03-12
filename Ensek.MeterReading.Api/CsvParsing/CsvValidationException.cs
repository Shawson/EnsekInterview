namespace Ensek.MeterReading.Api.Cqrs.Commands
{
    [System.Serializable]
    public class CsvValidationException : System.Exception
    {
        public CsvValidationException() { }
        public CsvValidationException(string message) : base(message) { }
        public CsvValidationException(string message, System.Exception inner) : base(message, inner) { }
        protected CsvValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
