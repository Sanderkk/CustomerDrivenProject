using System;
using System.Collections.Generic;

namespace src.Database
{
    public class DataEventArgs : EventArgs
    {
        public int NumColumns { get; set; }

        public List<string> Record { get; set; }

        public string SensorName { get; set; }

        public string[] HeaderArrayQuery { get; set; }

        public int SensorID { get; set; }


        public DataEventArgs(int numColumns, List<string> record, string sensorName, string[] headerArrayQuery, int sensorID) // ToDo: Ensure correct parameters
        {
            this.NumColumns = numColumns; 

            this.Record = record;

            this.SensorName = sensorName;

            this.HeaderArrayQuery = headerArrayQuery;

            this.SensorID = sensorID;
        }
    }
}
