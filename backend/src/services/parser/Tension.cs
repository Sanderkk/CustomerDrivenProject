using System;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace parser
{
    class Tension : DataClass
    {
        public Tension() {
            measurement="Tension";
            headerRow=2;
            headerExtra="";
            timeIndex=0;
            timeFormat="yyyy.MM.dd HH:mm:ss.fff";
            timePrecision=WritePrecision.Ms;
            columnSeparator="\t";
        }
    }
}