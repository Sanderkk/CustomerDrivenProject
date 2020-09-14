using System;
using InfluxDB.Client.Api.Domain;


namespace parser
{
    public class DataClass {
        public string measurement {get; set;}  // name of measurement/data
        public int headerRow {get; set;}  // the row of the header
        public string headerExtra {get; set;}  // redundant words in header to be removed when parsed
        public int timeIndex {get; set;}  // column index of timestamp
        public string timeFormat {get; set;}  // format of time string
        public WritePrecision timePrecision {get; set;}  // time precision to be stored in database
        public string columnSeparator {get; set;}  // separator between columns (e.g. ";", "\t")
    }    
}