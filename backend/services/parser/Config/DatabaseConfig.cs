﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parser.Config
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string DatabaseConnectionString { get; set; }
    }

    public interface IDatabaseConfig
    {
        string DatabaseConnectionString { get; set; }
    }
}
