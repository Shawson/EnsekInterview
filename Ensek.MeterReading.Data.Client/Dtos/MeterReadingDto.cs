using System;
using System.Collections.Generic;
using System.Text;

namespace Ensek.MeterReading.Data.Client.Dtos
{
    public class MeterReadingDto
    {
        public int MeterReadingId { get; set; }
        public int AccountId { get; set; }
        public int MeterReadValue { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
    }
}
