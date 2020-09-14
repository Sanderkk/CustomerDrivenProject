using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using src.Pipeline;

namespace src.Pipeline
{
    // Record Time	C1RPh [Deg]	Status	C2RPh [Deg]	Status	C1Amp [mV]	Status	C2Amp [mV]	Status	RawTemp [mV]	Status	
    class Optode
    {
        [Index(0)]
        public string RecordTime { get; set; }
        [Index(1)]
        public int RecordNumber { get; set; }
        [Index(2)]
        public string SensorStatus { get; set; }
        [Index(3)]
        public int O2Concentration { get; set; }
        [Index(4)]
        public int AirSaturation { get; set; }
        [Index(5)]
        public int Temperature { get; set; }
        public int CalPhase { get; set; }
        public int TcPhase { get; set; }
        public int C1RPh { get; set; }
        public int C2RPh { get; set; }
        public int C1Amp { get; set; }
        public int C2Amp { get; set; }
        public int RawTemp { get; set; }

        public List<Optode> FromCSV(string name)
        {
            TextReader reader = new StreamReader("import.txt");
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<Optode>();

            Console.WriteLine(records);
            return records.ToList();

        }
    }
}

class OptodeMapping : ClassMap<Optode>
{
    public OptodeMapping() : base()
    {
        Map(x => x.RecordTime).Name("RecordTime");
        Map( x => x.RecordNumber).Name("RecordNumber");
        Map( x => x.SensorStatus).Name("SensorStatus");
        Map( x => x.O2Concentration).Name("O2Concentration");
        Map( x => x.AirSaturation).Name("AirSaturation");
        Map( x => x.Temperature).Name("Temperature");
        Map( x => x.CalPhase).Name("Calphase");
        Map(x => x.TcPhase).Name("TcPhase");
        Map( x => x.C1RPh).Name("C1RPh");
        Map( x => x.C2RPh).Name("C2RPh");
        Map( x => x.C1Amp).Name("C1Amp");
        Map( x => x.C2Amp).Name("C2Amp");
        Map( x => x.RawTemp).Name("RawTemp");
    }
}