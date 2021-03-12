using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Data.Api.Database.Entities
{
    [Table("CustomerAccount")]
    public class CustomerAccount
    {
        [Key]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<MeterReading> MeterReadings { get; set; }
    }
}
