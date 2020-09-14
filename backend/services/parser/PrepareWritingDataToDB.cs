using System;
using System.Collections.Generic;
using System.Globalization;
using src.Database;

namespace parser
{
    public class PrepareWritingDataToDB
    {
        public static void PrepareQuery(List<String> record, DataClass dataClass, string[] headerArrayQuery)
        {

            int numColumns = dataClass.fieldIndexes.Item2;  // number of columns in the file

            string createTable = CreateTable(dataClass, headerArrayQuery, numColumns, record);

            // create a sql string for doing bulk insert
            string copyInto = CreateCopy(dataClass, headerArrayQuery, numColumns);
            
            WriteToDB.WriteData(createTable, copyInto, numColumns, record, dataClass.measurement);
        }

        public static string CreateTable(DataClass dataClass, string[] headerArrayQuery, int numColumns, List<String> record) {
            string createTable = @"CREATE TABLE IF NOT EXISTS "+dataClass.measurement+@" (";
            
            for (int i = 0; i < numColumns; i++) {
                if (i == 0) {
                    createTable += "time     TIMESTAMP       NOT NULL,";
                }
                else {
                    if (Decimal.TryParse(record[i], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal f)) {
                        createTable += headerArrayQuery[i] + " NUMERIC       NULL,";
                    }
                    else {
                        createTable += headerArrayQuery[i] + " TEXT          NULL,";
                    }
                }
            }
            createTable = createTable.Remove(createTable.Length - 1) + ");";
            return createTable;
        }

        public static string CreateCopy(DataClass dataClass, string[] headerArrayQuery, int numColumns) {
            string insertInto = "COPY "+dataClass.measurement+@"(time,";

            for (int i = 1; i < numColumns-1; i++) {
                insertInto += headerArrayQuery[i] + ", ";
            }

            insertInto += headerArrayQuery[numColumns-1] + ") FROM STDIN";
            return insertInto;
        }
    }
}