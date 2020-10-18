using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Npgsql;
using src.Config;
using Newtonsoft.Json;
using src.Api.Inputs;
using src.Api.Types;
using src.Database.User;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace src.Database.User
{
    public class UserRepository : IUserRepository
    {

        private readonly IDatabaseConfig _databaseSettings;

        public UserRepository(IDatabaseConfig databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<Dashboard> GetDashboard(string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            
            Dashboard result = null;
            while (dataReader.Read())
            {
                result = new Dashboard()
                {
                    Id = dataReader.GetFieldValue<int>(0),
                    Name = dataReader.GetFieldValue<string>(1),
                    Description = dataReader.GetFieldValue<string>(2),
                    Data = JsonSerializer.Serialize(dataReader.GetFieldValue<JsonElement>(3))
                };
            };
            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }
        
        public async Task<List<Dashboard>> GetDashboards(string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();
            
            
            var fieldsCount = dataReader.GetColumnSchema().Count();
            List<Dashboard> result = new List<Dashboard>();
            while (dataReader.Read())
            {
                Dashboard dashboard = new Dashboard()
                {
                    Id = dataReader.GetFieldValue<int>(0),
                    Name = dataReader.GetFieldValue<string>(1),
                    Description = dataReader.GetFieldValue<string>(2),
                    Data = JsonSerializer.Serialize(dataReader.GetFieldValue<JsonElement>(3))
                };
                result.Add(dashboard);
            };
            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }

        public async Task<bool> UpdateDashboard(DashboardInput input)
        {
            JsonElement jsonData = JsonSerializer.Deserialize<JsonElement>(input.data);
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await npgsqlConnection.OpenAsync();
            if (input.dashboardId != null)
            {
                int id = input.dashboardId.GetValueOrDefault();
                string queryString =
                    UserQueryBuilder.UpdateDashboardQueryString(id, input.name, input.description,
                        jsonData);
                await executeQuery(npgsqlConnection, queryString);
            }
            else
            {
                await CreateDashboard(input, jsonData, npgsqlConnection);
            }
            
            await npgsqlConnection.CloseAsync();
            return true;
        }
        
        public async Task<bool> DeleteDashboard(int dashboardId)
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await npgsqlConnection.OpenAsync();
            string queryString = UserQueryBuilder.DeleteDashboardQueryString(dashboardId);
            await using NpgsqlCommand cmd = new NpgsqlCommand(queryString);
            cmd.Connection = npgsqlConnection;
            await cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();
            string accessQueryString =
                UserQueryBuilder.DeleteUserAccessToDashboardQueryString(dashboardId);
            await using NpgsqlCommand cmd2 = new NpgsqlCommand(accessQueryString);
            cmd2.Connection = npgsqlConnection;
            await cmd2.ExecuteNonQueryAsync();
            cmd2.Parameters.Clear();
            cmd.Parameters.Clear();
            await npgsqlConnection.CloseAsync();
            
            return true;
        }

        public async Task CreateDashboard(DashboardInput input, JsonElement jsonData, NpgsqlConnection connection)
        {
                string queryString = UserQueryBuilder.CreateDashboardQueryString(input.name, input.description, jsonData);
                await using NpgsqlCommand cmd = new NpgsqlCommand(queryString);
                cmd.Connection = connection;
                int dashboardId = (int) cmd.ExecuteScalar();
                Console.WriteLine(dashboardId);
                cmd.Parameters.Clear();
                
                string accessQueryString =
                    UserQueryBuilder.InsertUserAccessToDashboardQueryString(input.userId, dashboardId, input.accessLevel);
                await executeQuery(connection, accessQueryString);

        }
        
        public Task<bool> CreateDashboard(string queryString)
        {
            return executeMutation(queryString);
        }


        public async Task<bool> executeMutation(string queryString) {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await npgsqlConnection.OpenAsync();
            await using NpgsqlCommand cmd = new NpgsqlCommand(queryString);
            cmd.Connection = npgsqlConnection;
            cmd.ExecuteNonQuery();
            
            cmd.Parameters.Clear();
            await npgsqlConnection.CloseAsync();
            return true;
        }

        public async Task executeQuery(NpgsqlConnection connection, string query)
        {
            await using NpgsqlCommand cmd = new NpgsqlCommand(query);
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
    }
}
