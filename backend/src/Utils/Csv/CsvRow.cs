using System;
using System.Collections.Generic;
using System.Linq;

namespace src.Utils
{
    public class CsvRow
    {
        private List<CsvValue> _values = new List<CsvValue>();
        private int _size;
        private DateTime _timestamp;

        public CsvRow(int size, List<CsvValue> values, DateTime timestamp)
        {
            _size = size;
            _values = values;
            _timestamp = timestamp;
        }

        public CsvRow(int size, DateTime timestamp)
        {
            _timestamp = timestamp;
            _size = size;
        }

        public string Header { get; set; }
        public int Size { get; set; }
        public int Values { get; set; }
        public DateTime Timestamp { get; set; }

        public void AddValue(CsvValue csvValue)
        {
            if (_values != null)
            {
                _values.Add(csvValue);
            }
        }

        public void AddValue(string header, string value)
        {
            AddValue(new CsvValue(header, value));
        }
        
        public void AddValue(string header, float value)
        {
            AddValue(new CsvValue(header, value));
        }

        public string[] getValues()
        {
            string[] values = new string[_values.Count];
            for (int i = 0; i < _values.Count; i++)
            {
                values[i] = _values[i].Value;
            }

            return values;
        }

        public void PrintValue()
        {
            foreach (var value in _values)
            {
                value.PrintValue();
            }
        }

        public string ToString(string delimiter)
        {
            string s = string.Join(delimiter, _values);
            return $"{_timestamp}{delimiter}{s}";
        }
    }
}