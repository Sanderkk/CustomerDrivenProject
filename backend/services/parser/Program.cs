using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;


namespace parser
{
    public class Program
    {
        static public void Main(string[] args)
        {
            //DataClass dataClass = new Optode();
            //(List<String>, string[]) parsedFile = ParseFile("Data/Optode/20200812T082107.txt", dataClass, false);

            //DataClass dataClass = new Tension();
            //(List<String>, string[]) parsedFile = ParseFile("Data/Tension/2020-08-25 22.42.24.txt", dataClass, false);

            //DataClass dataClass = new Wavedata();
            //(List<String>, string[]) parsedFile = ParseFile("Data/ACE_Buoy_Wavedata.csv", dataClass, false);

            DataClass dataClass = new Metocean();
            (List<String>, string[]) parsedFile = ParseFile("Data/ACE_Buoy_Metoceandata.csv", dataClass, false);
            PrepareWritingDataToDB.PrepareQuery(parsedFile.Item1, dataClass, parsedFile.Item2, 1);
        }

        public static string RemoveWhiteSpaces(string str)
        {
            /*
            Fast code to remove withspace

            :param string with witespace
            :return string without withespace
            */

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (!Char.IsWhiteSpace(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

         public static (List<String>, string[]) ParseFile(string filePath, DataClass dataClass, bool debug=false)
         {
            /*
            Parse file into list of data points

            :param str file_path: file to read
            :param DataClass data_class: type of data
            :param debug: enables logging of parsed lines
            :return: (List<String>, string[]) of parsed data and headers
            */

            StreamReader reader = File.OpenText(filePath);

            List<String> record = new List<String>();  // array of parsed lines
            string line = null;
            for (int i = 0; i < dataClass.headerRow; i++) {
                Console.WriteLine(i);
                line = reader.ReadLine();
            }

            string[] headerArray = line.Split(dataClass.columnSeparator);
            var headerArrayQuery = headerArray.Where(val => val != dataClass.headerExtra).ToArray();    // remove entries of extra text
            headerArrayQuery = Array.ConvertAll(headerArrayQuery, d => d.ToLower());                    // all headers to lower since column names in timescal needs to be lower                   
            headerArrayQuery = headerArrayQuery.Select(x => x.Replace(" ", string.Empty).Replace(".", string.Empty)
                                .Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", "_").Replace("[", "_")
                                .Replace("]", "_").Replace("%", "percent").Replace("/", "_per_")).ToArray();   // remove charachters that timescaledb can not use in column names

            if (debug) {
                Console.WriteLine(headerArrayQuery);
            }

            while ((line = reader.ReadLine()) != null) {
                line = line.Replace("\t\t", "\t"); // replace double tab with single tab
                string[] lineArray = line.Split(dataClass.columnSeparator);
                DateTime date;
                if (DateTime.TryParseExact(lineArray[dataClass.timeIndex], dataClass.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                }
                else {
                    Console.WriteLine("'{0}' is not in an acceptable format.", lineArray[dataClass.timeIndex]);
                }

                record.Add(date.ToString(dataClass.timeFormatTimescaleDB)); // adding the date on necessary format for timescaledb
                
                for (int i = dataClass.tagIndexes.Item1; i < dataClass.tagIndexes.Item2; i++) {
                    record.Add(lineArray[i]);
                }
                for (int i = dataClass.fieldIndexes.Item1; i < dataClass.fieldIndexes.Item2; i++) {
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
