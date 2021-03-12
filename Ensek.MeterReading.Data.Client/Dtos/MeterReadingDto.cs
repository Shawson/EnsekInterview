using System;

namespace Ensek.MeterReading.Data.Client.Dtos
{
    public class MeterReadingDto
    {
		public MeterReadingDto()
		{

		}

        public int MeterReadingId { get; set; }
        public int AccountId { get; set; }
        public int MeterReadValue { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
    }
}
