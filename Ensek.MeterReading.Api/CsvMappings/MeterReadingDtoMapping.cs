using CsvHelper.Configuration;
using Ensek.MeterReading.Data.Client.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Mappings
{
    public class MeterReadingDtoMap : ClassMap<MeterReadingDto>
    {
        public static readonly Regex DigitRegex = new Regex("^[0-9]{5}$");

        public MeterReadingDtoMap()
        {
			Map(m => m.AccountId)
				.Index(0);

			Map(m => m.MeterReadValue)
				.Index(2)
				.Validate(expression => {
					return DigitRegex.IsMatch(expression.Field);
					});

			Map(m => m.MeterReadingDateTime)
				.Index(1)
				.TypeConverterOption.Format("dd/MM/yyyy HH:mm");

			Map(m => m.MeterReadingId)
				.Ignore();
			
        }
    }
}
