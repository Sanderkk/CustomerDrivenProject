using System;
using System.Linq;

namespace src.Database
{
    public class SubscriptionObject
    {
        public int[] sensorIds { get; }
        public DateTime fromDate { get; }
        public DateTime toDate { get;  }
        
        public SubscriptionObject(int[] sensorIds, DateTime fromDate, DateTime toDate)
        {
            this.sensorIds = sensorIds;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }
    }
}