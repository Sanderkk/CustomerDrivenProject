using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using parser.Config;


namespace parser
{
    public interface IParser
    {
        (List<String>, string[]) ParseFile(string filePath, bool debug=false);
    }
}