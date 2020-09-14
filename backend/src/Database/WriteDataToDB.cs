using Npgsql;
using System.Collections.Generic;

namespace src.Database
{
    public class WriteToDB {
        public static void WriteData(string createTable, string copyInto, int numColumns, List<string> record, string sensorName)
        {
            var cs = "Host=localhost;Username=postgres;Password=password;Database=bjorn";
            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;


            cmd.CommandText = createTable;
            cmd.ExecuteNonQuery();
            
            // transforms the datatable to a time series database
            cmd.CommandText = "SELECT create_hypertable('"+sensorName+@"', 'time', if_not_exists => TRUE)";
            cmd.ExecuteNonQuery();

            string row = "";
            
            using (var writer = con.BeginTextImport(copyInto)) {

                for (int i = 1; i< record.Count/numColumns; i++) {
                    row += record[numColumns*i] + "\t";
                    
                    for (int j = 1; j < numColumns-1; j++) {
                        row += record[j+numColumns*i] + "\t";
                    }
                    row += record[numColumns-1] + "\n";
                    writer.Write(row);
                    row = "";
                }   
            }
            con.Close(); 
        }
    }
}