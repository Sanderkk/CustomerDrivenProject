
namespace parser
{
    public class Tension : DataClass
    {
        public Tension() {
            measurement="Tension";
            headerRow=2;
            headerExtra="";
            tagIndexes= (0, 0);
            fieldIndexes=(1, 9);
            timeIndex=0;
            timeFormat="yyyy.MM.dd HH:mm:ss.FFF";
            timeFormatTimescaleDB= "yyyy-MM-dd HH:mm:ss.FFF";
            columnSeparator="\t";
        }
    }
}