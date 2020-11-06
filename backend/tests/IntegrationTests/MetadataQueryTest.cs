using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using src.Api.Inputs;
using src.Api.Queries;
using src;
using src.Database;
using src.Api.Types;
using src.Api.Mutations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests.IntegrationTests
{
    public class MetadataQueryTest
    {
        private string dbConnectionString;
        public MetadataQueryTest()
        {
            TestDatabaseConfig config = new TestDatabaseConfig();
            dbConnectionString = config.DatabaseConnectionString;
        }

        [Fact]
        public async Task AllMetadataQuery()
        {
            //Arrange
            //Connect to database
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IMetadataRepository, MetadataRepository>()
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
                .SetQuery(@"{metadata(sensorID:1)
                            {metadataID,sensorID,locationID,name,serialNumber,number, modelNumber,department,company, ownerID,
                            purchaseDate,lending,lendingPrice,cableLength,checkOnInspectionRound,inspectionRound, company, servicePartner,voltage,coordinate, 
                            altitude, locationDescription, cableLength, identificator, measureArea, picture,
                            plannedDisposal, actualDisposal, warrantyDate, voltage, signal, tag1, tag2, tag3, timeless, tollerance
                            createdAt, updatedAt, outdatedFrom}}")
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
                .AddSingleton<IMetadataRepository, MetadataRepository>()
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
                .SetQuery(@"{metadata(sensorID:1, onlyLast:true)
                            {metadataID,sensorID,locationID,name,serialNumber,number, modelNumber,department,company, ownerID,
                            purchaseDate,lending,lendingPrice,cableLength,checkOnInspectionRound,inspectionRound, company, servicePartner,voltage,coordinate, 
                            altitude, locationDescription, cableLength, identificator, measureArea, picture,
                            plannedDisposal, actualDisposal, warrantyDate, voltage, signal, tag1, tag2, tag3, timeless, tollerance
                            createdAt, updatedAt, outdatedFrom}}")
                .SetServices(serviceProvider)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Assert
            Snapshot.Match(result);
            //result.MatchSnapshot();
        }

        [Fact]
        public async Task LastMetadataByNumberQuery()
        {
            //Arrange
            //Connect to database
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IMetadataRepository, MetadataRepository>()
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
                .SetQuery(@"{metadata(sensorNumber: ""RO.038-50m"", onlyLast:true)
                            {metadataID,sensorID,locationID,name,serialNumber,number, modelNumber,department,company, ownerID,
                            purchaseDate,lending,lendingPrice,cableLength,checkOnInspectionRound,inspectionRound, company, servicePartner,voltage,coordinate, 
                            altitude, locationDescription, cableLength, identificator, measureArea, picture,
                            plannedDisposal, actualDisposal, warrantyDate, voltage, signal, tag1, tag2, tag3, timeless, tollerance
                            createdAt, updatedAt, outdatedFrom}}")
                .SetServices(serviceProvider)
                .Create();

            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Assert
            Snapshot.Match(result);
            //result.MatchSnapshot();
        }

              [Fact]
        public async Task AddMetadataQuery()
        {
            //Arrange
            //Connect to database
            IServiceProvider serviceProvider =
                new ServiceCollection()
                .AddSingleton<IMetadataRepository, MetadataRepository>()
                .AddSingleton<IDatabaseConfig>(sp =>
                new DatabaseConfig()
                {
                    DatabaseConnectionString = dbConnectionString
                }
                )
                .BuildServiceProvider();

            IQueryExecutor executor = Schema.Create(c =>
            {
                c.RegisterQueryType(new ObjectType<MetadataMutation>(d => d.Name("Mutation")));
                c.RegisterType<List<MetadataType>>();
            })
            .MakeExecutable(); 
            var input = new MetadataInput(){
                SensorID = 5,
                Name = "Jasmine",
                Number = "55T77",
                Coordinate = "Moholt",
                Altitude = 55,
                LocationDescription = "samsy like always",
                Company = "kundestyrt and co",
                ServicePartner = "kundestyrt and co",
                Department = "IDI",
                OwnerID = "33333",
                SerialNumber ="55yh3kk3-4443-455ddw2",
                Tag1 ="nada",
                Tag2 = "tag222222",
                Tag3 = "im tagged boi",
                Identificator = "nothing special here",
                PurchaseDate =DateTime.Parse("2002-12-25T00:00:00.000Z"),
                WarrantyDate = DateTime.Parse("2007-12-25T00:00:00.000Z"),
                NextService =DateTime.Parse("2003-12-25T00:00:00.000Z"),
                PlannedDisposal = DateTime.Parse("2022-12-25T00:00:00.000Z"),
                ActualDisposal = DateTime.Parse("2022-12-25T00:00:00.000Z"),
                ModelNumber = "jjhj445",
                Picture = "Some picture link or svg?",
                Signal ="Wireless",
                MeasureArea = "550 550",
                Website = "www.sanderkk.com",
                InspectionRound = "same like the others",
                Lending = true,
                Timeless = false,
                CheckOnInspectionRound = false,
                Tollerance =true,
                LendingPrice = 9000,
                CableLength = 125,
                Voltage = "24volt"
            
            };
            IReadOnlyQueryRequest request =
                QueryRequestBuilder.New()
                .SetQuery(@"mutation addMetadata($someMetadata : MetadataInput!)
                            { addMetadata(newMetadata:$someMetadata)
                            {sensorID,locationID,name,serialNumber,number, modelNumber,department,company, ownerID,
                            purchaseDate,lending,lendingPrice,cableLength,checkOnInspectionRound,inspectionRound, company, servicePartner,voltage,coordinate, 
                            altitude, locationDescription, cableLength, identificator, measureArea, picture,
                            plannedDisposal, actualDisposal, warrantyDate, voltage, signal, tag1, tag2, tag3, timeless, tollerance
                           , outdatedFrom}}")
                .SetServices(serviceProvider)
                .AddVariableValue("someMetadata", input)
                .Create();
            // act
            IExecutionResult result = await executor.ExecuteAsync(request);
            //Assert
            Snapshot.Match(result);
        }

    }
}