using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using parser.Config;


namespace parser
{
    public class ParserString : IParser
    {

        private string measurement;
        private int headerRow;
        private string headerExtra;
        private int[] tagIndexes;
        private int[] fieldIndexes;
        private int timeIndex;
        private string timeFormat;
        private string timeFormatTimescaleDB;
        private string columnSeparator;
        public ParserString(IParserConfig parserSettings)
        {
            measurement = parserSettings.measurement;
            headerRow = parserSettings.headerRow;
            headerExtra = parserSettings.headerExtra;
            tagIndexes = parserSettings.tagIndexes;
            fieldIndexes = parserSettings.fieldIndexes;
            timeIndex = parserSettings.timeIndex;
            timeFormat = parserSettings.timeFormat;
            timeFormatTimescaleDB = parserSettings.timeFormatTimescaleDB;
            columnSeparator = parserSettings.columnSeparator;
        }

        public (List<String>, string[]) ParseFile(string data, bool debug=false)
        {
            /*
            Parse file into list of data points

            :param str data: one long string with all the data
            :param debug: enables logging of parsed lines
            :return: (List<String>, string[]) of parsed data and headers
            */

            string[] dataList = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            List<String> record = new List<String>();  // array of parsed lines
            string line = null;
            for (int i = 0; i < headerRow-1; i++) {
                dataList = dataList.Skip(1).ToArray();
            }
            string[] headerArray = dataList[0].Split(columnSeparator);
            var headerArrayQuery = headerArray.Where(val => val != headerExtra).ToArray();    // remove entries of extra text
            headerArrayQuery = headerArrayQuery.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (debug) {
                foreach (string str in headerArrayQuery) {
                    Console.WriteLine(str);
                }
            }

            dataList = dataList.Skip(1).ToArray(); // skips the header after saving it to a new variable

            foreach (string row in dataList) {
                if (row == "") {
                    break;
                }
                line = row.Replace("\t\t", "\t"); // replace double tab with single tab
                string[] lineArray = line.Split(columnSeparator);
                DateTime date;

                if (DateTime.TryParseExact(lineArray[timeIndex], timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date)) {
                }
                else {
                    Console.WriteLine("'{0}' is not in an acceptable format.", lineArray[timeIndex]);
                }

                record.Add(date.ToString(timeFormatTimescaleDB, CultureInfo.InvariantCulture)); // adding the date on necessary format for timescaledb
                
                for (int i = tagIndexes[0]; i < tagIndexes[1]; i++) {
                    record.Add(lineArray[i]);
                }
                for (int i = fieldIndexes[0]; i < fieldIndexes[1]; i++) {
                    record.Add(lineArray[i].Replace("\n", "").Replace("\r", "")); // CultureInfo.InvariantCulture because the numbers contain "."
                }
                if (debug) {
                    foreach (string str in headerArrayQuery) {
                        Console.WriteLine(str);
                    }
                }
                
            }
            (List<String>, string[]) result = (record, headerArrayQuery);

            return result;
        }
    }
}