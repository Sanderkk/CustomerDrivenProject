using Npgsql;
using System.Collections.Generic;

namespace src.Database
{
    public class WriteToDB {
        public static void WriteData(string createTable, string copyInto, int numColumns, List<string> record, string sensorName, string[] headerArrayQuery, int sensorID)
        {
            var cs = "Host=localhost;Username=postgres;Password=password;Database=fishfarm";
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
                    row += sensorID + "\t";
                    
                    for (int j = 0; j < numColumns-1; j++) {
                        row += record[j+numColumns*i] + "\t";
                    }
                    row += record[numColumns-1] + "\n";
                    writer.Write(row);
                    row = "";
                }   
            }

            
            using (var cmd2 = new NpgsqlCommand("UPDATE sensor SET table_name = COALESCE('table_name', '"+ sensorName +"'), column_name = COALESCE('column_name', '"+ string.Join(".", headerArrayQuery) +"') WHERE sensor.id='"+ sensorID+@"';", con))
            {
                cmd2.ExecuteNonQuery();
            }
            con.Close(); 
        }
    }
}