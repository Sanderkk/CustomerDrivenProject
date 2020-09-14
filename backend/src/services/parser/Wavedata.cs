using System;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace parser
{
    class Wavedata : DataClass
    {
        public Wavedata() {
            measurement="Wavedata";
            headerRow=2;
            headerExtra="";
            timeIndex=0;
            timeFormat="dd.MM.yyyy HH:mm:ss";
            timePrecision=WritePrecision.S;
            columnSeparator=";";
        }
    }
}