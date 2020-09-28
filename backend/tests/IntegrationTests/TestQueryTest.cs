using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using src.Api.Queries;
using src.Api.Types;
using src.Config;
using src.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests.IntegrationTests
{
    public class TestQueryTest
    {
        [Fact]
        public async Task TestTestQuery()
        {
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IFishFarmRepository, FishFarmRepository>()
                .AddSingleton<IDatabaseConfig>(sp =>
                new DatabaseConfig()
                {
                    DatabaseConnectionString = "Host=sanderkk.com;Username=sintef;Password=123456;Database=fishfarm"
                }
                )
                .BuildServiceProvider();

            IQueryExecutor executor = Schema.Create(c =>
            {
                c.RegisterQueryType(new ObjectType<TestQuery>(d => d.Name("Query")));
            })
            .MakeExecutable();

            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery("{test {name,description}}")
                .SetServices(serviceProvider)
                .AddProperty("Key", "value")
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Snapshot.Match(result);
            result.MatchSnapshot();
        }
    }
}
