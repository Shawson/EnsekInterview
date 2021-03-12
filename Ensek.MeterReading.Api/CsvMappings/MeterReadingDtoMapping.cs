using CsvHelper.Configuration;
using Ensek.MeterReading.Data.Client.Dtos;
using System.Text.RegularExpressions;

namespace Ensek.MeterReading.Api.Mappings
{
    public class MeterReadingDtoMap : ClassMap<MeterReadingDto>
    {
        public static readonly Regex _digitRegex = new Regex("^[0-9]{5}$");

        public MeterReadingDtoMap()
        {
			Map(m => m.AccountId)
				.Index(0);

			Map(m => m.MeterReadValue)
				.Index(2)
				.Validate(expression => _digitRegex.IsMatch(expression.Field));

			Map(m => m.MeterReadingDateTime)
				.Index(1)
				.TypeConverterOption.Format("dd/MM/yyyy HH:mm");

			Map(m => m.MeterReadingId)
				.Ignore();
			
        }
    }
}
