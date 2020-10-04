using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.AspNetCore;
using src.Api.Queries;
using src.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using src.Database;
using src.Api.Inputs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HotChocolate.AspNetCore.Interceptors;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using src.Api.Mutations;
using src.Database.User;

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

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Audience = Configuration["AAD:ResourceId"];
                    opt.Authority = $"{Configuration["AAD:Instance"]}{Configuration["AAD:TenantId"]}";
                });
            

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Default", builder =>
                    builder.RequireAuthenticatedUser()
                    );
            });

            // Config file
            services.Configure<DatabaseConfig>(
                Configuration.GetSection(nameof(DatabaseConfig)));
            services.AddSingleton<IDatabaseConfig>(sp =>
                sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);

            // Database connection
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IFishFarmRepository, FishFarmRepository>();
            services.AddSingleton<IMetadataRepository, MetadataRepository>();
            services.AddErrorFilter<GraphQLErrorFilter>();


            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType(d => d.Name("Query"))
                .AddAuthorizeDirectiveType()
                .AddMutationType(d => d.Name("Mutation"))
                .AddType<TimeSeriesQuery>()
                .AddType<TestQuery>()
                .AddAuthorizeDirectiveType()
                .AddType<MetadataQuery>()
                .AddType<DashboardQuery>()
                .AddType<DashboardMutation>()
                .Create()
            );

            //Overwrite basic error messages with ones with more info
            services.AddErrorFilter<GraphQLErrorFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
