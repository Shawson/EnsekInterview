using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ensek.MeterReading.Data.Client.Dtos;

namespace Ensek.MeterReading.Data.Api.Mapping
{
	public class MeterReadingMappings : Profile
	{
		public MeterReadingMappings()
		{
			CreateMap<Database.Entities.MeterReading, MeterReadingDto>()
				.ReverseMap();
		}
	}
}
