
namespace parser
{
    public class Optode : DataClass {
        public Optode() {
            measurement="Optode";
            headerRow=2;
            headerExtra="Status";
            tagIndexes=(1, 3);
            fieldIndexes=(3, 13);
            timeIndex=0;
            timeFormat="yyyy-MM-dd HH.mm.ss";
            timeFormatTimescaleDB= "yyyy-MM-dd HH:mm:ss.FFF";
            columnSeparator="\t";
        }
    }
}
        