using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Dtos
{
    public class MeterReadingUploadResult
    {
        public int SuccessfulRows { get; set; }
        public int ErrorRows { get; set; }
    }
}
