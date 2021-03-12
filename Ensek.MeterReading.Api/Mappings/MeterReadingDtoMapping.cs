using CsvHelper.Configuration;
using Ensek.MeterReading.Data.Client.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Mappings
{
    public class MeterReadingDtoMap : ClassMap<MeterReadingDto>
    {
        public static readonly Regex DigitRegex = new Regex("[0-9]{5}");

        public MeterReadingDtoMap()
        {
            Map(m => m.AccountId)
                .Name("AccountId");

            Map(m => m.MeterReadValue)
                .Name("MeterReadValue")
                .Validate(expression => DigitRegex.IsMatch(expression.ToString()));

            Map(m => m.MeterReadingDateTime)
                .Name("MeterReadingDateTime");
        }
    }
}
