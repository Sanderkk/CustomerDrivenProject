using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using src.Api.Inputs;
using src.Api.Queries;
using src.Config;
using src.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests.IntegrationTests
{
    public class TimeSeriesQueryTest
    {

        private string dbConnectionString = "Host=sanderkk.com;Username=sintef;Password=123456;Database=fishfarm";

        [Fact]
        public async Task TestTimeSeriesQuery()
        {
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IFishFarmRepository, FishFarmRepository>()
                .AddSingleton<IDatabaseConfig>(sp =>
                new DatabaseConfig()
                {
                    DatabaseConnectionString = dbConnectionString
                }
                )
                .BuildServiceProvider();

            IQueryExecutor executor = Schema.Create(c =>
            {
                c.RegisterQueryType(new ObjectType<TimeSeriesQuery>(d => d.Name("Query")));
                c.RegisterType<GenericObject>();
            })
            .MakeExecutable();

            var input = new TimeSeriesRequestInput()
            {
                TableName = "tension",
                ColumnNames = new List<string>(),
                From = DateTime.Parse("2020-08-25T23:59:58.000Z"),
                To = DateTime.Parse("2020-08-26T00:00:00.000Z")
            };

            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"query TimeSeries($input: TimeSeriesRequestInput!){
                              timeSeries(input: $input) {
                                table,
                                startDate,
                                endDate,
                                time,
                                data {
                                  key,
                                  value
                                },
                                numberData {
                                  key,
                                  value
                                }
                              }
                            }")
                .SetServices(serviceProvider)
                .AddVariableValue("input", input)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Snapshot.Match(result);
            result.MatchSnapshot();
        }

        public async Task TestSensorsQuery()
        {
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IFishFarmRepository, FishFarmRepository>()
                .AddSingleton<IDatabaseConfig>(sp =>
                new DatabaseConfig()
                {
                    DatabaseConnectionString = dbConnectionString
                }
                )
                .BuildServiceProvider();

            IQueryExecutor executor = Schema.Create(c =>
            {
                c.RegisterQueryType(new ObjectType<TimeSeriesQuery>(d => d.Name("Query")));
            })
            .MakeExecutable();

            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"query sensors{
                              sensors {
                                sensorId,
                                sensorTabel,
                                sensorColumns
                              }
                            }")
                .SetServices(serviceProvider)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Snapshot.Match(result);
            result.MatchSnapshot();
        }

        [Fact]
        public async Task TestTimeSeriesPeriode()
        {
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IFishFarmRepository, FishFarmRepository>()
                .AddSingleton<IDatabaseConfig>(sp =>
                new DatabaseConfig()
                {
                    DatabaseConnectionString = dbConnectionString
                }
                )
                .BuildServiceProvider();

            IQueryExecutor executor = Schema.Create(c =>
            {
                c.RegisterQueryType(new ObjectType<TimeSeriesQuery>(d => d.Name("Query")));
                c.RegisterType<GenericObject>();
            })
            .MakeExecutable();

            var input = new TimeSeriesPeriodeInput()
            {
                TableName = "Tension",
                SensorId = 2
            };

            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"query timeSeriesPeriode($input: TimeSeriesPeriodeInput){
                              timeSeriesPeriode(input: $input) {
                                from,
                                to
                              }
                            }")
                .SetServices(serviceProvider)
                .AddVariableValue("input", input)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Snapshot.Match(result);
            result.MatchSnapshot();
        }

    }
}
