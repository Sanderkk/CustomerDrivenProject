﻿using System.Collections.Generic;

namespace src.Api.Inputs
{
    public class CellGraphInput
    {
        public string From { get; set; }
        public string To { get; set; } 
        public bool SpecifiedTimePeriod { get; set; }
        public List<int> Sensors { get; set; }
    }
}