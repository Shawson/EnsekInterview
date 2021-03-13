using Ensek.MeterReading.Data.Client.Dtos;

namespace Ensek.MeterReading.Data.Api.Tests.Controllers
{
	public class MeterReadingTestCase
	{
		public MeterReadingDto Dto { get; set; }
		public int ExpectedStatusCode { get; set; }
		public string ExpectedMessage { get; set; }
		public string TestName { get; set; }
	}
}
