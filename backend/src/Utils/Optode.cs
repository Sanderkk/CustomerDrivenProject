using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using src.Pipeline;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace src.Pipeline
{
    // Record Time	C1RPh [Deg]	Status	C2RPh [Deg]	Status	C1Amp [mV]	Status	C2Amp [mV]	Status	RawTemp [mV]	Status	
    class Optode
    {
        public string RecordTime { get; set; }
        public int RecordNumber { get; set; }
        public string SensorStatus { get; set; }
        public int O2Concentration { get; set; }
        public int AirSaturation { get; set; }
        public int Temperature { get; set; }
        public int CalPhase { get; set; }
        public int TcPhase { get; set; }
        public int C1RPh { get; set; }
        public int C2RPh { get; set; }
        public int C1Amp { get; set; }
        public int C2Amp { get; set; }
        public int RawTemp { get; set; }

        public List<Optode> ParseTextFile(string name)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<Optode>(csvParserOptions, new OptodeMapping());

            var records = csvParser.ReadFromFile(name, Encoding.UTF8);
            Console.WriteLine(records);
            return records.Select(x => x.Result).ToList();

            Console.WriteLine("it's working");
        }
    }
}

class OptodeMapping : CsvMapping<Optode>
{
    public OptodeMapping() : base()
    {
        MapProperty(0, x => x.RecordTime);
        MapProperty(1, x => x.RecordNumber);
        MapProperty(2, x => x.SensorStatus);
        MapProperty(3, x => x.O2Concentration);
        MapProperty(4, x => x.AirSaturation);
        MapProperty(5, x => x.Temperature);
        MapProperty(6, x => x.CalPhase);
        MapProperty(7, x => x.TcPhase);
        MapProperty(8, x => x.C1RPh);
        MapProperty(9, x => x.C2RPh);
        MapProperty(10, x => x.C1Amp);
        MapProperty(11, x => x.C2Amp);
        MapProperty(12, x => x.RawTemp);
    }
}