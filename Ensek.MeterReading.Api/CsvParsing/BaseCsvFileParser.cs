using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Cqrs.Commands
{
    public abstract class BaseCsvFileParser<TTargetType, TMapping> 
        where TMapping : ClassMap
    {
        public async Task<ParseCsvFileResult<TTargetType>> Parse(TextReader reader, CancellationToken cancellationToken) 
        {
			if (reader == null)
			{
				throw new ArgumentException("Reader cannot be null");
			}

            var result = new ParseCsvFileResult<TTargetType>();

			try
			{
				using (var csv = new CsvReader(reader,
					new CsvConfiguration(CultureInfo.InvariantCulture)
					{
						Delimiter = ",",
						Encoding = new UTF8Encoding(false)
					}))
				{
					csv.Context.RegisterClassMap<TMapping>();

					var records = new List<TTargetType>();
					await csv.ReadAsync();
					csv.ReadHeader();
					while (await csv.ReadAsync())
					{
						try
						{
							var record = csv.GetRecord<TTargetType>();
							result.ValidRows.Add(record);
						}
						catch (FieldValidationException fve)
						{
							result.Errors.Add(new CsvRowError
							{
								LineNumber = fve.Context.Parser.Row,
								FieldName = fve.Context.Reader.HeaderRecord[fve.Context.Reader.CurrentIndex],
								FieldValue = fve.Field,
								ErrorMessage = fve.Message
							});
						}
					}

					return result;
				}
			}
			catch (ReaderException ex)
			{
				if (ex.Message.Contains("No header record was found"))
					throw new MalformedFileException("No header record was found");

				throw new MalformedFileException(ex.Message);
			}
		}
    }


	[Serializable]
	public class MalformedFileException : Exception
	{
		public MalformedFileException() { }
		public MalformedFileException(string message) : base(message) { }
		public MalformedFileException(string message, Exception inner) : base(message, inner) { }
		protected MalformedFileException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
