
namespace parser
{
    public class Wavedata : DataClass
    {
        public Wavedata() {
            measurement="Wavedata";
            headerRow=2;
            headerExtra="";
            tagIndexes= (0, 0);
            fieldIndexes=(1, 18);
            timeIndex=0;
            timeFormat="dd.MM.yyyy HH:mm:ss";
            timeFormatTimescaleDB= "yyyy-MM-dd HH:mm:ss.FFF";
            columnSeparator=";";
        }
    }
}