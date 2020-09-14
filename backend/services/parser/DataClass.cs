
namespace parser
{
    public class DataClass {
        public string measurement {get; set;}  // name of measurement/data
        public int headerRow {get; set;}  // the row of the header
        public string headerExtra {get; set;}  // redundant words in header to be removed when parsed
        public (int, int) tagIndexes {get; set;}  // (start, stop) column index of tag data columns
        public (int, int) fieldIndexes {get; set;}  // (start, stop) column index of field values columns,
        public int timeIndex {get; set;}  // column index of timestamp
        public string timeFormat {get; set;}  // format of time string
        public string timeFormatTimescaleDB {get; set;} // format of time string for timescaledb Copy
        public string columnSeparator {get; set;}  // separator between columns (e.g. ";", "\t")
    }    
}