using System;
using System.Globalization;
using System.Collections.Generic;
using src.Utils;
using Xunit;
using parser;

namespace tests
{
    public class TestParser
    {
        [Fact]
        public void ParseOptode()
        {
            DataClass dataClass = new Optode();
            (List<String>, string[]) parsedFile = Program.ParseFile("../../../../services/parser/Data/Optode/20200812T082107.txt", dataClass, false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "recordtime", "recordnumber", "sensorstatus", "o2concentration_um_", "airsaturation_percent_",
                "temperature_degc_", "calphase_deg_", "tcphase_deg_", "c1rph_deg_", "c2rph_deg_", "c1amp_mv_",
                "c2amp_mv_", "rawtemp_mv_"
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
            for (int i=0; i < dataClass.fieldIndexes.Item2; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 7467;
            Assert.Equal(expectedNumRows, record.Count/dataClass.fieldIndexes.Item2);
        }

        [Fact]
        public void ParseTension()
        {
            DataClass dataClass = new Tension();
            (List<String>, string[]) parsedFile = Program.ParseFile("../../../../services/parser/Data/Tension/2020-08-25 22.42.24.txt", dataClass, false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "timestamp",
                "analogchannel0rv009_133148",
                "analogchannel1rv004_99871",
                "analogchannel2rv011_90471",
                "analogchannel3rv010_133149",
                "analogchannel4rv005_99875",
                "analogchannel5rv008_133147",
                "analogchannel6",
                "analogchannel7"
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
            for (int i=0; i < dataClass.fieldIndexes.Item2; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 28794; // number of rows
            Assert.Equal(expectedNumRows, record.Count/dataClass.fieldIndexes.Item2);
        }

        [Fact]
        public void ParseWavedata()
        {
            DataClass dataClass = new Wavedata();
            (List<String>, string[]) parsedFile = Program.ParseFile("../../../../services/parser/Data/ACE_Buoy_Wavedata.csv", dataClass, false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "time", "hm0", "hm0a", "hm0b", "hmax", "mdir", "mdira", "mdirb", "sprtp", "thhf", "thmax",
                "thtp", "tm01", "tm02", "tm02a", "tm02b", "tp", "uptime"
            };
            string[] expectedValues =
            {
                "2020-07-27 00:00:00", "0.62988", "0.51758", "0.35645", "1.01807", "253.82813",
                "254.5313", "84.02344", "73.8281", "124.10156", "24.1211", "257.34375", "7.7148",
                "5.2734", "17.7930", "3.0859", "23.8281", "3426662.00000000"
            };
            for (int i=0; i < dataClass.fieldIndexes.Item2; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 744;
            Assert.Equal(expectedNumRows, record.Count/dataClass.fieldIndexes.Item2);
        }

        [Fact]
        public void ParseMetocean()
        {
            DataClass dataClass = new Metocean();
            (List<String>, string[]) parsedFile = Program.ParseFile("../../../../services/parser/Data/ACE_Buoy_Metoceandata.csv", dataClass, false);
            List<String> record = parsedFile.Item1;
            string[] headerArrayQuery = parsedFile.Item2;
            string[] expected =
            {
                "timestampiso_8601utc", "airhumiditypercent", "airpressurehpa", "airtemperaturec",
                "conductivity1mms_per_cm", "currentdirection05mdeg", "currentdirection08mdeg",
                "currentdirection11mdeg", "currentdirection14mdeg", "currentdirection17mdeg",
                "currentdirection20mdeg", "currentdirection23mdeg", "currentdirection26mdeg",
                "currentdirection29mdeg", "currentdirection32mdeg", "currentdirection35mdeg",
                "currentdirection38mdeg", "currentdirection41mdeg", "currentdirection44mdeg",
                "currentdirection47mdeg", "currentdirection50mdeg", "currentdirection53mdeg",
                "currentdirection56mdeg", "currentdirection59mdeg", "currentdirection62mdeg",
                "currentspeed05mcm_per_s", "currentspeed08mcm_per_s", "currentspeed11mcm_per_s",
                "currentspeed14mcm_per_s", "currentspeed17mcm_per_s", "currentspeed20mcm_per_s",
                "currentspeed23mcm_per_s", "currentspeed26mcm_per_s", "currentspeed29mcm_per_s",
                "currentspeed32mcm_per_s", "currentspeed35mcm_per_s", "currentspeed38mcm_per_s",
                "currentspeed41mcm_per_s", "currentspeed44mcm_per_s", "currentspeed47mcm_per_s",
                "currentspeed50mcm_per_s", "currentspeed53mcm_per_s", "currentspeed56mcm_per_s",
                "currentspeed59mcm_per_s", "currentspeed62mcm_per_s", "salinity1mppt",
                "temperature1mdegc", "watertempnortekdegc", "winddirdeg", "windgustm_per_s",
                "windspeedm_per_s"
            };

            // added 2 hours on the text file, since it was in utc
            string[] expectedValues =
            {
                "2020-07-27 02:00:00", "57.65625", "1001.56250", "19.62891", "37.43591",
                "5.27344", "359.64844", "295.66406", "213.75000", "202.85156", "226.05469",
                "170.15625", "184.21875", "173.67188", "113.55469", "117.42188", "301.99219",
                "96.67969", "55.54688", "159.60938", "180.00000", "152.57813", "150.46875",
                "185.97656", "146.25000", "22.55859", "13.18359", "3.51563", "7.32422", "7.03125",
                "3.80859", "2.92969", "1.75781", "4.39453", "2.63672", "2.92969", "1.75781", 
                "7.91016", "2.34375", "0.87891", "3.51563", "2.92969", "2.63672", "2.05078",
                "0.58594", "31.65039", "13.00659", "12.99561", "125.85938", "15.52734", "10.72266"
            };
            for (int i=0; i < dataClass.fieldIndexes.Item2; i++) {
                Assert.Equal(expected[i], headerArrayQuery[i]);
                Assert.Equal(expectedValues[i], record[i]);
            }

            int expectedNumRows = 1488;
            Assert.Equal(expectedNumRows, record.Count/dataClass.fieldIndexes.Item2);
        }
    }
}