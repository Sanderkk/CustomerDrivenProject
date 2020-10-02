using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.AspNetCore;
using src.Api.Types;
using src.Api.Queries;
using src.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using src.Database;
using src.Api.Inputs;

namespace src
{
    public class Startup
    {

        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            // Config file
            services.Configure<DatabaseConfig>(
                Configuration.GetSection(nameof(DatabaseConfig)));
            services.AddSingleton<IDatabaseConfig>(sp =>
                sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);

            // Database connection
            services.AddSingleton<IFishFarmRepository, FishFarmRepository>();


            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)

                .AddQueryType(d => d.Name("Query"))
                //.AddMutationType(d => d.Name("Mutation"))
                //.AddQueryType<TimeSeriesQuery>()
                //.AddQueryType<TestQuery>()

                .AddType<TimeSeriesQuery>()
                .AddType<TestQuery>()

                .Create()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
