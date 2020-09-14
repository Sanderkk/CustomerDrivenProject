using System;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace parser
{
    class Metocean : DataClass
    {
        public Metocean() {
            measurement="Metocean";
            headerRow=1;
            headerExtra="";
            timeIndex=0;
            timeFormat="yyyy-MM-ddTHH:mm:ssZ";
            timePrecision=WritePrecision.S;
            columnSeparator=";";
        }
    }
}