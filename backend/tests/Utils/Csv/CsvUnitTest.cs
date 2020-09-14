using parser;
using src.Utils;
using Xunit;
using parser;

namespace tests.Utils.Csv
{
    public class CsvUnitTest
    {
        [Fact]
        public void ShouldParseOptode()
        {
            DataClass dataClass = new Optode();
            src.Utils.Csv csv = src.Utils.Csv.FromTextFile("../../../resources/Optode.txt", dataClass);
            string[] expected =
            {
                "Record Time", "Record Number", "Sensor Status", "O2Concentration [uM]", "AirSaturation [%]",
                "Temperature [Deg.C]", "CalPhase [Deg]", "TCPhase [Deg]", "C1RPh [Deg]", "C2RPh [Deg]", "C1Amp [mV]",
                "C2Amp [mV]", "RawTemp [mV]"
            };
            string[] expectedValues =
            {
                "2020-08-12 08.21.19",
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
            string[] actualValues = csv.CsvRows[0].getValues();
            Assert.Equal(expected, csv.Header);
            Assert.Equal(13, csv.Header.Length);
            Assert.Equal(expectedValues, actualValues);
        }

        [Fact]
        public void ShouldParseTension()
        {
            DataClass dataClass = new Optode();
            src.Utils.Csv csv = src.Utils.Csv.FromTextFile("../../../resources/Tension.txt", dataClass);

            string[] expected =
            {
                "Timestamp",
                "Analog channel 0 (RV.009 - 133148)",
                "Analog channel 1 (RV.004 - 99871)",
                "Analog channel 2 (RV.011 - 90471)",
                "Analog channel 3 (RV.010 - 133149)",
                "Analog channel 4 (RV.005 - 99875)",
                "Analog channel 5 (RV.008 - 133147)",
                "Analog channel 6",
                "Analog channel 7"
            };
            string[] expectedValues =
            {
                "2020.08.25 22:42:18.578",
                "0.962897",
                "0.989190",
                "0.972100",
                "0.966841",
                "0.975715",
                "-0.000752",
                "-0.000752",
                "-0.001081"
            };
            string[] actualValues = csv.CsvRows[0].getValues();
            Assert.Equal(expected, csv.Header);
            Assert.Equal(9, csv.Header.Length);
            Assert.Equal(expectedValues, actualValues);
        }
    }
}