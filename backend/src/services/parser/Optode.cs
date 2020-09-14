using System;
using InfluxDB.Client.Api.Domain;


namespace parser
{
    public class Optode : DataClass {
        public Optode() {
            measurement="Optode";
            headerRow=2;
            headerExtra="Status";
            timeIndex=0;
            timeFormat="yyyy-MM-dd HH.mm.ss";
            timePrecision=WritePrecision.S;
            columnSeparator="\t";
        }
    }
}
        