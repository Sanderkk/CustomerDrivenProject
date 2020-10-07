using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using src.Api.Inputs;
using src.Api.Queries;
using src.Config;
using src.Database;
using src.Api.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests.IntegrationTests
{
    public class MetadataQueryTest
    {
        //Database Connect config (move to own config file)
        private string dbConnectionString = "Host=sanderkk.com;Username=sintef;Password=123456;Database=fishfarm2";

        [Fact]
        public async Task AllMetadataQuery()
        {
            //Arrange
            //Connect to database
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
                c.RegisterQueryType(new ObjectType<MetadataQuery>(d => d.Name("Query")));
                c.RegisterType<List<MetadataType>>();
            })
            .MakeExecutable();

            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"{allMetadata(sensorID:1){metadataID,sensorID,name,serialNumber,number,department,company,purchaseDate,lending,lendingPrice,cableLength,checkOnInspectionRound,company,voltage,coordinate, altitude, locationDescription, createdAt, updatedAt, outdatedFrom}}")
                .SetServices(serviceProvider)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Assert
            Snapshot.Match(result);
        }

        [Fact]
        public async Task LastMetadataQuery()
        {
            //Arrange
            //Connect to database
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
                c.RegisterQueryType(new ObjectType<MetadataQuery>(d => d.Name("Query")));
                c.RegisterType<List<MetadataType>>();
            })
            .MakeExecutable();

            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"{lastMetadata(sensorID:1){metadataID,sensorID,name,serialNumber,number,department,company,purchaseDate,lending,lendingPrice,cableLength,checkOnInspectionRound,company,voltage,coordinate, altitude, locationDescription, createdAt, updatedAt, outdatedFrom}}")
                .SetServices(serviceProvider)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Assert
            Snapshot.Match(result);
            //result.MatchSnapshot();
        }
    }
}