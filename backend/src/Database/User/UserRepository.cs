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
                    DashboardId = dataReader.GetFieldValue<int>(0),
                    Name = dataReader.GetFieldValue<string>(1),
                    Description = dataReader.GetFieldValue<string>(2),
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
            
            List<Dashboard> result = new List<Dashboard>();
            while (dataReader.Read())
            {
                Dashboard dashboard = new Dashboard()
                {
                    DashboardId = dataReader.GetFieldValue<int>(0),
                    Name = dataReader.GetFieldValue<string>(1),
                    Description = dataReader.GetFieldValue<string>(2),
                };
                result.Add(dashboard);
            };
            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }

        public async Task<Cell> GetCell(string queryString)
        {
            
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();

            Cell cell = null;
            while (dataReader.Read())
            {
                cell = new Cell()
                {
                    CellId = dataReader.GetFieldValue<int>(0),
                    DashboardId = dataReader.GetFieldValue<int>(1),
                    Input = JsonSerializer.Serialize(dataReader.GetFieldValue<JsonElement>(2)),
                    Options = JsonSerializer.Serialize(dataReader.GetFieldValue<JsonElement>(3)),
                };
            };
            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return cell;
        }

        public async Task<List<Cell>> GetCells(string queryString)
        {
            NpgsqlConnection _npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await _npgsqlConnection.OpenAsync();
            using var cmd = new NpgsqlCommand(queryString);
            cmd.Connection = _npgsqlConnection;
            var dataReader = await cmd.ExecuteReaderAsync();
            
            List<Cell> result = new List<Cell>();
            while (dataReader.Read())
            {
                Cell cell = new Cell()
                {
                    CellId = dataReader.GetFieldValue<int>(0),
                    DashboardId = dataReader.GetFieldValue<int>(1),
                    Input = JsonSerializer.Serialize(dataReader.GetFieldValue<JsonElement>(2)),
                    Options = JsonSerializer.Serialize(dataReader.GetFieldValue<JsonElement>(3)),
                };
                result.Add(cell);
            };
            cmd.Parameters.Clear();
            await dataReader.CloseAsync();
            await _npgsqlConnection.CloseAsync();
            return result;
        }
        
        public async Task<bool> UpdateDashboard(DashboardInput input)
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await npgsqlConnection.OpenAsync();
            if (input.dashboardId != null)
            {
                int id = input.dashboardId.GetValueOrDefault();
                string queryString = UserQueryBuilder.UpdateDashboardQueryString(id, input.name, input.description);
                await executeQuery(npgsqlConnection, queryString);
            }
            else
            {
                await CreateDashboard(input, npgsqlConnection);
            }
            
            await npgsqlConnection.CloseAsync();
            return true;
        }
        
        public async Task<bool> DeleteDashboard(string userId, int dashboardId)
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_databaseSettings.DatabaseConnectionString);
            await npgsqlConnection.OpenAsync();
            string queryString = UserQueryBuilder.DeleteDashboardQueryString(userId, dashboardId);
            await using NpgsqlCommand cmd = new NpgsqlCommand(queryString);
            cmd.Connection = npgsqlConnection;
            await cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();
            string accessQueryString =
                UserQueryBuilder.DeleteUserAccessToDashboardQueryString(userId, dashboardId);
            await using NpgsqlCommand cmd2 = new NpgsqlCommand(accessQueryString);
            cmd2.Connection = npgsqlConnection;
            await cmd2.ExecuteNonQueryAsync();
            cmd2.Parameters.Clear();
            cmd.Parameters.Clear();
            await npgsqlConnection.CloseAsync();
            
            return true;
        }

        public async Task CreateDashboard(DashboardInput input, NpgsqlConnection connection)
        {
                string queryString = UserQueryBuilder.CreateDashboardQueryString(input.name, input.description);
                await using NpgsqlCommand cmd = new NpgsqlCommand(queryString);
                cmd.Connection = connection;
                int dashboardId = (int) cmd.ExecuteScalar();
                Console.WriteLine(dashboardId);
                cmd.Parameters.Clear();
                
                string accessQueryString =
                    UserQueryBuilder.InsertUserAccessToDashboardQueryString(input.userId, dashboardId, input.accessLevel);
                await executeQuery(connection, accessQueryString);

        }
        
        public async Task<bool> UpdateCell(CellDataInput input)
        {
            JsonElement options = JsonSerializer.Deserialize<JsonElement>(input.options);
            JsonElement inputQuery = JsonSerializer.Deserialize<JsonElement>(input.input);
            if (input.cellId != null)
            {
                int id = input.cellId.GetValueOrDefault();
                string queryString =
                    UserQueryBuilder.UpdateCellQueryString(id, input.dashboardId, options, inputQuery);
                return await executeMutation(queryString);
            }
            else
            {
                return await CreateCell(input.dashboardId, options, inputQuery);
            }
        }

        private async Task<bool> CreateCell(int dashboardId, JsonElement options, JsonElement input)
        {
                string queryString = UserQueryBuilder.CreateCellQueryString(dashboardId, options, input);
                return await executeMutation(queryString);
        }

        public Task<bool> DeleteDashboardCell(string queryString)
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
