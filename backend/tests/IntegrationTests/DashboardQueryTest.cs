using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using src.Api.Inputs;
using src.Api.Queries;
using src.Api.Types;
using src;
using src.Database;
using src.Database.User;
using Xunit;

namespace tests.IntegrationTests
{
    public class DashboardQueryTest
    {
        private string dbConnectionString;

        public DashboardQueryTest()
        {
            TestDatabaseConfig config = new TestDatabaseConfig();
            dbConnectionString = config.DatabaseConnectionString;
        }
        [Fact]
        public async Task GetDashboard()
        {

            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IDatabaseConfig>(sp =>
                new DatabaseConfig()
                {
                    DatabaseConnectionString = dbConnectionString
                }
                )
                .BuildServiceProvider();

            IQueryExecutor executor = Schema.Create(c =>
            {
                c.RegisterQueryType(new ObjectType<DashboardQuery>(d => d.Name("Query")));
                c.RegisterType<Dashboard>();
            })
            .MakeExecutable();
            
            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"{allDashboards{ id, name, description, data}}")
                .SetServices(serviceProvider)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            result.MatchSnapshot();
        }

    }
}