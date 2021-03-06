using Ensek.MeterReading.Data.Api.Database;
using Ensek.MeterReading.Data.Api.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ensek.MeterReading.Data.Api.Middleware;
using MediatR;
using Microsoft.OpenApi.Models;

namespace Ensek.MeterReading.Data.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

			services.AddAutoMapper(typeof(Startup));
			services.AddMediatR(typeof(Startup).Assembly);

			services.AddDbContext<EnsekDbContext>(
                options => options.UseSqlServer(Configuration.GetValue<string>("DbConnectionString")));

			services.AddScoped<DbContext, EnsekDbContext>();

			services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
				{
					In = ParameterLocation.Header, 
					Name = "X-MeterReadingData-ApiKey", 
					Type = SecuritySchemeType.ApiKey, 
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
							},
							new List<string>()
						}
					});
			});

            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

			});

            app.UseRouting();

            app.UseAuthorization();

			app.UseMiddleware<ApiKeyMiddleware>();

			app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
