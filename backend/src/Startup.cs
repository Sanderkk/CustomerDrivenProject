using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.AspNetCore;
using src.Api.Queries;
using src.Api.Mutations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using src.Database;
using src.Api.Inputs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HotChocolate.AspNetCore.Interceptors;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using src.Api.Subscriptions;
using src.Database.User;
using src.Api.Models;

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


            services.AddQueryRequestInterceptor(AuthenticationInterceptor());
            
            // Database connection
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITimeSeriesRepository, TimeSeriesRepository>();
            services.AddSingleton<IMetadataRepository, MetadataRepository>();
            services.AddSingleton<IUploadDataRepository, UploadDataRepository>();

            services.AddInMemorySubscriptionProvider();
    
            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddQueryType(d => d.Name("Query"))
                .AddMutationType(d => d.Name("Mutation"))
                .AddSubscriptionType(d => d.Name("Subscription"))
                .AddType<TimeSeriesQuery>()
                .AddType<TestQuery>()
                .AddType<MetadataQuery>()
                .AddType<DataSubscription>()
                .AddType<DashboardQuery>()
                .AddType<DashboardMutation>()
                .AddType<UploadDataMutation>()
                .AddType<DataSubscription>()
                .AddAuthorizeDirectiveType()
                .AddType<MetadataMutation>()
                .AddAuthorizeDirectiveType()
                .Create()
            );
        }

        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, token) =>
            {
                if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty("currentUser",
                        new CurrentUser(context.User.Identities.First().Name));
                }

                return Task.CompletedTask;
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseWebSockets();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
