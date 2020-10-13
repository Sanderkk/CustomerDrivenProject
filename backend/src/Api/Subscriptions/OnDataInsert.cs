using System;
using System.Collections.Generic;
using HotChocolate.Language;
using HotChocolate.Subscriptions;
using src.Database;
using src.Api.Types;


namespace src.Api.Subscriptions
{
    public class OnDataInsert
        : EventMessage
    {
        public OnDataInsert(int sensorId, CreatedDataValues data)
            :base(CreateEventDescription(sensorId), data)
        {

        }

        private static EventDescription CreateEventDescription(int sensorId)
        {
            return new EventDescription("onData", 
                    new ArgumentNode("sensorId", 
                            new IntValueNode(sensorId)
                        )
                );
        }
    }
}
