using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using parser;
using static System.String;

namespace src.Utils
{
    public class Csv
    {
        private List<CsvRow> csvRows;
        private readonly string _delimiter = ",";
        private readonly string[] _header;

        public Csv(List<CsvRow> csvRows, string[] header)
        {
            this.csvRows = csvRows;
            _header = header;
        }

        public string[] Header => _header;
        public string Delimiter => _delimiter;
        public List<CsvRow> CsvRows => csvRows;

        public static Csv FromTextFile(string filePath, DataClass dataClass)
        {
            /*
            Parse file into list of data points

            :param str file_path: file to read
            :param DataClass data_class: type of data
            :param debug: enables logging of parsed lines
            :return: List<PointData> of parsed data
            */

            List<CsvRow> record = new List<CsvRow>(); // array of parsed lines
            IEnumerable<string> lines = File.ReadLines(filePath);
            string header = lines.Skip(dataClass.headerRow - 1).Take(1).First();
            string[] headerArray = header.Split(dataClass.columnSeparator).ToArray();
            string[] headerArrayQuery =
                headerArray.Where(s => s != dataClass.headerExtra)
                    .Where(s => !IsNullOrEmpty(s))
                    .ToArray(); // remove entries of extra text


            foreach (string input in lines.Skip(dataClass.headerRow))
            {
                string cleanedLine = input.Replace("\t\t", "\t");
                string[] lineArray = cleanedLine
                    .Split(dataClass.columnSeparator)
                    .Where(s => !IsNullOrEmpty(s))
                    .ToArray();
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
            using StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true));
            sw.WriteLine(Join(_delimiter, _header));
            foreach (CsvRow row in csvRows)
            {
                sw.WriteLine(row.ToString(_delimiter));
            }
        }

        public void PrintCsv()
        {
            foreach (CsvRow line in csvRows)
            {
                line.PrintValue();
            }
        }
    }
}