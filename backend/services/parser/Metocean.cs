
namespace parser
{
    public class Metocean : DataClass
    {
        public Metocean() {
            measurement="Metocean";
            headerRow=1;
            headerExtra="";
            tagIndexes= (0, 0);
            fieldIndexes=(1, 51);
            timeIndex=0;
            timeFormat="yyyy-MM-ddTHH:mm:ssZ";
            timeFormatTimescaleDB= "yyyy-MM-dd HH:mm:ss.FFF";
            columnSeparator=";";
        }
    }
}