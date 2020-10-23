namespace parser.Config
{
    public class ParserConfig : IParserConfig
    {
        public string measurement {get; set;}  // name of measurement/data
        public int headerRow {get; set;}  // the row of the header
        public string headerExtra {get; set;}  // redundant words in header to be removed when parsed
        public int[] tagIndexes {get; set;}  // (start, stop) column index of tag data columns
        public int[] fieldIndexes {get; set;}  // (start, stop) column index of field values columns,
        public int timeIndex {get; set;}  // column index of timestamp
        public string timeFormat {get; set;}  // format of time string
        public string timeFormatTimescaleDB {get; set;} // format of time string for timescaledb Copy
        public string columnSeparator {get; set;}  // separator between columns (e.g. ";", "\t")
        public int[] sensors {get; set;} // indexes which specifies where a sensor starts
    }

    public interface IParserConfig
    {
        string measurement {get; set;}  // name of measurement/data
        int headerRow {get; set;}  // the row of the header
        string headerExtra {get; set;}  // redundant words in header to be removed when parsed
        int[] tagIndexes {get; set;}  // (start, stop) column index of tag data columns
        int[] fieldIndexes {get; set;}  // (start, stop) column index of field values columns,
        int timeIndex {get; set;}  // column index of timestamp
        string timeFormat {get; set;}  // format of time string
        string timeFormatTimescaleDB {get; set;} // format of time string for timescaledb Copy
        string columnSeparator {get; set;}  // separator between columns (e.g. ";", "\t")
        int[] sensors {get; set;} // indexes which specifies where a sensor starts
    }
}