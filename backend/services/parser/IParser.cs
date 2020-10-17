using System;
using System.Collections.Generic;


namespace parser
{
    public interface IParser
    {
        (List<String>, string[]) ParseFile(string filePath, bool debug=false);
    }
}