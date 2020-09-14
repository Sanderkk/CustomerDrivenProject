using System;
using System.Globalization;

namespace src.Utils
{
    public class CsvValue
    {
        private string _header;
        private string _value;

        public CsvValue( string header, string value)
        {
            _value = value;
            _header = header;
        }
        public CsvValue( string header, float value)
        {
            _value = value.ToString(CultureInfo.InvariantCulture);
            _header = header;
        }

        public void PrintValue()
        {
            Console.WriteLine("{0}: {1}", this._header, this._value);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}