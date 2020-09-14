using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using parser;
using Optode = src.Pipeline.Optode;

namespace src.Utils
{
    public class Csv
    {
        private List<CsvRow> _csvRows;
        private string _delimiter = ",";
        private string[] _header;

        public Csv(List<CsvRow> csvRows, string[] header)
        {
            _csvRows = csvRows;
            _header = header;
        }

        public static Csv FromTextFile(string filePath, DataClass dataClass, bool debug = false)
        {
            /*
            Parse file into list of data points

            :param str file_path: file to read
            :param DataClass data_class: type of data
            :param debug: enables logging of parsed lines
            :return: List<PointData> of parsed data
            */

            var record = new List<CsvRow>(); // array of parsed lines
            IEnumerable<string> lines = File.ReadLines(filePath);
            string header = lines.Skip(dataClass.headerRow - 1).Take(1).First();
            string[] headerArray = header.Split(dataClass.columnSeparator).ToArray();

            string[] headerArrayQuery =
                headerArray.Where(s => s != dataClass.headerExtra).ToArray(); // remove entries of extra text


            foreach (var input in lines.Skip(dataClass.headerRow))
            {
                string cleanedLine = input.Replace("\t\t", "\t");
                string[] lineArray = cleanedLine.Split(dataClass.columnSeparator);
                DateTime timestamp = DateTimeUtils.ParseDate(lineArray[dataClass.timeIndex], dataClass.timeFormat);
                CsvRow csvRow = new CsvRow(lineArray.Length, timestamp);
                for (int i = 0; i < lineArray.Length; i++)
                {
                    csvRow.AddValue(headerArrayQuery[i], lineArray[i]);
                }

                record.Add(csvRow);
            }

            return new Csv(record, headerArrayQuery);
        }

        public void WriteCsvFile(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                sw.WriteLine(string.Join(_delimiter, _header));
                foreach (CsvRow row in this._csvRows)
                {
                    sw.WriteLine(row.ToString(_delimiter));
                }
            }
        }

        public void PrintCsv()
        {
            foreach (var line in _csvRows)
            {
                line.PrintValue();
            }
        }
    }
}