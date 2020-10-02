using HotChocolate;
using HotChocolate.Types;
using src.Api.Types;
using src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Api.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class TestQuery
    {
        public TestType GetTest()
        {
            TestType test = new TestType()
            {
                Name = "Test name",
                Description = "Test description"
            };
            return test;
        }
        
    }
}
