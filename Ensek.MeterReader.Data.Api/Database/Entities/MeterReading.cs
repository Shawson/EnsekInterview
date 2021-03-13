using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ensek.MeterReading.Data.Api.Database.Entities
{
    [Table("MeterReading")]
    public class MeterReading
    {
        [Key]
        public int MeterReadingId { get; set; }

        [ForeignKey("CustomerAccount")]
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }

        public virtual CustomerAccount CustomerAccount { get; set; }
    }
}
