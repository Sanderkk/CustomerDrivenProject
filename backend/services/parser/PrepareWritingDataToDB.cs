using System;
using System.Collections.Generic;
using System.Globalization;
using src.Database;
using System.Linq;
using parser.Config;

namespace parser
{
    public class PrepareWritingDataToDB
    {
        public static void PrepareQueryAndWrite(List<String> record,  ParserConfig dataConfig, string[] headerArrayQuery, int[] id)
        {   
            
            headerArrayQuery = cleanString(headerArrayQuery);
            
            for (int i=0; i < id.Length; i++) {

                int numColumns = dataConfig.sensors[i+1] - dataConfig.sensors[i];
                string createTable = CreateTable(dataConfig, headerArrayQuery, dataConfig.sensors[i], numColumns, id[i], record);

                // create a sql string for doing bulk insert
                string copyInto = CreateCopy(dataConfig, headerArrayQuery, dataConfig.sensors[i], numColumns);
                WriteToDB.WriteData(createTable, copyInto, dataConfig.sensors[i], numColumns, record, headerArrayQuery[dataConfig.sensors[i]], headerArrayQuery, id[i]);
            }
        }

        public static string CreateTable(ParserConfig dataConfig, string[] headerArrayQuery, int startIndex, int numTableColumns, int sensorID, List<String> record) {
            string createTable = @"CREATE TABLE IF NOT EXISTS "+headerArrayQuery[startIndex] + @" (";
            
            createTable += "sensorid     INTEGER       NOT NULL,";
            createTable += "time     TIMESTAMPTZ       NOT NULL,";
            for (int i = startIndex; i < startIndex + numTableColumns; i++) {
                if (Decimal.TryParse(record[i], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal f)) {
                    createTable += headerArrayQuery[i] + " NUMERIC       NULL,";
                }
                else {
                    createTable += headerArrayQuery[i] + " TEXT          NULL,";
                }
            }
            createTable += " PRIMARY KEY (sensorid, time));";
            return createTable;
        }

        public static string CreateCopy(ParserConfig dataConfig, string[] headerArrayQuery, int startIndex, int numTableColumns) {
            string insertInto = "COPY "+headerArrayQuery[startIndex] + @"(sensorid, time,";

            for (int i = startIndex; i < startIndex+numTableColumns-1; i++) {
                insertInto += headerArrayQuery[i] + ", ";
            }

            insertInto += headerArrayQuery[startIndex+numTableColumns-1] + ") FROM STDIN";
            return insertInto;
        }

        private static string[] cleanString(string[] str) {
            // make the column names on a format that the database can handle
            str = Array.ConvertAll(str, d => d.ToLower());                    // all headers to lower since column names in timescale needs to be lower                   
            str = str.Select(x => x.Replace(" ", string.Empty).Replace(".", string.Empty)
                                .Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", "_").Replace("[", "_")
                                .Replace("]", "_").Replace("%", "percent").Replace("/", "_per_")).ToArray();   // remove charachters that timescaledb can not use in column names
            str = str.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            return str;
        }
    }
}