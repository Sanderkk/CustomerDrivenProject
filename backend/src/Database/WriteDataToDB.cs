using Npgsql;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using src.Config;

namespace src.Database
{
    public class WriteToDB {
        public static void WriteData(string createTable, string copyInto, int startIndex, int numTableColumns, List<string> record, string sensorName, string[] headerArrayQuery, int sensorID)
        {

            int numColumns = headerArrayQuery.Length;
            
            // Get config from appsetting.json file
            var path = Directory.GetCurrentDirectory();
            var newPath = Path.GetFullPath(Path.Combine(path, @"../../src/"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var databaseSettings = builder.GetSection("DatabaseConfig").Get<DatabaseConfig>();
            var cs = databaseSettings.DatabaseConnectionString;

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "INSERT INTO sensor (id) VALUES ('" + sensorID +"')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = createTable;
            cmd.ExecuteNonQuery();
            
            // transforms the datatable to a time series database
            cmd.CommandText = "SELECT create_hypertable('"+sensorName +@"', 'time', if_not_exists => TRUE)";
            cmd.ExecuteNonQuery();

            write(con, copyInto, startIndex, numTableColumns, record, numColumns, sensorID);

            using (var cmd2 = new NpgsqlCommand("UPDATE sensor SET table_name = '"+sensorName+"', column_name = '"+ string.Join(".", sensorName) +"' WHERE sensor.id='"+ sensorID+@"';", con))
            {
                cmd2.ExecuteNonQuery();
            }
            con.Close(); 
        }

        public static void write(NpgsqlConnection con, string copyInto, int startIndex, int numTableColumns, List<string> record, int numColumns, int sensorID) {
            string row = "";
            
            using (var writer = con.BeginTextImport(copyInto)) {

                for (int i = 0; i< record.Count/numColumns; i++) {
                    row += sensorID + "\t";
                    row += record[i*numColumns] + "\t";

                    for (int j = startIndex; j < startIndex+numTableColumns-1; j++) {
                        
                        row += record[j+numColumns*i] + "\t";
                    }
                    row += record[i*numColumns + startIndex+numTableColumns-1] + "\n";
                    writer.Write(row);
                    row = "";
                }   
            }
        }
    }
}