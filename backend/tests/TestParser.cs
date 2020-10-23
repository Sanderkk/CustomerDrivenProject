using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using parser.Config;
using Xunit;
using parser;

namespace tests
{
    public class TestParser
    {
        [Fact]
        public void ParseOptode()
        {
            // Get config from parser.json file
            var path = Directory.GetCurrentDirectory();
            var newPath = Path.GetFullPath(Path.Combine(path, @"../../../../services/parser/"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("parser.json")
                .Build();

            var dataConfig = builder.GetSection("ParserConfig:Optode").Get<ParserConfig>();

            ParserFilePath parser = new ParserFilePath(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile("../../../../services/parser/Data/Optode/20200812T082107.txt", false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "Record Time", "Record Number", "Sensor Status", "O2Concentration [uM]", "AirSaturation [%]", 
                "Temperature [Deg.C]",  "CalPhase [Deg]",  "TCPhase [Deg]",  "C1RPh [Deg]",  "C2RPh [Deg]", 
                "C1Amp [mV]",  "C2Amp [mV]",  "RawTemp [mV]"
            };
            string[] expectedValues =
            {
                "2020-08-12 08:21:19",
                "2",
                "(0) OK",
                "2.490039E+02",
                "7.844926E+01",
                "1.464006E+01",
                "3.365621E+01",
                "3.365621E+01",
                "4.080542E+01",
                "7.149208E+00",
                "7.785655E+02",
                "7.585151E+02",
                "3.018734E+02"
            };
            for (int i=0; i < dataConfig.fieldIndexes[1]; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 7467;
            Assert.Equal(expectedNumRows, record.Count/dataConfig.fieldIndexes[1]);
        }

        [Fact]
        public void ParseTension()
        {
            // Get config from parser.json file
            var path = Directory.GetCurrentDirectory();
            var newPath = Path.GetFullPath(Path.Combine(path, @"../../../../services/parser/"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("parser.json")
                .Build();

            var dataConfig = builder.GetSection("ParserConfig:Tension").Get<ParserConfig>();

            ParserFilePath parser = new ParserFilePath(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile("../../../../services/parser/Data/Tension/2020-08-25 22.42.24.txt", false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "Timestamp", "Analog channel 0 (RV.009 - 133148)", "Analog channel 1 (RV.004 - 99871)",
                "Analog channel 2 (RV.011 - 90471)", "Analog channel 3 (RV.010 - 133149)",
                "Analog channel 4 (RV.005 - 99875)", "Analog channel 5 (RV.008 - 133147)", "Analog channel 6",
                "Analog channel 7"
            };
            string[] expectedValues =
            {
                "2020-08-25 22:42:18.578",
                "0.962897",
                "0.989190",
                "0.972100",
                "0.966841",
                "0.975715",
                "-0.000752",
                "-0.000752",
                "-0.001081"
            };
            for (int i=0; i < dataConfig.fieldIndexes[1]; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 28794; // number of rows
            Assert.Equal(expectedNumRows, record.Count/dataConfig.fieldIndexes[1]);
        }

        [Fact]
        public void ParseWavedata()
        {
            // Get config from parser.json file
            var path = Directory.GetCurrentDirectory();
            var newPath = Path.GetFullPath(Path.Combine(path, @"../../../../services/parser/"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("parser.json")
                .Build();

            var dataConfig = builder.GetSection("ParserConfig:Wavedata").Get<ParserConfig>();

            ParserFilePath parser = new ParserFilePath(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile("../../../../services/parser/Data/ACE_Buoy_Wavedata.csv", false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "Time", "hm0", "hm0a", "hm0b", "hmax", "mdir", "mdira", "mdirb", "sprtp", "thhf", "thmax",
                "thtp", "tm01", "tm02", "tm02a", "tm02b", "tp", "uptime"
            };
            string[] expectedValues =
            {
                "2020-07-27 00:00:00", "0.62988", "0.51758", "0.35645", "1.01807", "253.82813",
                "254.5313", "84.02344", "73.8281", "124.10156", "24.1211", "257.34375", "7.7148",
                "5.2734", "17.7930", "3.0859", "23.8281", "3426662.00000000"
            };
            for (int i=0; i < dataConfig.fieldIndexes[1]; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 744;
            Assert.Equal(expectedNumRows, record.Count/dataConfig.fieldIndexes[1]);
        }

        [Fact]
        public void ParseMetocean()
        {
            // Get config from parser.json file
            var path = Directory.GetCurrentDirectory();
            var newPath = Path.GetFullPath(Path.Combine(path, @"../../../../services/parser/"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(newPath)
                .AddJsonFile("parser.json")
                .Build();

            var dataConfig = builder.GetSection("ParserConfig:Metocean").Get<ParserConfig>();

            ParserFilePath parser = new ParserFilePath(dataConfig);
            (List<String>, string[]) parsedFile = parser.ParseFile("../../../../services/parser/Data/ACE_Buoy_Metoceandata.csv", false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "TIMESTAMP (ISO-8601) UTC", "airHumidity %", "airPressure hPa", "airTemperature C",
                "conductivity1m mS/cm", "currentDirection05m deg", "currentDirection08m deg",
                "currentDirection11m deg", "currentDirection14m deg", "currentDirection17m deg",
                "currentDirection20m deg", "currentDirection23m deg", "currentDirection26m deg",
                "currentDirection29m deg", "currentDirection32m deg", "currentDirection35m deg",
                "currentDirection38m deg", "currentDirection41m deg", "currentDirection44m deg",
                "currentDirection47m deg", "currentDirection50m deg", "currentDirection53m deg",
                "currentDirection56m deg", "currentDirection59m deg", "currentDirection62m deg",
                "currentSpeed05m cm/s", "currentSpeed08m cm/s", "currentSpeed11m cm/s", "currentSpeed14m cm/s",
                "currentSpeed17m cm/s", "currentSpeed20m cm/s", "currentSpeed23m cm/s", "currentSpeed26m cm/s",
                "currentSpeed29m cm/s", "currentSpeed32m cm/s", "currentSpeed35m cm/s", "currentSpeed38m cm/s",
                "currentSpeed41m cm/s", "currentSpeed44m cm/s", "currentSpeed47m cm/s", "currentSpeed50m cm/s",
                "currentSpeed53m cm/s", "currentSpeed56m cm/s", "currentSpeed59m cm/s", "currentSpeed62m cm/s",
                "salinity1m ppt", "temperature1m degC", "waterTempNortek degC", "WindDir deg", "WindGust m/s",
                "WindSpeed m/s"
            };

            string[] expectedValues =
            {
                "2020-07-27 00:00:00", "57.65625", "1001.56250", "19.62891", "37.43591",
                "5.27344", "359.64844", "295.66406", "213.75000", "202.85156", "226.05469",
                "170.15625", "184.21875", "173.67188", "113.55469", "117.42188", "301.99219",
                "96.67969", "55.54688", "159.60938", "180.00000", "152.57813", "150.46875",
                "185.97656", "146.25000", "22.55859", "13.18359", "3.51563", "7.32422", "7.03125",
                "3.80859", "2.92969", "1.75781", "4.39453", "2.63672", "2.92969", "1.75781", 
                "7.91016", "2.34375", "0.87891", "3.51563", "2.92969", "2.63672", "2.05078",
                "0.58594", "31.65039", "13.00659", "12.99561", "125.85938", "15.52734", "10.72266"
            };
            for (int i=0; i < dataConfig.fieldIndexes[1]; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 1488;
            Assert.Equal(expectedNumRows, record.Count/dataConfig.fieldIndexes[1]);
        }
    }
}