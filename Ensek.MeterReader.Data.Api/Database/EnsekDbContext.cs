using Ensek.MeterReading.Data.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Data.Api.Database
{
    public class EnsekDbContext : DbContext
    {
        public EnsekDbContext(DbContextOptions<EnsekDbContext> options)
            : base(options)
        {
        }

        public DbSet<Entities.CustomerAccount> CustomerAccount { get; set; }
        public DbSet<Entities.MeterReading> MeterReading { get; set; }
    }
}
