using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using parser.Config;


namespace parser
{
    public class ParserFilePath : IParser
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
        public ParserFilePath(IParserConfig parserSettings)
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

        public (List<String>, string[]) ParseFile(string filePath, bool debug=false)
        {
            /*
            Parse file into list of data points

            :param str file_path: file to read
            :param debug: enables logging of parsed lines
            :return: (List<String>, string[]) of parsed data and headers
            */

            StreamReader reader = File.OpenText(filePath);

            List<String> record = new List<String>();  // array of parsed lines
            string line = null;
            for (int i = 0; i < headerRow; i++) {
                line = reader.ReadLine();
            }

            string[] headerArray = line.Split(columnSeparator);
            var headerArrayQuery = headerArray.Where(val => val != headerExtra).ToArray();    // remove entries of extra text

            if (debug) {
                Console.WriteLine(headerArrayQuery);
            }

            while ((line = reader.ReadLine()) != null) {
                line = line.Replace("\t\t", "\t"); // replace double tab with single tab
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
                    record.Add(lineArray[i]); // CultureInfo.InvariantCulture because the numbers contain "."
                }
                if (debug) {
                    Console.WriteLine(headerArrayQuery);
                }
            }
            (List<String>, string[]) result = (record, headerArrayQuery);

            return result;
        }
    }
}